using Feedro.Tasks;
using Feedro.View;
using Feedro.ViewModel;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Resources;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;

namespace Feedro
{
    sealed partial class App : Application
    {
        public const int BackgroundTaskInterval = 15;
        public const string TaskName = "NotificationTask";

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected async override void OnLaunched(LaunchActivatedEventArgs args)
        {
            var rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                if (!rootFrame.Navigate(typeof(MainPage), args.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            Window.Current.Activate();

            SettingsPane.GetForCurrentView().CommandsRequested += OnCommandsRequested;

            await RegisterBackgroundTask();
        }

        #region Setting charm support

        private void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            UICommandInvokedHandler showSettingsPanel = (cmd) =>
                {
                    new GeneralSettingsFlyout().Show();
                };

            var loader = new ResourceLoader();

            args.Request.ApplicationCommands.Clear();
            args.Request.ApplicationCommands.Add(
                new SettingsCommand(
                    "General",
                    loader.GetString("CaptionGeneralSettings"),
                    showSettingsPanel));
        }
        
        #endregion

        protected override async void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
        {
            Uri uri = await args.ShareOperation.Data.GetWebLinkAsync();

            var page = new SharePage()
            {
                
                ShareOperation = args.ShareOperation,

                DataContext = new NewSubscriptionViewModel()
                {
                    SubscriptionUri = uri.AbsoluteUri
                }
            };

            Window.Current.Content = page;
            Window.Current.Activate();
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            // var deferral = e.SuspendingOperation.GetDeferral();
            // deferral.Complete();
        }

        public static async Task RegisterBackgroundTask()
        {
            var tasks = BackgroundTaskRegistration.AllTasks.Values;

            if (tasks.Any(t => t.Name.Equals(TaskName)))
                return;

            var builder = new BackgroundTaskBuilder()
            {
                Name = TaskName,
                TaskEntryPoint = typeof(NotificationTask).FullName
            };

            builder.SetTrigger(new TimeTrigger(BackgroundTaskInterval, false));
            builder.Register();

            await BackgroundExecutionManager.RequestAccessAsync();
        }

        public static void UnregisterBackgroundTask()
        {
            var tasks = BackgroundTaskRegistration.AllTasks.Values;
            var task = tasks.SingleOrDefault(t => t.Name.Equals(TaskName));

            if (task == null)
                return;

            task.Unregister(false);
        }
    }
}
