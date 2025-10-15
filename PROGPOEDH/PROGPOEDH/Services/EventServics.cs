using Microsoft.AspNetCore.Mvc;
using PROGPOEDH.Controllers;
using PROGPOEDH.Models;
using System.Collections.Generic;

namespace PROGPOEDH.Services
{
    public class EventServics
    {
        public static Dictionary<string, List<LocalEvent>> eventsByCategory = new Dictionary<string, List<LocalEvent>>();
        public static Dictionary<DateTime, List<LocalEvent>> eventsByDate = new Dictionary<DateTime, List<LocalEvent>>();

        public Stack<LocalEvent> stack => new Stack<LocalEvent>(EventsController.Events);

        public void addEvent(LocalEvent localEvent)
        {
            // add to global list
            EventsController.Events.Add(localEvent);

            // maintain category dictionary
            if (!eventsByCategory.ContainsKey(localEvent.Category))
                eventsByCategory[localEvent.Category] = new List<LocalEvent>();
            eventsByCategory[localEvent.Category].Add(localEvent);

            // maintain date dictionary (key is date only)
            var dateKey = localEvent.Date.Date;
            if (!eventsByDate.ContainsKey(dateKey))
                eventsByDate[dateKey] = new List<LocalEvent>();
            eventsByDate[dateKey].Add(localEvent);

            // update controller sets (you currently hold sets in controller)
            EventsController.uniqueCategories.Add(localEvent.Category);
            EventsController.uniqueDates.Add(dateKey);
        }

        public static Stack<LocalEvent> ConvertListToStack(List<LocalEvent> Events)
        {
            return new Stack<LocalEvent>(Events);
        }

        public List<LocalEvent> GetEventsByCategory(string category)
        {
            if (eventsByCategory.ContainsKey(category))
                return eventsByCategory[category];
            return new List<LocalEvent>();
        }
        public List<LocalEvent> SearchEventsByTitle(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<LocalEvent>();

            var qLower = query.Trim().ToLowerInvariant();

            // Search the global Events list (case-insensitive substring)
            return EventsController.Events
                .Where(e => !string.IsNullOrEmpty(e.Title) && e.Title.ToLowerInvariant().Contains(qLower))
                .ToList();
        }
    }
}
/*using Microsoft.AspNetCore.Mvc;
using PROGPOEDH.Controllers;
using PROGPOEDH.Models;

namespace PROGPOEDH.Services
{
    public class EventServics
    {
        public static Dictionary<string, List<LocalEvent>> eventsByCategory = new Dictionary<string, List<LocalEvent>>();

        public Stack<LocalEvent> stack = new Stack<LocalEvent>(EventsController.Events);
        *//*public void addEvent(LocalEvent localEvent)
        {
            EventsController.Events.Add(localEvent);

            foreach (var item in EventsController.Events)
            {
                Console.WriteLine(item);
            }
        }*//*

        public void addEvent(LocalEvent localEvent)
        {
            EventsController.Events.Add(localEvent);

            // Group events by category using a dictionary
            if (!eventsByCategory.ContainsKey(localEvent.Category))
            {
                eventsByCategory[localEvent.Category] = new List<LocalEvent>();
            }

            eventsByCategory[localEvent.Category].Add(localEvent);
        }
        public static Stack<LocalEvent> ConvertListToStack(List<LocalEvent> Events)
        {
            Stack<LocalEvent> stack = new Stack<LocalEvent>(Events);
            return stack;
        }

        public static void DisplayStack(Stack<LocalEvent> eventStack)
        {
            Console.WriteLine("📦 Displaying all events from the stack:\n");
            foreach (var ev in eventStack)
            {
                Console.WriteLine(ev);
                Console.WriteLine(new string('-', 40));
            }
        }

        public List<LocalEvent> GetEventsByCategory(string category)
        {
            if (eventsByCategory.ContainsKey(category))
            {
                return eventsByCategory[category];
            }

            return new List<LocalEvent>();
        }

        public SortedDictionary<DateTime, List<LocalEvent>> SortEventsByDate(List<LocalEvent> events)
        {
            SortedDictionary<DateTime, List<LocalEvent>> sortedEvents = new SortedDictionary<DateTime, List<LocalEvent>>();

            foreach (var ev in events)
            {
                if (!sortedEvents.ContainsKey(ev.Date.Date))
                {
                    sortedEvents[ev.Date.Date] = new List<LocalEvent>();
                }
                sortedEvents[ev.Date.Date].Add(ev);
            }

            return sortedEvents;
        }


    }
}
*/