using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Am.ApplicationCore.Domain
{
    public class SousCategorieFinanciere
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("nom")]
        public string? Nom { get; set; }

        [Column("position")]
        public int Position { get; set; }

        [ForeignKey("CategorieFinanciere")]
        [Column("categorie_financiere_id")]
        public int CategorieFinanciereId { get; set; }

        public CategorieFinanciere? CategorieFinanciere { get; set; }

        public ICollection<LigneFinanciere>? Lignes { get; set; }

        [Column("couleur")]
        public string? Couleur { get; set; }
    }
}