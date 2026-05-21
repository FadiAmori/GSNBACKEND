using Am.ApplicationCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Am.ApplicationCore.Interfaces
{
    public interface IServiceCourbDash
    {
        void Add(CourbDash entity);
        void Update(CourbDash entity);
        void Delete(CourbDash entity);
        void Delete(Expression<Func<CourbDash, bool>> where);
        CourbDash GetById(params object[] keyValues);
        IEnumerable<CourbDash> GetAll();
        IEnumerable<CourbDash> GetMany(Expression<Func<CourbDash, bool>> where);
        CourbDash Get(Expression<Func<CourbDash, bool>> where);
        void Commit();
    }
}
