using NebzzClient.DTOs;
using NebzzClient.DTOs.Responses;
using NebzzClient.Messages;

namespace NebzzClient.Handlers
{
    public class LoginHandler: Handler<LoginResponseDTO>
    {
        public LoginHandler(): base(MessageType.Login)
        {

        }

        internal override void Handle(LoginResponseDTO body)
        {
            Connection.Instance.SessionUUID = body.SessionUUID;
            foreach (var user in body.LoggedInUsers)
            {
                MessageRepo.Instance.UserStatus(user, true);
            }
        }
    }
}
