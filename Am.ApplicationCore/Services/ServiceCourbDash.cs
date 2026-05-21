using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Am.ApplicationCore.Services
{
    public class ServiceCourbDash : IServiceCourbDash
    {
        private readonly IRepository<CourbDash> _repository;

        public ServiceCourbDash(IRepository<CourbDash> repository)
        {
            _repository = repository;
        }

        public void Add(CourbDash entity) => _repository.Add(entity);
        public void Update(CourbDash entity) => _repository.Update(entity);
        public void Delete(CourbDash entity) => _repository.Delete(entity);
        public void Delete(Expression<Func<CourbDash, bool>> where) => _repository.Delete(where);
        public CourbDash GetById(params object[] keyValues) => _repository.GetById(keyValues);
        public IEnumerable<CourbDash> GetAll() => _repository.GetAll();
        public IEnumerable<CourbDash> GetMany(Expression<Func<CourbDash, bool>> where) => _repository.GetMany(where);
        public CourbDash Get(Expression<Func<CourbDash, bool>> where) => _repository.Get(where);
        public void Commit() => _repository.Commit();
    }
}
