using PROGPOEDH.Controllers;
using PROGPOEDH.Models;
using System.Xml.Linq;

namespace PROGPOEDH.Services
{

    //creating the structure of the node within my linked list
    public class ReportNode
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }

        public byte[] Data { get; set; }

        //the next feature refernces the next node within the linked list allowing them to be "linked"
        public ReportNode Next { get; set; }

        //creating a constructor for the node 
        public ReportNode(string Name, string Description, string Location)
        {
            this.Name = Name;
            this.Description = Description;
            this.Location = Location;


            //when initiating a new instance of the node the next node will always be null 
            Next = null;
        }
        //A method to print out the Node 
        public override string ToString()
        {
            return $"[{Location}] {Name}: {Description}";
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
        public void AddReport(string Name, string Description, string Location)
        {
            ReportNode newNode = new ReportNode(Name, Description, Location);


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
                list.Add(new FormModel
                {
                    Location = current.Location,
                    Name = current.Name,
                    Description = current.Description
                });
                current = current.Next;
            }
            return list;

        }

        public void DeleteNode(string Name)
        {
            if (FormController.reports.Head == null)
                return;

            // Special case: deleting the head node
            if (FormController.reports.Head.Name == Name)
            {
                FormController.reports.Head = FormController.reports.Head.Next;
                return;
            }


            var current = FormController.reports.Head;
            while (current.Next != null)
            {
                if (current.Next.Name == Name)
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

        public void PopulateList()
        {
            FormController.reports.AddReport("michael", "found a pothole", "cape town");
            FormController.reports.AddReport("Dean", "found a burst pipe", "Joburg");
            FormController.reports.AddReport("Lia", "found a car", "Durban");
            FormController.reports.AddReport("kevin", "found a bump", "stellenbosch");
        }



    }
}

