using NebzzClient.DTOs;
using NebzzClient.DTOs.Requests;
using NebzzClient.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace NebzzClient.Messages
{
    public class MessageRepo
    {
        public static readonly MessageRepo Instance = new MessageRepo();

        public event EventHandler MessageEvent;
        public event EventHandler UserEvent;

        private readonly SortedList<long, Message> messages = new SortedList<long, Message>(new DuplicateKeyComparer<long>());
        private readonly HashSet<string> users = new HashSet<string>();
        private static long HighestID = 0;

        private MessageRepo()
        {
            _ = Task.Run(ThumbnailEngine.Instance.ProcessingLoopAsync);
        }

        public void ReceivedMessage(BroadcastMessageResponseDTO messageData)
        {
            HighestID++;
            var message = new Message(HighestID, messageData.Username, messageData.TimeSentServer, messageData.Message);
            messages.Add(messageData.TimeSentServer, message);
            ThumbnailEngine.Instance.EnqueueMessage(message);
            MessageEvent?.Invoke(this, new EventArgs());
        }

        public void SendMessage(string message)
        {
            Connection.Instance.PushMessage(MessageType.Message, new MessageRequestDTO
            {
                SessionUUID = Connection.Instance.SessionUUID,
                Message = message,
                TimeSent = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            });
        }

        public void UserStatus(string username, bool online)
        {
            if (online)
            {
                users.Add(username);
            }
            else
            {
                users.Remove(username);
            }
            UserEvent?.Invoke(this, new EventArgs());
        }

        public IEnumerable<Message> Messages => messages.Select(msg => msg.Value);
        public int MessageCount => messages.Count;
        public HashSet<string> Users => users.ToHashSet();

        internal void UpdateImage(long ID, Thumbnail thumbnail)
        {
            if (thumbnail == default) return;
            var values = messages.Values;
            for (int i = 0; i < messages.Count; i++)
            {
                var message = values[i];
                if (message.ID == ID)
                {
                    messages.RemoveAt(i);
                    messages.Add(message.UtcTime.ToUnixTimeMilliseconds(), new Message(message, thumbnail));
                    MessageEvent?.Invoke(this, new EventArgs());
                    break;
                }
            }
        }
    }

    internal class DuplicateKeyComparer<TKey>: IComparer<TKey> where TKey : IComparable
    {
        public int Compare(TKey x, TKey y)
        {
            int result = x.CompareTo(y);

            if (result == 0)
                return 1;   // Handle equality as being greater
            else
                return result;
        }
    }
}
