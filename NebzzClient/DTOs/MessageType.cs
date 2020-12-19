using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace NebzzClient.DTOs
{
    [JsonConverter(typeof(StringEnumConverter), new object[] { false })]
    internal enum MessageType
    {
        [EnumMember(Value = "login")]
        Login,
        [EnumMember(Value = "register")]
        Register,
        [EnumMember(Value = "motd")]
        Motd,
        [EnumMember(Value = "warn")]
        Warn,
        [EnumMember(Value = "message")]
        Message,
        [EnumMember(Value = "broadcast")]
        Broadcast,
        [EnumMember(Value = "broadcastlogin")]
        BroadcastLogin,
        [EnumMember(Value = "broadcastmessage")]
        BroadcastMessage,
        [EnumMember(Value = "info")]
        Info,
        [EnumMember(Value = "error")]
        Error
    }
}
