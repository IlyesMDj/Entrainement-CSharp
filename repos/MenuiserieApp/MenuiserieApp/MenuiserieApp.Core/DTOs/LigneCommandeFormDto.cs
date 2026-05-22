using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MenuiserieApp.Core.DTOs
{
    public partial class LigneCommandeFormDto : ObservableValidator
    {
        [ObservableProperty]
        [Required(ErrorMessage = "La désignation est obligatoire.")]
        [MinLength(3, ErrorMessage = "Le nom doit faire au moins 3 caractères.")]
        public partial string Designation { get; set; } = string.Empty;
        public int HauteurMm { get; set; }
        public int LargeurMm { get; set; }
        public string? Couleur { get; set; }
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "La quantité est obligatoire.")]
        [Range(2, 10000, ErrorMessage = "La quantité doit être au minimum de 1.")]
        public partial int Quantite { get; set; }
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Range(0.01, 1000000.0, ErrorMessage = "Le prix doit être supérieur à 0.")]
        public partial decimal PrixUnitaire { get; set; }
        public string? CheminImage { get; set; }

    }
}
