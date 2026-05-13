using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exercice3.Models;

public partial class Client
{
    public int Id { get; set; }

    public string Nom { get; set; } = null!;

    public string? Email { get; set; }

    [NotMapped]
    public bool EstSelectionne { get; set; }

    public List<Commande> Commandes { get; set; } = new List<Commande>();
}
