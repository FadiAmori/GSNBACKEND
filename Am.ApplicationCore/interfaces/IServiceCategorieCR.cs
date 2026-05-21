using Am.ApplicationCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Am.ApplicationCore.Interfaces
{
    public interface IServiceCategorieCR
    {
        void Add(CategorieCR entity);
        void Update(CategorieCR entity);
        void Delete(CategorieCR entity);
        void Delete(Expression<Func<CategorieCR, bool>> where);
        CategorieCR GetById(params object[] keyValues);
        IEnumerable<CategorieCR> GetAll();
        IEnumerable<CategorieCR> GetMany(Expression<Func<CategorieCR, bool>> where);
        CategorieCR Get(Expression<Func<CategorieCR, bool>> where);
        void Commit();
    }
}
