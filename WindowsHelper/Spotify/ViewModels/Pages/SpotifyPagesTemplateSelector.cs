using System;
using System.Windows;
using System.Windows.Controls;
using WindowsHelper.Spotify.Interfaces;

namespace WindowsHelper.Spotify.ViewModels.Pages
{
    public class SpotifyPagesTemplateSelector : DataTemplateSelector //TODO: Are all pages added?
    {
        public DataTemplate HomePageTemplate { private get; set; }
        public DataTemplate ArtistPageTemplate { private get; set; }
        public DataTemplate AlbumPageTemplate { private get; set; }
        public DataTemplate SongsPageTemplate { private get; set; }
        public DataTemplate RecentlyPlayedPageTemplate { private get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
                return null;

            if (!(item is ISpotifyPage page))
                throw new InvalidOperationException("Item is not a Spotify Page");

            if (page is SpotifyHomePageViewModel)
                return HomePageTemplate;
            else if (page is SpotifyArtistPageViewModel)
                return ArtistPageTemplate;
            else if (page is SpotifyAlbumPageViewModel)
                return AlbumPageTemplate;
            else if (page is SpotifySongsPageViewModel)
                return SongsPageTemplate;
            else if (page is SpotifyRecentlyPlayedPageViewModel)
                return RecentlyPlayedPageTemplate;
            else
                throw new NotImplementedException(); //TODO: What to do in that case?
        }
    }
}