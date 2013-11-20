using Feedro.Model.Data;
using Feedro.Model.Net;
using System.Threading.Tasks;

namespace Feedro.ViewModel
{
    public class NewSubscriptionViewModel : Monitorable
    {
        #region Properties

        public Store Store
        {
            get;
            private set;
        }

        private string _subscriptionUri;

        public string SubscriptionUri
        {
            get
            {
                return _subscriptionUri;
            }
            set
            {
                if (value == _subscriptionUri)
                    return;

                _subscriptionUri = value;
                NotifyPropertyChanged();
            }
        }

        private bool _isSubscribing;

        public bool IsSubscribing
        {
            get
            {
                return _isSubscribing;
            }
            set
            {
                if (value == _isSubscribing)
                    return;

                _isSubscribing = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region .ctor

        public NewSubscriptionViewModel()
        {
            Store = new Store();
        }

        #endregion

        #region Public methods

        public async Task<Subscription> AddSubscription()
        {
            IsSubscribing = true;

            try
            {
                Subscription subscription = await SubscriptionFactory.CreateSubscriptionAsync(SubscriptionUri);
                await Store.Refresh();
                Store.AddSubscription(subscription);
                await Store.SaveChanges();
                return subscription;
            }
            finally
            {
                IsSubscribing = false;
            }
        }

        #endregion
    }
}
