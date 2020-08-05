using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;

namespace Infrastructure.Layer.Helpers
{
    public static class JsonHelper
    {
        public static string Serialize<T>(T obj, bool indented = false)
        {
            var formatting = Formatting.None;

            if (indented)
            {
                formatting = Formatting.Indented;
            }

            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var jsonSerializerSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, ContractResolver = contractResolver };

            return JsonConvert.SerializeObject(obj, formatting, jsonSerializerSettings);
        }

        public static T Deserialize<T>(string json, bool ignoreReferenceLoopHandling = true)
        {
            if (ignoreReferenceLoopHandling)
            {
                return JsonConvert.DeserializeObject<T>(json,
                    new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            }

            return JsonConvert.DeserializeObject<T>(json);
        }

        public static void SerializeToFile<T>(string filePath, T obj, bool ignoreNullValue = false, bool indented = false)
        {
            using (var streamWriter = new StreamWriter(filePath))
            using (var writer = new JsonTextWriter(streamWriter))
            {
                var serializer = new JsonSerializer
                {

                    NullValueHandling = ignoreNullValue ? NullValueHandling.Ignore : NullValueHandling.Include
                };

                writer.Formatting = indented ? Formatting.Indented : Formatting.None;

                serializer.Serialize(writer, obj);

                writer.Close();
                streamWriter.Close();
            }
        }

        public static T DeserializeFromFile<T>(string filePath)
        {
            var objectToDeserialize = File.ReadAllText(filePath);

            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            return (T)JsonConvert.DeserializeObject(objectToDeserialize, typeof(T), jsonSettings);
        }
    }
}
