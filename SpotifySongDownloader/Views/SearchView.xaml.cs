using HandyControl.Controls;
using HandyControl.Data;
using Newtonsoft.Json.Linq;
using SpotifyAPI.Web;
using SpotifySongDownloader.Models;
using SpotifySongDownloader.Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;


namespace SpotifySongDownloader.Views
{
    public partial class SearchView : Page
    {

        SpotifyService spotifyService;
        public SearchView()
        {
            InitializeComponent();
            spotifyService = new SpotifyService();
        }
        private async void SearchClick(object sender, RoutedEventArgs e)
        {
            ToggleButton button = (ToggleButton)sender;
            button.Content = "Searching...";
            string playlistLink = SearchInput.Text;

            try
            {
                // getting playlist id from the playlist link
                int pFrom = playlistLink.IndexOf("playlist/") + "playlist/".Length;
                int pTo = playlistLink.LastIndexOf("?si");
                string playlistID = playlistLink.Substring(pFrom, pTo - pFrom);

                // getting playlist tracks metadata
                var tracks = await spotifyService.getTracks(playlistID);
                Globals.CardTracksList = ConvertList(tracks);

                var searchResultView = new SearchResultsView();

                await searchResultView.InitTask;

                NavigationService.Navigate(searchResultView);

            }
            catch (Exception ex)
            {
                Growl.Error("Invalid playlist link \nSpecific Error: " + ex.Message);
            }

            button.IsChecked = false;
            button.Content = "Search";
        }

        // converting the spotify playlist tracks list to a list that can be used to display data in cards
        private List<Models.Track> ConvertList(List<TrackDto> tracks)
        {
            var list = new List<Models.Track>();

            foreach (var spotifyTrack in tracks)
            {
                var track = new Models.Track()
                {
                    Name = spotifyTrack.Track.Name,
                    ImageUrl = spotifyTrack.Track.Album.Images[0].Url,
                    Duration = spotifyTrack.Track.Duration_ms,
                };
                track.Artists = GetArtistsNames(spotifyTrack.Track.Artists);
                list.Add(track);
            }

            return list;
        }

        // converting artists list for a track to a simple array of strings
        private string[] GetArtistsNames(List<Artist> artists)
        {
            var list = new string[artists.Count];

            for (int i = 0; i < list.Length; i++)
            {
                list[0] = artists[0].Name;
            }

            return list;
        }
    }
}
