using Am.ApplicationCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Am.ApplicationCore.Interfaces
{
    public interface IServiceCBDash
    {
        void Add(CBDash entity);
        void Update(CBDash entity);
        void Delete(CBDash entity);
        void Delete(Expression<Func<CBDash, bool>> where);
        CBDash GetById(params object[] keyValues);
        IEnumerable<CBDash> GetAll();
        IEnumerable<CBDash> GetMany(Expression<Func<CBDash, bool>> where);
        CBDash Get(Expression<Func<CBDash, bool>> where);
        void Commit();
    }
}
