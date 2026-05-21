using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Am.ApplicationCore.Domain
{
    public class Produit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("nom")]
        public string? Nom { get; set; }

        [Column("code")]
        public string? Code { get; set; }

        [Column("poids_prevu")]
        public double PoidsPrevu { get; set; }

        [Column("taux_poids")]
        public double TauxPoids { get; set; }

        [Column("tps_unitaire")]
        public double TpsUnitaire { get; set; }

        [Column("temps_global")]
        public double TempsGlobal { get; set; }

        [Column("cout_mod_par_heure")]
        public double CoutMODParHeure { get; set; }

        [ForeignKey("TypeClient")]
        [Column("type_client_id")]
        public int? TypeClientId { get; set; }

        public TypeClient? TypeClient { get; set; }

        [ForeignKey("FamilleProduit")]
        [Column("famille_produit_id")]
        public int? FamilleProduitId { get; set; }

        public FamilleProduit? FamilleProduit { get; set; }

        public ICollection<CR>? CRs { get; set; }
    }
}
