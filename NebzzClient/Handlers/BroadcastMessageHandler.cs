using NebzzClient.Audio;
using NebzzClient.DTOs;
using NebzzClient.DTOs.Responses;
using NebzzClient.Messages;
using System;

namespace NebzzClient.Handlers
{
    public class BroadcastMessageHandler : Handler<BroadcastMessageResponseDTO>
    {
        private readonly CachedSound notificationSound;
        private readonly IntPtr windowHandle;

        public BroadcastMessageHandler(IntPtr windowHandle): base(MessageType.BroadcastMessage)
        {
            notificationSound = new CachedSound("notif.mp3");
            AudioPlaybackEngine.Instance.Volume = 0.5f;
            this.windowHandle = windowHandle;
        }

        internal override void Handle(BroadcastMessageResponseDTO body)
        {
            if (body.Username != Connection.Instance.Username)
            {
                AudioPlaybackEngine.Instance.PlaySound(notificationSound);
                if (windowHandle != null)
                {
                    FlashWindow.Flash(windowHandle);
                }
            }
            MessageRepo.Instance.ReceivedMessage(body);
        }
    }
}
