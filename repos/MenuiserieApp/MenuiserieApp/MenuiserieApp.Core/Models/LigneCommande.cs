using System.ComponentModel.DataAnnotations.Schema;
namespace MenuiserieApp.Core.Models
{
    public class LigneCommande
    {
        public int Id { get; set; }
        public int CommandeId { get; set; }
        public Commande? Commande { get; set; }
        public string? Designation { get; set; }
        public int HauteurMm { get; set; }
        public int LargeurMm { get; set; }
        public string? Couleur { get; set; }
        public int Quantite { get; set; }
        public decimal PrixUnitaire { get; set; }
        public string? CheminImage { get; set; }

        [NotMapped]
        public decimal TotalLigne => PrixUnitaire * Quantite;
    }
}
