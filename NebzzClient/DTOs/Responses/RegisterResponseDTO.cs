using Newtonsoft.Json;

namespace NebzzClient.DTOs.Responses
{
    public class RegisterResponseDTO
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
