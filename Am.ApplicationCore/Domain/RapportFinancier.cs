using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Am.ApplicationCore.Domain
{
    public class RapportFinancier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("type")]
        public TypeRapport Type { get; set; }

        [Column("annee")]
        public int Annee { get; set; }

        [ForeignKey("Societe")]
        [Column("societe_id")]
        public int SocieteId { get; set; }

        public Societe? Societe { get; set; }

        public ICollection<CategorieFinanciere>? Categories { get; set; }

        public ICollection<CategorieCR>? CategoriesCR { get; set; }

        // One-to-one navigation to CR
        public CR? CR { get; set; }
    }
}