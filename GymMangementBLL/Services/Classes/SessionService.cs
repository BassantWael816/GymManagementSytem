using AutoMapper;
using GymMangementBLL.Services.Interfaces;
using GymMangementBLL.ViewModels.SessionViewModels;
using GymMangementDAL.Entities;
using GymMangementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementBLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public bool CreateSession(CreateSessionViewModel createdSession)
        {
            try
            {
                //check if Trainer Exsits
                if (!IsTrainerExsits(createdSession.TrainerId)) return false;
                //check if Category
                if (!IsCategoryExsits(createdSession.CategoryId)) return false;
                //check if StartTime < EndTime
                if (!IsValidSessionTime(createdSession.StartDate, createdSession.EndDate)) return false;
                //check if Capacity > 0 & < 25
                if (createdSession.Capacity <= 0 || createdSession.Capacity > 25) return false;

                var mappedSession = mapper.Map<CreateSessionViewModel, Session>(createdSession);
                unitOfWork.GetRepository<Session>().Add(mappedSession);
                return unitOfWork.SaveChanges() > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Create Session Faild: {e}");
                return false;
            }
        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var sessions = unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategory();
            if (!sessions.Any()) return [];

            var mappedSessions = mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(sessions);
            foreach (var session in mappedSessions)
                session.AvailableSlots = session.Capacity - unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id);

            return mappedSessions;
        }

        public SessionViewModel? GetSessionById(int sessionId)
        {
            var session = unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(sessionId);
            if (session is null) return null;

            var mappedSession = mapper.Map<Session, SessionViewModel>(session);
            mappedSession.AvailableSlots = mappedSession.Capacity - unitOfWork.SessionRepository.GetCountOfBookedSlots(mappedSession.Id);
            return mappedSession;
        }

        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
            var session = unitOfWork.SessionRepository.GetById(sessionId);
            if (!IsSessionAvailableToUpdate(session!)) return null;
            if (!IsTrainerExsits(session!.TrainerId)) return null;
            if (!IsValidSessionTime(session.StartDate, session.EndDate)) return null;

            var mappedSession = mapper.Map<Session, UpdateSessionViewModel>(session);
            return mappedSession;

        }

        public bool UpdateSession(UpdateSessionViewModel updatedSession, int sessionId)
        {
            try
            {
                var session = unitOfWork.SessionRepository.GetById(sessionId);
                if (!IsSessionAvailableToUpdate(session!)) return false;
                if (!IsTrainerExsits(updatedSession.TrainerId)) return false;
                if(!IsValidSessionTime(updatedSession.StartDate, updatedSession.EndDate)) return false;

                mapper.Map<UpdateSessionViewModel, Session>(updatedSession, session!);
                session!.UpdatedAt = DateTime.Now;

                unitOfWork.GetRepository<Session>().Update(session);
                return unitOfWork.SaveChanges() > 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update Session Faild :{ex}");
                return false;
            }
        }

        public bool DeleteSession(int sessionId)
        {
            var session = unitOfWork.SessionRepository.GetById(sessionId);
            if(!IsSessionAvailableToRemove(session!)) return false;

            unitOfWork.SessionRepository.Delete(session!);
            return unitOfWork.SaveChanges() > 0;
        }

        public IEnumerable<TrainerSelectViewModel> GetTrainersForDropDown()
        {
            var trainers = unitOfWork.GetRepository<Trainer>().GetAll();
            return mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);
        }

        public IEnumerable<CategorySelectViewModel> GetCategoryForDropDown()
        {
            var categories = unitOfWork.GetRepository<Category>().GetAll();
            return mapper.Map<IEnumerable<CategorySelectViewModel>>(categories);
        }

        #region Helper Methods

        private bool IsSessionAvailableToUpdate(Session session)
        {
            if (session is null) return false;
            //If Session Completed -- No Update
            if (session.EndDate < DateTime.Now) return false;
            //If Session Started -- No Update
            if (session.StartDate <= DateTime.Now) return false;
            //If Has Available Bookung -- No Update
            var HasActiveBooking = unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if (HasActiveBooking) return false;

            return true;
        }
        private bool IsSessionAvailableToRemove(Session session)
        {
            if (session is null) return false;
            //If Session Started -- No Remove

            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now) return false;

            //If Session Is UpComing -- No Remove
            if(session.StartDate >  DateTime.Now) return false;

            //If Has Available Bookung -- No Remove
            var HasActiveBooking = unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if (HasActiveBooking) return false;

            return true;
        }
        private bool IsTrainerExsits(int trainerId)
        {
            return unitOfWork.GetRepository<Trainer>().GetById(trainerId) is not null;
        }
        private bool IsCategoryExsits(int categoryId)
        {
            return unitOfWork.GetRepository<Category>().GetById(categoryId) is not null;
        }
        private bool IsValidSessionTime(DateTime startDate, DateTime endDate)
        {
            return startDate < endDate && startDate > DateTime.Now;
        }

        #endregion
    }
}
