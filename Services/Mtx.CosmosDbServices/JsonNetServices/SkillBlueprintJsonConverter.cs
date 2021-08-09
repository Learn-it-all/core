using Mtx.LearnItAll.Core.Blueprints;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //// First, just read the JSON as a JObject

            //// Then look at the $type property:
            //var typeName = obj["$type"]?.Value<string>();
            //switch (typeName)
            //{
            //    case "fileItem":
            //        // Deserialize as a FileItem
            //        return obj.ToObject<FileItem>(serializer);
            //    case "folderItem":
            //        // Deserialize as a FolderItem
            //        return obj.ToObject<FolderItem>(serializer);
            //    default:
            //        throw new InvalidOperationException($"Unknown type name '{typeName}'");
            //}
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotSupportedException("This converter handles only deserialization, not serialization.");
        }
    }
}
