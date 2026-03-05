using GymMangementDAL.Data.Contexts;
using GymMangementDAL.Entities;
using GymMangementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementDAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> repositories = new ();
        private readonly GymDbContext dbContext;

        public UnitOfWork(GymDbContext dbContext , ISessionRepository sessionRepository)
        {
            this.dbContext = dbContext;
            SessionRepository = sessionRepository;
        }

        public ISessionRepository SessionRepository { get; }

        public IGenaricRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var EntityType = typeof(TEntity);
            if (repositories.TryGetValue(EntityType , out var Repo))
                return (IGenaricRepository<TEntity>)Repo;

            var NewRepo = new GenaricRepository<TEntity>(dbContext);
            repositories[EntityType] = NewRepo;
            return NewRepo;

        }

        public int SaveChanges()
        {
           return dbContext.SaveChanges();
        }
    }
}
