using Microsoft.AspNetCore.Mvc;
using PROGPOEDH.Models;
using PROGPOEDH.Services;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Controller responsible for handling HTTP requests related to local community events.
/// Manages event display, search operations, and data structure demonstrations (Stack, Queue, Dictionary).
/// </summary>

namespace PROGPOEDH.Controllers

{
    public class EventsController : Controller
    {


        /// <summary>
        /// Static in-memory storage for all events. Shared across requests but resets on app restart.
        /// Initialized with sample data on first load.
        /// </summary>
        
        private static List<LocalEvent> Events = new List<LocalEvent>();
        private EventServics _eventService;

        public EventsController()
        {
            _eventService ??= new EventServics();

            // Populate on first load if empty
            if (!Events.Any())
            {
                PopulateEvents();
            }
        }

        // -------------------- Populate Events --------------------
        /// <summary>
        /// HTTP POST action to initialize and populate the events collection with sample data.
        /// Populates both the main list and service dictionaries (category/date groupings).
        /// </summary

        [HttpPost]
        public IActionResult PopulateEvents()
        {
            _eventService ??= new EventServics();

            Events = new List<LocalEvent>
            {
                new LocalEvent { Id = 1, Title = "Tree Planting Drive", Category = "Environmental", Date = DateTime.Now.AddDays(3), Priority = 2, Description = "Plant trees in community park." },
                new LocalEvent { Id = 2, Title = "Community Cleanup", Category = "Community", Date = DateTime.Now.AddDays(7), Priority = 1, Description = "Litter pick and recycle." },
                new LocalEvent { Id = 3, Title = "Charity Fun Run", Category = "Sports", Date = DateTime.Now.AddDays(10), Priority = 3, Description = "5k for local charities." },
                new LocalEvent { Id = 4, Title = "Food Donation Drive", Category = "Charity", Date = DateTime.Now.AddDays(5), Priority = 2, Description = "Collect non-perishables." },
                new LocalEvent { Id = 5, Title = "Art Exhibition", Category = "Arts", Date = DateTime.Now.AddDays(14), Priority = 4, Description = "Local artists display work." },
                new LocalEvent { Id = 6, Title = "Coding Workshop", Category = "Education", Date = DateTime.Now.AddDays(12), Priority = 3, Description = "Intro to C# workshop." },
                new LocalEvent { Id = 7, Title = "Fundraising Gala", Category = "Charity", Date = DateTime.Now.AddDays(9), Priority = 1, Description = "Evening fundraiser." },
                new LocalEvent { Id = 8, Title = "Sports Day", Category = "Sports", Date = DateTime.Now.AddDays(2), Priority = 4, Description = "School sports activities." },
                new LocalEvent { Id = 9, Title = "Volunteer Meetup", Category = "Community", Date = DateTime.Now.AddDays(6), Priority = 2, Description = "Network and plan." },
                new LocalEvent { Id = 10, Title = "Environmental Awareness Talk", Category = "Environmental", Date = DateTime.Now.AddDays(15), Priority = 3, Description = "Talk on sustainability." },
                new LocalEvent { Id = 11, Title = "Beach Cleanup", Category = "Environmental", Date = DateTime.Now.AddDays(18), Priority = 1, Description = "Join volunteers to clean the beachfront." },
new LocalEvent { Id = 12, Title = "Neighborhood Safety Workshop", Category = "Community", Date = DateTime.Now.AddDays(21), Priority = 3, Description = "Learn about community safety practices." },
new LocalEvent { Id = 13, Title = "Charity Bake Sale", Category = "Charity", Date = DateTime.Now.AddDays(25), Priority = 2, Description = "Sell baked goods to raise funds for local shelters." },
new LocalEvent { Id = 14, Title = "Art in the Park", Category = "Arts", Date = DateTime.Now.AddDays(17), Priority = 4, Description = "Outdoor art exhibition and live painting demos." },
new LocalEvent { Id = 15, Title = "Recycling Workshop", Category = "Environmental", Date = DateTime.Now.AddDays(20), Priority = 2, Description = "Learn effective recycling and composting." },
new LocalEvent { Id = 16, Title = "Book Donation Drive", Category = "Charity", Date = DateTime.Now.AddDays(27), Priority = 3, Description = "Donate books for underprivileged schools." },
new LocalEvent { Id = 17, Title = "Local Football Tournament", Category = "Sports", Date = DateTime.Now.AddDays(23), Priority = 1, Description = "Amateur teams compete in a friendly tournament." },
new LocalEvent { Id = 18, Title = "Community Talent Show", Category = "Community", Date = DateTime.Now.AddDays(16), Priority = 4, Description = "Showcase talents from all age groups." },
new LocalEvent { Id = 19, Title = "Photography Exhibition", Category = "Arts", Date = DateTime.Now.AddDays(19), Priority = 2, Description = "Exhibit of nature and street photography." },
new LocalEvent { Id = 20, Title = "STEM Education Fair", Category = "Education", Date = DateTime.Now.AddDays(30), Priority = 3, Description = "Interactive exhibits promoting science learning." },

            };

            _eventService.PopulateEvents(Events);


            return View("Views/Events/EventIndex.cshtml", Events); 
        }

        // -------------------- Main Page (Hub) --------------------
        /// <summary>
        /// HTTP GET action serving as the events index.
        /// Displays all available event related actions
        /// </summary>
        [HttpGet]
        public IActionResult Main()
        {
            ViewBag.Message = TempData["Message"];
            return View("Views/Events/EventIndex.cshtml", Events);
        }

        // -------------------- Display Views --------------------
        /// <summary>
        /// Displays events grouped by category using Dictionary data structure.
        /// Leverages pre-populated eventsByCategory dictionary for O(1) lookups.
        /// </summary>

        [HttpGet]
        public IActionResult DisplayAllByCategory()
        {
            if (!EventServics.eventsByCategory.Any())
                _eventService.PopulateEvents(Events);

            return View("Views/Events/DisplayByCategory.cshtml", EventServics.eventsByCategory);
        }

        /// <summary>
        /// Demonstrates Stack (LIFO) data structure by converting events list to stack.
        /// Events are processed in reverse order (last in, first out) Allows users to see past created events in the order of how long ago they were made.
        /// </summary>
        /// 


        [HttpGet]
        public IActionResult DisplayByStack()
        {
            // Order oldest -> newest, then push so pop returns newest first (LIFO)
            var orderedOldestFirst = Events.OrderBy(e => e.Date).ToList();

            var stack = _eventService.ConvertListToStack(orderedOldestFirst);
            var stackedList = _eventService.DisplayStack(stack);

            return View("Views/Events/DisplayByStack.cshtml", stackedList);
        }


        /// <summary>
        /// Demonstrates Queue (FIFO) data structure with events ordered by date (most recent first).
        /// Allows users to see most recently created events.
        /// </summary>
        /// <returns>Queue-based event display view</returns>
        /// 

        [HttpGet]
        public IActionResult DisplayByQueue()
        {
            // Order oldest -> newest, enqueue in that order so dequeue returns oldest first (FIFO)
            var orderedOldestFirst = Events.OrderBy(e => e.Date).ToList();

            var queue = _eventService.ConvertListToQueue(orderedOldestFirst);
            var queuedList = _eventService.DisplayQueue(queue);

            return View("Views/Events/DisplayByQueue.cshtml", queuedList);
        }

      

        // -------------------- Search by Category --------------------
        /// <summary>
        /// Searches events by category using pre-built Dictionary lookup.
        /// Records search analytics and provides personalized suggestions.
        /// </summary>
        [HttpGet]
        public IActionResult SearchCategory(string category)
        {
            ViewBag.AllCategories = _eventService.GetUniqueCategories(Events);

            if (string.IsNullOrEmpty(category))
            {
                ViewBag.Suggestions = _eventService.SuggestEvents(5);
                return View("Views/Events/SearchCategory.cshtml", new List<LocalEvent>());
            }

            _eventService.RecordCategorySearch(category);

            var results = EventServics.eventsByCategory.ContainsKey(category)
                ? EventServics.eventsByCategory[category]
                : new List<LocalEvent>();

            ViewBag.SelectedCategory = category;
            ViewBag.Suggestions = _eventService.SuggestEvents(5);

            return View("Views/Events/SearchCategory.cshtml", results);
        }

        // -------------------- Search by Date --------------------
        /// <summary>
        /// Searches events by specific date using normalized DateTime dictionary keys.
        /// Supports date-only searches (ignores time component).
        /// </summary>
        [HttpGet]
        public IActionResult SearchDate(DateTime? date)
        {
            ViewBag.AllDates = _eventService.GetUniqueDates(Events);

            if (!date.HasValue)
            {
                ViewBag.Suggestions = _eventService.SuggestEvents(5);
                return View("Views/Events/SearchDate.cshtml", new List<LocalEvent>());
            }

            var key = date.Value.Date;
            _eventService.RecordDateSearch(key);

            var results = EventServics.eventsByDate.ContainsKey(key)
                ? EventServics.eventsByDate[key]
                : new List<LocalEvent>();

            ViewBag.SelectedDate = key.ToShortDateString();
            ViewBag.Suggestions = _eventService.SuggestEvents(5);

            return View("Views/Events/SearchDate.cshtml", results);
        }

        // -------------------- Search by Title --------------------
        /// <summary>
        /// Performs case-insensitive title search using LINQ filtering.
        /// Provides query-based suggestions and records search analytics.
        /// </summary>
        /// 

        [HttpGet]
        public IActionResult SearchTitle(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                ViewBag.Suggestions = _eventService.SuggestEvents(5);
                return View("Views/Events/SearchByTitle.cshtml", new List<LocalEvent>());
            }

            // Record the title search for analytics
            _eventService.RecordTitleSearch(q);

            // Perform the search and get results
            var results = _eventService.SearchEventsByTitle(q);

            // Set query for display and get query-specific suggestions
            ViewBag.Query = q;
            ViewBag.Suggestions = _eventService.SuggestBasedOnTitleQuery(q, 5);

            return View("Views/Events/SearchByTitle.cshtml", results);
        }

        /* [HttpGet]
         public IActionResult SearchTitle(string q)
         {
             if (string.IsNullOrWhiteSpace(q))
             {
                 ViewBag.Suggestions = _eventService.SuggestEvents(5);
                 return View("Views/Events/SearchByTitle.cshtml", new List<LocalEvent>());
             }

             _eventService.RecordTitleSearch(q);
             var results = _eventService.SearchEventsByTitle(q);

             ViewBag.Query = q;
             ViewBag.Suggestions = _eventService.SuggestBasedOnTitleQuery(q, 5);

             return View("Views/Events/SearchByTitle.cshtml", results);
         }*/
    }
}

