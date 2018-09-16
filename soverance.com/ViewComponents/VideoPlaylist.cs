using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
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
