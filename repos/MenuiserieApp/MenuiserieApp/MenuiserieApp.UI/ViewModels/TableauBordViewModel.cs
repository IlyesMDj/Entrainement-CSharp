using CommunityToolkit.Mvvm.ComponentModel;
using MenuiserieApp.Core.Interfaces;
using MenuiserieApp.Core.Models;
using System.Collections.ObjectModel;

namespace MenuiserieApp.UI.ViewModels
{
    public partial class TableauBordViewModel : ObservableObject
    {
        private readonly ICommandeRepository _commandeRepo;

        [ObservableProperty]
        private decimal chiffreAffairesMois;

        [ObservableProperty]
        private int nombreCommandesMois;

        [ObservableProperty]
        private ObservableCollection<Commande> dernieresCommandes = new();

        public TableauBordViewModel(ICommandeRepository commandeRepo)
        {
            _commandeRepo = commandeRepo;
            _ = ChargerStatistiquesAsync();
        }

        public async Task ChargerStatistiquesAsync()
        {
            var toutesLesCommandes = await _commandeRepo.ObtenirToutesLesCommandesAsync();
            var moisActuel = DateTime.Now.Month;
            var anneeActuelle = DateTime.Now.Year;

            var commandeDuMois = toutesLesCommandes.Where(c => c.DateCommande.Month == moisActuel && c.DateCommande.Year == anneeActuelle).ToList();

            NombreCommandesMois = commandeDuMois.Count;

            ChiffreAffairesMois = commandeDuMois.Where(cmd => cmd.LigneCommandes != null).SelectMany(cmd => cmd.LigneCommandes).Sum(ligne => ligne.TotalLigne);

            var top5 = toutesLesCommandes.OrderByDescending(c => c.DateCommande).Take(5);

            DernieresCommandes = new ObservableCollection<Commande>(top5);
        }
    }
}
