using NebzzClient.DTOs;
using NebzzClient.DTOs.Responses;

namespace NebzzClient.Handlers
{
    public class MotdHandler : Handler<MotdResponseDTO>
    {
        public MotdHandler() : base(MessageType.Motd)
        {

        }

        internal override void Handle(MotdResponseDTO body)
        {
            //MessageBox.Show(body.Motd);
        }
    }
}
