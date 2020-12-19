using Newtonsoft.Json;

namespace NebzzClient.DTOs.Responses
{
    public class LoginResponseDTO
    {
        [JsonProperty("session_uuid")]
        public byte[] SessionUUID { get; set; }

        [JsonProperty("logged_in_users")]
        public string[] LoggedInUsers { get; set; }
    }
}
