using Newtonsoft.Json;

namespace NebzzClient.DTOs.Responses
{
    public class WarnResponseDTO
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
