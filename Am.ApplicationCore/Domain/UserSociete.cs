using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Am.ApplicationCore.Domain
{
    public class UserSociete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("nom")]
        public string? Nom { get; set; }

        [Column("prenom")]
        public string? Prenom { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("telephone")]
        public string? Telephone { get; set; }

        [Column("adresse")]
        public string? Adresse { get; set; }

        [Column("password")]
        public string? Password { get; set; }

        [Column("active")]
        public bool Active { get; set; }

        [Column("date_affectation")]
        public DateTime DateAffectation { get; set; }

        [Column("role")]
        public RoleSociete Role { get; set; }

        [ForeignKey("Societe")]
        [Column("societe_id")]
        public int SocieteId { get; set; }

        public Societe? Societe { get; set; }
    }
}
