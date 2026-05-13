using Exercice3.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exercice3.Services
{
    public interface IProduitService
    {
        Task<List<Produit>> ObtenirTousLesProduitsAsync();
        Task<Produit?> ObtenirProduitParIdAsync(int id);
        Task SauvegardeChangementsAsync();
        Task AjouterProduitAsync(Produit produit);
        Task SupprimerProduitAsync(Produit produit);
        Task<List<Produit>> RechercherProduitsAsync(string motCle);
    }
}
