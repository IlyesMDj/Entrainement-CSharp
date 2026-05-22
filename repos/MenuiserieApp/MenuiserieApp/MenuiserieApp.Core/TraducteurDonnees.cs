using MenuiserieApp.Core.DTOs;
using MenuiserieApp.Core.Models;

namespace MenuiserieApp.Core.Traductions
{
    public static class TraducteurDonnees
    {
        public static Client VersClientEntite(ClientFormDto dto)
        {
            return new Client
            {
                RaisonSociale = dto.RaisonSociale,
                Nom = dto.Nom,
                Prenom = dto.Prenom,
                Telephone = dto.Telephone,
                Email = dto.Email,
                Adresse = dto.Adresse,
                DateCreation = DateTime.Now
            };
        }

        public static LigneCommande VersLigneEntite(LigneCommandeFormDto dto)
        {
            return new LigneCommande
            {
                Designation = dto.Designation,
                Couleur = dto.Couleur,
                HauteurMm = dto.HauteurMm,
                LargeurMm = dto.LargeurMm,
                Quantite = dto.Quantite,
                PrixUnitaire = dto.PrixUnitaire,
                CheminImage = dto.CheminImage
            };
        }
    }
}
