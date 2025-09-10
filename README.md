# PROG7312poe ST10396724 

PROG7312 POE

This is a program for municipal service deliveries developed in ASP.NET Core MVC. It enables citizens to report situations like potholes, water issues, street lighting faults, and garbage disposal. Reports are saved in memory in a custom linked list formation comprising ReportLinkedList and ReportNode classes. Reports hold a category, description, location, and may attach images or documents in evidence.



\##To run the app, first build it in Visual Studio or the .NET CLI. Ensure you have the .NET 6 SDK or later installed. After building, run the project and open it from a browser. 



\##Navigation to various features is done through the home page. On the "Report Issue" page, you can add a new report through a selection of category, inputting a description, inputting a location, and the option to input an image or document.



\##Reports are saved in the associated linked list and can then be accessed on the "Show Reports" page, where images and documents are shown through the use of Base64 encoding.



\##The system can also delete reports by description. A separate "Populate" function is implemented, where a sample pothole report with a pre-uploaded picture and PDF document from the project's wwwroot/Images/ folder is added automatically. It can be utilized for testing and demonstration purposes.



\##The program only reflects basic CRUD functionality (create, read, delete) without a database. It reflects, however, a manner in which a linked list may be implemented in memory to retain records. It may then be easily extended by connecting the system to a database, introducing authentication, or geolocation functionality in order to more readily track issues reported.



References 

https://www.geeksforgeeks.org/dsa/linked-list-data-structure/



GitHub Link: 

https://github.com/Michaelamm84/PROG7312poe

