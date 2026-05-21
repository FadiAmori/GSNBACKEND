using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Am.ApplicationCore.Domain
{
    public class Societe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("nom")]
        public string? Nom { get; set; }

        [Column("adresse")]
        public string? Adresse { get; set; }

        [Column("ville")]
        public string? Ville { get; set; }

        [Column("pays")]
        public string? Pays { get; set; }

        [Column("telephone")]
        public string? Telephone { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("password")]
        public string? Password { get; set; }

        [Column("active")]
        public bool Active { get; set; }

        [Column("date_affectation")]
        public DateTime DateAffectation { get; set; }

        [Column("date_creation")]
        public DateTime DateCreation { get; set; }

        public ICollection<UserSociete>? Users { get; set; }
        public ICollection<ClesDeRepartition>? ClesDeRepartition { get; set; }
        public ICollection<RapportFinancier>? RapportsFinanciers { get; set; }
    }
}