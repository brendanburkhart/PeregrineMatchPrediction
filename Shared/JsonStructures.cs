using Newtonsoft.Json;

namespace Shared
{
    public class TeamStats
    {
        [JsonProperty("team")]
        public string TeamNumber { get; set; }

        [JsonProperty("summary")]
        public Stat[] Stats { get; set; }
    }

    public class Stat
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("max")]
        public double Max { get; set; }

        [JsonProperty("avg")]
        public double Average { get; set; }
    }

    public class Match
    {
        [JsonProperty("redScore")]
        public int RedScore { get; set; }

        [JsonProperty("blueScore")]
        public int BlueScore { get; set; }

        [JsonProperty("redAlliance")]
        public string[] RedAlliance { get; set; }

        [JsonProperty("blueAlliance")]
        public string[] BlueAlliance { get; set; }
    }

    public class Schema
    {
        [JsonProperty("schema")]
        public StatDescription[] stats { get; set; }
    }

    public class StatDescription
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
