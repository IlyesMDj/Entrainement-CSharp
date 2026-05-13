using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Exercice1
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _nomComplet;
        
        public string NomComplet
        {
            get { return _nomComplet; }

            set {
                _nomComplet = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
