using Newtonsoft.Json;

namespace Stately.Models
{
    public class Settings
    {
        [JsonProperty("propertyAlias")]
        public string PropertyAlias { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("cssClss")]
        public string CssClass { get; set; }

        [JsonProperty("cssColor")]
        public string CssColor { get; set; }

        [JsonProperty("disabled")]
        public bool Disabled { get; set; }
    }
}
