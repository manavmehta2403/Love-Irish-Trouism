using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataAccessLayer.Models
{
    public class PDFGenerationModel
    {

        public string ItineraryID { get; set; }
        public string ItineraryName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        //public DateTime? ItineraryStartDate { get; set; }
        //public DateTime? ItineraryEndDate { get; set; }

        public string ItineraryStartDate { get; set; }
        public string ItineraryEndDate { get; set; }

        public string InclusionNotes { get; set; }
        public decimal Personcost { get; set; }
        public decimal Deposit { get; set;}
        public decimal Totalamount { get; set; }
        public long Daycount { get; set; }
        public string NameofDate { get; set; }

        public string NameofItineraryStartDate { get; set; }
        public string NameofItineraryEndDate { get; set; }

        public long Bkid { get; set; }
        public string BookingName { get; set; }
        //public DateTime? EndDate { get; set; }
        public string EndDate { get; set; }
        public string ItemDescription { get; set; }
        public string ItinCurrency { get; set; }
        public string Netunit { get; set; }
        public int NightsDays { get; set; }
        public string ServiceName { get; set; }
        // public DateTime? StartDate { get; set; }
        public string StartDate { get; set; }
        public string Description { get; set; }
        public string SupplierID { get; set; }



        public string ContentID { get; set; }
        public string ContentName { get; set; }
        public string ContentFor { get; set; }
        public string Heading { get; set; }
        public string ReportImage { get; set; }
        public string OnlineImage { get; set; }


        public string Paxid { get; set; }

        public int PaxNumbers { get; set; }


        public decimal Amount { get; set; }
        public decimal GrossfinalAmount { get; set; }
        //public string PaymentID { get; set; }
        //
        //public string Details { get; set; }
        //public decimal Fee { get; set; }
        //public decimal FeePercent { get; set; }
        //public decimal PaymentAmount { get; set; }
        //public string Personname { get; set; }
        //public decimal Sale { get; set; }

        public string status { get; set; }

        public string TextField { get; set; }
        public string BodyHtml { get; set; }

        public string FinalEmailContent { get; set; }

        public string City { get; set; }
        public string Type { get; set; }

        public long ItineraryAutoId { get; set; }
        public string nightdayvalues { get; set; }

        public string Tourtype { get; set; }


    }


    public class CoachBookingModel
    {

        public string ItineraryID { get; set; }
        public string ItineraryName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        //public DateTime? ItineraryStartDate { get; set; }
        //public DateTime? ItineraryEndDate { get; set; }

        public string ItineraryStartDate { get; set; }
        public string ItineraryEndDate { get; set; }

        public string InclusionNotes { get; set; }
        public decimal Personcost { get; set; }
        public decimal Deposit { get; set; }
        public decimal Totalamount { get; set; }
        public long Daycount { get; set; }
        public string NameofDate { get; set; }

        public string NameofItineraryStartDate { get; set; }
        public string NameofItineraryEndDate { get; set; }

        public long Bkid { get; set; }
        public string BookingName { get; set; }
        //public DateTime? EndDate { get; set; }
        public string EndDate { get; set; }
        public string ItemDescription { get; set; }
        public string ItinCurrency { get; set; }
        public string Netunit { get; set; }
        public int NightsDays { get; set; }
        public string ServiceName { get; set; }
        // public DateTime? StartDate { get; set; }
        public string StartDate { get; set; }
        public string Description { get; set; }
        public string SupplierID { get; set; }



        public string ContentID { get; set; }
        public string ContentName { get; set; }
        public string ContentFor { get; set; }
        public string Heading { get; set; }
        public string ReportImage { get; set; }
        public string OnlineImage { get; set; }


        public string Paxid { get; set; }

        public int PaxNumbers { get; set; }


        public decimal Amount { get; set; }
        public decimal GrossfinalAmount { get; set; }


        public string status { get; set; }

        public string TextField { get; set; }
        public string BodyHtml { get; set; }

        public string FinalEmailContent { get; set; }

        public string City { get; set; }
        public string Type { get; set; }

        public long ItineraryAutoId { get; set; }
        public string nightdayvalues { get; set; }

        public string Tourtype { get; set; }
        public List<PassengerNameRoomlist> Passengerroomlist{ get; set; }
        public List<Attractionlist> Attractionlistvalues { get; set; }

    }

    public class PassengerNameRoomlist
    {
        public string Passengerid { get; set; }
        public string PassengerName { get; set; }
        public string Roomtype { get; set; }
        public string OptionTypesName { get; set; }
        public string PassengerName_Room { get; set; }
    }

    public class Attractionlist
    {
        public string Attractions { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Bkid { get; set; }
        public string Attractionwithdate { get; set; }
    }
}
