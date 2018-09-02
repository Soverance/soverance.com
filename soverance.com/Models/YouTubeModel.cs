using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;

namespace soverance.com.Models
{
    public static class YouTubeModel
    {
        public static List<YouTubeData> GetVideos(string GoogleApiKey)
        {
            int MaxResults = 50;
            List<YouTubeData> VideoList = new List<YouTubeData>();

            try
            {
                var yt = new YouTubeService(new BaseClientService.Initializer() { ApiKey = GoogleApiKey });
                var channelsListRequest = yt.Channels.List("contentDetails");
                channelsListRequest.ForUsername = "soverancestudios";  // youtube channel name
                var channelsListResponse = channelsListRequest.Execute();
                foreach (var channel in channelsListResponse.Items)
                {
                    // of videos uploaded to the channel.
                    var uploadsListId = channel.ContentDetails.RelatedPlaylists.Uploads;
                    var nextPageToken = "";
                    while (nextPageToken != null)
                    {
                        var playlistItemsListRequest = yt.PlaylistItems.List("snippet");
                        playlistItemsListRequest.PlaylistId = uploadsListId;
                        playlistItemsListRequest.MaxResults = MaxResults;
                        playlistItemsListRequest.PageToken = nextPageToken;
                        // Retrieve the list of videos uploaded to the authenticated user's channel.
                        var playlistItemsListResponse = playlistItemsListRequest.Execute();
                        foreach (var playlistItem in playlistItemsListResponse.Items)
                        {
                            YouTubeData VideoDataObject = new YouTubeData();
                            
                            // query the video array to see if we've already added it
                            var query = (from video in VideoList where video.Title == playlistItem.Snippet.Title select video).FirstOrDefault();
                            if (query == null)
                            {
                                VideoDataObject.VideoEmbedLink = "https://www.youtube.com/embed/" + playlistItem.Snippet.ResourceId.VideoId;
                                VideoDataObject.VideoWatchLink = "https://www.youtube.com/watch?v=" + playlistItem.Snippet.ResourceId.VideoId;
                                VideoDataObject.Title = playlistItem.Snippet.Title;
                                VideoDataObject.Descriptions = playlistItem.Snippet.Description;
                                VideoDataObject.ImageUrl = playlistItem.Snippet.Thumbnails.High.Url;
                                VideoDataObject.IsValid = true;
                                VideoList.Add(VideoDataObject);

                            }
                        }
                        nextPageToken = playlistItemsListResponse.NextPageToken;
                    }
                }
            }
            catch (Exception e)
            {
                string ErrorMessage = "Some exception occured" + e;
                //return RedirectToAction("GetYouTube");
            }

            return VideoList;
        }
    }
}
