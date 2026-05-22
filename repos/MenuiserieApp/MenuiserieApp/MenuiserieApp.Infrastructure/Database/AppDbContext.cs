using MenuiserieApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace MenuiserieApp.Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<LigneCommande> LignesCommande { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Menuiserie.db");
        }
    }
}
