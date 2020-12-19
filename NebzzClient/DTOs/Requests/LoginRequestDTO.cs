using Newtonsoft.Json;

namespace NebzzClient.DTOs.Requests
{
    public class LoginRequestDTO
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
