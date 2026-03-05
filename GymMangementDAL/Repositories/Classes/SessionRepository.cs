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
    public class SessionRepository : GenaricRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext dbContext;

        public SessionRepository(GymDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
        public IEnumerable<Session> GetAllSessionsWithTrainerAndCategory()
        {
            return dbContext.Sessions.Include(s => s.SessionTrainer).Include(s => s.SessionCategory).ToList();
        }

        public int GetCountOfBookedSlots(int sessionId)
        {
            return dbContext.MemberSessions.Count(ms => ms.SessionId == sessionId);
        }

        public Session? GetSessionWithTrainerAndCategory(int sessionId)
        {
            return dbContext.Sessions.Include(s => s.SessionTrainer).Include(s => s.SessionCategory)
                            .FirstOrDefault(s => s.Id == sessionId);
            
        }
    }
}
