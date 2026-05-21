using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Am.ApplicationCore.Domain;

namespace Am.Infrastructure
{
    public class AmContext : DbContext
    {
        public AmContext(DbContextOptions<AmContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure one-to-one relation between LigneFinanciere and ExcelVariable
            modelBuilder.Entity<ExcelVariable>()
                .HasIndex(e => e.LigneFinanciereId)
                .IsUnique();

            modelBuilder.Entity<LigneFinanciere>()
                .HasOne(l => l.ExcelVariable)
                .WithOne(e => e.LigneFinanciere)
                .HasForeignKey<ExcelVariable>(e => e.LigneFinanciereId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure one-to-one relation between LigneCalculee and ExcelLigneCalculee
            modelBuilder.Entity<ExcelLigneCalculee>()
                .HasIndex(e => e.LigneCalculeeId)
                .IsUnique();

            modelBuilder.Entity<LigneCalculee>()
                .HasOne(l => l.ExcelLigneCalculee)
                .WithOne(e => e.LigneCalculee)
                .HasForeignKey<ExcelLigneCalculee>(e => e.LigneCalculeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure one-to-one relation between RapportFinancier and CR
            modelBuilder.Entity<RapportFinancier>()
                .HasOne(r => r.CR)
                .WithOne(c => c.RapportFinancier)
                .HasForeignKey<CR>(c => c.RapportFinancierId)
                .OnDelete(DeleteBehavior.Cascade);

            // Set cascade delete for all relationships by default
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Cascade;
            }

            base.OnModelCreating(modelBuilder);
        }



        public DbSet<Admin> Admins { get; set; } = null!;
        public DbSet<Societe> Societes { get; set; } = null!;
        public DbSet<UserSociete> UserSocietes { get; set; } = null!;
        public DbSet<ClesDeRepartition> ClesDeRepartitions { get; set; } = null!;
        public DbSet<RapportFinancier> RapportsFinanciers { get; set; } = null!;
        public DbSet<CategorieFinanciere> CategoriesFinancieres { get; set; } = null!;
        public DbSet<SousCategorieFinanciere> SousCategoriesFinancieres { get; set; } = null!;
        public DbSet<CategorieCR> CategoriesCR { get; set; } = null!;
        public DbSet<SousCategorieCR> SousCategoriesCR { get; set; } = null!;
        public DbSet<ExcelVariable> ExcelVariables { get; set; } = null!;
        public DbSet<ExcelLigneCalculee> ExcelLigneCalculees { get; set; } = null!;
        public DbSet<CBDash> CBDashes { get; set; } = null!;
        public DbSet<CountDash> CountDashes { get; set; } = null!;
        public DbSet<CourbDash> CourbDashes { get; set; } = null!;
        public DbSet<LigneCalculee> LignesCalculees { get; set; } = null!;
        public DbSet<LigneFinanciere> LignesFinancieres { get; set; } = null!;
        public DbSet<CR> CRs { get; set; } = null!;
        public DbSet<TypeClient> TypeClients { get; set; } = null!;
        public DbSet<FamilleProduit> FamillesProduit { get; set; } = null!;
        public DbSet<Produit> Produits { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var conn = Environment.GetEnvironmentVariable("AM_CONNECTION_STRING")
                           ?? "Host=localhost;Port=5432;Database=Budget;Username=postgres;Password=12345678";

                optionsBuilder.UseNpgsql(conn);
            }

            base.OnConfiguring(optionsBuilder);

        }
    }

}
