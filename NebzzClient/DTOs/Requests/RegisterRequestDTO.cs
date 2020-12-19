using Newtonsoft.Json;

namespace NebzzClient.DTOs.Requests
{
    public class RegisterRequestDTO
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
