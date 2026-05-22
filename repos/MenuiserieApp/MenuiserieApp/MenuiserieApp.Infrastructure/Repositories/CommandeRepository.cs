using MenuiserieApp.Core.Interfaces;
using MenuiserieApp.Core.Models;
using MenuiserieApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace MenuiserieApp.Infrastructure.Repositories
{
    public class CommandeRepository : ICommandeRepository
    {
        private readonly AppDbContext _context;

        public CommandeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Commande>> ObtenirToutesLesCommandesAsync()
        {
            return await _context.Commandes.Include(c => c.LigneCommandes).Include(c => c.Client).ToListAsync();
        }

        public async Task<Commande?> ObtenirCommandeAvecLignesAsync(int id)
        {
            return await _context.Commandes.Include(c => c.Client).Include(c => c.LigneCommandes).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AjouterCommandeAsync(Commande commande)
        {
            _context.Commandes.Add(commande);
            await _context.SaveChangesAsync();
        }

        public async Task ModifierCommandeAsync(Commande commande)
        {
            _context.Commandes.Update(commande);
            await _context.SaveChangesAsync();
        }

        public async Task SupprimerCommandeAsync(int id)
        {
            var commande = await _context.Commandes.FindAsync(id);
            if (commande != null)
            {
                _context.Commandes.Remove(commande);
                await _context.SaveChangesAsync();
            }
        }
    }
}
