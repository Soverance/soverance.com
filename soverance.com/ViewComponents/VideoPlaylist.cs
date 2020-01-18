using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using soverance.com.Models;

namespace soverance.com.ViewComponents
{
    public class VideoPlaylistViewComponent : ViewComponent
    {
        private readonly IOptions<SecretConfig> SovSecretConfig;
        

        public VideoPlaylistViewComponent(IOptions<SecretConfig> _SovSecretConfig)
        {
            SovSecretConfig = _SovSecretConfig;
        }

        public async Task<IViewComponentResult> InvokeAsync(string playlistId)
        {
            string GoogleApiKey = SovSecretConfig.Value.GoogleApiKey;
            List<YouTubeData> Playlist = YouTubeModel.GetVideos(GoogleApiKey, false, playlistId);
            ViewBag.Playlist = Playlist;
            ViewBag.ShowcaseVideo = Playlist[0];
            return View();
        }
    }
}
