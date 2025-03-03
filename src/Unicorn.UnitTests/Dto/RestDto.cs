using Newtonsoft.Json;

namespace Unicorn.UnitTests.Dto
{
    public class RestDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
