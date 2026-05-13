using System;
using System.Collections.Generic;
using System.Text;

namespace Exercice3.Models
{
    public class Produit
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public double Prix { get; set; }
        public int Quantite { get; set; }
    }
}
