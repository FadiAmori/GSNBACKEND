using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Am.ApplicationCore.Domain
{
    public class ClesDeRepartition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("mois")]
        public string? Mois { get; set; }

        [Column("saisonalite_ca")]
        public double SaisonaliteCA { get; set; }

        [Column("saisonalite_poids")]
        public double SaisonalitePoids { get; set; }

        [Column("cles_cout_fixes")]
        public double ClesCoutFixes { get; set; }

        [ForeignKey("Societe")]
        [Column("societe_id")]
        public int SocieteId { get; set; }

        public Societe? Societe { get; set; }
    }
}
