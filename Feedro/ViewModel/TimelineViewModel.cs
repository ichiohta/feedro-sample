using Feedro.Model.Data;
using Feedro.Model.Net;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Feedro.ViewModel
{
    public class TimelineViewModel : Monitorable
    {
        #region Properties

        private readonly Store Store;

        public Entry _selectedEntry;

        public Entry SelectedEntry
        {
            get
            {
                return _selectedEntry;
            }
            set
            {
                if (value == _selectedEntry)
                    return;

                _selectedEntry = value;
                NotifyPropertyChanged();

                IsSelected = _selectedEntry != null;
            }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            private set
            {
                if (value == _isSelected)
                    return;

                _isSelected = value;
                NotifyPropertyChanged();
            }
        }

        public ReadOnlyObservableCollection<Entry> Entries
        {
            get
            {
                return Store.Entries;
            }
        }

        private bool _isUpdating;

        public bool IsUpdating
        {
            get
            {
                return _isUpdating;
            }
            private set
            {
                if (value == _isUpdating)
                    return;

                _isUpdating = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region .ctor

        public TimelineViewModel()
        {
            Store = new Store();
        }

        #endregion

        #region Helper methods

        private async Task Update(Task task)
        {
            try
            {
                IsUpdating = true;
                await task;
            }
            finally
            {
                IsUpdating = false;
            }
        }

        #endregion

        #region Public methods

        public async Task Refresh()
        {
            await Update(new SubscriptionUpdater().Update(Store));
        }

        public async Task Load()
        {
            await Update(Store.Refresh());
        }

        public async Task Bookmark()
        {
            if (!IsSelected)
                return;

            SelectedEntry.IsFavorite = !SelectedEntry.IsFavorite;

            Task task = Store.SaveChanges();
            await Update(task);
        }

        #endregion
    }
}
