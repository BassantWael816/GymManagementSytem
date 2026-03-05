using GymMangementBLL.ViewModels.TrainerViewModels;

namespace GymMangementBLL.Services.Interfaces
{
	public interface ITrainerService
	{
		IEnumerable<TrainerViewModel> GetAllTrainers();
		bool CreateTrainer(CreateTrainerViewModel createdTrainer);
		TrainerViewModel? GetTrainerDetails(int trainerId);
		TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId);
		bool UpdateTrainerDetails(TrainerToUpdateViewModel updatedTrainer, int trainerId);
		bool RemoveTrainer(int trainerId);
	}
}
