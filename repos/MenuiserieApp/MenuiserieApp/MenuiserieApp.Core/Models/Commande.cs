namespace MenuiserieApp.Core.Models
{
    public class Commande
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public Client? Client { get; set; }
        public string? NumeroReference { get; set; }
        public DateTime DateCommande { get; set; }
        public string? Statut { get; set; }
        public required List<LigneCommande> LigneCommandes { get; set; }

    }
}
