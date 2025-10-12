using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROGPOEDH.Models;
using PROGPOEDH.Services;
using System.Reflection.Metadata;

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

            //coverts IFrom file to byte array so it can be turned into a string 
            using (var ms = new MemoryStream())
            {
                model.PictureFile.CopyTo(ms);
                model.Picture = ms.ToArray();
            }
            //coverts IFrom file to byte array so it can be turned into a string 
            using (var ms = new MemoryStream())
            { model.DocumentFile.CopyTo(ms);
            model.Document = ms.ToArray();
            }
            // add the form data into the linked list
            reports.AddReport(model.Category, model.Description, model.Location, model.Document, model.Picture, model.viewPicture, model.PictureFile, model.DocumentFile, model.viewDocument);

                // Redirect to show all reports
                return RedirectToAction("AllReports");
           
        }

        //A method that displays all stored reports 
        [HttpGet]
        public IActionResult AllReports()
        {
            //calls the method to show all reports 
            var allReports = reports.GetAllReports();

            // pass list of FormModel to view
            return View("Views/Home/ShowReports.cshtml", allReports); 
        }

        //"deleteReport" Calls a method that deletes the report with a matching description 
        [HttpPost]
        public IActionResult DeleteReport(string Description)
        {
            reports.DeleteNode(Description);
            return RedirectToAction("AllReports");
        }
    }
}