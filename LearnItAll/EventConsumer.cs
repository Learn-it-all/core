using Microsoft.Azure.Cosmos;
using Mtx.LearnItAll.Core.Blueprints;
using System.Net;

namespace LearnItAll
{
	// ----------------------------------------------------------------------------------------------------------
	// Prerequisites - 
	// 
	// 1. An Azure Cosmos account - 
	//    https://docs.microsoft.com/azure/cosmos-db/create-cosmosdb-resources-portal
	//
	// 2. Microsoft.Azure.Cosmos NuGet package - 
	//    http://www.nuget.org/packages/Microsoft.Azure.Cosmos/ 
	// ----------------------------------------------------------------------------------------------------------
	// https://github.com/Azure/azure-cosmos-dotnet-v3/blob/master/Microsoft.Azure.Cosmos.Samples/Usage/ChangeFeed/Program.cs
	// Sample - demonstrates common Change Feed operations
	//
	// 1. Listening for changes that happen after a Change Feed Processor is started.
	//
	// 2. Listening for changes that happen after a certain point in time.
	//
	// 3. Listening for changes that happen since the container was created.
	//
	// 4. Generate Estimator metrics to expose current Change Feed Processor progress as a push notification
	//
	// 5. Generate Estimator metrics to expose current Change Feed Processor progress on demand
	//
	// 6. Code migration template from existing Change Feed Processor library V2
	//
	// 7. Error handling and advanced logging
	//-----------------------------------------------------------------------------------------------------------


	class EventConsumer
	{
		private  readonly string monitoredContainer = "skill-events";
		private  readonly string skillNamesContainer = "skill-names-in-use";
		private  readonly string leasesContainer = "skills-leases";
		private  readonly string partitionKeyPath = "/aggregateId";
		private  CosmosClient client;
		private IConfiguration configuration;
		async Task Main(IConfiguration configuration)
		{
			this.configuration = configuration;
			try
			{
				string endpoint = configuration["CosmosDb:Endpoint"];
				if (string.IsNullOrEmpty(endpoint))
				{
					throw new ArgumentNullException("Please specify a valid endpoint in the appSettings.json");
				}

				string authKey = configuration["CosmosDb:Key"];
				if (string.IsNullOrEmpty(authKey))
				{
					throw new ArgumentException("Please specify a valid AuthorizationKey in the appSettings.json");
				}

				using (client = new CosmosClient(endpoint, authKey))
				{
					Console.WriteLine($"\n3. Listening for changes that happen since the container was created.");
					await RunStartFromBeginningChangeFeed(configuration["CosmosDb:DbName"], client);
				}
			}
			finally
			{
				Console.WriteLine("End of demo, press any key to exit.");
				Console.ReadKey();
			}
		}
		/// <summary>
		/// Reading the Change Feed since the beginning of time.
		/// </summary>
		/// <remarks>
		/// StartTime only works if the leaseContainer is empty or contains no leases for the particular processor name.
		/// </remarks>
		public async Task RunStartFromBeginningChangeFeed(
			string databaseId,
			CosmosClient client)
		{
			await InitializeAsync(databaseId, client);

			// <StartFromBeginningInitialization>
			Container leaseContainer = client.GetContainer(databaseId, leasesContainer);
			Container monitored = client.GetContainer(databaseId, monitoredContainer);
			ChangeFeedProcessor changeFeedProcessor = monitored
				.GetChangeFeedProcessorBuilder<SkillBlueprintCreated>("changeFeedBeginning", HandleChangesAsync)
					.WithInstanceName("skill-created-event-reactor")
					.WithLeaseContainer(leaseContainer)
					.WithStartTime(DateTime.MinValue.ToUniversalTime())
					.Build();
			// </StartFromBeginningInitialization>

			Console.WriteLine($"Starting Change Feed Processor with changes since the beginning...");
			await changeFeedProcessor.StartAsync();
			Console.WriteLine("Change Feed Processor started.");

			//// Wait random time for the delegate to output all messages after initialization is done
			//await Task.Delay(5000);
			//Console.WriteLine("Press any key to continue with the next demo...");
			//Console.ReadKey();
			//await changeFeedProcessor.StopAsync();
		}

		/// <summary>
		/// The delegate receives batches of changes as they are generated in the change feed and can process them.
		/// </summary>
		// <Delegate>
		async Task HandleChangesAsync(ChangeFeedProcessorContext context, IReadOnlyCollection<SkillBlueprintCreated> changes, CancellationToken cancellationToken)
		{
			Console.WriteLine($"Started handling changes for lease {context.LeaseToken}...");
			Console.WriteLine($"Change Feed request consumed {context.Headers.RequestCharge} RU.");
			// SessionToken if needed to enforce Session consistency on another client instance
			Console.WriteLine($"SessionToken ${context.Headers.Session}");

			// We may want to track any operation's Diagnostics that took longer than some threshold
			if (context.Diagnostics.GetClientElapsedTime() > TimeSpan.FromSeconds(1))
			{
				Console.WriteLine($"Change Feed request took longer than expected. Diagnostics:" + context.Diagnostics.ToString());
			}

			foreach (SkillBlueprintCreated item in changes)
			{

				Console.WriteLine($"\tDetected operation for item with id {item.Name}, created at {item.OccurredOn}.");
				Container skillNames = client.GetContainer(configuration["CosmosDb:DbName"], monitoredContainer);


				// Simulate work
				await Task.Delay(1000);
			}
		}
		// </Delegate>
		/// <summary>
		/// Setup notification APIs for events.
		/// </summary>
		public  async Task RunWithNotifications(
			string databaseId,
			CosmosClient client)
		{
			await InitializeAsync(databaseId, client);

			Container leaseContainer = client.GetContainer(databaseId, leasesContainer);
			Container monitored = client.GetContainer(databaseId,	monitoredContainer);

			ManualResetEventSlim manualResetEventSlim = new ManualResetEventSlim();
			Container.ChangeFeedHandler<SkillBlueprintCreated> handleChanges = (ChangeFeedProcessorContext context, IReadOnlyCollection<SkillBlueprintCreated> changes, CancellationToken cancellationToken) =>
			{
				Console.WriteLine($"Started handling changes for lease {context.LeaseToken} but throwing an exception to bubble to notifications.");
				manualResetEventSlim.Set();
				throw new Exception("This is an unhandled exception from inside the delegate");
			};

			// <StartWithNotifications>
			Container.ChangeFeedMonitorLeaseAcquireDelegate onLeaseAcquiredAsync = (string leaseToken) =>
			{
				Console.WriteLine($"Lease {leaseToken} is acquired and will start processing");
				return Task.CompletedTask;
			};

			Container.ChangeFeedMonitorLeaseReleaseDelegate onLeaseReleaseAsync = (string leaseToken) =>
			{
				Console.WriteLine($"Lease {leaseToken} is released and processing is stopped");
				return Task.CompletedTask;
			};

			Container.ChangeFeedMonitorErrorDelegate onErrorAsync = (string LeaseToken, Exception exception) =>
			{
				if (exception is ChangeFeedProcessorUserException userException)
				{
					Console.WriteLine($"Lease {LeaseToken} processing failed with unhandled exception from user delegate {userException.InnerException}");
				}
				else
				{
					Console.WriteLine($"Lease {LeaseToken} failed with {exception}");
				}

				return Task.CompletedTask;
			};

			ChangeFeedProcessor changeFeedProcessor = monitored
				.GetChangeFeedProcessorBuilder<SkillBlueprintCreated>("changeFeedNotifications", handleChanges)
					.WithLeaseAcquireNotification(onLeaseAcquiredAsync)
					.WithLeaseReleaseNotification(onLeaseReleaseAsync)
					.WithErrorNotification(onErrorAsync)
					.WithInstanceName("consoleHost")
					.WithLeaseContainer(leaseContainer)
					.Build();
			// </StartWithNotifications>

			Console.WriteLine($"Starting Change Feed Processor with logging enabled...");
			await changeFeedProcessor.StartAsync();
			Console.WriteLine("Change Feed Processor started.");
			Console.WriteLine("Generating 10 items that will be picked up by the delegate...");
			//await EventConsumer.GenerateItems(10, client.GetContainer(databaseId, EventConsumer.monitored));
			// Wait random time for the delegate to output all messages after initialization is done
			manualResetEventSlim.Wait();
			await Task.Delay(1000);
			await changeFeedProcessor.StopAsync();
		}


		private async Task InitializeAsync(
			string databaseId,
			CosmosClient client)
		{
			Database database;
			// Recreate database
			try
			{
				database = await client.GetDatabase(databaseId).ReadAsync();
				await database.DeleteAsync();
			}
			catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
			{
			}

			database = await client.CreateDatabaseAsync(databaseId);

			await database.CreateContainerIfNotExistsAsync(new ContainerProperties(monitoredContainer, partitionKeyPath));

			await database.CreateContainerIfNotExistsAsync(new ContainerProperties(leasesContainer, partitionKeyPath));
		}
	}

	/// <summary>
	/// Basic change feed functionality.
	/// </summary>
	/// <remarks>
	/// When StartAsync is called, the Change Feed Processor starts an initialization process that can take several milliseconds, 
	/// in which it starts connections and initializes locks in the leases container.
	/// </remarks>
	//public static async Task RunBasicChangeFeed(
	//		string databaseId,
	//		CosmosClient client)
	//	{
	//		await EventConsumer.InitializeAsync(databaseId, client);

	//		// <BasicInitialization>
	//		Container leaseContainer = client.GetContainer(databaseId, EventConsumer.leasesContainer);
	//		Container monitored = client.GetContainer(databaseId, EventConsumer.monitored);
	//		ChangeFeedProcessor changeFeedProcessor = monitored
	//			.GetChangeFeedProcessorBuilder<SkillBlueprintCreated>("changeFeedBasic", EventConsumer.HandleChangesAsync)
	//				.WithInstanceName("consoleHost")
	//				.WithLeaseContainer(leaseContainer)
	//				.Build();
	//		// </BasicInitialization>

	//		Console.WriteLine("Starting Change Feed Processor...");
	//		await changeFeedProcessor.StartAsync();
	//		Console.WriteLine("Change Feed Processor started.");

	//		//Console.WriteLine("Generating 10 items that will be picked up by the delegate...");
	//		//await EventConsumer.GenerateItems(10, monitored);

	//		//// Wait random time for the delegate to output all messages after initialization is done
	//		//await Task.Delay(5000);
	//		//Console.WriteLine("Press any key to continue with the next demo...");
	//		//Console.ReadKey();
	//		//await changeFeedProcessor.StopAsync();
	//	}

	/// <summary>
	/// StartTime will make the Change Feed Processor start processing changes at a particular point in time, all previous changes are ignored.
	/// </summary>
	/// <remarks>
	/// StartTime only works if the leaseContainer is empty or contains no leases for the particular processor name.
	/// </remarks>
	//public static async Task RunStartTimeChangeFeed(
	//	string databaseId,
	//	CosmosClient client)
	//{
	//	await EventConsumer.InitializeAsync(databaseId, client);
	//	Console.WriteLine("Generating 5 items that will not be picked up.");
	//	await EventConsumer.GenerateItems(5, client.GetContainer(databaseId, EventConsumer.monitored));
	//	Console.WriteLine($"Items generated at {DateTime.UtcNow}");
	//	// Generate a future point in time
	//	await Task.Delay(2000);
	//	DateTime particularPointInTime = DateTime.UtcNow;

	//	Console.WriteLine("Generating 5 items that will be picked up by the delegate...");
	//	await EventConsumer.GenerateItems(5, client.GetContainer(databaseId, EventConsumer.monitored));

	//	// <TimeInitialization>
	//	Container leaseContainer = client.GetContainer(databaseId, EventConsumer.leasesContainer);
	//	Container monitored = client.GetContainer(databaseId, EventConsumer.monitored);
	//	ChangeFeedProcessor changeFeedProcessor = monitored
	//		.GetChangeFeedProcessorBuilder<SkillBlueprintCreated>("changeFeedTime", EventConsumer.HandleChangesAsync)
	//			.WithInstanceName("consoleHost")
	//			.WithLeaseContainer(leaseContainer)
	//			.WithStartTime(particularPointInTime)
	//			.Build();
	//	// </TimeInitialization>

	//	Console.WriteLine($"Starting Change Feed Processor with changes after {particularPointInTime}...");
	//	await changeFeedProcessor.StartAsync();
	//	Console.WriteLine("Change Feed Processor started.");

	//	// Wait random time for the delegate to output all messages after initialization is done
	//	await Task.Delay(5000);
	//	Console.WriteLine("Press any key to continue with the next demo...");
	//	Console.ReadKey();
	//	await changeFeedProcessor.StopAsync();
	//}



	///// <summary>
	///// Exposing progress with the Estimator.
	///// </summary>
	///// <remarks>
	///// The Estimator uses the same processorName and the same lease configuration as the existing processor to measure progress.
	///// </remarks>
	//public static async Task RunEstimatorChangeFeed(
	//	string databaseId,
	//	CosmosClient client)
	//{
	//	await EventConsumer.InitializeAsync(databaseId, client);

	//	// <StartProcessorEstimator>
	//	Container leaseContainer = client.GetContainer(databaseId, EventConsumer.leasesContainer);
	//	Container monitored = client.GetContainer(databaseId, EventConsumer.monitored);
	//	ChangeFeedProcessor changeFeedProcessor = monitored
	//		.GetChangeFeedProcessorBuilder<SkillBlueprintCreated>("changeFeedEstimator", EventConsumer.HandleChangesAsync)
	//			.WithInstanceName("consoleHost")
	//			.WithLeaseContainer(leaseContainer)
	//			.Build();
	//	// </StartProcessorEstimator>

	//	Console.WriteLine($"Starting Change Feed Processor...");
	//	await changeFeedProcessor.StartAsync();
	//	Console.WriteLine("Change Feed Processor started.");

	//	Console.WriteLine("Generating 10 items that will be picked up by the delegate...");
	//	await EventConsumer.GenerateItems(10, client.GetContainer(databaseId, EventConsumer.monitored));

	//	// Wait random time for the delegate to output all messages after initialization is done
	//	await Task.Delay(5000);

	//	// <StartEstimator>
	//	ChangeFeedProcessor changeFeedEstimator = monitored
	//		.GetChangeFeedEstimatorBuilder("changeFeedEstimator", EventConsumer.HandleEstimationAsync, TimeSpan.FromMilliseconds(1000))
	//		.WithLeaseContainer(leaseContainer)
	//		.Build();
	//	// </StartEstimator>

	//	Console.WriteLine($"Starting Change Feed Estimator...");
	//	await changeFeedEstimator.StartAsync();
	//	Console.WriteLine("Change Feed Estimator started.");

	//	Console.WriteLine("Generating 10 items that will be picked up by the delegate and reported by the Estimator...");
	//	await EventConsumer.GenerateItems(10, client.GetContainer(databaseId, EventConsumer.monitored));

	//	Console.WriteLine("Press any key to continue with the next demo...");
	//	Console.ReadKey();
	//	await changeFeedProcessor.StopAsync();
	//	await changeFeedEstimator.StopAsync();
	//}

	///// <summary>
	///// Exposing progress with the Estimator with the detailed iterator.
	///// </summary>
	///// <remarks>
	///// The Estimator uses the same processorName and the same lease configuration as the existing processor to measure progress.
	///// The iterator exposes detailed, per-lease, information on estimation and ownership.
	///// </remarks>
	//public static async Task RunEstimatorPullChangeFeed(
	//	string databaseId,
	//	CosmosClient client)
	//{
	//	await EventConsumer.InitializeAsync(databaseId, client);

	//	// <StartProcessorEstimatorDetailed>
	//	Container leaseContainer = client.GetContainer(databaseId, EventConsumer.leasesContainer);
	//	Container monitored = client.GetContainer(databaseId, EventConsumer.monitored);
	//	ChangeFeedProcessor changeFeedProcessor = monitored
	//		.GetChangeFeedProcessorBuilder<SkillBlueprintCreated>("changeFeedEstimator", EventConsumer.HandleChangesAsync)
	//			.WithInstanceName("consoleHost")
	//			.WithLeaseContainer(leaseContainer)
	//			.Build();
	//	// </StartProcessorEstimatorDetailed>

	//	Console.WriteLine($"Starting Change Feed Processor...");
	//	await changeFeedProcessor.StartAsync();
	//	Console.WriteLine("Change Feed Processor started.");

	//	// Wait some seconds for instances to acquire leases
	//	await Task.Delay(5000);

	//	Console.WriteLine("Generating 10 items that will be picked up by the delegate...");
	//	await EventConsumer.GenerateItems(10, client.GetContainer(databaseId, EventConsumer.monitored));

	//	// Wait random time for the delegate to output all messages after initialization is done
	//	await Task.Delay(5000);

	//	// <StartEstimatorDetailed>
	//	ChangeFeedEstimator changeFeedEstimator = monitored
	//		.GetChangeFeedEstimator("changeFeedEstimator", leaseContainer);
	//	// </StartEstimatorDetailed>

	//	// <GetIteratorEstimatorDetailed>
	//	Console.WriteLine("Checking estimation...");
	//	using FeedIterator<ChangeFeedProcessorState> estimatorIterator = changeFeedEstimator.GetCurrentStateIterator();
	//	while (estimatorIterator.HasMoreResults)
	//	{
	//		FeedResponse<ChangeFeedProcessorState> states = await estimatorIterator.ReadNextAsync();
	//		foreach (ChangeFeedProcessorState leaseState in states)
	//		{
	//			string host = leaseState.InstanceName == null ? $"not owned by any host currently" : $"owned by host {leaseState.InstanceName}";
	//			Console.WriteLine($"Lease [{leaseState.LeaseToken}] {host} reports {leaseState.EstimatedLag} as estimated lag.");
	//		}
	//	}
	//	// </GetIteratorEstimatorDetailed>

	//	Console.WriteLine("Stopping processor to show how the lag increases if no processing is happening.");
	//	await changeFeedProcessor.StopAsync();

	//	// Wait for processor to shutdown completely so the next items generate lag
	//	await Task.Delay(5000);

	//	Console.WriteLine("Generating 10 items that will be seen by the Estimator...");
	//	await EventConsumer.GenerateItems(10, client.GetContainer(databaseId, EventConsumer.monitored));

	//	Console.WriteLine("Checking estimation...");
	//	using FeedIterator<ChangeFeedProcessorState> estimatorIteratorAfter = changeFeedEstimator.GetCurrentStateIterator();
	//	while (estimatorIteratorAfter.HasMoreResults)
	//	{
	//		FeedResponse<ChangeFeedProcessorState> states = await estimatorIteratorAfter.ReadNextAsync();
	//		foreach (ChangeFeedProcessorState leaseState in states)
	//		{
	//			// Host ownership should be empty as we have already stopped the estimator
	//			string host = leaseState.InstanceName == null ? $"not owned by any host currently" : $"owned by host {leaseState.InstanceName}";
	//			Console.WriteLine($"Lease [{leaseState.LeaseToken}] {host} reports {leaseState.EstimatedLag} as estimated lag.");
	//		}
	//	}



	//	Console.WriteLine("Press any key to continue with the next demo...");
	//	Console.ReadKey();
	//}

	///// <summary>
	///// Example of a code migration template from Change Feed Processor V2 to SDK V3.
	///// </summary>
	///// <returns></returns>
	//public static async Task RunMigrationSample(
	//	string databaseId,
	//	CosmosClient client,
	//	IConfiguration configuration)
	//{
	//	await EventConsumer.InitializeAsync(databaseId, client);

	//	Console.WriteLine("Generating 10 items that will be picked up by the old Change Feed Processor library...");
	//	await EventConsumer.GenerateItems(10, client.GetContainer(databaseId, EventConsumer.monitored));

	//	// This is how you would initialize the processor in V2
	//	// <ChangeFeedProcessorLibrary>
	//	ChangeFeedProcessorLibrary.DocumentCollectionInfo monitoredCollectionInfo = new ChangeFeedProcessorLibrary.DocumentCollectionInfo()
	//	{
	//		DatabaseName = databaseId,
	//		CollectionName = EventConsumer.monitored,
	//		Uri = new Uri(configuration["EndPointUrl"]),
	//		MasterKey = configuration["AuthorizationKey"]
	//	};

	//	ChangeFeedProcessorLibrary.DocumentCollectionInfo leaseCollectionInfo = new ChangeFeedProcessorLibrary.DocumentCollectionInfo()
	//	{
	//		DatabaseName = databaseId,
	//		CollectionName = EventConsumer.leasesContainer,
	//		Uri = new Uri(configuration["EndPointUrl"]),
	//		MasterKey = configuration["AuthorizationKey"]
	//	};

	//	ChangeFeedProcessorLibrary.ChangeFeedProcessorBuilder builder = new ChangeFeedProcessorLibrary.ChangeFeedProcessorBuilder();
	//	var oldChangeFeedProcessor = await builder
	//		.WithHostName("consoleHost")
	//		.WithProcessorOptions(new ChangeFeedProcessorLibrary.ChangeFeedProcessorOptions
	//		{
	//			StartFromBeginning = true,
	//			LeasePrefix = "MyLeasePrefix",
	//			MaxItemCount = 10,
	//			FeedPollDelay = TimeSpan.FromSeconds(1)
	//		})
	//		.WithFeedCollection(monitoredCollectionInfo)
	//		.WithLeaseCollection(leaseCollectionInfo)
	//		.WithObserver<ChangeFeedObserver>()
	//		.BuildAsync();
	//	// </ChangeFeedProcessorLibrary>

	//	await oldChangeFeedProcessor.StartAsync();

	//	// Wait random time for the delegate to output all messages after initialization is done
	//	await Task.Delay(5000);
	//	Console.WriteLine("Now we will stop the V2 Processor and start a V3 with the same parameters to pick up from the same state, press any key to continue...");
	//	Console.ReadKey();
	//	await oldChangeFeedProcessor.StopAsync();

	//	Console.WriteLine("Generating 5 items that will be picked up by the new Change Feed Processor...");
	//	await EventConsumer.GenerateItems(5, client.GetContainer(databaseId, EventConsumer.monitored));

	//	// This is how you would do the same initialization in V3
	//	// <ChangeFeedProcessorMigrated>
	//	Container leaseContainer = client.GetContainer(databaseId, EventConsumer.leasesContainer);
	//	Container monitored = client.GetContainer(databaseId, EventConsumer.monitored);
	//	ChangeFeedProcessor changeFeedProcessor = monitored
	//		.GetChangeFeedProcessorBuilder<SkillBlueprintCreated>("MyLeasePrefix", EventConsumer.HandleChangesAsync)
	//			.WithInstanceName("consoleHost")
	//			.WithLeaseContainer(leaseContainer)
	//			.WithMaxItems(10)
	//			.WithPollInterval(TimeSpan.FromSeconds(1))
	//			.WithStartTime(DateTime.MinValue.ToUniversalTime())
	//			.Build();
	//	// </ChangeFeedProcessorMigrated>

	//	await changeFeedProcessor.StartAsync();

	//	// Wait random time for the delegate to output all messages after initialization is done
	//	await Task.Delay(5000);
	//	Console.WriteLine("Press any key to continue with the next demo...");
	//	Console.ReadKey();
	//	await changeFeedProcessor.StopAsync();
	//}





	//internal class ChangeFeedObserver : IChangeFeedObserver
	//{
	//	public Task CloseAsync(IChangeFeedObserverContext context, ChangeFeedObserverCloseReason reason)
	//	{
	//		return Task.CompletedTask;
	//	}

	//	public Task OpenAsync(IChangeFeedObserverContext context)
	//	{
	//		return Task.CompletedTask;
	//	}

	//	public Task ProcessChangesAsync(IChangeFeedObserverContext context, IReadOnlyList<Microsoft.Azure.Documents.Document> docs, CancellationToken cancellationToken)
	//	{
	//		foreach (Microsoft.Azure.Documents.Document doc in docs)
	//		{
	//			Console.WriteLine($"\t[OLD Processor] Detected operation for item with id {doc.Id}, created at {doc.GetPropertyValue<DateTime>("creationTime")}.");
	//		}

	//		return Task.CompletedTask;
	//	}
	//}
}
