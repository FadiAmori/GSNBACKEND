using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Am.ApplicationCore.Domain
{
    public class LigneFinanciere
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("nom")]
        public string? Nom { get; set; }

        [Column("unite")]
        public string? Unite { get; set; }

        [Column("montant")]
        public double Montant { get; set; }

        [Column("position")]
        public int Position { get; set; }

        [Column("mois")]
        public int Mois { get; set; }

        [Column("annee")]
        public int Annee { get; set; }

        [ForeignKey("SousCategorieFinanciere")]
        [Column("sous_categorie_financiere_id")]
        public int SousCategorieFinanciereId { get; set; }

        public SousCategorieFinanciere? SousCategorieFinanciere { get; set; }

        [ForeignKey("Cr")]
        [Column("cr_id")]
        public int? CrId { get; set; }

        public CR? Cr { get; set; }

        [ForeignKey("SousCategorieCR")]
        [Column("sous_categorie_cr_id")]
        public int? SousCategorieCrId { get; set; }

        public SousCategorieCR? SousCategorieCR { get; set; }

        public ExcelVariable? ExcelVariable { get; set; }

        [Column("couleur")]
        public string? Couleur { get; set; }
    }
}
