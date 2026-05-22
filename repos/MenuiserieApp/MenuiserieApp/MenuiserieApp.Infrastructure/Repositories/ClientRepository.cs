using MenuiserieApp.Core.Interfaces;
using MenuiserieApp.Core.Models;
using MenuiserieApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace MenuiserieApp.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly AppDbContext _context;

        public ClientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Client>> ObtenirTousLesClientsAsync()
        {
            return await _context.Clients.ToListAsync();
        }

        public async Task<Client?> ObtenirClientParIdAsync(int id)
        {
            return await _context.Clients.FindAsync(id);
        }

        public async Task AjouterClientAsync(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
        }

        public async Task ModifierClientAsync(Client client)
        {
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
        }

        public async Task SupprimerClientAsync(int id)
        {
            var client = await ObtenirClientParIdAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }
        }

    }
}
