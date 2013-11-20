using Feedro.Model.Data;
using Feedro.Model.Net;
using System;
using System.Collections.Generic;
using Windows.Storage;

namespace Feedro.ViewModel
{
    public class GeneralSettingsViewModel : Monitorable
    {
        #region Constants

        private static readonly int[] _daysToKeepOptions = new int[] { 7, 14, 30, 60, 90, 180, 365 };

        #endregion

        #region Properties

        public Store Store
        {
            get;
            private set;
        }

        public IReadOnlyCollection<int> DaysToKeepOptions
        {
            get
            {
                return _daysToKeepOptions;
            }
        }

        public int SelectedDaysToKeep
        {
            get
            {
                return Store.DaysToKeep;
            }

            set
            {
                if (value == Store.DaysToKeep)
                    return;

                Store.DaysToKeep = value;

                NotifyPropertyChanged();
            }
        }

        #endregion

        #region .ctor

        public GeneralSettingsViewModel()
        {
            Store = new Store();
        }

        #endregion
    }
}
