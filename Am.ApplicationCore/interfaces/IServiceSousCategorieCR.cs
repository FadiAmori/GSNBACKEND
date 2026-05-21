using Am.ApplicationCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Am.ApplicationCore.Interfaces
{
    public interface IServiceSousCategorieCR
    {
        void Add(SousCategorieCR entity);
        void Update(SousCategorieCR entity);
        void Delete(SousCategorieCR entity);
        void Delete(Expression<Func<SousCategorieCR, bool>> where);
        SousCategorieCR GetById(params object[] keyValues);
        IEnumerable<SousCategorieCR> GetAll();
        IEnumerable<SousCategorieCR> GetMany(Expression<Func<SousCategorieCR, bool>> where);
        SousCategorieCR Get(Expression<Func<SousCategorieCR, bool>> where);
        void Commit();
    }
}
