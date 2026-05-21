using Am.ApplicationCore.Domain;

namespace Am.ApplicationCore.Interfaces
{
    public interface IServiceAdmin : IRepository<Admin>
    {
        // Les méthodes CRUD sont héritées de IRepository<Admin>
        // Ajoutez ici uniquement les méthodes spécifiques à Admin si nécessaire
    }
}
