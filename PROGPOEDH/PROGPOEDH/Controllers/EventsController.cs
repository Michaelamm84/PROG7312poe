using Microsoft.AspNetCore.Mvc;
using PROGPOEDH.Models;
using PROGPOEDH.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PROGPOEDH.Controllers
{
    public class EventsController : Controller
    {
        public static List<LocalEvent> Events = new List<LocalEvent>();
        public EventServics EventServics = new EventServics();

        // sets for unique values (you already had these)
        public static HashSet<string> uniqueCategories = new HashSet<string>();
        public static SortedSet<DateTime> uniqueDates = new SortedSet<DateTime>();

        // ---------------- Populate (keeps all structures in sync) ----------------
        public IActionResult PopulateEvents()
        {
            Events.Clear();
            EventServics.eventsByCategory.Clear();
            EventServics.eventsByDate.Clear();
            uniqueCategories.Clear();
            uniqueDates.Clear();

            Events.AddRange(new List<LocalEvent>
            {
                new LocalEvent { Id = 1, Title = "Tree Planting Drive", Category = "Environmental", Date = DateTime.Now.AddDays(3), Priority = 2 },
                new LocalEvent { Id = 2, Title = "Community Cleanup", Category = "Community", Date = DateTime.Now.AddDays(7), Priority = 1 },
                new LocalEvent { Id = 3, Title = "Charity Fun Run", Category = "Sports", Date = DateTime.Now.AddDays(10), Priority = 3 },
                new LocalEvent { Id = 4, Title = "Food Donation Drive", Category = "Charity", Date = DateTime.Now.AddDays(5), Priority = 2 },
                new LocalEvent { Id = 5, Title = "Art Exhibition", Category = "Arts", Date = DateTime.Now.AddDays(14), Priority = 4 },
                new LocalEvent { Id = 6, Title = "Coding Workshop", Category = "Education", Date = DateTime.Now.AddDays(12), Priority = 3 },
                new LocalEvent { Id = 7, Title = "Fundraising Gala", Category = "Charity", Date = DateTime.Now.AddDays(9), Priority = 1 },
                new LocalEvent { Id = 8, Title = "Sports Day", Category = "Sports", Date = DateTime.Now.AddDays(2), Priority = 4 },
                new LocalEvent { Id = 9, Title = "Volunteer Meetup", Category = "Community", Date = DateTime.Now.AddDays(6), Priority = 2 },
                new LocalEvent { Id = 10, Title = "Environmental Awareness Talk", Category = "Environmental", Date = DateTime.Now.AddDays(15), Priority = 3 }
            });

            // populate dictionaries and sets
            foreach (var e in Events)
            {
                // categories/dictionary
                if (!EventServics.eventsByCategory.ContainsKey(e.Category))
                    EventServics.eventsByCategory[e.Category] = new List<LocalEvent>();
                EventServics.eventsByCategory[e.Category].Add(e);

                // dates dict & sets
                var d = e.Date.Date;
                if (!EventServics.eventsByDate.ContainsKey(d))
                    EventServics.eventsByDate[d] = new List<LocalEvent>();
                EventServics.eventsByDate[d].Add(e);

                uniqueCategories.Add(e.Category);
                uniqueDates.Add(d);
            }

            return View("Views/Events/EventIndex.cshtml");
        }

        // ---------------- View all (dictionary) ----------------
        public IActionResult DisplayAllByCategory()
        {
            // Pass the dictionary straight to the view
            return View("Views/Events/DisplayByCategory.cshtml", EventServics.eventsByCategory);
        }

        // ---------------- Stack (LIFO) - newest first ----------------
        public IActionResult DisplayByStack()
        {
            var stack = EventServics.ConvertListToStack(Events); // LIFO
            return View("Views/Events/DisplayByStack.cshtml", stack);
        }

        // ---------------- Queue option (we'll enqueue such that dequeue gives newest->oldest) ----------------
        // You asked to keep "queues" as an option to see events from recently created -> oldest.
        // To make a queue produce that order we enqueue events ordered by Date descending.
        public IActionResult DisplayByQueue()
        {
            var orderedRecentFirst = Events.OrderByDescending(e => e.Date).ToList();
            var q = new Queue<LocalEvent>(orderedRecentFirst); // first dequeued = most recent
            return View("Views/Events/DisplayByQueue.cshtml", q);
        }

        // ---------------- Search by Category (uses set for choices and dictionary for lookup) ----------------
        [HttpGet]
        public IActionResult SearchCatagory(string category)
        {
            ViewBag.AllCategories = uniqueCategories; // set shows unique category choices

            if (string.IsNullOrEmpty(category))
                return View("Views/Events/SearchCatagory.cshtml", new List<LocalEvent>());

            if (EventServics.eventsByCategory.ContainsKey(category))
            {
                ViewBag.SelectedCategory = category;
                return View("Views/Events/SearchCatagory.cshtml", EventServics.eventsByCategory[category]);
            }

            ViewBag.Message = "No events found for that category.";
            return View("Views/Events/SearchCatagory.cshtml", new List<LocalEvent>());
        }

        // ---------------- Search by Date (uses uniqueDates set + eventsByDate dict) ----------------
        [HttpGet]
        public IActionResult SearchDate(DateTime? date)
        {
            ViewBag.AllDates = uniqueDates; // set of unique dates
            if (!date.HasValue)
                return View("Views/Events/SearchDate.cshtml", new List<LocalEvent>());

            var key = date.Value.Date;
            if (EventServics.eventsByDate.ContainsKey(key))
            {
                ViewBag.SelectedDate = key.ToShortDateString();
                return View("Views/Events/SearchDate.cshtml", EventServics.eventsByDate[key]);
            }

            ViewBag.Message = "No events on that date.";
            return View("Views/Events/SearchDate.cshtml", new List<LocalEvent>());
        }



      // search by title 

        [HttpGet]
        public IActionResult SearchByTitle(string q)
        {
            // If no query provided, show the empty search form
            if (string.IsNullOrWhiteSpace(q))
            {
                return View("Views/Events/SearchByTitle.cshtml", new List<LocalEvent>());
            }

            // Use the service to search
            var results = EventServics.SearchEventsByTitle(q);

            ViewBag.Query = q;
            return View("Views/Events/SearchByTitle.cshtml", results);
        }

    }
}
/*using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using PROGPOEDH.Models;
using PROGPOEDH.Services;

namespace PROGPOEDH.Controllers
{
    public class EventsController : Controller
    {
        public static List<LocalEvent> Events = new List<LocalEvent>();
        public EventServics EventServics = new EventServics();
        public static HashSet<string> uniqueCategories = new HashSet<string>();
        public static SortedSet<DateTime> uniqueDates = new SortedSet<DateTime>();


        public IActionResult SubmitEvent (LocalEvent localEvent){

            EventServics eventServics = new EventServics();
            eventServics.addEvent(localEvent);
            //Console.WriteLine(localEvent);
            return View("Views/Events/EventIndex.cshtml");
        }








        //last in first out (LIFO)
        public IActionResult DisplayEvent()
        {
            var stack = EventServics.ConvertListToStack(Events);

            // Display most recent event first
            ViewBag.RecentEvent = stack.Peek(); // top of the stack
            return View("Views/Events/DisplayEvents.cshtml", stack);
        }








        public IActionResult Search()
        {
            return View("Views/Events/Search.cshtml");
        }

        public IActionResult UniqueValues()
        {
            ViewBag.Categories = uniqueCategories;
            ViewBag.Dates = uniqueDates;
            return View("Views/Events/UniqueValues.cshtml");
        }




        [HttpGet]
        public IActionResult FilterByCategory(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                // Show all categories
                ViewBag.AllCategories = EventServics.eventsByCategory.Keys;
                return View("Views/Events/SearchCatagory.cshtml", new List<LocalEvent>());
            }

            // Filter specific category
            if (EventServics.eventsByCategory.ContainsKey(category))
            {
                var filteredEvents = EventServics.eventsByCategory[category];
                ViewBag.SelectedCategory = category;
                ViewBag.AllCategories = EventServics.eventsByCategory.Keys;
                return View("Views/Events/SearchCatagory.cshtml", filteredEvents);
            }

            ViewBag.Message = "No events found for that category.";
            ViewBag.AllCategories = EventServics.eventsByCategory.Keys;
            return View("Views/Events/SearchCatagory.cshtml", new List<LocalEvent>());
        }

        /// <summary>
        /// Dates 
        /// </summary>
        /// <returns></returns>
        public IActionResult DisplayUniqueDates()
        {
            return View("Views/Events/SearchDate.cshtml", uniqueDates);
        }

        [HttpGet]
        public IActionResult SearchByDate(DateTime? date)
        {
            // Create hash table for dates
            var eventsByDate = new System.Collections.Hashtable();

            foreach (var e in Events)
            {
                if (!eventsByDate.ContainsKey(e.Date.Date))
                    eventsByDate[e.Date.Date] = new List<LocalEvent>();

                ((List<LocalEvent>)eventsByDate[e.Date.Date]).Add(e);
            }

            // If a date was provided, show only that date’s events
            if (date.HasValue)
            {
                if (eventsByDate.ContainsKey(date.Value.Date))
                {
                    var selectedEvents = (List<LocalEvent>)eventsByDate[date.Value.Date];
                    ViewBag.SelectedDate = date.Value.ToShortDateString();
                    return View("Views/Events/SearchDate.cshtml", selectedEvents);
                }
                else
                {
                    ViewBag.SelectedDate = date.Value.ToShortDateString();
                    ViewBag.Message = "No events found for this date.";
                    return View("Views/Events/SearchDate.cshtml", new List<LocalEvent>());
                }
            }

            // Default: show search form
            return View("Views/Events/SearchDate.cshtml", null);
        }
  
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////
       
        *//*  public IActionResult FilterByCategory(string category)
          {
              var filteredEvents = EventServics.GetEventsByCategory(category);
              return View("Views/Events/DisplayEvents.cshtml", filteredEvents);
          }

  *//*

        public IActionResult PopulateEvents()
        {
            // Optional: clear existing data to avoid duplicates
            Events.Clear();
            EventServics.eventsByCategory.Clear();

            // Create and populate events directly into the global list
            Events.AddRange(new List<LocalEvent>
    {
        new LocalEvent { Id = 1, Title = "Tree Planting Drive", Category = "Environmental", Date = DateTime.Now.AddDays(3), Priority = 2 },
        new LocalEvent { Id = 2, Title = "Community Cleanup", Category = "Community", Date = DateTime.Now.AddDays(7), Priority = 1 },
        new LocalEvent { Id = 3, Title = "Charity Fun Run", Category = "Sports", Date = DateTime.Now.AddDays(10), Priority = 3 },
        new LocalEvent { Id = 4, Title = "Food Donation Drive", Category = "Charity", Date = DateTime.Now.AddDays(5), Priority = 2 },
        new LocalEvent { Id = 5, Title = "Art Exhibition", Category = "Arts", Date = DateTime.Now.AddDays(14), Priority = 4 },
        new LocalEvent { Id = 6, Title = "Coding Workshop", Category = "Education", Date = DateTime.Now.AddDays(12), Priority = 3 },
        new LocalEvent { Id = 7, Title = "Fundraising Gala", Category = "Charity", Date = DateTime.Now.AddDays(9), Priority = 1 },
        new LocalEvent { Id = 8, Title = "Sports Day", Category = "Sports", Date = DateTime.Now.AddDays(2), Priority = 4 },
        new LocalEvent { Id = 9, Title = "Volunteer Meetup", Category = "Community", Date = DateTime.Now.AddDays(6), Priority = 2 },
        new LocalEvent { Id = 10, Title = "Environmental Awareness Talk", Category = "Environmental", Date = DateTime.Now.AddDays(15), Priority = 3 }
    });

            // Add events to hash table (dictionary) by category
            foreach (var e in Events)
            {
                // Add to dictionary by category
                if (!EventServics.eventsByCategory.ContainsKey(e.Category))
                {
                    EventServics.eventsByCategory[e.Category] = new List<LocalEvent>();
                }
                EventServics.eventsByCategory[e.Category].Add(e);

                // Add unique category and date to sets
                uniqueCategories.Add(e.Category);
                uniqueDates.Add(e.Date.Date);
            }
            // Debug check (optional, helps show the dictionary integration)
            Console.WriteLine("✅ Events populated and categorized successfully!");
            foreach (var category in EventServics.eventsByCategory.Keys)
            {
                Console.WriteLine($"Category: {category} - {EventServics.eventsByCategory[category].Count} event(s)");
            }

            return View("Views/Events/EventIndex.cshtml");
        }


        
          

  






    }
}
*/