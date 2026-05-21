using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Am.ApplicationCore.Domain
{
    public class CountDash
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("nom_entity")]
        public string? NomEntity { get; set; }

        [Column("couleur")]
        public string? Couleur { get; set; }

        [ForeignKey("Societe")]
        [Column("societe_id")]
        public int SocieteId { get; set; }

        public Societe? Societe { get; set; }
    }
}
