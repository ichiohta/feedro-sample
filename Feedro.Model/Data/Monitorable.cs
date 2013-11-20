using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Feedro.Model.Data
{
    [DataContract]
    public abstract class Monitorable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string name = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
