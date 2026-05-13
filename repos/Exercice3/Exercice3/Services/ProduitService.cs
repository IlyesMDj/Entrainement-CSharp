using Exercice3.Models;
using Microsoft.EntityFrameworkCore;

namespace Exercice3.Services
{
    public class ProduitService : IProduitService
    {
        private readonly ErptestContext _db;

        public ProduitService(ErptestContext db)
        {
            _db = db;
        }

        public async Task<List<Produit>> ObtenirTousLesProduitsAsync()
        {
            return await _db.Produits.ToListAsync();
        }

        public async Task<Produit?> ObtenirProduitParIdAsync(int id)
        {
            return await _db.Produits.FindAsync(id);
        }

        public async Task SauvegardeChangementsAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task AjouterProduitAsync(Produit produit)
        {
            _db.Add(produit);
            await _db.SaveChangesAsync();
        }

        public async Task SupprimerProduitAsync(Produit produit)
        {
            _db.Produits.Remove(produit);
            await _db.SaveChangesAsync();
        }
        public async Task<List<Produit>> RechercherProduitsAsync(string motCle)
        {
            if (string.IsNullOrEmpty(motCle))
            {
                return await ObtenirTousLesProduitsAsync();
            }

            return await _db.Produits
        .FromSqlRaw("SELECT * FROM Produits WHERE Id IN (SELECT rowid FROM ProduitsIndex WHERE ProduitsIndex MATCH {0})", motCle + "*")
        .ToListAsync();
        }
    }
}
