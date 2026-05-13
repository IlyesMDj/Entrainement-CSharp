using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Exercice3.Models;

public partial class ErptestContext : DbContext
{
    public ErptestContext()
    {
    }

    public ErptestContext(DbContextOptions<ErptestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }
    public DbSet<Commande> Commandes { get; set; }
    public DbSet<Produit> Produits { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
  //      => optionsBuilder.UseNpgsql("Host=localhost;Database=erptest;Username=postgres;Password=Si_2026");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("client_pkey");

            entity.ToTable("client");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Nom)
                .HasMaxLength(100)
                .HasColumnName("nom");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
