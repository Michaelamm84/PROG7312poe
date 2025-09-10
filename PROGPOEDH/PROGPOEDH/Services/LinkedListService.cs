using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROGPOEDH.Controllers;
using PROGPOEDH.Models;
using System.Reflection.Metadata;
using System.Xml.Linq;


namespace PROGPOEDH.Services
{

    //creating the structure of the node within my linked list
    public class ReportNode
    {
        public string Category { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public byte[] Document { get; set; }
        public byte[] Picture { get; set; }
        public string viewPicture { get; set; }
        public string viewDocument { get; set; }
        public IFormFile PictureFile { get; set; }
        public IFormFile DocuemntFile { get; set; }
       

        //the next feature refernces the next node within the linked list allowing them to be "linked"
        public ReportNode Next { get; set; }

        //creating a constructor for the node 
        public ReportNode(string Category, string Description, string Location, byte[] Document, byte[] Picture, string viewPicture, IFormFile PictureFile
            ,IFormFile DocumentFile, string viewDocument)
        {
            
            this.Category = Category;
            this.Description = Description;
            this.Location = Location;
            this.Document = Document;
            this.Picture = Picture;
            this.viewPicture = viewPicture;
            this.PictureFile = PictureFile;
            this.viewDocument = viewDocument;
            this.DocuemntFile = DocuemntFile;

            //when initiating a new instance of the node the next node will always be null 
            Next = null;
            this.viewDocument = viewDocument;
        }
        //A method to print out the Node 
        public override string ToString()
        {
            return $"[{Location}] {Category}: {Description} : {Document}: {viewPicture}: {viewDocument}"; // added last two recently 
        }
    }

    //The class used to create the structure of the linked list 
    public class ReportLinkedList
    {
        //storing the value of the prevous and following node 
        private ReportNode Head;
        private ReportNode Tail;


        //if a new instance is created 
        public ReportLinkedList()
        {
            Head = null;
            Tail = null;
        }
        public void AddReport(string Name, string Description, string Location, byte[] Document, byte[] Picture, string viewPicture, IFormFile PictureFile,
            IFormFile DocumentFile, string viewDocument)
        {
            ReportNode newNode = new ReportNode( Name, Description, Location, Document, Picture, viewPicture, PictureFile, DocumentFile, viewDocument);


            if (Head == null) // first bvalue in linked list 
            {
                Head = newNode;
                Tail = newNode;
            }
            else
            {
                Tail.Next = newNode; //links previous node to current node
                Tail = newNode;// makes new node the tail 
            }
        }
        // A method that creates a list to display 
        public List<FormModel> GetAllReports()
        {
            List<FormModel> list = new List<FormModel>();
            ReportNode current = Head;

            while (current != null)
            {
                list.Add(new FormModel
                {
                    Location = current.Location,
                    Category = current.Category,
                    Description = current.Description,
                    Document = current.Document,
                    Picture = current.Picture,
                    viewPicture = $"data:image/jpeg;base64,{Convert.ToBase64String(current.Picture)}",
                    PictureFile = current.PictureFile,
                    DocumentFile = current.DocuemntFile,
                    viewDocument = $"data:application/pdf;base64,{Convert.ToBase64String(current.Document)}"
                });
                current = current.Next;
            }
            return list;
        }

        public void DeleteNode(string Description)
        {
            if (FormController.reports.Head == null)
                return;

            // Special case: deleting the head node
            if (FormController.reports.Head.Description == Description)
            {
                FormController.reports.Head = FormController.reports.Head.Next;
                return;
            }

            var current = FormController.reports.Head;
            //iterates through linked list until descriptions match 
            while (current.Next != null)
            {
                // links the the current node to the node following of the node with descriptions that match 
                //This cuts out the desire node 
                if (current.Next.Description == Description)
                {
                    current.Next = current.Next.Next;
                    return;
                }
                else
                {
                    current = current.Next;
                }
            }
        }
       


        public void PopulateList()
        {
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "POTHOLE.jpg");
            byte[] potHolePicture = System.IO.File.ReadAllBytes(imagePath);

            string pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "ivoda.pdf");
            byte[] potHolePDF = System.IO.File.ReadAllBytes(pdfPath);

            FormController.reports.AddReport("Cape Town", "found a pothole", "cape town", potHolePDF, potHolePicture, "this is string", null,null, "this is docuement"); 

        }
    }
}

