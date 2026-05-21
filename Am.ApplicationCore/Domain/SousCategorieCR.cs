using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Am.ApplicationCore.Domain
{
    public class SousCategorieCR
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("nom")]
        public string? Nom { get; set; }

        [Column("position")]
        public int Position { get; set; }

        [ForeignKey("CategorieCR")]
        [Column("categorie_cr_id")]
        public int CategorieCrId { get; set; }

        public CategorieCR? CategorieCR { get; set; }

        public ICollection<LigneFinanciere>? Lignes { get; set; }
    }
}
