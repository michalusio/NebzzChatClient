using NebzzClient.DTOs;
using NebzzClient.DTOs.Responses;
using System.Windows;

namespace NebzzClient.Handlers
{
    public class InfoHandler : Handler<InfoResponseDTO>
    {
        public InfoHandler(): base(MessageType.Info)
        {

        }

        internal override void Handle(InfoResponseDTO body)
        {
            MessageBox.Show(body.Message);
        }
    }
}
