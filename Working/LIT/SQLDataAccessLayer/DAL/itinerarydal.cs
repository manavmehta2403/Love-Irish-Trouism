using LITModels.LITModels.Models;
using SQLDataAccessLayer.Models;
using SQLDataAccessLayer.SQLHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

namespace SQLDataAccessLayer.DAL
{
    public class ItineraryDAL
    {
        Errorlog errobj = new Errorlog();

        #region "Loadfunction Start"
        // SQLHelper.SQLHelper_Test sqlHelper = new SQLHelper.SQLHelper_Test();
        public DataSet LoadCommonValues(string Purpose)
        {
            return GetDbvalue(Purpose, "SP_LoadCommonValues");

        }
        public DataSet GetDbvalue(string Purpose, string spname = null)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlHelper.parameters = null;
                if (!string.IsNullOrEmpty(Purpose))
                {

                    SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                }
                //  ds = sqlHelper.FetchDataset(spname, string.Empty);
                ds = SqlHelper.ExecuteDataset(DBConfiguration.instance.ConnectionString, spname, SqlHelper.parameters);
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("ItineraryDAL", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return ds;
        }


        #endregion "Load End"

        #region "Itineraray SaveUpdate Start"
        public string SaveUpdateItinerary(string Purpose, ItineraryModels objitm)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                SqlHelper.inputparams("@ItineraryID", 100, Guid.Parse(objitm.ItineraryID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ItineraryName", 500, objitm.ItineraryName, SqlDbType.VarChar);
                SqlHelper.inputparams("@DisplayName", 500, objitm.DisplayName, SqlDbType.VarChar);
                SqlHelper.inputparams("@Email", 100, objitm.Email, SqlDbType.VarChar);
                SqlHelper.inputparams("@Phone", 100, objitm.Phone, SqlDbType.VarChar);
                SqlHelper.inputparams("@ItineraryStartDate", 100, objitm.ItineraryStartDate, SqlDbType.Date);
                SqlHelper.inputparams("@ItineraryEndDate", 100, objitm.ItineraryEndDate, SqlDbType.Date);
                SqlHelper.inputparams("@ArrivalCity", 100, Guid.Parse(objitm.ArrivalCity), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ArrivalFlight", 200, objitm.ArrivalFlight, SqlDbType.VarChar);
                SqlHelper.inputparams("@DepartureCity", 100, Guid.Parse(objitm.DepartureCity), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@DepartureFlight", 100, objitm.DepartureFlight, SqlDbType.VarChar);
                SqlHelper.inputparams("@Agent", 100, Guid.Parse(objitm.Agent), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@AgentAssignedTo", 100, Guid.Parse(objitm.AgentAssignedTo), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Enteredby", 100, objitm.Enteredby, SqlDbType.VarChar);
                SqlHelper.inputparams("@Status", 100, Guid.Parse(objitm.Status), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Source", 100, Guid.Parse(objitm.Source), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Customerid", 100, Guid.Parse(objitm.Customerid), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Bookingid", 100, Guid.Parse(objitm.Bookingid), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Supplierid", 100, Guid.Parse(objitm.Supplierid), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Clientsid", 100, Guid.Parse(objitm.Clientsid), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ItineraryFolderPath", 500, objitm.ItineraryFolderPath, SqlDbType.VarChar);
                SqlHelper.inputparams("@CreatedBy", 100, Guid.Parse(objitm.CreatedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ModifiedBy", 100, Guid.Parse(objitm.ModifiedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@IsDeleted", 100, objitm.IsDeleted, SqlDbType.Bit);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(objitm.DeletedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ItineraryAutoId", 100, objitm.ItineraryAutoId, SqlDbType.BigInt);
                SqlHelper.inputparams("@InclusionNotes", 1000, objitm.InclusionNotes, SqlDbType.VarChar);
                SqlHelper.inputparams("@DateCreated", 1000, objitm.DateCreated, SqlDbType.DateTime2);
                SqlHelper.inputparams("@TourlistID", 100, Guid.Parse(objitm.TourlistID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ClientFirstname", 100, objitm.ClientFirstname, SqlDbType.VarChar);
                SqlHelper.inputparams("@ClientLastname", 100, objitm.ClientLastname, SqlDbType.VarChar);
                SqlHelper.inputparams("@ClientDisplayname", 100, objitm.ClientDisplayname, SqlDbType.VarChar);

                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_ItinerarySaveUpdate", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("ItineraryDAL", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), objitm.CreatedBy);
            }

            return retstr;

        }
        #endregion "Itineraray SaveUpdate End"

        #region "Itineraray Retrive Start"
        public DataSet ItineraryRetrive(string Purpose, Guid ItineraryID)
        {
            DataSet ds = new DataSet();
            SqlHelper.parameters = null;
            try
            {
                if (!string.IsNullOrEmpty(Purpose))
                {
                    SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                    SqlHelper.inputparams("@ItineraryID", 100, ItineraryID, SqlDbType.UniqueIdentifier);
                    ds = SqlHelper.ExecuteDataset(DBConfiguration.instance.ConnectionString, "SP_ItineraryReterive", SqlHelper.parameters);
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("ItineraryDAL", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return ds;

        }
        #endregion "Itineraray Retrive end"


        #region "Booking SaveUpdate Start"
        public string SaveUpdateBooking(string Purpose, BookingItems objbitm)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                SqlHelper.inputparams("@ItineraryID", 100, Guid.Parse(objbitm.ItineraryID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Bkid", 500, objbitm.BookingID, SqlDbType.BigInt);
                SqlHelper.inputparams("@BookingName", 100, objbitm.BookingName, SqlDbType.VarChar);
                SqlHelper.inputparams("@BookingAutoID", 100, objbitm.BookingAutoID, SqlDbType.BigInt);
                SqlHelper.inputparams("@City", 100, objbitm.City, SqlDbType.VarChar);
                SqlHelper.inputparams("@Comments", 500, objbitm.Comments, SqlDbType.VarChar);
                SqlHelper.inputparams("@Day", 100, objbitm.Day, SqlDbType.VarChar);
                SqlHelper.inputparams("@EndDate", 200, objbitm.Enddate, SqlDbType.DateTime2);
                SqlHelper.inputparams("@EndTime", 100, objbitm.EndTime, SqlDbType.VarChar);
                SqlHelper.inputparams("@ExchRate", 2500, objbitm.ExchRate, SqlDbType.VarChar);
                SqlHelper.inputparams("@GrossAdj", 100, objbitm.GrossAdj, SqlDbType.Decimal);
                SqlHelper.inputparams("@Grossfinal", 100, objbitm.Grossfinal, SqlDbType.Decimal);
                SqlHelper.inputparams("@Grosstotal", 100, objbitm.Grosstotal, SqlDbType.Decimal);
                SqlHelper.inputparams("@Grossunit", 100, objbitm.Grossunit, SqlDbType.VarChar);
                SqlHelper.inputparams("@Invoiced", 100, objbitm.Invoiced, SqlDbType.Bit);
                SqlHelper.inputparams("@ItemDescription", 4000, objbitm.ItemDescription, SqlDbType.VarChar);
                SqlHelper.inputparams("@ItinCurrency", 100, objbitm.ItinCurrency, SqlDbType.VarChar);
                SqlHelper.inputparams("@Netfinal", 100, objbitm.Netfinal, SqlDbType.Decimal);
                SqlHelper.inputparams("@Nettotal", 100, objbitm.Nettotal, SqlDbType.Decimal);
                SqlHelper.inputparams("@Netunit", 100, objbitm.Netunit, SqlDbType.VarChar);
                SqlHelper.inputparams("@NightsDays", 100, objbitm.NtsDays, SqlDbType.Int);
                SqlHelper.inputparams("@PaymentDueDate", 100, objbitm.PaymentDueDate, SqlDbType.DateTime2);
                SqlHelper.inputparams("@Ref", 100, objbitm.Ref, SqlDbType.VarChar);
                SqlHelper.inputparams("@Region", 100, objbitm.Region, SqlDbType.VarChar);
                SqlHelper.inputparams("@ServiceName", 100, objbitm.ServiceName, SqlDbType.VarChar);
                SqlHelper.inputparams("@StartDate", 500, objbitm.StartDate, SqlDbType.DateTime2);
                SqlHelper.inputparams("@StartTime", 100, objbitm.StartTime, SqlDbType.VarChar);
                SqlHelper.inputparams("@Status", 100, Guid.Parse(objbitm.Status), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Type", 500, objbitm.Type, SqlDbType.VarChar);
                SqlHelper.inputparams("@Description", 100, objbitm.ItemDescription, SqlDbType.VarChar);
                SqlHelper.inputparams("@Quantity", 100, objbitm.Qty, SqlDbType.Int);
                SqlHelper.inputparams("@DaysValid", 500, objbitm.DaysValid, SqlDbType.VarChar);
                SqlHelper.inputparams("@AgentCommission", 100, objbitm.AgentCommission, SqlDbType.VarChar);
                SqlHelper.inputparams("@AgentCommissionPercentage", 100, objbitm.AgentCommissionPercentage, SqlDbType.VarChar);
                SqlHelper.inputparams("@BkgCurrencyName", 100, objbitm.BkgCurrencyName, SqlDbType.VarChar);
                SqlHelper.inputparams("@SupplierID", 100, Guid.Parse(objbitm.SupplierID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ServiceTypeID", 100, Guid.Parse(objbitm.ServiceTypeID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ServiceID", 100, Guid.Parse(objbitm.ServiceID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@BkgCurrencyID", 100, Guid.Parse(objbitm.BkgCurrencyID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@PricingOptionId", 100, Guid.Parse(objbitm.PricingOptionId), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@PricingRateID", 100, Guid.Parse(objbitm.PricingRateID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@CreatedBy", 100, Guid.Parse(objbitm.CreatedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ModifiedBy", 100, Guid.Parse(objbitm.ModifiedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@IsDeleted", 100, objbitm.IsDeleted, SqlDbType.Bit);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(objbitm.DeletedBy), SqlDbType.UniqueIdentifier);
                //SqlHelper.inputparams("@CustomCode", 100, objbitm.CustomCode, SqlDbType.VarChar);
                SqlHelper.inputparams("@CityID", 100, Guid.Parse(objbitm.CityID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@RegionID", 100, Guid.Parse(objbitm.RegionID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@MarkupPercentage", 100, objbitm.MarkupPercentage, SqlDbType.VarChar);
                SqlHelper.inputparams("@CommissionPercentage", 100, objbitm.CommissionPercentage, SqlDbType.VarChar);
                SqlHelper.inputparams("@PaymentTerms", 100, objbitm.PaymentTerms, SqlDbType.VarChar);
                SqlHelper.inputparams("@ItinCurrencyID", 100, Guid.Parse(objbitm.ItinCurrencyID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ChangeCurrencyID", 100, Guid.Parse(objbitm.ChangeCurrencyID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@NewNetUnitNotinSupptbl", 100, objbitm.NewNetUnitNotinSupptbl, SqlDbType.Bit);
                SqlHelper.inputparams("@bookingidIdentifier", 100, Guid.Parse(objbitm.BookingidIdentifier), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@PickupLocation", 100, Guid.Parse(objbitm.Pickuplocation), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@DropLocation", 100, Guid.Parse(objbitm.Droplocation), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@SupplierPaymentTermsindays", 100, objbitm.SupplierPaymentTermsindays, SqlDbType.Int);
                SqlHelper.inputparams("@SupplierPaymentDepositAmount", 100, objbitm.SupplierPaymentDepositAmount, SqlDbType.Decimal);
                SqlHelper.inputparams("@SupplierPaymentTermsOverrideindays", 100, objbitm.SupplierPaymentTermsOverrideindays, SqlDbType.Int);
                SqlHelper.inputparams("@SupplierPaymentOverrideDepositAmount", 100, objbitm.SupplierPaymentOverrideDepositAmount, SqlDbType.Decimal); 
                
                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_BookingSaveUpdate", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("ItineraryDAL", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), objbitm.CreatedBy);
            }

            return retstr;

        }
        #endregion "Booking SaveUpdate End"


        public List<BookingItems> BookingItemsRetrive(Guid ItineraryID, string ChangeCurrencyID = null)
        {
            List<BookingItems> listbkgitems = new List<BookingItems>();

            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@ItineraryID", 100, ItineraryID, SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ChangeCurrencyID", 100, (!string.IsNullOrEmpty(ChangeCurrencyID) ? Guid.Parse(ChangeCurrencyID) : Guid.Empty), SqlDbType.UniqueIdentifier);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "GetItineraryBooking", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {
                        BookingItems SPOobj = new BookingItems();
                        SPOobj.ItineraryID = ((dataReader["ItineraryId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ItineraryId"]).ToString();
                        SPOobj.BookingID = ((dataReader["Bkid"] == DBNull.Value) ? 0 : (Int64)dataReader["Bkid"]);
                        SPOobj.BookingName = (dataReader["BookingName"] == DBNull.Value) ? string.Empty : (string)dataReader["BookingName"];
                        SPOobj.BookingAutoID = (dataReader["BookingAutoID"] == DBNull.Value) ? 0 : (Int64)dataReader["BookingAutoID"];
                        SPOobj.City = ((dataReader["City"] == DBNull.Value) ? string.Empty : (string)dataReader["City"]).ToString();
                        SPOobj.CityID = ((dataReader["CityID"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["CityID"]).ToString();
                        SPOobj.Comments = (dataReader["Comments"] == DBNull.Value) ? string.Empty : (string)dataReader["Comments"];
                        SPOobj.Day = (dataReader["Day"] == DBNull.Value) ? string.Empty : (string)dataReader["Day"];
                        SPOobj.Enddate = (dataReader["Enddate"] == DBNull.Value) ? DateTime.Now : Convert.ToDateTime(dataReader["Enddate"]);
                        SPOobj.EndTime = ((dataReader["EndTime"] == DBNull.Value) ? string.Empty : dataReader["EndTime"]).ToString();
                        SPOobj.ExchRate = (dataReader["ExchRate"] == DBNull.Value) ? string.Empty : (string)dataReader["ExchRate"];
                        SPOobj.GrossAdj = (dataReader["GrossAdj"] == DBNull.Value) ? 0 : (decimal)dataReader["GrossAdj"];
                        SPOobj.Grossfinal = (dataReader["Grossfinal"] == DBNull.Value) ? 0 : (decimal)dataReader["Grossfinal"];
                        SPOobj.Grosstotal = (dataReader["Grosstotal"] == DBNull.Value) ? 0 : (decimal)dataReader["Grosstotal"];
                        SPOobj.Grossunit = ((dataReader["Grossunit"] == DBNull.Value) ? string.Empty : (string)dataReader["Grossunit"]).ToString();
                        SPOobj.Invoiced = (dataReader["Invoiced"] == DBNull.Value) ? false : (bool)dataReader["Invoiced"];
                        SPOobj.ItemDescription = (dataReader["ItemDescription"] == DBNull.Value) ? string.Empty : (string)dataReader["ItemDescription"];
                        SPOobj.ItinCurrency = (dataReader["ItinCurrency"] == DBNull.Value) ? string.Empty : (string)dataReader["ItinCurrency"];
                        SPOobj.Netfinal = (dataReader["Netfinal"] == DBNull.Value) ? 0 : (decimal)dataReader["Netfinal"];
                        SPOobj.Nettotal = (dataReader["Nettotal"] == DBNull.Value) ? 0 : (decimal)dataReader["Nettotal"];
                        SPOobj.Netunit = ((dataReader["Netunit"] == DBNull.Value) ? string.Empty : (string)dataReader["Netunit"]).ToString();
                        SPOobj.NtsDays = Convert.ToInt32((dataReader["NightsDays"] == DBNull.Value) ? 0 : (decimal)dataReader["NightsDays"]);
                        SPOobj.PaymentDueDate = (dataReader["PaymentDueDate"] == DBNull.Value) ? DateTime.Now : Convert.ToDateTime(dataReader["PaymentDueDate"]);
                        SPOobj.Ref = (dataReader["Ref"] == DBNull.Value) ? string.Empty : (string)dataReader["Ref"];
                        SPOobj.RegionID = ((dataReader["RegionID"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["RegionID"]).ToString();
                        SPOobj.Region = ((dataReader["Region"] == DBNull.Value) ? string.Empty : (string)dataReader["Region"]).ToString();
                        SPOobj.ServiceName = (dataReader["ServiceName"] == DBNull.Value) ? string.Empty : (string)dataReader["ServiceName"];
                        SPOobj.StartDate = (dataReader["StartDate"] == DBNull.Value) ? DateTime.Now : Convert.ToDateTime(dataReader["StartDate"]);
                        SPOobj.StartTime = ((dataReader["StartTime"] == DBNull.Value) ? string.Empty : (dataReader["StartTime"])).ToString();
                        SPOobj.Status = ((dataReader["Status"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Status"]).ToString();
                        SPOobj.Type = (dataReader["Type"] == DBNull.Value) ? string.Empty : (string)dataReader["Type"];
                        SPOobj.ItemDescription = (dataReader["Description"] == DBNull.Value) ? string.Empty : (string)dataReader["Description"];
                        SPOobj.Qty = Convert.ToInt32((dataReader["Quantity"] == DBNull.Value) ? 0 : (decimal)dataReader["Quantity"]);
                        //(dataReader["Quantity"] == DBNull.Value) ? 0 : (Int32)dataReader["Quantity"];
                        SPOobj.DaysValid = (dataReader["DaysValid"] == DBNull.Value) ? string.Empty : (string)dataReader["DaysValid"];
                        SPOobj.AgentCommission = (dataReader["AgentCommission"] == DBNull.Value) ? string.Empty : (string)dataReader["AgentCommission"];
                        SPOobj.AgentCommissionPercentage = (dataReader["AgentCommissionPercentage"] == DBNull.Value) ? string.Empty : (string)dataReader["AgentCommissionPercentage"];
                        SPOobj.BkgCurrencyName = (dataReader["BkgCurrencyName"] == DBNull.Value) ? string.Empty : (string)dataReader["BkgCurrencyName"];
                        SPOobj.SupplierID = ((dataReader["SupplierID"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["SupplierID"]).ToString();
                        SPOobj.ServiceTypeID = ((dataReader["ServiceTypeID"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ServiceTypeID"]).ToString();
                        SPOobj.ServiceID = ((dataReader["ServiceID"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ServiceID"]).ToString();
                        SPOobj.BkgCurrencyID = ((dataReader["BkgCurrencyID"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["BkgCurrencyID"]).ToString();
                        SPOobj.PricingOptionId = ((dataReader["PricingOptionId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["PricingOptionId"]).ToString();
                        SPOobj.PricingRateID = ((dataReader["PricingRateID"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["PricingRateID"]).ToString();
                        SPOobj.CreatedBy = ((dataReader["CreatedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["CreatedBy"]).ToString();
                        SPOobj.ModifiedBy = ((dataReader["ModifiedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ModifiedBy"]).ToString();
                        SPOobj.DeletedBy = ((dataReader["DeletedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["DeletedBy"]).ToString();
                        SPOobj.IsDeleted = (dataReader["IsDeleted"] == DBNull.Value) ? false : (bool)dataReader["IsDeleted"];
                        SPOobj.MarkupPercentage = (dataReader["MarkupPercentage"] == DBNull.Value) ? string.Empty : (string)dataReader["MarkupPercentage"];
                        SPOobj.CommissionPercentage = (dataReader["CommissionPercentage"] == DBNull.Value) ? string.Empty : (string)dataReader["CommissionPercentage"];
                        SPOobj.PaymentTerms = (dataReader["PaymentTerms"] == DBNull.Value) ? string.Empty : (string)dataReader["PaymentTerms"];
                        SPOobj.ItinCurDisplayFormat = (dataReader["ItineraryCurrencyFormat"] == DBNull.Value) ? string.Empty : (string)dataReader["ItineraryCurrencyFormat"];
                        SPOobj.BkgCurDisplayFormat = (dataReader["BookingCurrencyFormat"] == DBNull.Value) ? string.Empty : (string)dataReader["BookingCurrencyFormat"];
                        SPOobj.ItinCurrencyID = ((dataReader["ItinCurrencyID"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ItinCurrencyID"]).ToString();
                        SPOobj.ChangeCurrencyFormat = (dataReader["ChangeCurrencyFormat"] == DBNull.Value) ? string.Empty : (string)dataReader["ChangeCurrencyFormat"];
                        SPOobj.BookingNotesid = (dataReader["BookingNotesid"] == DBNull.Value) ? 0 : (Int64)dataReader["BookingNotesid"];
                        SPOobj.BookingNote = (dataReader["Booking"] == DBNull.Value) ? string.Empty : (string)dataReader["Booking"];
                        SPOobj.VoucherNote = (dataReader["Voucher"] == DBNull.Value) ? string.Empty : (string)dataReader["Voucher"];
                        SPOobj.Privatemsg = (dataReader["Privatemsg"] == DBNull.Value) ? string.Empty : (string)dataReader["Privatemsg"];
                        SPOobj.NewNetUnitNotinSupptbl = (dataReader["NewNetUnitNotinSupptbl"] == DBNull.Value) ? false : (bool)dataReader["NewNetUnitNotinSupptbl"];
                        SPOobj.BookingidIdentifier = ((dataReader["bookingidIdentifier"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["bookingidIdentifier"]).ToString();
                        SPOobj.Pickuplocation = ((dataReader["PickupLocation"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["PickupLocation"]).ToString();
                        SPOobj.Droplocation = ((dataReader["DropLocation"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["DropLocation"]).ToString();
                        SPOobj.SupplierPaymentTermsindays = (dataReader["SupplierPaymentTermsindays"] == DBNull.Value) ? 0 : (int)dataReader["SupplierPaymentTermsindays"];
                        SPOobj.SupplierPaymentDepositAmount = (dataReader["SupplierPaymentDepositAmount"] == DBNull.Value) ? 0: (decimal)dataReader["SupplierPaymentDepositAmount"];
                        SPOobj.SupplierPaymentTermsOverrideindays = (dataReader["SupplierPaymentTermsOverrideindays"] == DBNull.Value) ? 0 : (int)dataReader["SupplierPaymentTermsOverrideindays"];
                        SPOobj.SupplierPaymentOverrideDepositAmount = (dataReader["SupplierPaymentOverrideDepositAmount"] == DBNull.Value) ? 0 : (decimal)dataReader["SupplierPaymentOverrideDepositAmount"];
                     

                        listbkgitems.Add(SPOobj);
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("ItineraryDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listbkgitems;

        }

        public string DeleteBookingItems(BookingItems objBIt)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@ItineraryId", 100, Guid.Parse(objBIt.ItineraryID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Bkid", 100, objBIt.BookingID, SqlDbType.BigInt);
                SqlHelper.inputparams("@SupplierID", 100, Guid.Parse(objBIt.SupplierID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ServiceID", 100, Guid.Parse(objBIt.ServiceID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@PricingOptionId", 100, Guid.Parse(objBIt.PricingOptionId), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@PricingRateID", 100, Guid.Parse(objBIt.PricingRateID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(objBIt.DeletedBy), SqlDbType.UniqueIdentifier);
                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_BookingItemDelete", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), objBIt.DeletedBy);
            }
            return retstr;

        }


        public string SaveUpdateBookingNotes(string Purpose, BookingItems objbitm)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                SqlHelper.inputparams("@ItineraryID", 100, Guid.Parse(objbitm.ItineraryID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Bkid", 500, objbitm.BookingID, SqlDbType.BigInt);
                SqlHelper.inputparams("@BookingNotesid", 500, objbitm.BookingNotesid, SqlDbType.BigInt);
                SqlHelper.inputparams("@BookingNote", 100, objbitm.BookingNote, SqlDbType.VarChar);
                SqlHelper.inputparams("@VoucherNote", 100, objbitm.VoucherNote, SqlDbType.VarChar);
                SqlHelper.inputparams("@PrivateMsg", 100, objbitm.Privatemsg, SqlDbType.VarChar);
                SqlHelper.inputparams("@CreatedBy", 100, Guid.Parse(objbitm.CreatedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ModifiedBy", 100, Guid.Parse(objbitm.ModifiedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@IsDeleted", 100, objbitm.IsDeleted, SqlDbType.Bit);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(objbitm.DeletedBy), SqlDbType.UniqueIdentifier);

                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_BookingNotesSaveUpdate", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("ItineraryDAL", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), objbitm.CreatedBy);
            }

            return retstr;

        }

        /* Follow up notes*/

        public string SaveUpdateFollowupTasks(string purpose, FollowupModel follobj)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@Action", 100, purpose, SqlDbType.VarChar);
                SqlHelper.inputparams("@Taskid", 100, Guid.Parse(follobj.Taskid), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@TaskName", 500, follobj.TaskName, SqlDbType.VarChar);
                SqlHelper.inputparams("@Notes", 500, follobj.Notes, SqlDbType.VarChar);
                SqlHelper.inputparams("@DateDue", 100, follobj.DateDue, SqlDbType.DateTime2);
                SqlHelper.inputparams("@DateCreated", 100, follobj.DateCreated, SqlDbType.DateTime2);
                SqlHelper.inputparams("@Assignedto", 100, Guid.Parse(follobj.Assignedto), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Datecompleted", 100, follobj.Datecompleted, SqlDbType.DateTime2);
                SqlHelper.inputparams("@Bookingid", 100, follobj.Bookingid, SqlDbType.Int);
                SqlHelper.inputparams("@Itineraryid", 100, Guid.Parse(follobj.Itineraryid), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@CreatedBy", 100, Guid.Parse(follobj.CreatedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ModifiedBy", 100, Guid.Parse(follobj.ModifiedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@IsDeleted", 100, follobj.IsDeleted, SqlDbType.Bit);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(follobj.DeletedBy), SqlDbType.UniqueIdentifier);

                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SaveUpdateFollowupTask", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("ItineraryDAL", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), follobj.CreatedBy);
            }

            return retstr;
        }

        public string DeleteFollowupTasks(FollowupModel follobj)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@Taskid", 100, Guid.Parse(follobj.Taskid), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(follobj.DeletedBy), SqlDbType.UniqueIdentifier);

                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_FollowupTaskDelete", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("ItineraryDAL", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), follobj.CreatedBy);
            }

            return retstr;
        }


        public List<FollowupModel> FollowupRetrive(Guid ItineraryID, long Bookingid)
        {
            List<FollowupModel> listfw = new List<FollowupModel>();

            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@ItineraryID", 100, ItineraryID, SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Bookingid", 100, Bookingid, SqlDbType.Int);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_GetAllFollowupTask", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {
                        FollowupModel FWobj = new FollowupModel();

                        FWobj.Taskid = ((dataReader["Taskid"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Taskid"]).ToString();
                        FWobj.TaskName = (dataReader["TaskName"] == DBNull.Value) ? string.Empty : (string)dataReader["TaskName"];
                        FWobj.Notes = ((dataReader["Notes"] == DBNull.Value) ? string.Empty : (string)dataReader["Notes"]).ToString();
                        FWobj.DateDue = ((dataReader["DateDue"] == DBNull.Value) ? null : (DateTime)dataReader["DateDue"]);
                        FWobj.DateCreated = ((dataReader["DateCreated"] == DBNull.Value) ? null : (DateTime)dataReader["DateCreated"]);
                        FWobj.Assignedto = ((dataReader["Assignedto"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Assignedto"]).ToString();
                        FWobj.Datecompleted = (dataReader["Datecompleted"] == DBNull.Value) ? null : (DateTime)dataReader["Datecompleted"];
                        FWobj.Itineraryid = ((dataReader["Itineraryid"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Itineraryid"]).ToString();
                        FWobj.Bookingid = ((dataReader["Bookingid"] == DBNull.Value) ? 0 : (Int64)dataReader["Bookingid"]);
                        FWobj.CreatedBy = ((dataReader["CreatedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["CreatedBy"]).ToString();
                        FWobj.ModifiedBy = ((dataReader["ModifiedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ModifiedBy"]).ToString();
                        FWobj.DeletedBy = ((dataReader["DeletedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["DeletedBy"]).ToString();
                        FWobj.IsDeleted = (dataReader["IsDeleted"] == DBNull.Value) ? false : (bool)dataReader["IsDeleted"];
                        listfw.Add(FWobj);
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("ItineraryDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listfw;

        }

        #region "Booking SaveUpdate BookingFinalTotals"
        public string SaveUpdateBookingFinalTotals(string Purpose, BookingItems objbitm)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                SqlHelper.inputparams("@ItineraryBookingTotalId", 100, Guid.Parse(objbitm.ItineraryBookingTotalId), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ItineraryId", 500, Guid.Parse(objbitm.ItineraryID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@NetTotal", 100, objbitm.SumofNetTotal, SqlDbType.Decimal);
                SqlHelper.inputparams("@GrossTotal", 100, objbitm.SumofGrossTotal, SqlDbType.Decimal);
                SqlHelper.inputparams("@NetFinal", 100, objbitm.SumofNetFinal, SqlDbType.Decimal);
                SqlHelper.inputparams("@GrossFinal", 500, objbitm.SumofGrossFinal, SqlDbType.Decimal);
                SqlHelper.inputparams("@GrossAdjustment", 100, objbitm.SumofGrossAdjustment, SqlDbType.Decimal);
                SqlHelper.inputparams("@MarginAdjustmentOverrideall", 200, objbitm.MarginAdjustmentOverrideall, SqlDbType.Decimal);
                SqlHelper.inputparams("@MarginAdjustmentGross", 500, objbitm.MarginAdjustmentGross, SqlDbType.Decimal);
                SqlHelper.inputparams("@GrossAdjustmentMarkup", 100, objbitm.GrossAdjustmentMarkup, SqlDbType.Decimal);
                SqlHelper.inputparams("@GrossAdjustmentGross", 200, objbitm.GrossAdjustmentGross, SqlDbType.Decimal);
                SqlHelper.inputparams("@GrossAdjustmentFinalOverride", 200, objbitm.GrossAdjustmentFinalOverride, SqlDbType.Decimal);
                SqlHelper.inputparams("@FinalMargin", 500, objbitm.FinalMargin, SqlDbType.Decimal);
                SqlHelper.inputparams("@FinalSell", 100, objbitm.FinalSell, SqlDbType.Decimal);
                SqlHelper.inputparams("@FinalAgentCommission", 200, objbitm.FinalAgentCommission, SqlDbType.Decimal);
                SqlHelper.inputparams("@CreatedBy", 100, Guid.Parse(objbitm.CreatedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ModifiedBy", 100, Guid.Parse(objbitm.ModifiedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@IsDeleted", 100, objbitm.IsDeleted, SqlDbType.Bit);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(objbitm.DeletedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@FinalMarginpercentage", 100, objbitm.FinalMarginpercentage, SqlDbType.Decimal);
                



                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_ItineraryBookingTotalSaveUpdate", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("ItineraryDAL", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), objbitm.CreatedBy);
            }

            return retstr;

        }
        #endregion "Booking SaveUpdate BookingFinalTotals"


        public List<BookingItems> BookingFinalTotalsRetrive(Guid ItineraryID)
        {
            List<BookingItems> listbkgitems = new List<BookingItems>();

            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@ItineraryID", 100, ItineraryID, SqlDbType.UniqueIdentifier);                
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_GetItineraryBookingTotal", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {
                        BookingItems SPOobj = new BookingItems();

                        SPOobj.ItineraryBookingTotalId = ((dataReader["ItineraryBookingTotalId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ItineraryBookingTotalId"]).ToString(); 
                        SPOobj.ItineraryID = ((dataReader["ItineraryId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ItineraryId"]).ToString();
                        SPOobj.SumofNetTotal = (dataReader["NetTotal"] == DBNull.Value) ? 0 : (decimal)dataReader["NetTotal"];
                        SPOobj.SumofGrossTotal = (dataReader["GrossTotal"] == DBNull.Value) ? 0 : (decimal)dataReader["GrossTotal"];
                        SPOobj.SumofNetFinal = (dataReader["NetFinal"] == DBNull.Value) ? 0 : (decimal)dataReader["NetFinal"];
                        SPOobj.SumofGrossFinal = (dataReader["GrossFinal"] == DBNull.Value) ? 0 : (decimal)dataReader["GrossFinal"];
                        SPOobj.SumofGrossAdjustment = (dataReader["GrossAdjustment"] == DBNull.Value) ? 0 : (decimal)dataReader["GrossAdjustment"];
                        SPOobj.MarginAdjustmentOverrideall = (dataReader["MarginAdjustmentOverrideall"] == DBNull.Value) ? 0 : (decimal)dataReader["MarginAdjustmentOverrideall"];
                        SPOobj.MarginAdjustmentGross = (dataReader["MarginAdjustmentGross"] == DBNull.Value) ? 0 : (decimal)dataReader["MarginAdjustmentGross"];
                        SPOobj.GrossAdjustmentMarkup = (dataReader["GrossAdjustmentMarkup"] == DBNull.Value) ? 0 : (decimal)dataReader["GrossAdjustmentMarkup"];
                        SPOobj.GrossAdjustmentGross = (dataReader["GrossAdjustmentGross"] == DBNull.Value) ? 0 : (decimal)dataReader["GrossAdjustmentGross"];                       
                        SPOobj.GrossAdjustmentFinalOverride = (dataReader["GrossAdjustmentFinalOverride"] == DBNull.Value) ? 0 : (decimal)dataReader["GrossAdjustmentFinalOverride"];
                        SPOobj.FinalMargin = (dataReader["FinalMargin"] == DBNull.Value) ? 0 : (decimal)dataReader["FinalMargin"];
                        SPOobj.FinalSell = (dataReader["FinalSell"] == DBNull.Value) ? 0 : (decimal)dataReader["FinalSell"];
                        SPOobj.FinalAgentCommission = (dataReader["FinalAgentCommission"] == DBNull.Value) ? 0 : (decimal)dataReader["FinalAgentCommission"];
                        SPOobj.CreatedBy = ((dataReader["CreatedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["CreatedBy"]).ToString();
                        SPOobj.ModifiedBy = ((dataReader["ModifiedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ModifiedBy"]).ToString();
                        SPOobj.DeletedBy = ((dataReader["DeletedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["DeletedBy"]).ToString();
                        SPOobj.IsDeleted = (dataReader["IsDeleted"] == DBNull.Value) ? false : (bool)dataReader["IsDeleted"];
                        SPOobj.FinalMarginpercentage = (dataReader["FinalMarginpercentage"] == DBNull.Value) ? 0 : (decimal)dataReader["FinalMarginpercentage"];
                        
                        listbkgitems.Add(SPOobj);
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("ItineraryDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listbkgitems;

        }

       


    }

}
