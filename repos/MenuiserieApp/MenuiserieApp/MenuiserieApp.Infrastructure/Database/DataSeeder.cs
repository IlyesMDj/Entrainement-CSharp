using MenuiserieApp.Core.Models;
using MenuiserieApp.Infrastructure.Database;

namespace MenuiserieApp.Infrastructure.Data
{
    public static class DataSeeder
    {
        public static void InitialiserDonnees(AppDbContext context)
        {
            if (context.Clients.Any()) return;


            var client1 = new Client
            {
                Nom = "Dupont",
                Prenom = "Jean",
                RaisonSociale = "BâtiNormand SAS",
                Telephone = "0235412596",
                Email = "contact@batinormand.fr",
                Adresse = "45 Rue de la République, 76600 Le Havre"
            };

            var client2 = new Client
            {
                Nom = "Martin",
                Prenom = "Sophie",
                RaisonSociale = "",
                Telephone = "0612345678",
                Email = "sophie.martin@gmail.com",
                Adresse = "12 Avenue Foch, 76600 Le Havre"
            };

            var client3 = new Client
            {
                Nom = "Lefebvre",
                Prenom = "Thomas",
                RaisonSociale = "Menuiserie Harfleurise",
                Telephone = "0235221100",
                Email = "t.lefebvre@harfleurpvc.com",
                Adresse = "8 ZAC des Prés, 76700 Harfleur"
            };

            context.Clients.AddRange(client1, client2, client3);
            context.SaveChanges();

            var commandes = new List<Commande>
            {
                new Commande
                {
                    ClientId = client1.Id,
                    NumeroReference = "CMD_20260510_001",
                    DateCommande = new DateTime(2026, 05, 10, 14, 30, 0),
                    Statut = "Devis",
                    LigneCommandes = new List<LigneCommande>
                    {
                        new LigneCommande { Designation = "Fenêtre PVC 2 vantaux", HauteurMm = 1250, LargeurMm = 1000, Quantite = 4, PrixUnitaire = 280.00m, Couleur = "Blanc" },
                        new LigneCommande { Designation = "Porte-fenêtre PVC Oscar", HauteurMm = 2150, LargeurMm = 1400, Quantite = 1, PrixUnitaire = 650.00m, Couleur = "Gris Anthracite" }
                    }
                },

                new Commande
                {
                    ClientId = client2.Id,
                    NumeroReference = "CMD_20260512_002",
                    DateCommande = new DateTime(2026, 05, 12, 09, 15, 0),
                    Statut = "En production",
                    LigneCommandes = new List<LigneCommande>
                    {
                        new LigneCommande { Designation = "Soupirail PVC Standard", HauteurMm = 450, LargeurMm = 600, Quantite = 2, PrixUnitaire = 110.00m, Couleur = "Blanc" }
                    }
                },

                new Commande
                {
                    ClientId = client3.Id,
                    NumeroReference = "CMD_20260428_001",
                    DateCommande = new DateTime(2026, 04, 28, 16, 45, 0),
                    Statut = "Terminé",
                    LigneCommandes = new List<LigneCommande>
                    {
                        new LigneCommande { Designation = "Baie Vitrée Coulissante PVC", HauteurMm = 2150, LargeurMm = 2400, Quantite = 1, PrixUnitaire = 1200.00m, Couleur = "Gris Anthracite" },
                        new LigneCommande { Designation = "Fenêtre Châssis Fixe", HauteurMm = 600, LargeurMm = 600, Quantite = 3, PrixUnitaire = 150.00m, Couleur = "Blanc" }
                    }
                }
            };

            context.Commandes.AddRange(commandes);
            context.SaveChanges();
        }
    }
}