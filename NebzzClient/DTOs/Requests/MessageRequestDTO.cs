using Newtonsoft.Json;

namespace NebzzClient.DTOs.Requests
{
    public class MessageRequestDTO
    {
        [JsonProperty("session_uuid")]
        public byte[] SessionUUID { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("time_sent")]
        public long TimeSent { get; set; }
    }
}
