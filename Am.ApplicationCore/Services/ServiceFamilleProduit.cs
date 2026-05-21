using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Am.ApplicationCore.Services
{
    public class ServiceFamilleProduit : IServiceFamilleProduit
    {
        private readonly IRepository<FamilleProduit> _repository;

        public ServiceFamilleProduit(IRepository<FamilleProduit> repository)
        {
            _repository = repository;
        }

        public void Add(FamilleProduit entity)
        {
            _repository.Add(entity);
        }

        public void Update(FamilleProduit entity)
        {
            _repository.Update(entity);
        }

        public void Delete(FamilleProduit entity)
        {
            _repository.Delete(entity);
        }

        public void Delete(Expression<Func<FamilleProduit, bool>> where)
        {
            _repository.Delete(where);
        }

        public FamilleProduit GetById(params object[] keyValues)
        {
            return _repository.GetById(keyValues);
        }

        public IEnumerable<FamilleProduit> GetAll()
        {
            return _repository.GetAll();
        }

        public IEnumerable<FamilleProduit> GetMany(Expression<Func<FamilleProduit, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public FamilleProduit Get(Expression<Func<FamilleProduit, bool>> where)
        {
            return _repository.Get(where);
        }

        public void Commit()
        {
            _repository.Commit();
        }
    }
}
