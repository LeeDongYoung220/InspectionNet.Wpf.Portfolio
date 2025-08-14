using Newtonsoft.Json;

namespace InspectionNet.Core.StaticClasses
{
    public static class JsonHelper
    {
        public static T LoadObject<T>(string jsonFilePath)
        {
            using var sr = new StreamReader(jsonFilePath);
            var jsonStr = sr.ReadToEnd();
            var jsonObj = JsonConvert.DeserializeObject<T>(jsonStr);
            return jsonObj ?? throw new InvalidOperationException("Deserialization returned null.");
        }

        public static void SaveObject<T>(string jsonFilePath, T obj)
        {
            var jsonStr = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText(jsonFilePath, jsonStr);
        }
    }
}
