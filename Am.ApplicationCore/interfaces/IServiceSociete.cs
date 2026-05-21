using Am.ApplicationCore.Domain;

namespace Am.ApplicationCore.Interfaces
{
    public interface IServiceSociete : IRepository<Societe>
    {
        // Les méthodes CRUD sont héritées de IRepository<Societe>
    }
}
