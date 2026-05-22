using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MenuiserieApp.Core.DTOs;
using MenuiserieApp.Core.Interfaces;
using MenuiserieApp.Core.Models;
using MenuiserieApp.Core.Traductions;
using System.Collections.ObjectModel;
using System.Windows;

namespace MenuiserieApp.UI.ViewModels
{
    public partial class ClientViewModel : ObservableValidator
    {
        private readonly IClientRepository _clientRepo;

        [ObservableProperty]
        private ObservableCollection<Client> listeClients = new();

        [ObservableProperty]
        private ClientFormDto nouveauClient = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EnregistrerModificationsCommand))]
        private Client? clientSelectionne;

        public ClientViewModel(IClientRepository clientRepo)
        {
            _clientRepo = clientRepo;

            _ = ChargerTousLesClientsAsync();
        }

        [RelayCommand]
        private async Task ChargerTousLesClientsAsync()
        {
            var clients = await _clientRepo.ObtenirTousLesClientsAsync();

            ListeClients.Clear();
            foreach (var client in clients)
            {
                ListeClients.Add(client);
            }
        }

        [RelayCommand]
        private async Task EnregistrerClientAsync()
        {
            if (string.IsNullOrWhiteSpace(NouveauClient.Nom)) return;

            var entiteClient = TraducteurDonnees.VersClientEntite(NouveauClient);

            await _clientRepo.AjouterClientAsync(entiteClient);

            await ChargerTousLesClientsAsync();

            NouveauClient = new ClientFormDto();
        }

        [RelayCommand]
        private async Task SupprimerClientAsync(int id)
        {
            await _clientRepo.SupprimerClientAsync(id);
            await ChargerTousLesClientsAsync();
        }

        [RelayCommand(CanExecute = nameof(PeutEnregistrer))]
        private async Task EnregistrerModificationsAsync()
        {
            if (ClientSelectionne == null) return;

            await _clientRepo.ModifierClientAsync(ClientSelectionne);

            MessageBox.Show("Informations du client mises à jour avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

            await ChargerTousLesClientsAsync();
        }

        private bool PeutEnregistrer() => ClientSelectionne != null;
    }
}
