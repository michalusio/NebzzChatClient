using Newtonsoft.Json;

namespace NebzzClient.DTOs.Responses
{
    public class MotdResponseDTO
    {
        [JsonProperty("motd")]
        public string Motd { get; set; }
    }
}
