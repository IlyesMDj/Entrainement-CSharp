using MenuiserieApp.Core.Models;

namespace MenuiserieApp.Core.Interfaces
{
    public interface IClientRepository
    {
        Task<List<Client>> ObtenirTousLesClientsAsync();
        Task<Client?> ObtenirClientParIdAsync(int id);
        Task AjouterClientAsync(Client client);
        Task ModifierClientAsync(Client client);
        Task SupprimerClientAsync(int id);
    }
}
