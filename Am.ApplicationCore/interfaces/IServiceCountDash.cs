using Am.ApplicationCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Am.ApplicationCore.Interfaces
{
    public interface IServiceCountDash
    {
        void Add(CountDash entity);
        void Update(CountDash entity);
        void Delete(CountDash entity);
        void Delete(Expression<Func<CountDash, bool>> where);
        CountDash GetById(params object[] keyValues);
        IEnumerable<CountDash> GetAll();
        IEnumerable<CountDash> GetMany(Expression<Func<CountDash, bool>> where);
        CountDash Get(Expression<Func<CountDash, bool>> where);
        void Commit();
    }
}
