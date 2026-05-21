using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Am.ApplicationCore.Services
{
    public class ServiceRapportFinancier : IServiceRapportFinancier
    {
        private readonly IRepository<RapportFinancier> _repository;

        public ServiceRapportFinancier(IRepository<RapportFinancier> repository)
        {
            _repository = repository;
        }

        public void Add(RapportFinancier entity)
        {
            _repository.Add(entity);
        }

        public void Update(RapportFinancier entity)
        {
            _repository.Update(entity);
        }

        public void Delete(RapportFinancier entity)
        {
            _repository.Delete(entity);
        }

        public void Delete(Expression<Func<RapportFinancier, bool>> where)
        {
            _repository.Delete(where);
        }

        public RapportFinancier GetById(params object[] keyValues)
        {
            return _repository.GetById(keyValues);
        }

        public IEnumerable<RapportFinancier> GetAll()
        {
            return _repository.GetAll();
        }

        public IEnumerable<RapportFinancier> GetMany(Expression<Func<RapportFinancier, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public RapportFinancier Get(Expression<Func<RapportFinancier, bool>> where)
        {
            return _repository.Get(where);
        }

        public void Commit()
        {
            _repository.Commit();
        }
    }
}
