using System;
using System.Runtime.Serialization;
using Feedro.Model.Helper;

namespace Feedro.Model.Data
{
    [DataContract]
    public class Entry : Monitorable
    {
        private string _uri;

        [DataMember]
        public string Uri
        {
            get
            {
                return _uri;
            }
            set
            {
                if (value == _uri)
                    return;

                _uri = value;
                NotifyPropertyChanged();
            }
        }

        private string _title;

        [DataMember]
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (value == _title)
                    return;

                _title = value;
                NotifyPropertyChanged();
            }
        }

        private string _summary;

        [DataMember]
        public string Summary
        {
            get
            {
                return _summary;
            }
            set
            {
                if (value == _summary)
                    return;

                _summary = value;
                NotifyPropertyChanged();

                FormattedSummary = value.RemoveHtmlTags();
            }
        }

        private DateTimeOffset _dateTimePosted;

        [DataMember]
        public DateTimeOffset DateTimePosted
        {
            get
            {
                return _dateTimePosted;
            }
            set
            {
                if (value == _dateTimePosted)
                    return;

                _dateTimePosted = value;
                NotifyPropertyChanged();
            }
        }

        private string _subscriptionUri;

        [DataMember]
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

        private bool _isFavorite;

        [DataMember]
        public bool IsFavorite
        {
            get
            {
                return _isFavorite;
            }
            set
            {
                if (value == _isFavorite)
                    return;

                _isFavorite = value;
                NotifyPropertyChanged();
            }
        }

        #region Calculated properties

        private string _formattedSummary;

        public string FormattedSummary
        {
            get
            {
                return _formattedSummary;
            }
            private set
            {
                if (value == _formattedSummary)
                    return;

                _formattedSummary = value;
                NotifyPropertyChanged();
            }
        }

        private Subscription _subscription;

        public Subscription Subscription
        {
            get
            {
                return _subscription;
            }
            set
            {
                if (value == _subscription)
                    return;

                _subscription = value;
                NotifyPropertyChanged();
            }
        }

        #endregion
    }
}
