using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Exercice3.Models;
using Exercice3.DTOs;

namespace Exercice3.Mapping
{
    public class ClientProfile : Profile
    {
        public ClientProfile() 
        { 
            CreateMap<Client, ClientDto>();
            CreateMap<ClientDto, Client>();

            CreateMap<Commande, CommandeDto>();
            CreateMap<CommandeDto, Commande>();

            CreateMap<Produit, ProduitDto>();
            CreateMap<ProduitDto, Produit>();
        }
    }
    
}

