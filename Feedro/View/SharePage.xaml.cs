using Feedro.Model.Data;
using Feedro.ViewModel;
using System;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Feedro.View
{
    public sealed partial class SharePage : Page
    {
        public NewSubscriptionViewModel ViewModel
        {
            get
            {
                return DataContext as NewSubscriptionViewModel;
            }
        }

        public ShareOperation ShareOperation
        {
            get;
            set;
        }

        public SharePage()
        {
            this.InitializeComponent();
            Height = Window.Current.Bounds.Height;
        }

        private async void ButtonSubscribe_OnClick(object sender, RoutedEventArgs args)
        {
            var loader = new ResourceLoader();
            string message = null;

            try
            {
                Subscription subscription = await ViewModel.AddSubscription();

                message = string.Format(
                    loader.GetString("MessageSubscriptionComplete"),
                    subscription.Title);
            }
            catch (Exception e)
            {
                message = string.Format(
                    loader.GetString("MessageSubscriptionError"),
                    e.Message);
            }

            await new MessageDialog(message).ShowAsync();
            ShareOperation.ReportCompleted();
        }
    }
}
