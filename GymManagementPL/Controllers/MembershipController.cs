using GymMangementBLL.Services.Interfaces;
using GymMangementBLL.ViewModels.MembershipViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class MembershipController : Controller
    {
        private readonly IMembershipService membershipService;
        private readonly IPlanService planService;

        public MembershipController(IMembershipService membershipService, IPlanService planService)
        {
            this.membershipService = membershipService;
            this.planService = planService;
        }

        public IActionResult Index()
        {
            var memberships = membershipService.GetActiveMemberships();
            return View(memberships);
        }

        public IActionResult Create()
        {
            LoadDropDowns();
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateMembershipViewModel createdMembership)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDowns();
                return View(createdMembership);
            }

            var result = membershipService.CreateMembership(createdMembership);
            if (result)
            {
                TempData["SuccessMessage"] = "Membership Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Create Membership. Member May Already Have An Active Membership Or Plan Is Inactive.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Cancel(int memberId, int planId)
        {
            var result = membershipService.CancelMembership(memberId, planId);
            if (result)
            {
                TempData["SuccessMessage"] = "Membership Cancelled Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Cancel Membership";
            }
            return RedirectToAction(nameof(Index));
        }

        #region Helper Methods

        private void LoadDropDowns()
        {
            var members = membershipService.GetMembersForDropDown();
            ViewBag.Members = new SelectList(members, "Id", "Name");

            var plans = planService.GetAllPlans().Where(p => p.IsActive);
            ViewBag.Plans = new SelectList(plans, "Id", "Name");
        }

        #endregion
    }
}
