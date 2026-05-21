using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Am.ApplicationCore.Domain
{
    public class CBDash
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("type")]
        public string? Type { get; set; }

        [Column("category")]
        public string? Category { get; set; }

        [Column("sous_category")]
        public string? SousCategory { get; set; }

        [ForeignKey("Societe")]
        [Column("societe_id")]
        public int SocieteId { get; set; }

        public Societe? Societe { get; set; }
    }
}
