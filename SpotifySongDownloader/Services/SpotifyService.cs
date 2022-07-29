using HandyControl.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SpotifyAPI;
using SpotifyAPI.Web;
using System.Collections.Generic;
using SpotifySongDownloader.Models;

namespace SpotifySongDownloader.Services
{
    public class SpotifyService
    {
        private string _spotifyToken = "BQCZOHiFnSqDVqVYF9oL-d1lY93TJdLPGcTxuSmkOK9w0WmC5GN3cOZAm3V7JNUvPJMFZFP6JwpmvxqKZGMavujdHKlOClfREKJYOSKTBYY61VUupK4";

        public int FullTrack { get; private set; }

        // gets the Spotify token used to make requests
        public async Task getAuthToken()
        {
            HttpRequestMessage tokenRequest = new HttpRequestMessage(HttpMethod.Get, ServiceConfig.spotifyBaseUrl + "get_access_token?reason=transport&productType=web_player");
            tokenRequest.Headers.Add("User-Agent", ServiceConfig.UserAgent);

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.SendAsync(tokenRequest);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject spotifyJsonToken = JObject.Parse(result);
                    _spotifyToken = spotifyJsonToken.SelectToken("accessToken").ToString();
                    Growl.Success("Anonymous connection with Spotify established successfully!");
                }
                else
                {
                    Growl.Fatal("There was an error when trying to get the access token :(");
                }
            }
        }

        // gets any Spotify playlist tracks using the given playlist id
        public async Task<List<TrackDto>> getTracks(string playlistID)
        {
            string options = "?fields=items(track(name,duration_ms,artists(name),album(images(url))))";
            HttpRequestMessage playlistRequest = new HttpRequestMessage(HttpMethod.Get, ServiceConfig.spotifyApiUrl + $"v1/playlists/{playlistID}/tracks" + options);
            playlistRequest.Headers.Add("User-Agent", ServiceConfig.UserAgent);
            playlistRequest.Headers.Add("Authorization", "Bearer " + _spotifyToken);
            


            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.SendAsync(playlistRequest);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        var result = await response.Content.ReadAsStringAsync();
                        var responseJsonObject = JObject.Parse(result);
                        string tracksjsonString = responseJsonObject.SelectToken("items").ToString();

                        var tracksList = JsonConvert.DeserializeObject<List<TrackDto>>(tracksjsonString);
                        return tracksList;

                    case HttpStatusCode.Unauthorized:
                        Growl.Info("Token expired, getting a new one...");
                        await getAuthToken();
                        var trackList2 = await getTracks(playlistID);
                        return trackList2;

                    default:
                        Growl.Error("There was an error when trying to get the playlist tracks :(");
                        return null;
                }

            }
        }


    }
}
