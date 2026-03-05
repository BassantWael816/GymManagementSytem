using GymMangementBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementBLL.Services.Interfaces
{
    public interface ISessionService
    {
        public IEnumerable<SessionViewModel> GetAllSessions();
        public SessionViewModel? GetSessionById(int sessionId);
        bool CreateSession(CreateSessionViewModel createdSession);
        UpdateSessionViewModel? GetSessionToUpdate(int sessionId);
        bool UpdateSession(UpdateSessionViewModel updatedSession , int sessionId);
        bool DeleteSession(int sessionId);
        IEnumerable<TrainerSelectViewModel> GetTrainersForDropDown();
        IEnumerable<CategorySelectViewModel> GetCategoryForDropDown();
    }

}
