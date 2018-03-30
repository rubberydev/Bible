namespace Bible.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class ContentResponse_
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("error_level")]
        public long ErrorLevel { get; set; }

        [JsonProperty("results")]
        public List<Content_> Contents { get; set; }
    }
}
