using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Am.ApplicationCore.Domain
{
    public class CR
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("montant_consomme")]
        public double MontantConsomme { get; set; }

        [Column("annee")]
        public int Annee { get; set; }

        [ForeignKey("RapportFinancier")]
        [Column("rapport_financier_id")]
        public int RapportFinancierId { get; set; }

        public RapportFinancier? RapportFinancier { get; set; }

        public ICollection<LigneFinanciere>? Lignes { get; set; }
    }
}
