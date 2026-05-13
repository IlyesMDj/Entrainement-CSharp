using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Exercice2
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Tache> ListeTaches { get; set; }

        private string _nomNouvelleTache;


        public string NomNouvelleTache
        {
            get {  return _nomNouvelleTache;}
            set
            {
                _nomNouvelleTache = value;
                OnPropertyChanged(); 
            }
        }

        public ICommand AjouterTacheCommand { get; set; }

        public MainViewModel()
        {
            ListeTaches = new ObservableCollection<Tache>();

            AjouterTacheCommand = new RelayCommand(AjouterTache);

        }

        private void AjouterTache()
        {
            if (!string.IsNullOrWhiteSpace(NomNouvelleTache))
            {
                ListeTaches.Add(new Tache { Nom = NomNouvelleTache, EstTerminee = false });

                NomNouvelleTache = "";
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
