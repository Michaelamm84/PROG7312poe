using Microsoft.AspNetCore.Mvc;
using PROGPOEDH.Models;

namespace PROGPOEDH.Controllers
{
    public class EventsController : Controller
    {
        /*public IActionResult Index()
        {
            //var events = _db.Events.ToList();

            // Sorted by date
           *//* var sortedEvents = new SortedDictionary<DateTime, List<LocalEvent>>();
            foreach (var ev in events)
            {
                if (!sortedEvents.ContainsKey(ev.Date.Date))
                    sortedEvents[ev.Date.Date] = new List<LocalEvent>();
                sortedEvents[ev.Date.Date].Add(ev);
            }

            // Unique categories for dropdown filter
            ViewBag.Categories = events.Select(e => e.Category).ToHashSet();

            return View("EventIndex", sortedEvents);*//*
        }*/
    }
}
