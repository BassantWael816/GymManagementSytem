using GymMangementDAL.Data.Contexts;
using GymMangementDAL.Entities;
using GymMangementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementDAL.Repositories.Classes
{
    public class GenaricRepository<TEntity> : IGenaricRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext dbContext;

        public GenaricRepository(GymDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Add(TEntity entity) => dbContext.Set<TEntity>().Add(entity);

        public void Delete(TEntity entity) => dbContext.Set<TEntity>().Remove(entity);

        public IEnumerable<TEntity> GetAll(Func<TEntity, bool>? condition = null)
        {
            if(condition is null)
              return dbContext.Set<TEntity>().AsNoTracking().ToList();
            else
              return dbContext.Set<TEntity>().AsNoTracking().Where(condition).ToList();
            
        }

        public TEntity? GetById(int Id) => dbContext.Set<TEntity>().Find(Id);

        public void Update(TEntity entity) => dbContext.Set<TEntity>().Update(entity);
    }
}
