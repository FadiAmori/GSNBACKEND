using Am.ApplicationCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Am.ApplicationCore.Interfaces
{
    public interface IServiceExcelVariable
    {
        void Add(ExcelVariable entity);
        void Update(ExcelVariable entity);
        void Delete(ExcelVariable entity);
        void Delete(Expression<Func<ExcelVariable, bool>> where);
        ExcelVariable GetById(params object[] keyValues);
        IEnumerable<ExcelVariable> GetAll();
        IEnumerable<ExcelVariable> GetMany(Expression<Func<ExcelVariable, bool>> where);
        ExcelVariable Get(Expression<Func<ExcelVariable, bool>> where);
        void Commit();
    }
}
