using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROGPOEDH.Models;
using PROGPOEDH.Services;

namespace PROGPOEDH.Controllers
{
    public class FormController : Controller
    {
        // static forces all references of the class to use this list 
        public static ReportLinkedList reports = new ReportLinkedList();

        [HttpGet]
        public IActionResult ReportIssue()
        {
            return View(new FormModel()); // empty form
        }

        [HttpPost]
        public IActionResult SubmitReport(FormModel model)
        {

            //model.Picture = System.IO.File.ReadAllBytes(model.Picture);
            using (var ms = new MemoryStream())
            {
                model.PictureFile.CopyTo(ms);
                model.Picture = ms.ToArray();
            }
            //convert model docuemnt to byte array 
            // remove 


            // add the form data into the linked list
            reports.AddReport(model.Category, model.Description, model.Location, model.Document, model.Picture, model.viewPicture, model.PictureFile);

                // Redirect to show all reports
                return RedirectToAction("AllReports");
           
        }

        [HttpGet]
        public IActionResult AllReports()
        {
            var allReports = reports.GetAllReports();
            
            return View("Views/Home/ShowReports.cshtml", allReports); // pass list of FormModel to view
        }

        [HttpPost]
        public IActionResult DeleteReport(string Name)
        {
            reports.DeleteNode(Name);

            return RedirectToAction("AllReports");

        }
        // static so it persists between requests

        public IActionResult RenderImage(byte[] Picture)
        {

            
           

            return File(Picture, "image/jpeg");
        }
    }
}