using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MenuiserieApp.Core.DTOs;
using MenuiserieApp.Core.Interfaces;
using MenuiserieApp.Core.Models;
using MenuiserieApp.Core.Traductions;
using System.Collections.ObjectModel;

namespace MenuiserieApp.UI.ViewModels
{
    public partial class CommandeViewModel : ObservableValidator
    {
        private readonly ICommandeRepository _commandeRepo;
        private readonly IClientRepository _clientRepo;

        [ObservableProperty]
        private ObservableCollection<ClientSelectDto> clientsDisponibles = new();

        [ObservableProperty]
        private ClientSelectDto? clientActif;

        [ObservableProperty]
        private LigneCommandeFormDto nouvelleLigne = new();

        [ObservableProperty]
        private ObservableCollection<LigneCommande> panier = new();

        [ObservableProperty]
        private decimal totalCommande;

        [ObservableProperty]
        private bool panierVide = true;

        public CommandeViewModel(ICommandeRepository commandeRepo, IClientRepository clientRepo)
        {
            _commandeRepo = commandeRepo;
            _clientRepo = clientRepo;

            _ = ChargerClientsAsync();
        }

        [RelayCommand]
        private void AjouterLigne()
        {

            var ligneAjt = TraducteurDonnees.VersLigneEntite(NouvelleLigne);

            if (ligneAjt.Quantite == 0)
            {
                ligneAjt.Quantite = 1;
            }

            Panier.Add(ligneAjt);
            PanierVide = false;
            CalculerTotal();

            NouvelleLigne = new LigneCommandeFormDto();
        }

        [RelayCommand]
        private async Task ValiderCommandeAsync()
        {
            if (ClientActif == null || Panier.Count == 0) return;

            var nouvelleCommande = new Commande
            {
                ClientId = ClientActif.Id,
                NumeroReference = "CMD_" + System.DateTime.Now.ToString("yyyyMMdd_HHmm"),
                DateCommande = System.DateTime.Now,
                Statut = "Devis",
                LigneCommandes = new System.Collections.Generic.List<LigneCommande>(Panier)
            };

            await _commandeRepo.AjouterCommandeAsync(nouvelleCommande);

            System.Windows.MessageBox.Show(
            $"La commande {nouvelleCommande.NumeroReference} a bien été enregistrée !",
            "Succès",
            System.Windows.MessageBoxButton.OK,
            System.Windows.MessageBoxImage.Information);

            AnnulerCommande();
        }

        [RelayCommand]
        private void SupprimerLigne(LigneCommande ligne)
        {
            if (ligne != null)
            {
                Panier.Remove(ligne);
                CalculerTotal();

                if (Panier.Count == 0)
                {
                    PanierVide = true;
                }
            }
        }

        [RelayCommand]
        private void AnnulerCommande()
        {
            Panier.Clear();
            CalculerTotal();
            PanierVide = true;
            ClientActif = null;
            NouvelleLigne = new LigneCommandeFormDto();
        }

        [RelayCommand]
        private void RetourClients()
        {
            AnnulerCommande();
        }

        [RelayCommand]
        public async Task ChargerClientsAsync()
        {
            var listeClients = await _clientRepo.ObtenirTousLesClientsAsync();

            ClientsDisponibles.Clear();

            foreach (var client in listeClients)
            {
                ClientsDisponibles.Add(new ClientSelectDto
                {
                    Id = client.Id,
                    NomAffichage = !string.IsNullOrWhiteSpace(client.RaisonSociale) ? client.RaisonSociale : $"{client.Nom} {client.Prenom}"
                });
            }
        }

        [RelayCommand]
        private void AjouterImage(LigneCommande ligne)
        {
            if (ligne == null)
            {
                System.Windows.MessageBox.Show("Erreur : La ligne du tableau n'a pas été reconnue.", "Problème de liaison");
                return;
            }

            var fenetre = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Images|*.jpg;*.jpeg;*.png;*.webp",
                Title = "Choisir une image pour la menuiserie"
            };

            if (fenetre.ShowDialog() == true)
            {
                ligne.CheminImage = fenetre.FileName;

                var index = Panier.IndexOf(ligne);
                if (index >= 0)
                {
                    Panier.RemoveAt(index);
                    Panier.Insert(index, ligne);
                }
            }
        }

        public void CalculerTotal()
        {
            decimal calcul = 0;
            foreach (var ligne in Panier)
            {
                calcul += ligne.PrixUnitaire * ligne.Quantite;
            }
            TotalCommande = calcul;
        }
    }
}
