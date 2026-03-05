using AutoMapper;
using GymMangementBLL.Services.Interfaces;
using GymMangementBLL.ViewModels.TrainerViewModels;
using GymMangementDAL.Entities;
using GymMangementDAL.Repositories.Interfaces;

namespace GymMangementBLL.Services.Classes
{
	public class TrainerService : ITrainerService
	{
		private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        public TrainerService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
		public bool CreateTrainer(CreateTrainerViewModel createdTrainer)
		{
			try
			{
				var Repo = _unitOfWork.GetRepository<Trainer>();

				if (IsEmailExists(createdTrainer.Email) || IsPhoneExists(createdTrainer.Phone)) return false;
				var Trainer = mapper.Map<Trainer>(createdTrainer);

				Repo.Add(Trainer);

				return _unitOfWork.SaveChanges() > 0;


			}
			catch (Exception)
			{
				return false;
			}
		}

		public IEnumerable<TrainerViewModel> GetAllTrainers()
		{
			var Trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
			if (Trainers is null || !Trainers.Any()) return [];

			return mapper.Map<IEnumerable<TrainerViewModel>>(Trainers);
		}

		public TrainerViewModel? GetTrainerDetails(int trainerId)
		{
			var Trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
			if (Trainer is null) return null;


			return mapper.Map<TrainerViewModel>(Trainer);
		}
		public TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId)
		{
			var Trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
			if (Trainer is null) return null;

			return mapper.Map<TrainerToUpdateViewModel>(Trainer);
		}
		public bool RemoveTrainer(int trainerId)
		{
			var Repo = _unitOfWork.GetRepository<Trainer>();
			var TrainerToRemove = Repo.GetById(trainerId);
			if (TrainerToRemove is null || HasActiveSessions(trainerId)) return false;
			Repo.Delete(TrainerToRemove);
			return _unitOfWork.SaveChanges() > 0;
		}

		public bool UpdateTrainerDetails(TrainerToUpdateViewModel updatedTrainer, int trainerId)
		{
			var Repo = _unitOfWork.GetRepository<Trainer>();
			var TrainerToUpdate = Repo.GetById(trainerId);
			var emailExists = _unitOfWork.GetRepository<Trainer>()
				.GetAll(x => x.Email == updatedTrainer.Email && x.Id != trainerId);

            var phoneExists = _unitOfWork.GetRepository<Trainer>()
                .GetAll(x => x.Phone == updatedTrainer.Phone && x.Id != trainerId);

			if(emailExists.Any() || phoneExists.Any()) return false;
            mapper.Map(updatedTrainer, TrainerToUpdate);
			Repo.Update(TrainerToUpdate);
			return _unitOfWork.SaveChanges() > 0;
		}

		#region Helper Methods

		private bool IsEmailExists(string email)
		{
			var existing = _unitOfWork.GetRepository<Trainer>().GetAll(m => m.Email == email).Any();
			return existing;
		}

		private bool IsPhoneExists(string phone)
		{
			var existing = _unitOfWork.GetRepository<Trainer>().GetAll(m => m.Phone == phone).Any();
			return existing;
		}

		private bool HasActiveSessions(int Id)
		{
			var activeSessions = _unitOfWork.GetRepository<Session>().GetAll(s => s.TrainerId == Id && s.StartDate > DateTime.Now).Any();
			return activeSessions;
		}
		#endregion
	}
}
