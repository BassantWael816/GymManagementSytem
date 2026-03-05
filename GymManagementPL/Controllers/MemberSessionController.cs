using GymMangementBLL.Services.Interfaces;
using GymMangementBLL.ViewModels.MemberSessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class MemberSessionController : Controller
    {
        private readonly IMemberSessionService memberSessionService;

        public MemberSessionController(IMemberSessionService memberSessionService)
        {
            this.memberSessionService = memberSessionService;
        }

        // Shows all upcoming + ongoing sessions available for management
        public IActionResult Index()
        {
            var upcomingSessions = memberSessionService.GetUpcomingSessions();
            var ongoingSessions = memberSessionService.GetOngoingSessions();

            ViewBag.UpcomingSessions = upcomingSessions;
            ViewBag.OngoingSessions = ongoingSessions;

            return View();
        }

        // View booked members for an upcoming session (can cancel bookings here)
        public IActionResult GetMembersForUpcomingSession(int sessionId)
        {
            if (sessionId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction(nameof(Index));
            }

            var members = memberSessionService.GetMembersForUpcomingSession(sessionId);
            ViewBag.SessionId = sessionId;
            return View(members);
        }

        // View members for an ongoing session (can mark attendance here)
        public IActionResult GetMembersForOngoingSessions(int sessionId)
        {
            if (sessionId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction(nameof(Index));
            }

            var members = memberSessionService.GetMembersForOngoingSession(sessionId);
            ViewBag.SessionId = sessionId;
            return View(members);
        }

        // Create a new booking for an upcoming session
        public IActionResult Create(int sessionId)
        {
            if (sessionId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction(nameof(Index));
            }

            LoadMembersDropDown(sessionId);
            ViewBag.SessionId = sessionId;
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateBookingViewModel booking)
        {
            if (!ModelState.IsValid)
            {
                LoadMembersDropDown(booking.SessionId);
                ViewBag.SessionId = booking.SessionId;
                return View(booking);
            }

            var result = memberSessionService.CreateBooking(booking);
            if (result)
            {
                TempData["SuccessMessage"] = "Booking Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Create Booking. Session May Be Full Or Member Does Not Have An Active Membership.";
            }
            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { sessionId = booking.SessionId });
        }

        // Cancel a booking for an upcoming session
        [HttpPost]
        public IActionResult CancelBooking(int sessionId, int memberId)
        {
            var result = memberSessionService.CancelBooking(sessionId, memberId);
            if (result)
            {
                TempData["SuccessMessage"] = "Booking Cancelled Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Cancel Booking";
            }
            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { sessionId });
        }

        // Mark a member as attended in an ongoing session
        [HttpPost]
        public IActionResult MarkAttendance(int sessionId, int memberId)
        {
            var result = memberSessionService.MarkAttendance(sessionId, memberId);
            if (result)
            {
                TempData["SuccessMessage"] = "Attendance Marked Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Mark Attendance";
            }
            return RedirectToAction(nameof(GetMembersForOngoingSessions), new { sessionId });
        }

        #region Helper Methods

        private void LoadMembersDropDown(int sessionId)
        {
            var members = memberSessionService.GetMembersNotBookedForSession(sessionId);
            ViewBag.Members = new SelectList(members, "Id", "Name");
        }

        #endregion
    }
}
