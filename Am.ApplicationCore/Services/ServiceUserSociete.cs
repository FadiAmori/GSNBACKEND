using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Am.ApplicationCore.Services
{
    public class ServiceUserSociete : IServiceUserSociete
    {
        private readonly IRepository<UserSociete> _repository;

        public ServiceUserSociete(IRepository<UserSociete> repository)
        {
            _repository = repository;
        }

        public void Add(UserSociete entity)
        {
            _repository.Add(entity);
        }

        public void Update(UserSociete entity)
        {
            _repository.Update(entity);
        }

        public void Delete(UserSociete entity)
        {
            _repository.Delete(entity);
        }

        public void Delete(Expression<Func<UserSociete, bool>> where)
        {
            _repository.Delete(where);
        }

        public UserSociete GetById(params object[] keyValues)
        {
            return _repository.GetById(keyValues);
        }

        public IEnumerable<UserSociete> GetAll()
        {
            return _repository.GetAll();
        }

        public IEnumerable<UserSociete> GetMany(Expression<Func<UserSociete, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public UserSociete Get(Expression<Func<UserSociete, bool>> where)
        {
            return _repository.Get(where);
        }

        public void Commit()
        {
            _repository.Commit();
        }
    }
}
