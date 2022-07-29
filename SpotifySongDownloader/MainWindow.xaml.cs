using SpotifySongDownloader.Services;
using SpotifySongDownloader.Views;
using System.Windows;


namespace SpotifySongDownloader
{

    public partial class MainWindow : System.Windows.Window
    {
        SpotifyService spotifyService;

        public MainWindow()
        {
            InitializeComponent();

            // initializing the spotify service used to make HTTP calls
            spotifyService = new SpotifyService();

            NavigationFrame.Navigate(new SearchView());
        }

        
    }
}
