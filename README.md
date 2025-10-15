README.md: PROGPOEDH PART 2 Event Management App
Overview
PROGPOEDH is a simple ASP.NET Core MVC app to manage local community events. It uses in-memory storage to display, search, and suggest events with sample data (20 events). Ideal for learning MVC, data structures, and basic search algorithms.
Features

Event List: View all events (GET /Events/Main).
Display Options: Show events by category, stack (newest first), or queue (oldest first).
Search: Find by category, date, or title with suggestions.
Suggestions: Recommends events based on your search history.

Data Structures

List: Stores all events for easy access and sorting.
Dictionary: Groups events by category/date for fast lookups.
Stack/Queue: Demonstrates LIFO (newest first) and FIFO (oldest first) order.
HashSet/SortedSet: Gets unique categories/dates.
Dictionary (Analytics): Tracks search counts for suggestions.

Predictive Search Algorithm

SuggestEvents: Picks top 5 events from your most-searched categories/dates.
SuggestBasedOnTitle: Matches events by title/category keywords.
Uses simple frequency counting (no ML) for quick, personalized suggestions.

How to Run

Install: Ensure Visual Studio 2022 (Community or higher) with .NET 6+ SDK is installed.
Unzip: Extract the provided .zip file to a folder.
Open: In Visual Studio, click "Open a project or solution" and select the .csproj file inside the unzipped folder.
Run: Press F5 (Debug mode) or Ctrl + F5 (without debugger). This starts the app.
Access: Open a browser and go to https://localhost:{port}/Events/Main (port shown in VS output).
