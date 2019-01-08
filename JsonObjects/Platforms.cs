namespace GamesDBApi
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Collections.Generic;
    using System.Globalization;

    public partial class Platforms
    {
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public long? Code { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public Data Data { get; set; }

        [JsonProperty("remaining_monthly_allowance", NullValueHandling = NullValueHandling.Ignore)]
        public long? RemainingMonthlyAllowance { get; set; }

        [JsonProperty("extra_allowance", NullValueHandling = NullValueHandling.Ignore)]
        public long? ExtraAllowance { get; set; }

        [JsonProperty("allowance_refresh_timer")]
        public object AllowanceRefreshTimer { get; set; }
    }

    public partial class Data
    {
        /*
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public long? Count { get; set; }
        */

        [JsonProperty("platforms", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, Platform> Platforms { get; set; }
    }

    public partial class Platform
    {
        /*
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("alias")]
        public string Alias { get; set; }
        */

        [JsonProperty("icon", NullValueHandling = NullValueHandling.Ignore)]
        public string Icon { get; set; }

        [JsonProperty("console")]
        public string Console { get; set; }

        [JsonProperty("controller")]
        public string Controller { get; set; }

        [JsonProperty("developer")]
        public string Developer { get; set; }

        [JsonProperty("manufacturer")]
        public string Manufacturer { get; set; }

        [JsonProperty("media")]
        public string Media { get; set; }

        [JsonProperty("cpu")]
        public string Cpu { get; set; }

        [JsonProperty("memory")]
        public string Memory { get; set; }

        [JsonProperty("graphics")]
        public string Graphics { get; set; }

        [JsonProperty("sound")]
        public string Sound { get; set; }

        [JsonProperty("maxcontrollers")]
        public string Maxcontrollers { get; set; }

        [JsonProperty("display")]
        public string Display { get; set; }

        [JsonProperty("overview", NullValueHandling = NullValueHandling.Ignore)]
        public string Overview { get; set; }

        [JsonProperty("youtube")]
        public string Youtube { get; set; }
    }

    public partial class Platforms
    {
        public static Platforms FromJson(string json) => JsonConvert.DeserializeObject<Platforms>(json, PlatformsConverter.Settings);
    }

    public static class PlatformsSerialize
    {
        public static string ToJson(this Platforms self) => JsonConvert.SerializeObject(self, PlatformsConverter.Settings);
    }

    internal static class PlatformsConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
