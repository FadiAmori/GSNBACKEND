using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Am.ApplicationCore.Domain
{
    public class ExcelLigneCalculee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("variable")]
        public string? Variable { get; set; }

        [ForeignKey("LigneCalculee")]
        [Column("ligne_calculee_id")]
        public int LigneCalculeeId { get; set; }

        public LigneCalculee? LigneCalculee { get; set; }

        [ForeignKey("Societe")]
        [Column("societe_id")]
        public int SocieteId { get; set; }

        public Societe? Societe { get; set; }
    }
}
