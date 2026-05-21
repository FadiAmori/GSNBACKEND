using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Am.ApplicationCore.Services
{
    public class ServiceProduit : IServiceProduit
    {
        private readonly IRepository<Produit> _repository;

        public ServiceProduit(IRepository<Produit> repository)
        {
            _repository = repository;
        }

        public void Add(Produit entity)
        {
            _repository.Add(entity);
        }

        public void Update(Produit entity)
        {
            _repository.Update(entity);
        }

        public void Delete(Produit entity)
        {
            _repository.Delete(entity);
        }

        public void Delete(Expression<Func<Produit, bool>> where)
        {
            _repository.Delete(where);
        }

        public Produit GetById(params object[] keyValues)
        {
            return _repository.GetById(keyValues);
        }

        public IEnumerable<Produit> GetAll()
        {
            return _repository.GetAll();
        }

        public IEnumerable<Produit> GetMany(Expression<Func<Produit, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public Produit Get(Expression<Func<Produit, bool>> where)
        {
            return _repository.Get(where);
        }

        public void Commit()
        {
            _repository.Commit();
        }
    }
}
