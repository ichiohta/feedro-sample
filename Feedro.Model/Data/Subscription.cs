using System.Runtime.Serialization;

namespace Feedro.Model.Data
{
    [DataContract]
    public class Subscription : Monitorable
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

        private string _faviconUri;

        [DataMember]
        public string FaviconUri
        {
            get
            {
                return _faviconUri;
            }
            set
            {
                if (value == _faviconUri)
                    return;

                _faviconUri = value;
                NotifyPropertyChanged();
            }
        }
    }
}
