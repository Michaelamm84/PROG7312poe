using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROGPOEDH.Models;
using PROGPOEDH.Services;

namespace PROGPOEDH.Controllers
{
    public class FormController : Controller
    {
        private static ReportLinkedList reports = new ReportLinkedList();

        [HttpGet]
        public IActionResult ReportIssue()
        {
            return View(new FormModel()); // empty form
        }

        [HttpPost]
        public IActionResult SubmitReport(FormModel model)
        {
            if (ModelState.IsValid)
            {
                // add the form data into the linked list
                reports.AddReport(model.Name, model.Description, model.Location);

                // Redirect to show all reports
                return RedirectToAction("AllReports");
            }
            return View("Error state isnt valid ", model); // if validation fails, redisplay form
        }

        [HttpGet]
        public IActionResult AllReports()
        {
            var allReports = reports.GetAllReports();
            return View("Views/Home/ShowReports.cshtml", allReports); // pass list of FormModel to view
        }

    }
    // static so it persists between requests


}