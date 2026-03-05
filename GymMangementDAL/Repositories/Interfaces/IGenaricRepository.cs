using GymMangementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementDAL.Repositories.Interfaces
{
    public interface IGenaricRepository<TEntity> where TEntity : BaseEntity, new()
    {
        //GetAll
        IEnumerable<TEntity> GetAll(Func<TEntity,bool>? condition = null);
        //GetById
        TEntity? GetById(int Id);
        //Add
        void Add(TEntity entity);
        //Update
        void Update(TEntity entity);
        //Delete
        void Delete(TEntity entity);
    }
}
