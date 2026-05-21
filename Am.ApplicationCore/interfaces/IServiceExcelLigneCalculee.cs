using Am.ApplicationCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Am.ApplicationCore.Interfaces
{
    public interface IServiceExcelLigneCalculee
    {
        void Add(ExcelLigneCalculee entity);
        void Update(ExcelLigneCalculee entity);
        void Delete(ExcelLigneCalculee entity);
        void Delete(Expression<Func<ExcelLigneCalculee, bool>> where);
        ExcelLigneCalculee GetById(params object[] keyValues);
        IEnumerable<ExcelLigneCalculee> GetAll();
        IEnumerable<ExcelLigneCalculee> GetMany(Expression<Func<ExcelLigneCalculee, bool>> where);
        ExcelLigneCalculee Get(Expression<Func<ExcelLigneCalculee, bool>> where);
        void Commit();
    }
}
