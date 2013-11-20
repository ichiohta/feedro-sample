using Feedro.Model.Data;
using Feedro.ViewModel;
using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Feedro.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchResultPage : Page
    {
        private SearchResultViewModel ViewModel
        {
            get
            {
                return DataContext as SearchResultViewModel;
            }
        }

        public SearchResultPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.QueryText = e.Parameter as string;
            await ViewModel.ExecuteSearch();
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private void SearchResultClicked(object sender, SelectionChangedEventArgs e)
        {
            var results = sender as GridView;
            var result = results != null ? results.SelectedItem as Entry : null;

            if (result != null)
            {
                Frame.Navigate(typeof(MainPage), result);
            }

        }
    }
}
