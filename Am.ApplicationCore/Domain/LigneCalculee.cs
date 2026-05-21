using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Am.ApplicationCore.Domain
{
    public class LigneCalculee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("nom")]
        public string? Nom { get; set; }

        // Expression stored as text (e.g. "(MS_Direct + MS_Support) * 0.1")
        [Column("expression")]
        public string? Expression { get; set; }

        [Column("position")]
        public int Position { get; set; }

        [Column("date_creation")]
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;

        [ForeignKey("Societe")]
        [Column("societe_id")]
        public int SocieteId { get; set; }

        public Societe? Societe { get; set; }

        [ForeignKey("RapportFinancier")]
        [Column("rapport_financier_id")]
        public int? RapportFinancierId { get; set; }

        public RapportFinancier? RapportFinancier { get; set; }

        [Column("resultat")]
        public double? Resultat { get; set; }

        [Column("couleur")]
        public string? Couleur { get; set; }

        public ExcelLigneCalculee? ExcelLigneCalculee { get; set; }
    }
}
