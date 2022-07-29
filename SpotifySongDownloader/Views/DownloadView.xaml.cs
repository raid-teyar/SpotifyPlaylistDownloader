using ATL;
using HandyControl.Controls;
using MediaToolkit;
using MediaToolkit.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using YoutubeExplode;
using YoutubeExplode.Search;

namespace SpotifySongDownloader.Views
{
  
    public partial class DownloadView
    {
        public string DownloadPath { get; set; }

        public DownloadView(string downloadPath)
        {
            DownloadPath = downloadPath;
            InitializeComponent();
        }

        private async void WindowLoaded(object sender, RoutedEventArgs e)
        {
            var youtubeClient = new YoutubeClient();
            VisualText.Text = "Scanning playlist...";
            int listLenght = Globals.CardTracksList.Count;
            int progress = 1;

            VisualText.Text = "Saving song covers to a temp directory...";
            await Task.Run( async () => await saveTempImages(Globals.CardTracksList));

            foreach (var track in Globals.CardTracksList)
            {
                int Diversion = 60 * 1000;

                VisualText.Text = $"Searching for track: \"{track.Name}\"...";

                string downloadUrl = "";

                await foreach (VideoSearchResult result in youtubeClient.Search.GetVideosAsync(track.Name + " - " + track.Artists.First()))
                {
                    int resultAudioDuration = (int)result.Duration.Value.TotalMilliseconds;

                    if (track.Duration - Diversion > resultAudioDuration || resultAudioDuration > track.Duration + Diversion)
                    {
                        continue;
                    }

                    downloadUrl = result.Url;

                    break;
                }

                VisualText.Text = $"Getting available steams for track: \"{track.Name}\"...";
                var streamManifest = await youtubeClient.Videos.Streams.GetManifestAsync(downloadUrl);

                VisualText.Text = $"Getting audio only streams for track: \"{track.Name}\"...";
                var streamInfo = streamManifest.GetAudioOnlyStreams();

                // choosing best audio stream (comparing their Birate)
                var bestStream = streamInfo.OrderByDescending(x => x.Bitrate).FirstOrDefault();

                ProgressBar.Value = progress * 100 / listLenght;
                VisualText.Text = $"Downloading track: \"{track.Name}\"...";

                string savePath = DownloadPath + "\\" + track.Name + "." + bestStream.Container.Name;

                await youtubeClient.Videos.Streams.DownloadAsync(bestStream, savePath);
                 
                Growl.Success("Track " + track.Name + " downloaded successfully");

                // converting the track to mp3 format

                VisualText.Text = $"Converting track: \"{track.Name}\" to mp3 format... ";


                string convertedFilePath = savePath.Substring(0, savePath.Length - 4) + "mp3";

                var inputFile = new MediaFile { Filename = savePath };
                var outputFile = new MediaFile { Filename = convertedFilePath };
               



                 using (var engine = new Engine())
                {
                    
                        engine.GetMetadata(inputFile);
                        engine.Convert(inputFile, outputFile);

                    // deleting the before conversion file
                    VisualText.Text = $"Deleting the before conversion file... ";

                    File.Delete(savePath);
                }

                // adding the embedded song cover

                Track track1 = new Track(convertedFilePath);

                PictureInfo picture = PictureInfo.fromBinaryData(File.ReadAllBytes(track.ImageUrl), PictureInfo.PIC_TYPE.Generic);

                track1.EmbeddedPictures.Add(picture);
                track1.Save();
                progress++;
                
                VisualText.Text = $"Songs downloaded successfully";
                
            }
            ProgressBar.Visibility = Visibility.Hidden;
            warining.Visibility = Visibility.Hidden;
            Growl.Success("Finished downloading all songs :)");
        }



        // saving spotify covers to a temp folder before embedding them
        private async Task saveTempImages(List<Models.Track> tracks)
        {
            foreach (var track in tracks)
            {
                string url = track.ImageUrl;
                string tempPath = Path.GetTempFileName();
                track.ImageUrl = tempPath.Substring(0, tempPath.Length - 4) + ".jpg";

                using (WebClient client = new WebClient())
                {
                    byte[] dataArr =  client.DownloadData(url);
                    File.WriteAllBytes(track.ImageUrl, dataArr);
                }
            }

        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
