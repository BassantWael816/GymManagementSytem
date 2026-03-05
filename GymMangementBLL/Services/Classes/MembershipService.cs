using AutoMapper;
using GymMangementBLL.Services.Interfaces;
using GymMangementBLL.ViewModels.MembershipViewModels;
using GymMangementDAL.Entities;
using GymMangementDAL.Repositories.Interfaces;

namespace GymMangementBLL.Services.Classes
{
    public class MembershipService : IMembershipService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public MembershipService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public IEnumerable<MembershipViewModel> GetActiveMemberships()
        {
            var memberships = unitOfWork.GetRepository<Membership>()
                .GetAll(x => x.Status == "Active");

            if (!memberships.Any()) return Enumerable.Empty<MembershipViewModel>();

            return memberships.Select(ms => new MembershipViewModel
            {
                MemberId = ms.MemberId,
                PlanId = ms.PlanId,
                MemberName = unitOfWork.GetRepository<Member>().GetById(ms.MemberId)?.Name ?? "Unknown",
                PlanName = unitOfWork.GetRepository<Plan>().GetById(ms.PlanId)?.Name ?? "Unknown",
                StartDate = ms.CreatedAt.ToShortDateString(),
                EndDate = ms.EndDate.ToShortDateString(),
                Status = ms.Status
            });
        }

        public bool CreateMembership(CreateMembershipViewModel createdMembership)
        {
            try
            {
                // Member must exist
                var member = unitOfWork.GetRepository<Member>().GetById(createdMembership.MemberId);
                if (member is null) return false;

                // Plan must exist and be active
                var plan = unitOfWork.GetRepository<Plan>().GetById(createdMembership.PlanId);
                if (plan is null || !plan.IsActive) return false;

                // Member must not already have an active membership
                var hasActiveMembership = unitOfWork.GetRepository<Membership>()
                    .GetAll(x => x.MemberId == createdMembership.MemberId && x.Status == "Active")
                    .Any();
                if (hasActiveMembership) return false;

                var membership = new Membership
                {
                    MemberId = createdMembership.MemberId,
                    PlanId = createdMembership.PlanId,
                    EndDate = DateTime.Now.AddDays(plan.DurationDays)
                };

                unitOfWork.GetRepository<Membership>().Add(membership);
                return unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CancelMembership(int memberId, int planId)
        {
            try
            {
                var repo = unitOfWork.GetRepository<Membership>();
                var membership = repo.GetAll(x => x.MemberId == memberId && x.PlanId == planId && x.Status == "Active")
                    .FirstOrDefault();

                if (membership is null) return false;

                repo.Delete(membership);
                return unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<MemberSelectViewModel> GetMembersForDropDown()
        {
            var members = unitOfWork.GetRepository<Member>().GetAll();
            return members.Select(m => new MemberSelectViewModel { Id = m.Id, Name = m.Name });
        }
    }
}
