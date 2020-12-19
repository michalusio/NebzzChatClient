using Newtonsoft.Json;
using System;

namespace NebzzClient.DTOs.Responses
{
    public class BroadcastMessageResponseDTO
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("time_sent_client")]
        public long TimeSentClient { get; set; }

        [JsonProperty("time_sent_server")]
        public long TimeSentServer { get; set; }
    }
}
