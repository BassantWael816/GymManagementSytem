
using AutoMapper;
using GymMangementBLL.Services.AttachmentService;
using GymMangementBLL.Services.Interfaces;
using GymMangementBLL.ViewModels.MemberViewModels;
using GymMangementDAL.Entities;
using GymMangementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementBLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IAttachmentService attachmentService;

        public MemberService(IUnitOfWork unitOfWork , IMapper mapper, IAttachmentService attachmentService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.attachmentService = attachmentService;
        }

        public bool CreateMember(CreateMemberViewModel CreatedMember)
        {
            try
            {
                if (EmailExsits(CreatedMember.Email) || PhoneExsits(CreatedMember.Phone)) return false;

                var photoName = attachmentService.Upload("members" , CreatedMember.PhotoFile);
                if (string.IsNullOrEmpty(photoName)) return false;

                //add member ==> Mapping
                var member = mapper.Map<Member>(CreatedMember);
                member.Photo = photoName;
                unitOfWork.GetRepository<Member>().Add(member);
                var isCreated = unitOfWork.SaveChanges() > 0;
                if (!isCreated)
                {
                    attachmentService.Delete("members", photoName);
                    return false;
                }
                else
                {
                    return isCreated;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var members = unitOfWork.GetRepository<Member>().GetAll();
            if(members is null || !members.Any()) return Enumerable.Empty<MemberViewModel>();

            var MemberViewModels = mapper.Map<IEnumerable<MemberViewModel>>(members);
            return MemberViewModels;
        }

        public HealthRecordViewModel? GetHealthRecordDetails(int memberId)
        {
            var MemberHealthRecord = unitOfWork.GetRepository<HealthRecord>().GetById(memberId);
            if(MemberHealthRecord == null) return null;

            return mapper.Map<HealthRecordViewModel>(MemberHealthRecord);
        }

        public MemberViewModel? GetMemberDetails(int memberId)
        {
            var member = unitOfWork.GetRepository<Member>().GetById(memberId);
            if(member == null) return null;

            var ViewModel = mapper.Map<MemberViewModel>(member);

            //Active MemberShip
            var ActivememberShip = unitOfWork.GetRepository<Membership>().GetAll(x => x.Id == member.Id && x.Status == "Active").FirstOrDefault();
            if (ActivememberShip is not null)
            {
                ViewModel.MemberShipStartDate = ActivememberShip.CreatedAt.ToShortDateString();
                ViewModel.MemberShipEndDate = ActivememberShip.EndDate.ToShortDateString();

                var plan = unitOfWork.GetRepository<Plan>().GetById(ActivememberShip.PlanId);
                ViewModel.PlanName = plan?.Name;
            }

            return ViewModel;
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int memberId)
        {
            var member = unitOfWork.GetRepository<Member>().GetById(memberId);
            if(member == null) return null;
            return mapper.Map<MemberToUpdateViewModel>(member);
        }

        public bool RemoveMember(int MemberId)
        {
            var repo = unitOfWork.GetRepository<Member>();
            var member = repo.GetById(MemberId);
            if(member == null) return false;

            var SessionIds = unitOfWork.GetRepository<MemberSession>()
                .GetAll(x =>x.MemberId == MemberId).Select(x => x.SessionId);

            var HasFutureSessions = unitOfWork.GetRepository<Session>()
                .GetAll(x => SessionIds.Contains(x.Id) && x.StartDate > DateTime.Now).Any();

            if(HasFutureSessions) return false;

            var memberShipRepository = unitOfWork.GetRepository<Membership>();
            var memberShips = memberShipRepository.GetAll(x => x.MemberId == MemberId);
            try
            {
                if (memberShips.Any())
                {
                    foreach(var memberShip in memberShips)
                        memberShipRepository.Delete(memberShip);
                }

                repo.Delete(member);
                var isDeleted =  unitOfWork.SaveChanges() > 0;
                if (isDeleted)
                    attachmentService.Delete(member.Photo, "members");

                return isDeleted;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateMemberDetails(int Id, MemberToUpdateViewModel UpdatedMember)
        {
            try
            {
               var emailExists = unitOfWork.GetRepository<Member>()
                    .GetAll(x => x.Email ==  UpdatedMember.Email && x.Id != Id);

                var phoneExists = unitOfWork.GetRepository<Member>()
                    .GetAll(x => x.Phone == UpdatedMember.Phone && x.Id != Id);

                if(phoneExists.Any() || emailExists.Any()) return false;

                var repo = unitOfWork.GetRepository<Member>();
                var member = repo.GetById(Id);
                if(member == null) return false;

                mapper.Map(UpdatedMember , member);
                repo.Update(member);
                return unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }


        #region Helper Methods
        private bool EmailExsits(string email)
        {
            return unitOfWork.GetRepository<Member>().GetAll(x => x.Email == email).Any();
        }
        private bool PhoneExsits(string phone)
        {
            return unitOfWork.GetRepository<Member>().GetAll(x => x.Phone == phone).Any();
        }

        #endregion
    }
}
