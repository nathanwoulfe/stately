using Newtonsoft.Json;

namespace Stately.Models
{
    public class StatelySettings
    {
        [JsonProperty("propertyAlias")]
        public string PropertyAlias { get; set; }

        [JsonProperty("value")]
        public bool Value { get; set; }

        [JsonProperty("cssClass")]
        public string CssClass { get; set; }

        [JsonProperty("cssColor")]
        public string CssColor { get; set; }

        [JsonProperty("disabled")]
        public bool Disabled { get; set; }

        [JsonProperty("replace")]
        public bool Replace { get; set; }

        [JsonProperty("recolor")]
        public bool Recolor { get; set; }
    }
}
