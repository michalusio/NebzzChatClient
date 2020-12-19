using NebzzClient.DTOs;
using NebzzClient.DTOs.Responses;
using System.Windows;

namespace NebzzClient.Handlers
{
    public class WarnHandler : Handler<WarnResponseDTO>
    {
        public WarnHandler() : base(MessageType.Warn)
        {

        }

        internal override void Handle(WarnResponseDTO body)
        {
            MessageBox.Show(body.Message);
        }
    }
}
