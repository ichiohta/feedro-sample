using Feedro.Model.Data;
using Feedro.Model.Helper;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Feedro.ViewModel
{
    public class SearchResultViewModel : Monitorable
    {
        #region Properties

        public Store Store
        {
            get;
            private set;
        }

        private string _queryText;

        public string QueryText
        {
            get
            {
                return _queryText;
            }

            set
            {
                if (value == _queryText)
                    return;

                _queryText = value;

                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Entry> SearchResult
        {
            get;
            private set;
        }

        #endregion

        #region .ctor

        public SearchResultViewModel()
        {
            Store = new Store();
            SearchResult = new ObservableCollection<Entry>();
        }

        #endregion

        public async Task ExecuteSearch()
        {
            await Store.Refresh();

            SearchResult.Clear();

            foreach (Entry entry in Store.Entries.SelectByKeywords(QueryText))
                SearchResult.Add(entry);
        }
    }
}
