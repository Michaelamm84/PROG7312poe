using Microsoft.AspNetCore.Mvc;
using PROGPOEDH.Controllers;
using PROGPOEDH.Models;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace PROGPOEDH.Services
{

    //creating the structure of the node within my linked list
    public class ReportNode
    {
       // public int id { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }

        public byte[] Docuemnt { get; set; }
        public byte[] Picture { get; set; }

        public string viewPicture { get; set; }

        //the next feature refernces the next node within the linked list allowing them to be "linked"
        public ReportNode Next { get; set; }

        //creating a constructor for the node 
        public ReportNode(string Category, string Description, string Location, byte[] Docuemnt, byte[] Picture, string viewPicture )
        {
            //this.id = id++;
            this.Category = Category;
            this.Description = Description;
            this.Location = Location;
            this.Docuemnt = Docuemnt;
            this.Picture = Picture;
            this.viewPicture = viewPicture;


            //when initiating a new instance of the node the next node will always be null 
            Next = null;
        }
        //A method to print out the Node 
        public override string ToString()
        {
            return $"[{Location}] {Category}: {Description} : {Docuemnt}: {viewPicture}"; // added last two recently 
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
        public void AddReport(string Name, string Description, string Location, byte[] Document, byte[] Picture, string viewPicture)
        {
            ReportNode newNode = new ReportNode( Name, Description, Location, Document, Picture, viewPicture);


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

        public List<FormModel> GetAllReports()
        {
            List<FormModel> list = new List<FormModel>();
            ReportNode current = Head;

            while (current != null)
            {
                string pictureBase64 = null;
                if (current.Picture != null)
                {
                    pictureBase64 = $"data:image/jpeg;base64,{Convert.ToBase64String(current.Picture)}";
                }

                string documentBase64 = null;
                if (current.Docuemnt != null)
                {
                    documentBase64 = $"data:application/pdf;base64,{Convert.ToBase64String(current.Docuemnt)}";
                }

                list.Add(new FormModel
                {
                    Location = current.Location,
                    Category = current.Category,
                    Description = current.Description,
                    Picture = current.Picture, // keep the raw bytes if needed
                    viewPicture = pictureBase64 ?? documentBase64 // prefer image, fallback to pdf
                });

                current = current.Next;
            }

            return list;
        }


        /*public List<FormModel> GetAllReports()
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
                    Picture = current.Picture,

                    viewPicture = current.Picture != null
                ? $"data:image/jpeg;base64,{Convert.ToBase64String(current.Picture)}"
                : null
                });
                current = current.Next;
            }
            return list;

        }*/

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
            while (current.Next != null)
            {
                if (current.Next.Description == Description)
                {
                    
                    current.Next = current.Next.Next;
                    return;
                }
                else
                {
                    current.Next = current;
                }
            }


        }
        //public ActionResult RenderImage(byte[] picture) => base.File(picture, "image/png");


        public void PopulateList()
        {
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "POTHOLE.jpg");
            byte[] potHolePicture = System.IO.File.ReadAllBytes(imagePath);

            string pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "ivoda.pdf");
            byte[] potHolePDF = System.IO.File.ReadAllBytes(pdfPath);

            if (potHolePicture != null || potHolePDF != null)
            {
                Console.WriteLine("empty file error");
            }


            FormController.reports.AddReport("michael", "found a pothole", "cape town", potHolePDF, potHolePicture, "this is string "); 


         /*   FormController.reports.AddReport("Dean", "found a burst pipe", "Joburg");
            FormController.reports.AddReport("Lia", "found a car", "Durban");
            FormController.reports.AddReport("kevin", "found a bump", "stellenbosch");*/
        }



    }
}

