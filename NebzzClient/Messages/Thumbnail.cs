using System;
using System.Windows.Media.Imaging;

namespace NebzzClient.Messages
{
    public struct Thumbnail
    {
        public string Title { get; private set; }
        public string Description { get; private set; }

        public Uri Image { get; private set; }

        public Thumbnail(string title, string description, Uri image)
        {
            Title = title;
            Description = description;
            Image = image;
        }

        public static bool operator ==(Thumbnail a, Thumbnail b)
        {
            return a.Title == b.Title && a.Description == b.Description && a.Image == b.Image;
        }

        public static bool operator !=(Thumbnail a, Thumbnail b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return (obj is Thumbnail b) && (this == b);
        }

        public override int GetHashCode()
        {
            int hash = 7;
            unchecked
            {
                hash = hash * 13 + Title?.GetHashCode() ?? 0;
                hash = hash * 13 + Description?.GetHashCode() ?? 0;
                hash = hash * 13 + Image?.GetHashCode() ?? 0;
            }
            return hash;
        }
    }
}