using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Am.ApplicationCore.Domain
{
    public class CourbDash
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("category")]
        public string? Category { get; set; }

        [Column("sous_category")]
        public string? SousCategory { get; set; }

        [Column("rapport1")]
        public string? Rapport1 { get; set; }

        [Column("rapport2")]
        public string? Rapport2 { get; set; }

        [ForeignKey("Societe")]
        [Column("societe_id")]
        public int SocieteId { get; set; }

        public Societe? Societe { get; set; }
    }
}
