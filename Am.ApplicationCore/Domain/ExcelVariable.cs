using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Am.ApplicationCore.Domain
{
    public class ExcelVariable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("code")]
        public string? Code { get; set; }

        [ForeignKey("LigneFinanciere")]
        [Column("ligne_financiere_id")]
        public int LigneFinanciereId { get; set; }

        public LigneFinanciere? LigneFinanciere { get; set; }
        [Column("societe_id")]
        public int? SocieteId { get; set; }

    }
}
