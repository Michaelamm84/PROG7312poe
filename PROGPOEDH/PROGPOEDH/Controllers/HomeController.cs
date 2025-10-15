using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using PROGPOEDH.Models;
using System.Diagnostics;
using PROGPOEDH.Services;

namespace PROGPOEDH.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult EventIndex()
        {
            return View("Views/Events/EventIndex.cshtml");
        }

        public IActionResult SearchEvent()
        {
            return View("Views/Events/Search.cshtml");
        }

        public IActionResult AddEvent()
        {
            return View("Views/Events/AddEvent.cshtml");
        }


        public IActionResult DisplayEvents()
        {
            return View("Views/Events/DisplayIndex.cshtml");
        }

        public IActionResult reportIssue()
        {

            return View("Views/Home/ReportIssueView.cshtml");
        }
        //Populates application 
        public IActionResult Populate()
        {
            FormController.reports.PopulateList();
            var allreports = FormController.reports.GetAllReports();
            return View("Views/Home/ShowReports.cshtml", allreports);

        }
        //returns deleted view 
        public IActionResult deleteView()
        {
            return View("Views/Home/DeleteNode.cshtml");
        }
        public IActionResult ShowReports()
        {
            var list = FormController.reports.GetAllReports();

            return View("Views/Home/ShowReports.cshtml", list);

        }
    }
}
