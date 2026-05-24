using Am.ApplicationCore.Domain;

namespace Am.ApplicationCore.DTOs
{
    public class RapportFinancierCreateDto
    {
        public TypeRapport Type { get; set; }
        public int Annee { get; set; }
        // add other simple scalar properties if needed
    }
}