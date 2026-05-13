using System;
using System.Collections.Generic;
using System.Text;

namespace Exercice3.Models
{
    public class Commande
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Prix { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}
