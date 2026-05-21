using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Am.ApplicationCore.Domain
{
    public class Admin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("username")]
        public string? Username { get; set; }

        [Column("nom")]
        public string? Nom { get; set; }

        [Column("prenom")]
        public string? Prenom { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("adresse")]
        public string? Adresse { get; set; }

        [Column("identifiant")]
        public string? Identifiant { get; set; }

        [Column("password")]
        public string? Password { get; set; }

        [Column("date_creation")]
        public DateTime DateCreation { get; set; }
    }
}