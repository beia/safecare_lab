using Newtonsoft.Json;

namespace Safecare.BeiaDeviceDriver
{
    public class BarometerData
    {
        [JsonProperty("Light_level")]
        public double LightLevel { get; set; }

        [JsonProperty("Temperature_in_degrees")]
        public double TemperatureInDegrees { get; set; }

        [JsonProperty("Humidity_level")]
        public double HumidityLevel { get; set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this,
                new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    Formatting = Formatting.Indented,
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });
        }

        public static BarometerData Deserialize(string text)
        {
            return JsonConvert.DeserializeObject<BarometerData>(text);
        }
    }
}
