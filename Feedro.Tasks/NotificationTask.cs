using Feedro.Model.Data;
using Feedro.Model.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Resources;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace Feedro.Tasks
{
    public sealed class NotificationTask : IBackgroundTask
    {
        #region Model objects

        private readonly ResourceLoader loader = new ResourceLoader();

        #endregion

        #region Entry point

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            await CheckUpdates();
            deferral.Complete();
        }

        #endregion

        #region Update & notification

        private async Task CheckUpdates()
        {
            IEnumerable<Entry> updates = await new SubscriptionUpdater().Update(new Store());

            SendUpdatesForLiveTile(updates);
            SendBadgeUpdate(updates.Count());
            SendToasts(updates);
        }

        private XmlDocument ToLiveTileContent(Entry update)
        {
            XmlDocument content = TileUpdateManager.GetTemplateContent(
                TileTemplateType.TileWide310x150Text09);

            var nodes = content.GetElementsByTagName("text");

            nodes[0].AppendChild(content.CreateTextNode(update.Title));
            nodes[1].AppendChild(content.CreateTextNode(update.FormattedSummary));

            return content;
        }

        private void SendUpdatesForLiveTile(IEnumerable<Entry> updates)
        {
            const int MaxUpdatesForLiveTile = 5;

            TileUpdater updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);

            IEnumerable<TileNotification> notifications = updates.Take(MaxUpdatesForLiveTile)
                .Select(update => ToLiveTileContent(update))
                .Select(content => new TileNotification(content));

            foreach (var notification in notifications)
                updater.Update(notification);
        }

        private void SendBadgeUpdate(int count)
        {
            XmlDocument badgeXml = BadgeUpdateManager.GetTemplateContent(
                BadgeTemplateType.BadgeNumber);
            var badge = badgeXml.SelectSingleNode("/badge") as XmlElement;
            badge.SetAttribute("value", count.ToString());
            var notification = new BadgeNotification(badgeXml);
            BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(notification);
        }

        private XmlDocument ToToastContent(Subscription subscription, IEnumerable<Entry> updates)
        {
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(
                ToastTemplateType.ToastText01);

            var nodes = toastXml.GetElementsByTagName("text");

            string message = string.Format(
                loader.GetString("MessageUpdate"),
                subscription.Title,
                updates.Count());

            nodes[0].AppendChild(toastXml.CreateTextNode(message));

            return toastXml;
        }

        private void SendToasts(IEnumerable<Entry> updates)
        {
            ToastNotifier notifier = ToastNotificationManager.CreateToastNotifier();

            IEnumerable<ToastNotification> notifications = updates
                .GroupBy(g => g.Subscription)
                .Select(g => ToToastContent(g.Key, g))
                .Select(c => new ToastNotification(c));

            foreach (var notification in notifications)
                notifier.Show(notification);
        }

        #endregion
    }
}
