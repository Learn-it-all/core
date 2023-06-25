using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace SkillEventsTracker
{
    public static class SkillEventConsumer
    {
        [FunctionName("SkillEventConsumer")]
        public static void Run([CosmosDBTrigger(
            databaseName: "LearnItAllCoreDev",
            collectionName: "skill-events",
            ConnectionStringSetting = "CosmosDb:Cs",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input,
            ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                foreach(var inputItem in input)
                {
                    in
                }
                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("First document Id " + input[0].Id);
            }
        }
    }
}
