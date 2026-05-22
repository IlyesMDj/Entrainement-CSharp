using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MenuiserieApp.Core.DTOs
{
    public partial class ClientFormDto : ObservableValidator
    {
        public string? RaisonSociale { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string? Prenom { get; set; }
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Phone(ErrorMessage = "Le numéro de téléphone n'est pas valide.")]
        public partial string Telephone { get; set; } = string.Empty;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "L'adresse email est obligatoire")]
        [EmailAddress(ErrorMessage = "L'adresse email n'est pas valide")]
        public partial string Email { get; set; } = string.Empty;
        public string Adresse { get; set; } = string.Empty;
    }
}
