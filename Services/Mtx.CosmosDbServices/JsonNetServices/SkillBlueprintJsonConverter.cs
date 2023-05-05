
using Mtx.LearnItAll.Core.Blueprints;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Mtx.CosmosDbServices.JsonNetServices
{
    public class SkillBlueprintJsonConverter : JsonConverter
    {
        public override bool CanRead => true;
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(SkillBlueprint);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {

            var obj = JObject.Load(reader);
            return obj.ToObject<SkillBlueprint>(serializer);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotSupportedException("This converter handles only deserialization, not serialization.");
        }
    }
}
