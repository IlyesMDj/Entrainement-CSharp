using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Exercice3.Messages;
using Exercice3.Models;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Exercice3
{
    public partial class MainViewModel : ObservableObject, IRecipient<NotificationMessage>
    {
        private readonly ClientsViewModel _clientsViewModel;
        private readonly ProduitsViewModel _produitsViewModel;

        [ObservableProperty]
        private object _vueCourante;

        [ObservableProperty]
        private string _notificationGlobale = "Système prêt";

        public MainViewModel(ClientsViewModel clientsViewModel, ProduitsViewModel produitsViewModel)
        {
            _clientsViewModel = clientsViewModel;
            _produitsViewModel = produitsViewModel;

            WeakReferenceMessenger.Default.RegisterAll(this);

            AfficherPageProduits();
            AfficherPageClients();
        }

        [RelayCommand]
        public void AfficherPageClients()
        {
            VueCourante = _clientsViewModel;
        }

        [RelayCommand]
        public void AfficherPageProduits()
        {
            VueCourante = _produitsViewModel;
        }
        public void Receive(NotificationMessage message)
        {
            NotificationGlobale = message.Value;
        }

    }
}
