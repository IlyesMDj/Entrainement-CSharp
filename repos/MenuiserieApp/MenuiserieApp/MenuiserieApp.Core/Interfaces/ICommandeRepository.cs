using MenuiserieApp.Core.Models;

namespace MenuiserieApp.Core.Interfaces
{
    public interface ICommandeRepository
    {
        Task<List<Commande>> ObtenirToutesLesCommandesAsync();
        Task<Commande?> ObtenirCommandeAvecLignesAsync(int id);
        Task AjouterCommandeAsync(Commande commande);
        Task ModifierCommandeAsync(Commande commande);
        Task SupprimerCommandeAsync(int id);
    }
}