using System;
using System.Text;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;

namespace soverance.com.Models
{
    public partial class YouTubeData
    {
        public string Descriptions { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public bool IsValid { get; set; }
        public string VideoEmbedLink { get; set; }
        public string VideoWatchLink { get; set; }
    }
}
