using SQLDataAccessLayer.Models;
using SQLDataAccessLayer.SQLHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

namespace SQLDataAccessLayer.DAL
{
    public class PDFGenerationdal
    {
        Errorlog errobj = new Errorlog();

        public List<PDFGenerationModel> PDFGenerationView(Guid ItineraryID,string typeval)
        {
            List<PDFGenerationModel> listpdfGen = new List<PDFGenerationModel>();

            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@typevalue", 100, typeval, SqlDbType.VarChar);
                SqlHelper.inputparams("@ItineraryID", 100, ItineraryID, SqlDbType.UniqueIdentifier);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_GeneratePdfReport", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {
                        
                        PDFGenerationModel SPOobj = new PDFGenerationModel();
                        SPOobj.ItineraryID = ((dataReader["ItineraryId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ItineraryId"]).ToString();
                        SPOobj.ItineraryName = ((dataReader["ItineraryName"] == DBNull.Value) ? string.Empty : (string)dataReader["ItineraryName"]);
                        SPOobj.DisplayName = (dataReader["DisplayName"] == DBNull.Value) ? string.Empty : (string)dataReader["DisplayName"];
                        SPOobj.Email = (dataReader["Email"] == DBNull.Value) ? string.Empty : (string)dataReader["Email"];
                        SPOobj.Phone = (dataReader["Phone"] == DBNull.Value) ? string.Empty : (string)dataReader["Phone"];
                        SPOobj.ItineraryStartDate = ((dataReader["ItineraryStartDate"] == DBNull.Value) ? string.Empty : (string)dataReader["ItineraryStartDate"]);
                        SPOobj.ItineraryEndDate = ((dataReader["ItineraryEndDate"] == DBNull.Value) ? string.Empty : (string)dataReader["ItineraryEndDate"]);
                        SPOobj.NameofItineraryStartDate = (dataReader["NameofItineraryStartDate"] == DBNull.Value) ? string.Empty : (string)dataReader["NameofItineraryStartDate"];
                        SPOobj.NameofItineraryEndDate = (dataReader["NameofItineraryEndDate"] == DBNull.Value) ? string.Empty : (string)dataReader["NameofItineraryEndDate"];
                        SPOobj.InclusionNotes = ((dataReader["InclusionNotes"] == DBNull.Value) ? string.Empty : (string)dataReader["InclusionNotes"]);
                        SPOobj.status = ((dataReader["Status"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Status"]).ToString();
                        SPOobj.TextField = (dataReader["TextField"] == DBNull.Value) ? string.Empty : (string)dataReader["TextField"];
                        SPOobj.ItineraryAutoId = (dataReader["ItineraryAutoId"] == DBNull.Value) ? 0 : (long)dataReader["ItineraryAutoId"];

                        SPOobj.Bkid = (dataReader["Bkid"] == DBNull.Value) ? 0 : (Int64)dataReader["Bkid"];
                        SPOobj.BookingName = (dataReader["BookingName"] == DBNull.Value) ? string.Empty : (string)dataReader["BookingName"];
                        SPOobj.EndDate = (dataReader["Enddate"] == DBNull.Value) ? string.Empty : (string)(dataReader["Enddate"]);
                        SPOobj.ItemDescription = ((dataReader["ItemDescription"] == DBNull.Value) ? string.Empty : dataReader["ItemDescription"]).ToString();
                        SPOobj.ItinCurrency = (dataReader["ItinCurrency"] == DBNull.Value) ? string.Empty : (string)dataReader["ItinCurrency"];
                        SPOobj.Netunit = (dataReader["Netunit"] == DBNull.Value) ? string.Empty : (string)dataReader["Netunit"];
                        SPOobj.NightsDays =Convert.ToInt32((dataReader["NightsDays"] == DBNull.Value) ? 0 : dataReader["NightsDays"]);
                        SPOobj.ServiceName = (dataReader["ServiceName"] == DBNull.Value) ? string.Empty : (string)dataReader["ServiceName"];
                        SPOobj.StartDate = (dataReader["StartDate"] == DBNull.Value) ? string.Empty : (string)(dataReader["StartDate"]);
                        SPOobj.Description = ((dataReader["Description"] == DBNull.Value) ? string.Empty : (string)dataReader["Description"]).ToString();
                        SPOobj.SupplierID = ((dataReader["SupplierID"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["SupplierID"]).ToString();
                        SPOobj.Type = (dataReader["Type"] == DBNull.Value) ? string.Empty : (string)dataReader["Type"];
                        SPOobj.City = (dataReader["City"] == DBNull.Value) ? string.Empty : (string)dataReader["City"];

                        SPOobj.ContentID = ((dataReader["ContentID"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ContentID"]).ToString();
                        SPOobj.ContentName = (dataReader["ContentName"] == DBNull.Value) ? string.Empty : (string)dataReader["ContentName"];
                        SPOobj.ContentFor = (dataReader["ContentFor"] == DBNull.Value) ? string.Empty : (string)dataReader["ContentFor"];
                        SPOobj.Heading = (dataReader["Heading"] == DBNull.Value) ? string.Empty : (string)dataReader["Heading"];
                        SPOobj.ReportImage = (dataReader["ReportImage"] == DBNull.Value) ? string.Empty : (string)dataReader["ReportImage"];
                        SPOobj.OnlineImage = (dataReader["OnlineImage"] == DBNull.Value) ? string.Empty : (string)dataReader["OnlineImage"];
                        SPOobj.BodyHtml = (dataReader["BodyHtml"] == DBNull.Value) ? string.Empty : (string)dataReader["BodyHtml"];

                        SPOobj.Paxid = ((dataReader["Paxid"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Paxid"]).ToString();                        
                        SPOobj.PaxNumbers = Convert.ToInt32((dataReader["PaxNumbers"] == DBNull.Value) ? 0 : (Int32)dataReader["PaxNumbers"]);
                        SPOobj.GrossfinalAmount = (dataReader["GrossfinalAmount"] == DBNull.Value) ? 0 : (decimal)dataReader["GrossfinalAmount"];
                        SPOobj.Totalamount = (dataReader["Totalamount"] == DBNull.Value) ? 0 : (decimal)dataReader["Totalamount"];

                        SPOobj.Deposit = (dataReader["DepositAmount"] == DBNull.Value) ? 0 : (decimal)dataReader["DepositAmount"];
                        SPOobj.Daycount = (dataReader["Daycount"] == DBNull.Value) ? 0 : (Int64)dataReader["Daycount"];
                        SPOobj.NameofDate = (dataReader["NameofDate"] == DBNull.Value) ? string.Empty : (string)dataReader["NameofDate"];
                        SPOobj.nightdayvalues = ((dataReader["nightdayvalues"] == DBNull.Value) ? 0 : (decimal)dataReader["nightdayvalues"]).ToString();






                        //SPOobj.PaymentID = ((dataReader["PaymentID"] == DBNull.Value) ? Guid.Empty : (Guid)(dataReader["PaymentID"])).ToString();
                        //
                        //SPOobj.Details = (dataReader["Details"] == DBNull.Value) ? string.Empty : (string)dataReader["Details"];
                        //SPOobj.Fee = ((dataReader["Fee"] == DBNull.Value) ? 0 : (decimal)dataReader["Fee"]);
                        //SPOobj.FeePercent = ((dataReader["FeePercent"] == DBNull.Value) ? 0 : (decimal)dataReader["FeePercent"]);
                        //SPOobj.PaymentAmount = ((dataReader["PaymentAmount"] == DBNull.Value) ? 0 : (decimal)dataReader["PaymentAmount"]);

                        //SPOobj.Personname = ((dataReader["Personname"] == DBNull.Value) ? string.Empty : (string)dataReader["Personname"]).ToString();
                        //SPOobj.Sale = ((dataReader["Sale"] == DBNull.Value) ? 0 : (decimal)dataReader["Sale"]);

                        listpdfGen.Add(SPOobj);
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("PDFGenerationdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listpdfGen;

        }


        public List<CoachBookingModel> CoachBookingGenerationView(Guid ItineraryID)
        {
            List<CoachBookingModel> listpdfGen = new List<CoachBookingModel>();

            SqlHelper.parameters = null;
            try
            {                
                SqlHelper.inputparams("@ItineraryID", 100, ItineraryID, SqlDbType.UniqueIdentifier);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_GenerateCoachbookingReport", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {

                        CoachBookingModel SPOobj = new CoachBookingModel();
                        SPOobj.ItineraryID = ((dataReader["ItineraryId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ItineraryId"]).ToString();
                        SPOobj.ItineraryName = ((dataReader["ItineraryName"] == DBNull.Value) ? string.Empty : (string)dataReader["ItineraryName"]);
                        SPOobj.DisplayName = (dataReader["DisplayName"] == DBNull.Value) ? string.Empty : (string)dataReader["DisplayName"];
                        SPOobj.Email = (dataReader["Email"] == DBNull.Value) ? string.Empty : (string)dataReader["Email"];
                        SPOobj.Phone = (dataReader["Phone"] == DBNull.Value) ? string.Empty : (string)dataReader["Phone"];
                        SPOobj.ItineraryStartDate = ((dataReader["ItineraryStartDate"] == DBNull.Value) ? string.Empty : (string)dataReader["ItineraryStartDate"]);
                        SPOobj.ItineraryEndDate = ((dataReader["ItineraryEndDate"] == DBNull.Value) ? string.Empty : (string)dataReader["ItineraryEndDate"]);
                        SPOobj.NameofItineraryStartDate = (dataReader["NameofItineraryStartDate"] == DBNull.Value) ? string.Empty : (string)dataReader["NameofItineraryStartDate"];
                        SPOobj.NameofItineraryEndDate = (dataReader["NameofItineraryEndDate"] == DBNull.Value) ? string.Empty : (string)dataReader["NameofItineraryEndDate"];
                        SPOobj.InclusionNotes = ((dataReader["InclusionNotes"] == DBNull.Value) ? string.Empty : (string)dataReader["InclusionNotes"]);
                        SPOobj.status = ((dataReader["Status"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Status"]).ToString();
                        SPOobj.TextField = (dataReader["TextField"] == DBNull.Value) ? string.Empty : (string)dataReader["TextField"];
                        SPOobj.ItineraryAutoId = (dataReader["ItineraryAutoId"] == DBNull.Value) ? 0 : (long)dataReader["ItineraryAutoId"];

                        SPOobj.Bkid = (dataReader["Bkid"] == DBNull.Value) ? 0 : (Int64)dataReader["Bkid"];
                        SPOobj.BookingName = (dataReader["BookingName"] == DBNull.Value) ? string.Empty : (string)dataReader["BookingName"];
                        SPOobj.EndDate = (dataReader["Enddate"] == DBNull.Value) ? string.Empty : (string)(dataReader["Enddate"]);
                        SPOobj.ItemDescription = ((dataReader["ItemDescription"] == DBNull.Value) ? string.Empty : dataReader["ItemDescription"]).ToString();
                        SPOobj.ItinCurrency = (dataReader["ItinCurrency"] == DBNull.Value) ? string.Empty : (string)dataReader["ItinCurrency"];
                        SPOobj.Netunit = (dataReader["Netunit"] == DBNull.Value) ? string.Empty : (string)dataReader["Netunit"];
                        SPOobj.NightsDays = Convert.ToInt32((dataReader["NightsDays"] == DBNull.Value) ? 0 : dataReader["NightsDays"]);
                        SPOobj.ServiceName = (dataReader["ServiceName"] == DBNull.Value) ? string.Empty : (string)dataReader["ServiceName"];
                        SPOobj.StartDate = (dataReader["StartDate"] == DBNull.Value) ? string.Empty : (string)(dataReader["StartDate"]);
                        SPOobj.Description = ((dataReader["Description"] == DBNull.Value) ? string.Empty : (string)dataReader["Description"]).ToString();
                        SPOobj.SupplierID = ((dataReader["SupplierID"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["SupplierID"]).ToString();
                        SPOobj.Type = (dataReader["Type"] == DBNull.Value) ? string.Empty : (string)dataReader["Type"];
                        SPOobj.City = (dataReader["City"] == DBNull.Value) ? string.Empty : (string)dataReader["City"];

                        SPOobj.ContentID = ((dataReader["ContentID"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ContentID"]).ToString();
                        SPOobj.ContentName = (dataReader["ContentName"] == DBNull.Value) ? string.Empty : (string)dataReader["ContentName"];
                        SPOobj.ContentFor = (dataReader["ContentFor"] == DBNull.Value) ? string.Empty : (string)dataReader["ContentFor"];
                        SPOobj.Heading = (dataReader["Heading"] == DBNull.Value) ? string.Empty : (string)dataReader["Heading"];
                        SPOobj.ReportImage = (dataReader["ReportImage"] == DBNull.Value) ? string.Empty : (string)dataReader["ReportImage"];
                        SPOobj.OnlineImage = (dataReader["OnlineImage"] == DBNull.Value) ? string.Empty : (string)dataReader["OnlineImage"];
                        SPOobj.BodyHtml = (dataReader["BodyHtml"] == DBNull.Value) ? string.Empty : (string)dataReader["BodyHtml"];

                        SPOobj.Paxid = ((dataReader["Paxid"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Paxid"]).ToString();
                        SPOobj.PaxNumbers = Convert.ToInt32((dataReader["PaxNumbers"] == DBNull.Value) ? 0 : (Int32)dataReader["PaxNumbers"]);
                        SPOobj.GrossfinalAmount = Convert.ToInt32((dataReader["GrossfinalAmount"] == DBNull.Value) ? 0 : (decimal)dataReader["GrossfinalAmount"]);
                        SPOobj.Totalamount = (dataReader["Totalamount"] == DBNull.Value) ? 0 : (decimal)dataReader["Totalamount"];

                        SPOobj.Deposit = Convert.ToInt32((dataReader["DepositAmount"] == DBNull.Value) ? 0 : (decimal)dataReader["DepositAmount"]);
                        SPOobj.Daycount = (dataReader["Daycount"] == DBNull.Value) ? 0 : (Int64)dataReader["Daycount"];
                        SPOobj.NameofDate = (dataReader["NameofDate"] == DBNull.Value) ? string.Empty : (string)dataReader["NameofDate"];
                        SPOobj.nightdayvalues = ((dataReader["nightdayvalues"] == DBNull.Value) ? 0 : (decimal)dataReader["nightdayvalues"]).ToString();

                        listpdfGen.Add(SPOobj);
                    }
                    if (dataReader.NextResult())
                    {
                        CoachBookingModel SPOobjcb = new CoachBookingModel();
                        while (dataReader.Read())
                        {
                            PassengerNameRoomlist pnrlobj = new PassengerNameRoomlist();
                            pnrlobj.Passengerid = (dataReader["Passengerid"] == DBNull.Value) ? string.Empty : (string)dataReader["Passengerid"];
                            pnrlobj.PassengerName = (dataReader["PassengerName"] == DBNull.Value) ? string.Empty : (string)dataReader["PassengerName"];
                            pnrlobj.Roomtype = (dataReader["Rommtype"] == DBNull.Value) ? string.Empty : (string)dataReader["Rommtype"];
                            pnrlobj.OptionTypesName = (dataReader["OptionTypesName"] == DBNull.Value) ? string.Empty : (string)dataReader["OptionTypesName"];
                            pnrlobj.PassengerName_Room = (dataReader["PassengerName_Room"] == DBNull.Value) ? string.Empty : (string)dataReader["PassengerName_Room"];
                            SPOobjcb.Passengerroomlist.Add(pnrlobj);
                        }
                        listpdfGen.Add(SPOobjcb);
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("PDFGenerationdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listpdfGen;

        }

        public List<PassengerNameRoomlist> CoachBookingPassengerList(Guid ItineraryID)
        {
            List<PassengerNameRoomlist> listPassnameroomlst = new List<PassengerNameRoomlist>();

            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@ItineraryID", 100, ItineraryID, SqlDbType.UniqueIdentifier);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_GenerateCoachbookingPassengerList", SqlHelper.parameters))
                {                        
                        while (dataReader.Read())
                        {
                            PassengerNameRoomlist pnrlobj = new PassengerNameRoomlist();
                            pnrlobj.Passengerid = ((dataReader["Passengerid"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Passengerid"]).ToString();
                            pnrlobj.PassengerName = (dataReader["PassengerName"] == DBNull.Value) ? string.Empty : (string)dataReader["PassengerName"];
                            pnrlobj.Roomtype = ((dataReader["Rommtype"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Rommtype"]).ToString();
                            pnrlobj.OptionTypesName = (dataReader["OptionTypesName"] == DBNull.Value) ? string.Empty : (string)dataReader["OptionTypesName"];
                            pnrlobj.PassengerName_Room = (dataReader["PassengerName_Room"] == DBNull.Value) ? string.Empty : (string)dataReader["PassengerName_Room"];
                            listPassnameroomlst.Add(pnrlobj);
                        }                   
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("PDFGenerationdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listPassnameroomlst;

        }

        public List<Attractionlist> CoachBookingAttractionList(Guid ItineraryID)
        {
            List<Attractionlist> listatt = new List<Attractionlist>();

            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@ItineraryID", 100, ItineraryID, SqlDbType.UniqueIdentifier);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_GenerateCoachbookingAttractionList", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {
                        Attractionlist attobj = new Attractionlist();
                        attobj.Attractions = (dataReader["Attractions"] == DBNull.Value) ? string.Empty : (string)dataReader["Attractions"];
                        attobj.StartDate = (dataReader["StartDate"] == DBNull.Value) ? string.Empty : (string)dataReader["StartDate"];
                        attobj.EndDate = (dataReader["EndDate"] == DBNull.Value) ? string.Empty : (string)dataReader["EndDate"];
                        attobj.Attractionwithdate = (dataReader["Attractionwithdate"] == DBNull.Value) ? string.Empty : (string)dataReader["Attractionwithdate"];
                        attobj.Bkid = ((dataReader["Bkid"] == DBNull.Value) ? 0 : (long)dataReader["Bkid"]).ToString();
                        listatt.Add(attobj);
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("PDFGenerationdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listatt;

        }

    }
}

