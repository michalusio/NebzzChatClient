using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace NebzzClient.Messages
{
    internal class ThumbnailEngine
    {
        internal static ThumbnailEngine Instance = new ThumbnailEngine();

        private readonly Regex urlRegex = new Regex(@"(http(s)?:\/\/.)?(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        private readonly Queue<Message> toThumbnail = new Queue<Message>();
        private readonly HtmlWeb web;

        private ThumbnailEngine()
        {
            web = new HtmlWeb();
        }

        internal async Task ProcessingLoopAsync()
        {
            while (true)
            {
                await Task.Delay(50);
                if (toThumbnail.Count == 0) continue;

                var message = toThumbnail.Dequeue();
                var matchUrl = urlRegex.Match(message.Text);
                if (!matchUrl.Success) continue;

                HtmlDocument doc = web.Load(FormatUrl(matchUrl.Value));
                    
                if (doc == null) continue;

                var metaTags = doc.DocumentNode.SelectNodes("//head/meta");

                if (metaTags == null) continue;

                try
                {
                    var metaTagsContent = metaTags
                        .Select(tag => (type: tag.GetAttributeValue<string>("property", null), content: tag.GetAttributeValue<string>("content", null)))
                        .Where(tag => tag.type != null && tag.content != null)
                        .ToDictionary(tag => tag.type, tag => tag.content);

                    Uri image = null;
                    if (metaTagsContent.TryGetValue("og:image", out string _image) || metaTagsContent.TryGetValue("twitter:image", out _image))
                    {
                        image = new Uri(_image);
                    }

                    string description = null;
                    if (metaTagsContent.TryGetValue("og:description", out string _description) || metaTagsContent.TryGetValue("twitter:description", out _description))
                    {
                        description = _description;
                    }

                    string title = null;
                    if (metaTagsContent.TryGetValue("og:title", out string _title) || metaTagsContent.TryGetValue("twitter:title", out _title))
                    {
                        title = _title;
                    }

                    MessageRepo.Instance.UpdateImage(message.ID, new Thumbnail(title, description, image));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        internal void EnqueueMessage(Message message)
        {
            toThumbnail.Enqueue(message);
        }

        private static string FormatUrl(string url)
        {
            var hasHttp = url.StartsWith("http://") || url.StartsWith("https://");
            bool hasWWW;
            if (hasHttp)
            {
                hasWWW = url.StartsWith("http://www.") || url.StartsWith("https://www.");
            }
            else hasWWW = url.StartsWith("www.");
            switch ((hasHttp ? 1 : 0) + (hasWWW ? 2 : 0))
            {
                case 0:
                    url = "http://www." + url;
                    break;
                case 1:
                    if (url.StartsWith("http://"))
                    {
                        url = "http://www." + url.Substring(7);
                    }
                    else
                    {
                        url = "https://www." + url.Substring(8);
                    }
                    break;
                case 2:
                    url = "http://" + url;
                    break;
            }
            return url;
        }
    }
}
