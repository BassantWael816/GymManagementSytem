using GymMangementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementDAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        public ISessionRepository SessionRepository { get;}
        IGenaricRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity , new ();

        int SaveChanges();
    }
}
