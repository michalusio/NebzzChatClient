using NebzzClient.DTOs;
using NebzzClient.DTOs.Requests;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace NebzzClient
{
    public class Connection
    {
        public static readonly Connection Instance = new Connection();
        internal byte[] SessionUUID { get; set; }
        internal string Username { get; private set; }

        private readonly ClientWebSocket socket;
        private readonly Dictionary<MessageType, Handler> handlers = new Dictionary<MessageType, Handler>();
        private readonly Queue<(MessageType, object)> messagesToSend = new Queue<(MessageType, object)>();

        private Connection()
        {
            socket = new ClientWebSocket();
        }

        public async Task ConnectAsync(Uri uri)
        {
            if (socket.State == WebSocketState.None)
            {
                MessageLoop();
                await socket.ConnectAsync(uri, CancellationToken.None);
            }
        }

        public async Task DisconnectAsync()
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Bye bye", CancellationToken.None);
            }
        }

        public async Task ConnectAndLoginAsync(Uri url, string username, string password, bool withRegister)
        {
            await ConnectAsync(url);
            if (withRegister)
            {
                PushMessage(MessageType.Register, new RegisterRequestDTO
                {
                    Username = username,
                    Password = password
                });
            }

            PushMessage(MessageType.Login, new LoginRequestDTO
            {
                Username = username,
                Password = password
            });

            while (SessionUUID == null)
            {
                await Task.Delay(50);
            }
        }

        private async void MessageLoop()
        {
            while (socket.State != WebSocketState.Open)
            {
                await Task.Delay(50);
            }
            Console.WriteLine("Connected?");
            _ = Task.Run(SendingLoop);
            _ = Task.Run(async () => {
                await ReceivingLoop();
                await CloseSocket();
            });
        }

        private async Task SendingLoop()
        {
            while (socket.State == WebSocketState.Open)
            {
                try
                {
                    while (messagesToSend.Count == 0 && socket.State == WebSocketState.Open)
                    {
                        await Task.Delay(50);
                    }
                    if (socket.State != WebSocketState.Open) break;
                    
                    (var type, var body) = messagesToSend.Dequeue();

                    if (type == MessageType.Login)
                    {
                        Username = (body as LoginRequestDTO).Username;
                    }

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (BsonDataWriter writer = new BsonDataWriter(ms))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            serializer.Serialize(writer, new { type, body });
                        }

                        await socket.SendAsync(
                            new ArraySegment<byte>(ms.ToArray()),
                            WebSocketMessageType.Binary,
                            true,
                            CancellationToken.None
                        );
                        Console.WriteLine($"Message sent: {type}");
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private async Task ReceivingLoop()
        {
            while (socket.State == WebSocketState.Open)
            {
                try
                {
                    var message = Enumerable.Empty<byte>();
                    var receiveMore = true;
                    while (receiveMore)
                    {
                        var data = new byte[1024];
                        var result = await socket.ReceiveAsync(new ArraySegment<byte>(data, 0, data.Length), CancellationToken.None);
                        receiveMore = !result.EndOfMessage;
                        message = message.Concat(data.Take(result.Count));
                        if (result.CloseStatus.HasValue) return;
                    }

                    using (MemoryStream ms = new MemoryStream(message.ToArray()))
                    using (BsonDataReader reader = new BsonDataReader(ms, false, DateTimeKind.Utc))
                    {
                        var jObject = JObject.Load(reader);
                        var typeToken = jObject.GetValue("Type", StringComparison.CurrentCultureIgnoreCase);
                        MessageType typeValue = typeToken.ToObject<MessageType>();

                        if (typeValue == MessageType.Broadcast)
                        {
                            var broadcastToken = jObject.GetValue("Broadcast", StringComparison.CurrentCultureIgnoreCase);
                            typeValue = (MessageType)Enum.Parse(typeof(MessageType), nameof(MessageType.Broadcast) + broadcastToken.ToString(), true);
                        }
                        Console.WriteLine($"Message decoded: {typeValue}");
                        var bodyToken = jObject.GetValue("Body", StringComparison.CurrentCultureIgnoreCase);
                        handlers[typeValue].Handle(bodyToken.ToObject(handlers[typeValue].TypeHandled));
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private async Task CloseSocket()
        {
            var data = Array.Empty<byte>();
            switch (socket.State)
            {
                case WebSocketState.CloseSent:
                    var result = await socket.ReceiveAsync(new ArraySegment<byte>(data, 0, 0), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        MessageBox.Show("wtf sending messages to a closed websocket?");
                    }
                    break;
                case WebSocketState.CloseReceived:
                    await socket.SendAsync(new ArraySegment<byte>(data, 0, 0), WebSocketMessageType.Close, true, CancellationToken.None);
                    break;
            }
            Application.Current.Shutdown();
        }

        public void AddHandler<T>() where T: Handler, new()
        {
            var handler = new T();
            handlers.Add(handler.TypeHandledEnum, handler);
        }

        public void AddHandler<T>(T handler) where T: Handler
        {
            handlers.Add(handler.TypeHandledEnum, handler);
        }

        internal void PushMessage(MessageType msgType, object DTO)
        {
            messagesToSend.Enqueue((msgType, DTO));
        }
    }
}
