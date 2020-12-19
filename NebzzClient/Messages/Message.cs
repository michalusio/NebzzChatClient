using System;

namespace NebzzClient.Messages
{
    public struct Message
    {
        public string Author { get; private set; }
        public DateTimeOffset UtcTime { get; private set; }
        public string Text { get; private set; }

        public Thumbnail Thumbnail { get; private set; }
        public long ID { get; private set; }

        public Message(long id, string author, long utcTime, string text) : this(id, author, DateTimeOffset.FromUnixTimeMilliseconds(utcTime).ToUniversalTime(), text)
        {
        }

        public Message(long id, string author, DateTimeOffset utcTime, string text) : this(id, author, utcTime, text, default)
        {
        }

        public Message(long id, string author, DateTimeOffset utcTime, string text, Thumbnail thumbnail)
        {
            ID = id;
            Author = author;
            UtcTime = utcTime;
            Text = text;
            Thumbnail = thumbnail;
        }

        public Message(Message clone, Thumbnail thumbnail)
        {
            ID = clone.ID;
            Author = clone.Author;
            UtcTime = clone.UtcTime;
            Text = clone.Text;
            Thumbnail = thumbnail;
        }

        public void Deconstruct(out string author, out DateTimeOffset utcTime, out string text)
        {
            author = Author;
            utcTime = UtcTime;
            text = Text;
        }

        public void Deconstruct(out string author, out DateTimeOffset utcTime, out string text, out Thumbnail thumbnail)
        {
            author = Author;
            utcTime = UtcTime;
            text = Text;
            thumbnail = Thumbnail;
        }
    }
}
