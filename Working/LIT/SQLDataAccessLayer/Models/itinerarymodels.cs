using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataAccessLayer.Models
{
    public class ItineraryModels
    {
        public string ItineraryID { get; set; }
        public string ItineraryName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime ItineraryStartDate { get; set; }
        public DateTime ItineraryEndDate { get; set; }
        public string ArrivalCity { get; set; }
        public string ArrivalFlight { get; set; }
        public string DepartureCity { get; set; }
        public string DepartureFlight { get; set; }
        public string Agent { get; set; }
        public string AgentAssignedTo { get; set; }
        public string Enteredby { get; set; }
        public string Status { get; set; }
        public string Source { get; set; }
        public string Customerid { get; set; }
        public string Bookingid { get; set; }
        public string Supplierid { get; set; }
        public string Clientsid { get; set; }
        public string ItineraryFolderPath { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string DeletedBy { get; set; }
        public bool IsDeleted { get; set; }

        public long ItineraryAutoId { get; set; }

        public string InclusionNotes { get; set; }
        public DateTime? DateCreated { get; set; }

        public string TourlistID { get; set; }

        public string ClientFirstname { get; set; }

        public string ClientLastname { get; set; }
        public string ClientDisplayname { get; set; }

    }


    //public class TreeNodeView
    //{
    //    public string ItineraryName { get; set; }
    //    public string ItineraryFolderName { get; set; }
    //    public ObservableCollection<TreeNodeView> Childtreenodes { get; set; }
    //}



    public class ItineraryReteriveTreeView
    {
        public List<string> ItineraryName { get; set; }
        public List<string> ItineraryFolderName { get; set; }
        public List<string> ParentFoldername { get; set; }


    }




    

}
