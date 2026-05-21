using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Am.ApplicationCore.Services
{
    public class ServiceCBDash : IServiceCBDash
    {
        private readonly IRepository<CBDash> _repository;

        public ServiceCBDash(IRepository<CBDash> repository)
        {
            _repository = repository;
        }

        public void Add(CBDash entity) => _repository.Add(entity);
        public void Update(CBDash entity) => _repository.Update(entity);
        public void Delete(CBDash entity) => _repository.Delete(entity);
        public void Delete(Expression<Func<CBDash, bool>> where) => _repository.Delete(where);
        public CBDash GetById(params object[] keyValues) => _repository.GetById(keyValues);
        public IEnumerable<CBDash> GetAll() => _repository.GetAll();
        public IEnumerable<CBDash> GetMany(Expression<Func<CBDash, bool>> where) => _repository.GetMany(where);
        public CBDash Get(Expression<Func<CBDash, bool>> where) => _repository.Get(where);
        public void Commit() => _repository.Commit();
    }
}
