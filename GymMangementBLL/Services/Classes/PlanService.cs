using AutoMapper;
using GymMangementBLL.Services.Interfaces;
using GymMangementBLL.ViewModels.PlanViewModels;
using GymMangementDAL.Entities;
using GymMangementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementBLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public PlanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var plans = unitOfWork.GetRepository<Plan>().GetAll();

            if (plans == null || !plans.Any()) return Enumerable.Empty<PlanViewModel>();

            var planViewModel = mapper.Map<IEnumerable<PlanViewModel>>(plans);

            return planViewModel;
        }

        public PlanViewModel? GetPlanById(int id)
        {
            var plan = unitOfWork.GetRepository<Plan>().GetById(id);
            if (plan == null) return null;

            return mapper.Map<PlanViewModel>(plan);
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int planId)
        {
            var plan = unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan == null || plan.IsActive == false || HasActiveMemberShip(planId)) return null;

            return mapper.Map<UpdatePlanViewModel>(plan);
        }

        public bool UpdatePlan(int planId, UpdatePlanViewModel updatedPlan)
        {
            try
            {
                var plan = unitOfWork.GetRepository<Plan>().GetById(planId);
                if (plan is null || HasActiveMemberShip(planId)) return false;

                mapper.Map(updatedPlan , plan);
                unitOfWork.GetRepository<Plan>().Update(plan);
                return unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool ToggleStatus(int planId)
        {
            var repo = unitOfWork.GetRepository<Plan>();
            var plan = repo.GetById(planId);
            if (repo is null || HasActiveMemberShip(planId)) return false;

            plan.IsActive = plan.IsActive == true ? false : true;
            plan.UpdatedAt = DateTime.Now;

            try
            {
                repo.Update(plan);
                return unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }


        #region Helper Methods

        private bool HasActiveMemberShip(int planId)
        {
            var activeMemberShips = unitOfWork.GetRepository<Membership>().GetAll(x => x.PlanId == planId && x.Status == "Active");
            return activeMemberShips.Any();
        }

        #endregion
    }
}
