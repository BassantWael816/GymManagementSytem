using GymMangementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementDAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenaricRepository<Session>
    {
        IEnumerable<Session> GetAllSessionsWithTrainerAndCategory();

        int GetCountOfBookedSlots(int sessionId);

        Session? GetSessionWithTrainerAndCategory(int sessionId);
    }
}
