using NebzzClient.Messages;
using System;
using System.ComponentModel;

namespace NebzzClientFront
{
    public class NotifierMessage : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Author { get; private set; }
        public DateTimeOffset UtcTime { get; private set; }
        public string Text { get; private set; }

        public Thumbnail Thumbnail { get; private set; }

        public NotifierMessage(string author, DateTimeOffset utcTime, string text, Thumbnail thumbnail)
        {
            Author = author;
            UtcTime = utcTime;
            Text = text;
            Thumbnail = thumbnail;
        }

        public string MetaInfo
        {
            get
            {
                var timeSpent = DateTimeOffset.UtcNow - UtcTime;

                string timeString;
                if (timeSpent.TotalSeconds < 60)
                {
                    timeString = FormatTimeAgo("second", (timeSpent.Seconds / 10) * 10);
                }
                else if (timeSpent.TotalMinutes < 60)
                {
                    timeString = FormatTimeAgo("minute", timeSpent.Minutes);
                }
                else if (timeSpent.TotalHours < 24)
                {
                    timeString = FormatTimeAgo("hour", timeSpent.Hours);
                }
                else if (timeSpent.TotalDays < 7)
                {
                    timeString = FormatTimeAgo("day", timeSpent.Days);
                }
                else if (timeSpent.TotalDays < 31)
                {
                    timeString = FormatTimeAgo("week", timeSpent.Days / 7);
                }
                else
                {
                    timeString = FormatTimeAgo("month", timeSpent.Days / 31);
                }
                return $"{Author}, {timeString}";
            }
        }

        internal void Notify(object sender, PropertyChangedEventArgs eventArgs)
        {
            PropertyChanged?.Invoke(sender, eventArgs);
        }

        private static string FormatTimeAgo(string unit, int value)
        {
            return $"{value} {unit}{(value == 1 ? "" : "s")} ago";
        }

    }
}
