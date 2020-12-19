using Newtonsoft.Json;
using System;

namespace NebzzClient.DTOs.Responses
{
    public class BroadcastLoginResponseDTO
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("logged_in")]
        public bool LoggedIn { get; set; }
    }
}
