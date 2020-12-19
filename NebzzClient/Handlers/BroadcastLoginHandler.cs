using NebzzClient.DTOs;
using NebzzClient.DTOs.Responses;
using NebzzClient.Messages;

namespace NebzzClient.Handlers
{
    public class BroadcastLoginHandler : Handler<BroadcastLoginResponseDTO>
    {
        public BroadcastLoginHandler(): base(MessageType.BroadcastLogin)
        {

        }

        internal override void Handle(BroadcastLoginResponseDTO body)
        {
            MessageRepo.Instance.UserStatus(body.Username, body.LoggedIn);
        }
    }
}
