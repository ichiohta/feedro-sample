using Feedro.Model.Data;
using Feedro.View.Util;
using Feedro.ViewModel;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources;
using Windows.Graphics.Display;
using Windows.System;
using Windows.UI.ApplicationSettings;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Feedro.View
{
    public sealed partial class MainPage : Page
    {
        #region Properties

        private TimelineViewModel ViewModel
        {
            get
            {
                return DataContext as TimelineViewModel;
            }
        }

        #endregion

        #region .ctor

        public MainPage()
        {
            InitializeComponent();

            Loaded   += OnLoaded;
            Unloaded += OnUnloaded;
            SizeChanged += OnSizeChanged;
        }

        #endregion

        #region Multiple view support

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            switch (ViewStatus.CurrentViewMode)
            {
                case ViewStatus.ViewMode.Narrow:
                    LandscapeView.Visibility = Visibility.Collapsed;
                    PortraitView.Visibility = Visibility.Collapsed;
                    SnappedView.Visibility = Visibility.Visible;
                    break;

                case ViewStatus.ViewMode.Moderate:
                    LandscapeView.Visibility = Visibility.Collapsed;
                    PortraitView.Visibility = Visibility.Visible;
                    SnappedView.Visibility = Visibility.Collapsed;
                    break;

                case ViewStatus.ViewMode.Wide:
                    LandscapeView.Visibility = Visibility.Visible;
                    PortraitView.Visibility = Visibility.Collapsed;
                    SnappedView.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        #endregion

        #region Share (Source) Support

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            DataTransferManager.GetForCurrentView().DataRequested += ShareEntry;
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            DataTransferManager.GetForCurrentView().DataRequested -= ShareEntry;
        }


        private void ShareEntry(DataTransferManager sender, DataRequestedEventArgs args)
        {
            Entry entry = ViewModel.SelectedEntry;

            if (entry == null)
            {
                return;
            }

            DataPackage shared = args.Request.Data;

            shared.Properties.Title = entry.Title;
            shared.Properties.Description = entry.Subscription.Title;
            shared.SetWebLink(new Uri(entry.Uri));
            shared.SetText(entry.Title);
        }

        #endregion

        #region ViewModel initialization

        protected override async void OnNavigatedTo(NavigationEventArgs args)
        {
            await ViewModel.Load();

            var selectedEntry = args.Parameter as Entry;

            if (selectedEntry != null)
            {
                ViewModel.SelectedEntry = ViewModel.Entries
                    .Where(e => string.Compare(e.Uri, selectedEntry.Uri, StringComparison.OrdinalIgnoreCase) == 0)
                    .FirstOrDefault(e => e.DateTimePosted == selectedEntry.DateTimePosted);
            }
        }

        #endregion

        #region AppBar buttons

        private async void Refresh_OnClick(object sender, RoutedEventArgs e)
        {
            AppBar.IsOpen = false;
            await ViewModel.Refresh();
        }

        private async void Browse_OnClick(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(ViewModel.SelectedEntry.Uri));
        }

        private async void Favorite_OnClick(object sender, RoutedEventArgs e)
        {
            await ViewModel.Bookmark();
        }

        #endregion

        private void SearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            if (!string.IsNullOrEmpty(sender.QueryText))
                Frame.Navigate(typeof(SearchResultPage), sender.QueryText);
        }
    }
}
