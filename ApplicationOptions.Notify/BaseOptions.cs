
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ApplicationOptions
{
    public partial class BaseOptions<T> : INotifyPropertyChanged
    {
        private void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
