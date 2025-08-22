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

        public IActionResult reportIssue() {

            return View("Views/Home/ReportIssueView.cshtml");
        }

        public IActionResult Populate()
        {
            FormController.reports.PopulateList();
            var allreports = FormController.reports.GetAllReports();
            return View("Views/Home/ShowReports.cshtml" ,allreports);
            
        }

        public IActionResult deleteView()
        {
            return View("Views/Home/DeleteNode.cshtml");
        }
        public IActionResult ShowReports()
        {
            var list = FormController.reports.GetAllReports();

            return View("Views/Home/ShowReports.cshtml", list);
            

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
