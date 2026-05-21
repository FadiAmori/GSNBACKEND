using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Am.ApplicationCore.Services
{
    public class ServiceCountDash : IServiceCountDash
    {
        private readonly IRepository<CountDash> _repository;

        public ServiceCountDash(IRepository<CountDash> repository)
        {
            _repository = repository;
        }

        public void Add(CountDash entity) => _repository.Add(entity);
        public void Update(CountDash entity) => _repository.Update(entity);
        public void Delete(CountDash entity) => _repository.Delete(entity);
        public void Delete(Expression<Func<CountDash, bool>> where) => _repository.Delete(where);
        public CountDash GetById(params object[] keyValues) => _repository.GetById(keyValues);
        public IEnumerable<CountDash> GetAll() => _repository.GetAll();
        public IEnumerable<CountDash> GetMany(Expression<Func<CountDash, bool>> where) => _repository.GetMany(where);
        public CountDash Get(Expression<Func<CountDash, bool>> where) => _repository.Get(where);
        public void Commit() => _repository.Commit();
    }
}
