using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exercice3.DTOs
{
    public partial class ProduitDto : ObservableObject
    {
        public int Id { get; set; }

        [ObservableProperty]
        private string nom = string.Empty;

        [ObservableProperty]
        private double prix;

        [ObservableProperty]
        private int quantite;
    }
}
