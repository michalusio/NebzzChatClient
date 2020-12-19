using NebzzClient.DTOs;
using NebzzClient.DTOs.Responses;
using System.Windows;

namespace NebzzClient.Handlers
{
    public class ErrorHandler : Handler<InfoResponseDTO>
    {
        public ErrorHandler(): base(MessageType.Error)
        {

        }

        internal override void Handle(InfoResponseDTO body)
        {
            MessageBox.Show(body.Message);
        }
    }
}
