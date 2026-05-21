using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Am.ApplicationCore.Services
{
    public class ServiceTypeClient : IServiceTypeClient
    {
        private readonly IRepository<TypeClient> _repository;

        public ServiceTypeClient(IRepository<TypeClient> repository)
        {
            _repository = repository;
        }

        public void Add(TypeClient entity)
        {
            _repository.Add(entity);
        }

        public void Update(TypeClient entity)
        {
            _repository.Update(entity);
        }

        public void Delete(TypeClient entity)
        {
            _repository.Delete(entity);
        }

        public void Delete(Expression<Func<TypeClient, bool>> where)
        {
            _repository.Delete(where);
        }

        public TypeClient GetById(params object[] keyValues)
        {
            return _repository.GetById(keyValues);
        }

        public IEnumerable<TypeClient> GetAll()
        {
            return _repository.GetAll();
        }

        public IEnumerable<TypeClient> GetMany(Expression<Func<TypeClient, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public TypeClient Get(Expression<Func<TypeClient, bool>> where)
        {
            return _repository.Get(where);
        }

        public void Commit()
        {
            _repository.Commit();
        }
    }
}
