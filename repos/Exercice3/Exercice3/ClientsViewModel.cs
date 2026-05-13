using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Exercice3.DTOs;
using Exercice3.Messages;
using Exercice3.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Exercice3
{
    public partial class ClientsViewModel : ObservableValidator
    {
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(AjouterClientCommand))]
        [Required(ErrorMessage = "Le nom est obligatoire")]
        [MinLength(3, ErrorMessage = "Le nom doit faire au moins 3 caractères")]
        private string _nouveauNom = string.Empty;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(AjouterClientCommand))]
        [Required(ErrorMessage = "L'adresse email est obligatoire")]
        [EmailAddress(ErrorMessage = "L'adresse email n'est pas valide")]
        private string _nouveauMail = string.Empty;

        [ObservableProperty]
        private ObservableCollection<ClientDto> listeClients = new();

        [ObservableProperty]
        private ClientDto _clientSelectionne;

        [ObservableProperty]
        private string _nouvelleDescription = string.Empty;

        [ObservableProperty]
        private decimal _nouveauPrix;

        [ObservableProperty]
        private CommandeDto _commandeSelectionnee;

        [ObservableProperty]
        private string _recherche = string.Empty;

        [ObservableProperty]
        private string _statutMessage = "Prêt";

        [ObservableProperty]
        private bool _estEnChargement = false;

        private readonly ErptestContext _db;
        private readonly IMapper _mapper;

#pragma warning disable CS8618 // Un champ non-nullable doit contenir une valeur autre que Null lors de la fermeture du constructeur. Envisagez d’ajouter le modificateur « required » ou de déclarer le champ comme pouvant accepter la valeur Null.
        public ClientsViewModel(ErptestContext dbContext, IMapper mapper)
#pragma warning restore CS8618 // Un champ non-nullable doit contenir une valeur autre que Null lors de la fermeture du constructeur. Envisagez d’ajouter le modificateur « required » ou de déclarer le champ comme pouvant accepter la valeur Null.
        {
            _db = dbContext;
            _mapper = mapper;

            ValidateAllProperties();
            _ = RafraichirListeAsync();
        }

        [RelayCommand(CanExecute = nameof(PeutAjouterClient))]
        private async Task AjouterClient()
        {
            if (!PeutAjouterClient())
                return;

            var nouveauClient = new Client { Nom = NouveauNom, Email = NouveauMail };
            _db.Add(nouveauClient);
            await _db.SaveChangesAsync();

            WeakReferenceMessenger.Default.Send(new NotificationMessage($"Succès : Le client {NouveauNom} a été ajouté."));

            NouveauNom = string.Empty;
            NouveauMail = string.Empty;

            await RafraichirListeAsync();
        }

        [RelayCommand]
        private async Task SupprimerClients()
        {
            var lignesChoisies = ListeClients.Where(c => c.EstSelectionne && c.Id != 0).ToList();

            if (lignesChoisies.Count == 0)
                return;

            int positionCurseur = ListeClients.IndexOf(lignesChoisies.First());
            var idsASupprimer = lignesChoisies.Select(c => c.Id).ToList();

            var clientsASupprimer = _db.Clients.Where(c => idsASupprimer.Contains(c.Id)).ToList();
            _db.Clients.RemoveRange(clientsASupprimer);
            _db.SaveChanges();

            WeakReferenceMessenger.Default.Send(new NotificationMessage($"Succès : Le client {NouveauNom} a été supprimer."));

            await RafraichirListeAsync();

            if (ListeClients.Count > 0)
            {
                ClientSelectionne = positionCurseur >= ListeClients.Count
                    ? ListeClients[ListeClients.Count - 1]
                    : ListeClients[positionCurseur];
            }
        }

        [RelayCommand]
        private async Task ModifierClient()
        {
            if (ClientSelectionne != null)
            {
                var vraiClient = await _db.Clients.FindAsync(ClientSelectionne.Id);

                if (vraiClient != null)
                {
                    _mapper.Map(ClientSelectionne, vraiClient);

                    await _db.SaveChangesAsync();

                    WeakReferenceMessenger.Default.Send(new NotificationMessage($"Succès : Le client {NouveauNom} a été modifier."));
                }
            }

            await RafraichirListeAsync();
        }

        [RelayCommand]
        private async Task AjouterCommande()
        {
            if (ClientSelectionne == null || string.IsNullOrWhiteSpace(NouvelleDescription))
                return;

            var nouvelleCommande = new Commande
            {
                Description = NouvelleDescription,
                Prix = NouveauPrix,
                ClientId = ClientSelectionne.Id
            };

            _db.Commandes.Add(nouvelleCommande);
            await _db.SaveChangesAsync();

            NouvelleDescription = string.Empty;
            NouveauPrix = 0;

            await RafraichirListeAsync();
        }

        [RelayCommand]
        private async Task SupprimerCommande()
        {
            if (CommandeSelectionnee == null || ClientSelectionne == null)
                return;

            var vraieCommande = await _db.Commandes.FindAsync(CommandeSelectionnee.Id);
            if (vraieCommande != null)
            {
                _db.Commandes.Remove(vraieCommande);
                await _db.SaveChangesAsync();
            }

            await RafraichirListeAsync();
        }

        [RelayCommand]
        private async Task LancerAnalyse()
        {
            EstEnChargement = true;
            StatutMessage = "Extraction en cours...";
            await Task.Delay(4000);
            StatutMessage = "Terminé !";
            EstEnChargement = false;
        }

        [RelayCommand]
        private void CrashTest()
        {
            int a = 10;
            int b = 0;
            _ = a / b;
        }

        private readonly SemaphoreSlim _verrouBaseDeDonnees = new SemaphoreSlim(1, 1);

        private async Task RafraichirListeAsync()
        {
            await _verrouBaseDeDonnees.WaitAsync();

            try
            {
                var requete = _db.Clients.Include(c => c.Commandes).AsQueryable();

                if (!string.IsNullOrWhiteSpace(Recherche))
                {
                    requete = requete.Where(c => c.Nom.ToLower().Contains(Recherche.ToLower()));
                }

                var data = await requete.ToListAsync();

                var dataTraduite = _mapper.Map<List<ClientDto>>(data);
                ListeClients = new ObservableCollection<ClientDto>(dataTraduite);
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

        partial void OnRechercheChanged(string value)
        {
            _ = RafraichirListeAsync();
        }
        

        private bool PeutAjouterClient()
        {
            return !HasErrors
        && !string.IsNullOrWhiteSpace(NouveauNom)
        && !string.IsNullOrWhiteSpace(NouveauMail);
        }
    }
}
