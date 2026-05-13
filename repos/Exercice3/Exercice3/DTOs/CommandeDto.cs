using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Exercice3.DTOs
{
    public partial class CommandeDto : ObservableObject
    {
        public int Id { get; set; }

        [ObservableProperty]
        private string _description = string.Empty;

        [ObservableProperty]
        private decimal _prix;
    }
}
