using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Exercice3.DTOs;
using Exercice3.Models;
using AutoMapper;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.Messaging;
using Exercice3.Messages;
using Exercice3.Services;

namespace Exercice3
{
    public partial class ProduitsViewModel : ObservableValidator
    {
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(AjouterProduitCommand))]
        [Required(ErrorMessage = "Le nom du produit est obligatoire")]
        private string _nouveauNom = string.Empty;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(AjouterProduitCommand))]
        [Range(0.01, 100000, ErrorMessage = "Le prix doit être supérieur à 0")]
        private double _nouveauPrix;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(AjouterProduitCommand))]
        [Range(0, 10000, ErrorMessage = "La quantité ne peut pas être négative")]
        private int _nouvelleQuantite;

        [ObservableProperty]
        private ObservableCollection<ProduitDto> _listeProduits;

        [ObservableProperty]
        private ProduitDto? _produitSelectionne;

        [ObservableProperty]
        private string _texteRecherche = string.Empty;

        private readonly IProduitService _produitService; 
        private readonly IMapper _mapper;

        private readonly SemaphoreSlim _verrouBaseDeDonnees = new SemaphoreSlim(1, 1);


        public ProduitsViewModel(IProduitService produitService, IMapper mapper)
        {
            _produitService = produitService;
            _mapper = mapper;

            ValidateAllProperties();
            _ = RafraichirListeAsync(); 
        }

        [RelayCommand(CanExecute = nameof(PeutAjouterProduit))]
        private async Task AjouterProduit()
        {
            await Task.Delay(2000);
            if (!PeutAjouterProduit())
                return;

            var nouveauProduit = new Produit { Nom = NouveauNom, Prix = NouveauPrix, Quantite = NouvelleQuantite };

            await _produitService.AjouterProduitAsync(nouveauProduit);

            WeakReferenceMessenger.Default.Send(new NotificationMessage($"Succès : Le produit {NouveauNom} a été ajouté."));

            NouveauNom = string.Empty;
            NouveauPrix = 0;
            NouvelleQuantite = 0;

            await RafraichirListeAsync();
        }

        [RelayCommand]
        private async Task SupprimerProduit()
        {
            if (ProduitSelectionne != null)
            {
                var produitASupprimer = await _produitService.ObtenirProduitParIdAsync(ProduitSelectionne.Id);

                if (produitASupprimer != null)
                {
                    await _produitService.SupprimerProduitAsync(produitASupprimer);

                    WeakReferenceMessenger.Default.Send(new NotificationMessage($"Succès : Le produit {NouveauNom} a été supprimé."));
                }
            }
            await RafraichirListeAsync();
        }

        [RelayCommand]
        private async Task ModifierProduit()
        {
            if (ProduitSelectionne != null)
            {
                var vraiProduit = await _produitService.ObtenirProduitParIdAsync(ProduitSelectionne.Id);

                if (vraiProduit != null)
                {
                    _mapper.Map(ProduitSelectionne, vraiProduit);
                    await _produitService.SauvegardeChangementsAsync();

                    WeakReferenceMessenger.Default.Send(new NotificationMessage($"Succès : Le produit {NouveauNom} a été modifier."));
                }
            }
            await RafraichirListeAsync();
        }

        partial void OnTexteRechercheChanged(string value)
        {
            _ = ExecuterRechercheAsync();
        }

        private async Task ExecuterRechercheAsync()
        {
            var data = await _produitService.RechercherProduitsAsync(TexteRecherche);

            var dataTraduite = _mapper.Map<List<ProduitDto>>(data);
            ListeProduits = new ObservableCollection<ProduitDto>(dataTraduite);
        }

        private bool PeutAjouterProduit()
        {
            return !HasErrors && !string.IsNullOrWhiteSpace(NouveauNom);
        }

        private async Task RafraichirListeAsync()
        {
            await _verrouBaseDeDonnees.WaitAsync();

            try
            {
                var data = await _produitService.ObtenirTousLesProduitsAsync();

                var dataTraduite = _mapper.Map<List<ProduitDto>>(data);

                ListeProduits = new ObservableCollection<ProduitDto>(dataTraduite);
            }
            catch (Exception erreur)
            {
                System.Windows.MessageBox.Show($"Un problème est survenu : {erreur.Message}");
            }
            finally
            {
                _verrouBaseDeDonnees.Release();
            }
        }
    }
}
