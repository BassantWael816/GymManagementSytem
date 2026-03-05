using GymMangementBLL.Services.Classes;
using GymMangementBLL.Services.Interfaces;
using GymMangementBLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize(Roles ="SuperAdmin")]
    public class TrainerController : Controller
    {
        private readonly ITrainerService trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            this.trainerService = trainerService;
        }
        public ActionResult Index()
        {
            var trainers = trainerService.GetAllTrainers();
            return View(trainers);
        }

        public ActionResult TrainerDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Trainer Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var trainer = trainerService.GetTrainerDetails(id);
            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTrainer(CreateTrainerViewModel createdTrainer)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Check Data And Missing Fields");
                return View(nameof(Create), createdTrainer);
            }

            bool result = trainerService.CreateTrainer(createdTrainer);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Faild To Create , Check Phone And Email";
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Edit(int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Trainer Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var trainer = trainerService.GetTrainerToUpdate(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }

        [HttpPost]
        public ActionResult Edit([FromRoute]int id , TrainerToUpdateViewModel trainerToUpdate)
        {
            if (!ModelState.IsValid)
                return View(nameof(Create), trainerToUpdate);

            bool result = trainerService.UpdateTrainerDetails(trainerToUpdate, id);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Faild To Update";
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Trainer Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var trainer = trainerService.GetTrainerDetails(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.TrainerId = id;
            return View(trainer);
        }

        [HttpPost]
        public ActionResult DeleteConfirmed([FromForm]int id)
        {
            bool result = trainerService.RemoveTrainer(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Faild To Delete";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
