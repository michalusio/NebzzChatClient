using Newtonsoft.Json;

namespace NebzzClient.DTOs.Responses
{
    public class InfoResponseDTO
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
