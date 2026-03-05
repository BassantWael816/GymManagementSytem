using GymMangementBLL.Services.Classes;
using GymMangementBLL.Services.Interfaces;
using GymMangementBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private readonly ISessionService sessionService;

        public SessionController(ISessionService sessionService)
        {
            this.sessionService = sessionService;
        }
        public ActionResult Index()
        {
            var sessions = sessionService.GetAllSessions();
            return View(sessions);
        }

        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Session Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var session = sessionService.GetSessionById(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(session);
        }

        public ActionResult Create()
        {
            LoadDropDownForTrainers();
            LoadDropDownForCategories();
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateSessionViewModel createdSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDownForCategories();
                LoadDropDownForTrainers();
                return View(nameof(Create), createdSession);
            }

            var Session = sessionService.CreateSession(createdSession);
            if (Session)
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Session Faild To Create";
                LoadDropDownForCategories();
                LoadDropDownForTrainers();
                return View(createdSession);
            }

        }
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Session Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var session = sessionService.GetSessionToUpdate(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            LoadDropDownForTrainers();
            return View(session);
        }

        [HttpPost]
        public ActionResult Edit([FromRoute]int id , UpdateSessionViewModel updatedSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDownForTrainers();
                return View(updatedSession);
            }

            var Session = sessionService.UpdateSession(updatedSession,id);
            if (Session)
            {
                TempData["SuccessMessage"] = "Session Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Session Faild To Update";
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Session Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var session = sessionService.GetSessionById(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.SessionId = session.Id;
            return View(session);
        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var result = sessionService.DeleteSession(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Session Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Session Can Not Be Delete";
            }
            return RedirectToAction(nameof(Index));
        }

        #region Helper Method

        private void LoadDropDownForCategories()
        {
            var Categories = sessionService.GetCategoryForDropDown();
            ViewBag.Categories = new SelectList(Categories, "Id", "Name");
        }
        private void LoadDropDownForTrainers()
        {
            var Trainers = sessionService.GetTrainersForDropDown();
            ViewBag.Trainers = new SelectList(Trainers, "Id", "Name");
        }

        #endregion
    }
}
