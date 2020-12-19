using NebzzClient.DTOs;
using NebzzClient.DTOs.Responses;
using System.Windows;

namespace NebzzClient.Handlers
{
    public class RegisterHandler : Handler<RegisterResponseDTO>
    {
        public RegisterHandler(): base(MessageType.Register)
        {

        }

        internal override void Handle(RegisterResponseDTO body)
        {
            MessageBox.Show(body.Message);
        }
    }
}
