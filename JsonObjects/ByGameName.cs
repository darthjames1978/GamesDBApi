namespace GamesDBApi
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ByGameName
    {
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public long? Code { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public Data Data { get; set; }

        [JsonProperty("include", NullValueHandling = NullValueHandling.Ignore)]
        public Include Include { get; set; }

        [JsonProperty("pages", NullValueHandling = NullValueHandling.Ignore)]
        public Pages Pages { get; set; }

        [JsonProperty("remaining_monthly_allowance", NullValueHandling = NullValueHandling.Ignore)]
        public long? RemainingMonthlyAllowance { get; set; }

        [JsonProperty("extra_allowance", NullValueHandling = NullValueHandling.Ignore)]
        public long? ExtraAllowance { get; set; }

        [JsonProperty("allowance_refresh_timer", NullValueHandling = NullValueHandling.Ignore)]
        public long? AllowanceRefreshTimer { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public long? Count { get; set; }

        [JsonProperty("games", NullValueHandling = NullValueHandling.Ignore)]
        public Game[] Games { get; set; }
    }

    public partial class Game
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("game_title", NullValueHandling = NullValueHandling.Ignore)]
        public string GameTitle { get; set; }

        [JsonProperty("release_date")]
        public DateTimeOffset? ReleaseDate { get; set; }

        [JsonProperty("platform", NullValueHandling = NullValueHandling.Ignore)]
        public long? Platform { get; set; }

        [JsonProperty("players", NullValueHandling = NullValueHandling.Ignore)]
        public long? Players { get; set; }

        [JsonProperty("overview", NullValueHandling = NullValueHandling.Ignore)]
        public string Overview { get; set; }

        [JsonProperty("developers")]
        public long[] Developers { get; set; }

        [JsonProperty("genres")]
        public long[] Genres { get; set; }

        [JsonProperty("publishers")]
        public long[] Publishers { get; set; }

        [JsonProperty("alternates")]
        public string[] Alternates { get; set; }
    }

    public partial class Include
    {
        [JsonProperty("boxart", NullValueHandling = NullValueHandling.Ignore)]
        public Boxart Boxart { get; set; }

        [JsonProperty("platform", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, Platform> Platform { get; set; }
    }

    public partial class Boxart
    {
        [JsonProperty("base_url", NullValueHandling = NullValueHandling.Ignore)]
        public BaseUrl BaseUrl { get; set; }

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, Datum[]> Data { get; set; }
    }

    public partial class BaseUrl
    {
        [JsonProperty("original", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Original { get; set; }

        [JsonProperty("small", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Small { get; set; }

        [JsonProperty("thumb", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Thumb { get; set; }

        [JsonProperty("cropped_center_thumb", NullValueHandling = NullValueHandling.Ignore)]
        public Uri CroppedCenterThumb { get; set; }

        [JsonProperty("medium", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Medium { get; set; }

        [JsonProperty("large", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Large { get; set; }
    }

    public partial class Datum
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public TypeEnum? Type { get; set; }

        [JsonProperty("side", NullValueHandling = NullValueHandling.Ignore)]
        public Side? Side { get; set; }

        [JsonProperty("filename", NullValueHandling = NullValueHandling.Ignore)]
        public string Filename { get; set; }

        [JsonProperty("resolution")]
        public string Resolution { get; set; }
    }

    public partial class Platform
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("alias", NullValueHandling = NullValueHandling.Ignore)]
        public string Alias { get; set; }
    }

    public partial class Pages
    {
        [JsonProperty("previous")]
        public object Previous { get; set; }

        [JsonProperty("current", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Current { get; set; }

        [JsonProperty("next", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Next { get; set; }
    }

    public enum Side { Back, Front };

    public enum TypeEnum { Boxart };

    public partial class ByGameName
    {
        public static ByGameName FromJson(string json) => JsonConvert.DeserializeObject<ByGameName>(json, XSlideShow.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this ByGameName self) => JsonConvert.SerializeObject(self, XSlideShow.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                SideConverter.Singleton,
                TypeEnumConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class SideConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Side) || t == typeof(Side?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "back":
                    return Side.Back;
                case "front":
                    return Side.Front;
            }
            throw new Exception("Cannot unmarshal type Side");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Side)untypedValue;
            switch (value)
            {
                case Side.Back:
                    serializer.Serialize(writer, "back");
                    return;
                case Side.Front:
                    serializer.Serialize(writer, "front");
                    return;
            }
            throw new Exception("Cannot marshal type Side");
        }

        public static readonly SideConverter Singleton = new SideConverter();
    }

    internal class TypeEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TypeEnum) || t == typeof(TypeEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "boxart")
            {
                return TypeEnum.Boxart;
            }
            throw new Exception("Cannot unmarshal type TypeEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (TypeEnum)untypedValue;
            if (value == TypeEnum.Boxart)
            {
                serializer.Serialize(writer, "boxart");
                return;
            }
            throw new Exception("Cannot marshal type TypeEnum");
        }

        public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
    }
}
