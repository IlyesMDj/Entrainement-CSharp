using CommunityToolkit.Mvvm.ComponentModel;
using Exercice3.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Exercice3.DTOs
{
    public partial class ClientDto : ObservableObject
    {
        public int Id { get; set; }

        [ObservableProperty]
        public string _nom = string.Empty;

        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private bool _estSelectionne;

        public ObservableCollection<CommandeDto> Commandes { get; set; } = new();

        public decimal TotalAchats
        {
            get
            {
                decimal total = 0;
                foreach (var achat in Commandes)
                {
                    total += achat.Prix;
                }
                return total;
            }
        }
    }
}
