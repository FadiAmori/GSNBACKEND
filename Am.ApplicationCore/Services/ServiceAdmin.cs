using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Am.ApplicationCore.Services
{
    public class ServiceAdmin : IServiceAdmin
    {
        private readonly IRepository<Admin> _repository;

        public ServiceAdmin(IRepository<Admin> repository)
        {
            _repository = repository;
        }

        public void Add(Admin entity)
        {
            _repository.Add(entity);
        }

        public void Update(Admin entity)
        {
            _repository.Update(entity);
        }

        public void Delete(Admin entity)
        {
            _repository.Delete(entity);
        }

        public void Delete(Expression<Func<Admin, bool>> where)
        {
            _repository.Delete(where);
        }

        public Admin GetById(params object[] keyValues)
        {
            return _repository.GetById(keyValues);
        }

        public IEnumerable<Admin> GetAll()
        {
            return _repository.GetAll();
        }

        public IEnumerable<Admin> GetMany(Expression<Func<Admin, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public Admin Get(Expression<Func<Admin, bool>> where)
        {
            return _repository.Get(where);
        }

        public void Commit()
        {
            _repository.Commit();
        }
    }
}
