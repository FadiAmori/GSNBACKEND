using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Am.ApplicationCore.Services
{
    public class ServiceClesDeRepartition : IServiceClesDeRepartition
    {
        private readonly IRepository<ClesDeRepartition> _repository;

        public ServiceClesDeRepartition(IRepository<ClesDeRepartition> repository)
        {
            _repository = repository;
        }

        public void Add(ClesDeRepartition entity)
        {
            _repository.Add(entity);
        }

        public void Update(ClesDeRepartition entity)
        {
            _repository.Update(entity);
        }

        public void Delete(ClesDeRepartition entity)
        {
            _repository.Delete(entity);
        }

        public void Delete(Expression<Func<ClesDeRepartition, bool>> where)
        {
            _repository.Delete(where);
        }

        public ClesDeRepartition GetById(params object[] keyValues)
        {
            return _repository.GetById(keyValues);
        }

        public IEnumerable<ClesDeRepartition> GetAll()
        {
            return _repository.GetAll();
        }

        public IEnumerable<ClesDeRepartition> GetMany(Expression<Func<ClesDeRepartition, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public ClesDeRepartition Get(Expression<Func<ClesDeRepartition, bool>> where)
        {
            return _repository.Get(where);
        }

        public void Commit()
        {
            _repository.Commit();
        }
    }
}
