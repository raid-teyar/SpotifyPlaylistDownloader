using HandyControl.Controls;
using Microsoft.Win32;
using SpotifySongDownloader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ookii.Dialogs.Wpf;
using YoutubeExplode;
using HandyControl.Controls;
using HandyControl.Tools.Extension;

namespace SpotifySongDownloader.Views
{
    public partial class SearchResultsView : Page
    {


        public Task InitTask => Task.Run(() => Dispatcher.Invoke(() => Init()));

        public SearchResultsView()
        {
            InitializeComponent();
            Growl.Clear();
        }

        private void Init()
        {
            foreach (var track in Globals.CardTracksList)
            {
                TrackList.Items.Add(CreateCard(track));
            }
        }

        private Card CreateCard(Track track)
        {
            Card card = new Card()
            {
                MaxWidth = 240,
                Margin = new Thickness(8),
                BorderThickness = new Thickness(0),
                Effect = Helpers.Get<Effect>("EffectShadow2")
            };

            StackPanel outerStackPanel = new StackPanel();

            Border border = new Border()
            {
                CornerRadius = new CornerRadius(4, 4, 0, 0),
                Style = Helpers.Get<Style>("BorderClip"),
            };

            Image image = new Image()
            {
                Width = 240,
                Height = 240,
                Source = new BitmapImage(new Uri(track.ImageUrl)),
                Stretch = Stretch.Uniform
            };

            border.Child = image;
            outerStackPanel.Children.Add(border);

            StackPanel innerStackPanel = new StackPanel();
            innerStackPanel.Margin = new Thickness(10);

            TextBlock titelText = new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = track.Name,
                TextTrimming = TextTrimming.CharacterEllipsis,
                TextWrapping = TextWrapping.NoWrap,
                Style = Helpers.Get<Style>("TextBlockLargeBold")
            };

            TextBlock artisiText = new TextBlock()
            {
                Margin = new Thickness(0, 6, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = string.Join(", ", track.Artists),
                TextTrimming = TextTrimming.CharacterEllipsis,
                TextWrapping = TextWrapping.NoWrap,
                Style = Helpers.Get<Style>("TextBlockDefault")
            };

            innerStackPanel.Children.Add(titelText);
            innerStackPanel.Children.Add(artisiText);
            outerStackPanel.Children.Add(innerStackPanel);

            card.Content = outerStackPanel;

            return card;
        }

        private async void DownloadClick(object sender, RoutedEventArgs e)
        {
           VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            if (dialog.ShowDialog() == true) {
                DownloadView downloadView = new DownloadView(dialog.SelectedPath);
                Dialog.Show(downloadView);
                
                return;
            }
        }
    }
}
