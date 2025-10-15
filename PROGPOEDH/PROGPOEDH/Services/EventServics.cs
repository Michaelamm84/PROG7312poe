using PROGPOEDH.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// Service layer for event management, data structure operations, search functionality, 
/// and user behavior analytics. Provides separation of concerns from controller logic.
/// Uses static storage for simplicity (shared across requests, resets on app restart).
/// </summary>

namespace PROGPOEDH.Services
{
    public class EventServics
    {
        // =================== Core Storage ===================
        public static Dictionary<string, List<LocalEvent>> eventsByCategory = new();
        public static Dictionary<DateTime, List<LocalEvent>> eventsByDate = new();

        // =================== Search Analytics ===================
        private static Dictionary<string, int> categorySearchCounts = new();
        private static Dictionary<DateTime, int> dateSearchCounts = new();
        private static Dictionary<string, int> titleSearchCounts = new(StringComparer.OrdinalIgnoreCase);

        // =================== Event Population ===================
        public void PopulateEvents(List<LocalEvent> events)
        {
            foreach (var e in events)
            {
                if (!eventsByCategory.ContainsKey(e.Category))
                    eventsByCategory[e.Category] = new List<LocalEvent>();
                eventsByCategory[e.Category].Add(e);
                var dateKey = e.Date.Date;
                if (!eventsByDate.ContainsKey(dateKey))
                    eventsByDate[dateKey] = new List<LocalEvent>();
                eventsByDate[dateKey].Add(e);
            }
        }

        // =================== Dictionary/Stack/Queue Conversion ===================
        public Dictionary<string, List<LocalEvent>> ConvertListToDictionary(List<LocalEvent> events)
        {
            var dict = new Dictionary<string, List<LocalEvent>>();
            foreach (var e in events)
            {
                if (!dict.ContainsKey(e.Category))
                    dict[e.Category] = new List<LocalEvent>();
                dict[e.Category].Add(e);
            }
            return dict;
        }

        public Stack<LocalEvent> ConvertListToStack(List<LocalEvent> events) =>
            new Stack<LocalEvent>(events);

        public List<LocalEvent> DisplayStack(Stack<LocalEvent> stack) =>
            stack.ToList();

        public Queue<LocalEvent> ConvertListToQueue(List<LocalEvent> events) =>
            new Queue<LocalEvent>(events);

        public List<LocalEvent> DisplayQueue(Queue<LocalEvent> queue) =>
            queue.ToList();

        // =================== Helpers ===================
        /// <summary>
        /// Extracts unique categories using HashSet for O(1) uniqueness checks.
        /// LINQ Select projects events to categories, HashSet removes duplicates.
        /// </summary>
        public HashSet<string> GetUniqueCategories(List<LocalEvent> events) =>
            new HashSet<string>(events.Select(e => e.Category));

        /// <summary>
        /// Extracts unique dates using SortedSet for automatic sorting and uniqueness.
        /// Returns normalized dates (date-only) in ascending order.
        /// </summary>

        public SortedSet<DateTime> GetUniqueDates(List<LocalEvent> events) =>
            new SortedSet<DateTime>(events.Select(e => e.Date.Date));


      //=================== Search Methods ===================//
        public List<LocalEvent> SearchEventsByTitle(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<LocalEvent>();

            var all = eventsByCategory.Values.SelectMany(x => x).ToList();
            return all.Where(e => e.Title.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // =================== Simple Analytics ===================
        public void RecordCategorySearch(string category)
        {
            if (string.IsNullOrEmpty(category)) return;
            categorySearchCounts[category] = categorySearchCounts.GetValueOrDefault(category) + 1;
        }

        public void RecordDateSearch(DateTime date)
        {
            dateSearchCounts[date] = dateSearchCounts.GetValueOrDefault(date) + 1;
        }

        public void RecordTitleSearch(string title)
        {
            if (string.IsNullOrEmpty(title)) return;
            titleSearchCounts[title] = titleSearchCounts.GetValueOrDefault(title) + 1;
        }

        

        // =================== Suggestion Algorithms ===================/// 
        

        // Generates personalized event suggestions based on search history.
        /// Algorithm:
        /// 1. Identify top 3 most-searched categories and dates by use of a point system 
        /// 2. Filter events matching these patterns
        /// 3. Return up to maxSuggestions distinct results
        /// 4. Fallback to random selection if no search history
        /// Time complexity: O(M log M + N) where M=unique searches, N=events
        /// </summary>
        public List<LocalEvent> SuggestEvents(int maxSuggestions = 5)
        {
            var allEvents = eventsByCategory.Values.SelectMany(x => x).ToList();

            // Rank by what the user searches most often (categories, dates, and now titles)
            var topCategories = categorySearchCounts
                .OrderByDescending(kv => kv.Value)
                .Take(3)
                .Select(kv => kv.Key)
                .ToHashSet();

            var topDates = dateSearchCounts
                .OrderByDescending(kv => kv.Value)
                .Take(3)
                .Select(kv => kv.Key)
                .ToHashSet();

            var topTitles = titleSearchCounts
                .OrderByDescending(kv => kv.Value)
                .Take(3)
                .Select(kv => kv.Key)
                .ToHashSet();

            var suggestions = allEvents
                .Where(e => topCategories.Contains(e.Category) ||
                           topDates.Contains(e.Date.Date) ||
                           topTitles.Contains(e.Title))
                .Distinct()
                .Take(maxSuggestions)
                .ToList();

            // Fallback to random top 5 if no search history
            if (suggestions.Count == 0)
                suggestions = allEvents.OrderBy(e => Guid.NewGuid()).Take(maxSuggestions).ToList();

            return suggestions;
        }

        /// <summary>
        /// Query-specific suggestions matching title or category patterns.
        /// Simple containment search across title and category fields.
        /// </summary>
        public List<LocalEvent> SuggestBasedOnTitleQuery(string query, int max = 5)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<LocalEvent>();

            var all = eventsByCategory.Values.SelectMany(x => x).ToList();
            return all
                .Where(e => e.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                            e.Category.Contains(query, StringComparison.OrdinalIgnoreCase))
                .Take(max)
                .ToList();
        }
    }
}
