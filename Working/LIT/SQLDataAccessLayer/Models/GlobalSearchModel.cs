using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataAccessLayer.Models
{
    public class GlobalSearchModel
    {

    }

    public class GlobalSearchItinerary
    {
        public string ItineraryID { get; set; }
        public string ItineraryName { get; set; }
        public string DisplayName { get; set; }
        public string Customerid { get; set; }
        public string Email { get; set; }
        public string ItineraryStartDate { get; set; }
        public string Agent { get; set; }
        public string AgentAssignedTo { get; set; }
        public string Status { get; set; }
        public bool RecordIsActive { get; set; }
        public string DateCreated { get; set; }
        public string ItineraryAutoID { get; set; }
        public string FinalSell { get; set; }
    }

    public class GlobalSearchSupplier
    {
        public string SupplierId { get; set; }

        public string SupplierAutoID { get; set; }
        public string SupplierName { get; set; }
        public string Servicename { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public bool RecordIsActive { get; set; }
        public string CreatedOn { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
    }

    public class GlobalSearchBooking
    {
        public string ItineraryId { get; set; }
        public string Bkid { get; set; }
        public string BookingName { get; set; }
        public string ItemDescription { get; set; }
        public string BookingStartDate { get; set; }
        public string SupplierRef { get; set; }
        public string BookingStatus { get; set; }
        public string ItineraryAutoId { get; set; }
        public string ItineraryName { get; set; }
        public string Customerid { get; set; }
        public string Email { get; set; }
        public string ItineraryStartDate { get; set; }
        public string ItineraryStatus { get; set; }
        public string Agent { get; set; }
        public string AgentAssignedTo { get; set; }
        public bool RecordIsActive { get; set; }
        public string CreatedOn { get; set; }
    }

    public class GlobalSearchContact
    {
        public string ContactId { get; set; }
        public string ContactName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhoneWork { get; set; }
        public string PhoneHome { get; set; }
        public string Mobile { get; set; }
        public string EmailOne { get; set; }
        public string EmailTwo { get; set; }       
        public bool RecordIsActive { get; set; }
        public string CreatedOn { get; set; }
        public long Contactautoid { get; set; }
    }
    public class GlobalSearchfilters
    {
      public  List<string> filtersItineraries = new List<string>();
      public  List<string> filtersSuppliers = new List<string>();
      public  List<string> filtersContacts = new List<string>();
      public  List<string> filtersBookings = new List<string>();
    }
}
