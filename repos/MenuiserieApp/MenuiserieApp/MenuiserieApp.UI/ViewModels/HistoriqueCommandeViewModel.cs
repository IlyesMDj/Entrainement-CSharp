using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MenuiserieApp.Core.Interfaces;
using MenuiserieApp.Core.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace MenuiserieApp.UI.ViewModels
{
    public partial class HistoriqueCommandeViewModel : ObservableObject
    {
        private readonly ICommandeRepository _commandeRepo;

        [ObservableProperty]
        private ObservableCollection<Commande> listeCommandes = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EnregistrerModificationsCommand))]
        private Commande? commandeSelectionnee;

        [ObservableProperty]
        private decimal totalCommande;

        public HistoriqueCommandeViewModel(ICommandeRepository commandeRepo)
        {
            _commandeRepo = commandeRepo;
            _ = ChargerCommandesAsync();
        }

        [RelayCommand]
        public async Task ChargerCommandesAsync()
        {
            var commandes = await _commandeRepo.ObtenirToutesLesCommandesAsync();
            ListeCommandes.Clear();

            foreach (var commande in commandes)
            {
                ListeCommandes.Add(commande);
            }
        }

        [RelayCommand]
        public async Task AjouterLigneHistorique()
        {
            if (CommandeSelectionnee != null)
            {
                var nouvelleLigne = new LigneCommande
                {
                    Designation = "Nouveau produit",
                    HauteurMm = 0,
                    LargeurMm = 0,
                    Quantite = 1,
                    PrixUnitaire = 0,
                    Couleur = "Blanc"
                };

                CommandeSelectionnee.LigneCommandes.Add(nouvelleLigne);

                var temporaire = CommandeSelectionnee;
                CommandeSelectionnee = null;
                CommandeSelectionnee = temporaire;
            }
        }

        [RelayCommand]
        public async Task SupprimerCommandeAsync()
        {
            if (CommandeSelectionnee == null) return;

            var resultat = MessageBox.Show($"Êtes-vous sûr de vouloir supprimer la commande {CommandeSelectionnee.NumeroReference} ?",
                "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (resultat == MessageBoxResult.Yes)
            {
                await _commandeRepo.SupprimerCommandeAsync(CommandeSelectionnee.Id);
                CommandeSelectionnee = null;
                await ChargerCommandesAsync();
            }

        }

        [RelayCommand]
        private void SupprimerLigne(LigneCommande ligne)
        {
            if (CommandeSelectionnee != null && ligne != null)
            {
                CommandeSelectionnee.LigneCommandes.Remove(ligne);

                var temporaire = CommandeSelectionnee;
                CommandeSelectionnee = null;
                CommandeSelectionnee = temporaire;
            }
        }

        [RelayCommand(CanExecute = nameof(PeutEnregistrer))]
        private async Task EnregistrerModificationsAsync()
        {
            if (CommandeSelectionnee == null) return;

            await _commandeRepo.ModifierCommandeAsync(CommandeSelectionnee);

            MessageBox.Show("Modifications enregistrées avec succès !", "Succès",
                MessageBoxButton.OK, MessageBoxImage.Information);

            await ChargerCommandesAsync();
        }

        [RelayCommand]
        private void GenererDocument()
        {
            if (CommandeSelectionnee != null)
            {
                MenuiserieApp.Services.GenerateurPdf.CreerFacture(CommandeSelectionnee);
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

                var temporaire = CommandeSelectionnee;
                CommandeSelectionnee = null;
                CommandeSelectionnee = temporaire;
            }
        }

        public System.Collections.Generic.List<string> StatutsDisponibles { get; } = new()
        {
            "Devis",
            "En production",
            "Terminé",
            "Livré"
        };

        private bool PeutEnregistrer() => CommandeSelectionnee != null;

        public void CalculerTotal()
        {
            if (CommandeSelectionnee == null || CommandeSelectionnee.LigneCommandes == null)
            {
                TotalCommande = 0;
                return;
            }

            decimal calcul = 0;
            foreach (var ligne in CommandeSelectionnee.LigneCommandes)
            {
                calcul += ligne.PrixUnitaire * ligne.Quantite;
            }

            TotalCommande = calcul;
        }

        partial void OnCommandeSelectionneeChanged(Commande? value)
        {
            CalculerTotal();
        }
    }
}