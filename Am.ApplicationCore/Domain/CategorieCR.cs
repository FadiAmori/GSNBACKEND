using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Am.ApplicationCore.Domain
{
    public class CategorieCR
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("nom")]
        public string? Nom { get; set; }

        [Column("position")]
        public int Position { get; set; }

        [ForeignKey("RapportFinancier")]
        [Column("rapport_financier_id")]
        public int RapportFinancierId { get; set; }

        public RapportFinancier? RapportFinancier { get; set; }

        public ICollection<SousCategorieCR>? SousCategories { get; set; }
    }
}
