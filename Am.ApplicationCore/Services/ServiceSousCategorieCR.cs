using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Am.ApplicationCore.Services
{
    public class ServiceSousCategorieCR : IServiceSousCategorieCR
    {
        private readonly IRepository<SousCategorieCR> _repository;

        public ServiceSousCategorieCR(IRepository<SousCategorieCR> repository)
        {
            _repository = repository;
        }

        public void Add(SousCategorieCR entity)
        {
            _repository.Add(entity);
        }

        public void Update(SousCategorieCR entity)
        {
            _repository.Update(entity);
        }

        public void Delete(SousCategorieCR entity)
        {
            _repository.Delete(entity);
        }

        public void Delete(Expression<Func<SousCategorieCR, bool>> where)
        {
            _repository.Delete(where);
        }

        public SousCategorieCR GetById(params object[] keyValues)
        {
            return _repository.GetById(keyValues);
        }

        public IEnumerable<SousCategorieCR> GetAll()
        {
            return _repository.GetAll();
        }

        public IEnumerable<SousCategorieCR> GetMany(Expression<Func<SousCategorieCR, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public SousCategorieCR Get(Expression<Func<SousCategorieCR, bool>> where)
        {
            return _repository.Get(where);
        }

        public void Commit()
        {
            _repository.Commit();
        }
    }
}
