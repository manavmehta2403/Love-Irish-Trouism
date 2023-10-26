using SQLDataAccessLayer.Models;
using SQLDataAccessLayer.SQLHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SQLDataAccessLayer.DAL
{
    public class GlobalSearchdal
    {
        Errorlog errobj = new Errorlog();

        public List<GlobalSearchItinerary> GlobalSearchItinerary(string Fieldvalue, string wherecontains, string keyval)
        {
            List<GlobalSearchItinerary> listGSItinerary = new List<GlobalSearchItinerary>();

            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@Field", 100, Fieldvalue, SqlDbType.VarChar);
                SqlHelper.inputparams("@Where", 100, wherecontains, SqlDbType.VarChar);
                SqlHelper.inputparams("@Key", 100, keyval, SqlDbType.VarChar);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_SearchFilterItinerary", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {

                        GlobalSearchItinerary SPOobj = new GlobalSearchItinerary();
                        SPOobj.ItineraryID = ((dataReader["ItineraryID"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ItineraryID"]).ToString();
                        SPOobj.ItineraryAutoID = ((dataReader["ItineraryAutoID"] == DBNull.Value) ? 0 : (long)dataReader["ItineraryAutoID"]).ToString();
                        SPOobj.ItineraryName = ((dataReader["ItineraryName"] == DBNull.Value) ? string.Empty : (string)dataReader["ItineraryName"]);
                        SPOobj.DisplayName = (dataReader["DisplayName"] == DBNull.Value) ? string.Empty : (string)dataReader["DisplayName"];
                        SPOobj.Customerid = ((dataReader["Customerid"] == DBNull.Value) ? string.Empty : (string)dataReader["Customerid"]).ToString();
                        SPOobj.Email = (dataReader["Email"] == DBNull.Value) ? string.Empty : (string)dataReader["Email"];
                        SPOobj.ItineraryStartDate = ((dataReader["ItineraryStartDate"] == DBNull.Value) ? string.Empty : (string)dataReader["ItineraryStartDate"]);
                        SPOobj.Agent = ((dataReader["Agent"] == DBNull.Value) ? string.Empty : (string)dataReader["Agent"]);
                        SPOobj.AgentAssignedTo = ((dataReader["AgentAssignedTo"] == DBNull.Value) ? string.Empty : (string)dataReader["AgentAssignedTo"]);

                        SPOobj.Status = (dataReader["Status"] == DBNull.Value) ? string.Empty : (string)dataReader["Status"];
                        SPOobj.RecordIsActive = (dataReader["RecordIsActive"] == DBNull.Value) ? false : (bool)dataReader["RecordIsActive"];
                        SPOobj.DateCreated = (dataReader["DateCreated"] == DBNull.Value) ? string.Empty : (string)(dataReader["DateCreated"]);
                        SPOobj.FinalSell = ((dataReader["FinalSell"] == DBNull.Value) ? string.Empty: (string)dataReader["FinalSell"]);

                        listGSItinerary.Add(SPOobj);
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("GlobalSearchdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listGSItinerary;

        }

        public List<GlobalSearchSupplier> GlobalSearchSupplier(string Fieldvalue,string wherecontains, string keyval)
        {
            List<GlobalSearchSupplier> listGSSupplier = new List<GlobalSearchSupplier>();

            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@Field", 100, Fieldvalue, SqlDbType.VarChar);
                SqlHelper.inputparams("@Where", 100, wherecontains, SqlDbType.VarChar);
                SqlHelper.inputparams("@Key", 100, keyval, SqlDbType.VarChar);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_SearchFilterSupplier", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {
                        GlobalSearchSupplier SPOobj = new GlobalSearchSupplier();
                        SPOobj.SupplierId = ((dataReader["SupplierId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["SupplierId"]).ToString();
                        SPOobj.SupplierAutoID = ((dataReader["SupplierAutoId"] == DBNull.Value) ? 0 : (long)dataReader["SupplierAutoId"]).ToString();
                        SPOobj.SupplierName = ((dataReader["SupplierName"] == DBNull.Value) ? string.Empty : (string)dataReader["SupplierName"]);
                        SPOobj.Street = (dataReader["Street"] == DBNull.Value) ? string.Empty : (string)dataReader["Street"];
                        SPOobj.City = ((dataReader["City"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["City"]).ToString();
                        SPOobj.Email = (dataReader["Email"] == DBNull.Value) ? string.Empty : (string)dataReader["Email"];
                        SPOobj.Country = (((dataReader["Country"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Country"]).ToString());
                        SPOobj.RecordIsActive = ((dataReader["RecordIsActive"] == DBNull.Value) ? false: (bool)dataReader["RecordIsActive"]);
                        SPOobj.CreatedOn = ((dataReader["CreatedOn"] == DBNull.Value) ? string.Empty : (string)dataReader["CreatedOn"]);
                        SPOobj.Servicename = (dataReader["Servicename"] == DBNull.Value) ? string.Empty : (string)dataReader["Servicename"];
                        SPOobj.CityName = (dataReader["CityName"] == DBNull.Value) ? string.Empty : (string)dataReader["CityName"];
                        SPOobj.CountryName = (dataReader["CountryName"] == DBNull.Value) ? string.Empty : (string)dataReader["CountryName"];

                        listGSSupplier.Add(SPOobj);
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("GlobalSearchdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listGSSupplier;

        }


        public List<GlobalSearchBooking> GlobalSearchBooking(string Fieldvalue, string wherecontains, string keyval)
        {
            List<GlobalSearchBooking> listGSItinerary = new List<GlobalSearchBooking>();

            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@Field", 100, Fieldvalue, SqlDbType.VarChar);
                SqlHelper.inputparams("@Where", 100, wherecontains, SqlDbType.VarChar);
                SqlHelper.inputparams("@Key", 100, keyval, SqlDbType.VarChar);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_SearchFilterBooking", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {

                        GlobalSearchBooking SPOobj = new GlobalSearchBooking();
                        SPOobj.ItineraryId = ((dataReader["ItineraryId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ItineraryId"]).ToString();
                        SPOobj.Bkid = ((dataReader["Bkid"] == DBNull.Value) ? 0 : (long)dataReader["Bkid"]).ToString();
                        SPOobj.BookingName = (dataReader["BookingName"] == DBNull.Value) ? string.Empty : (string)dataReader["BookingName"];
                        SPOobj.ItemDescription = (dataReader["ItemDescription"] == DBNull.Value) ? string.Empty : (string)dataReader["ItemDescription"];
                        SPOobj.BookingStartDate = (dataReader["BookingStartDate"] == DBNull.Value) ? string.Empty : (string)dataReader["BookingStartDate"];
                        SPOobj.ItineraryStartDate = ((dataReader["ItineraryStartDate"] == DBNull.Value) ? string.Empty : (string)dataReader["ItineraryStartDate"]);
                        SPOobj.SupplierRef = ((dataReader["SupplierRef"] == DBNull.Value) ? string.Empty : (string)dataReader["SupplierRef"]);
                        SPOobj.BookingStatus = ((dataReader["BookingStatus"] == DBNull.Value) ? string.Empty : (string)dataReader["BookingStatus"]);
                        SPOobj.ItineraryAutoId = ((dataReader["ItineraryAutoId"] == DBNull.Value) ? 0 : (long)dataReader["ItineraryAutoId"]).ToString();
                        SPOobj.ItineraryName = ((dataReader["ItineraryName"] == DBNull.Value) ? string.Empty : (string)dataReader["ItineraryName"]);
                        SPOobj.Customerid = ((dataReader["Customerid"] == DBNull.Value) ? string.Empty : (string)dataReader["Customerid"]).ToString();
                        SPOobj.Email = ((dataReader["Email"] == DBNull.Value) ? string.Empty : (string)dataReader["Email"]);
                        SPOobj.ItineraryStartDate = ((dataReader["ItineraryStartDate"] == DBNull.Value) ? string.Empty : (string)dataReader["ItineraryStartDate"]);
                        SPOobj.ItineraryStatus = (dataReader["ItineraryStatus"] == DBNull.Value) ? string.Empty : (string)dataReader["ItineraryStatus"];
                        SPOobj.Agent = ((dataReader["Agent"] == DBNull.Value) ? string.Empty : (string)dataReader["Agent"]);
                        SPOobj.AgentAssignedTo = ((dataReader["AgentAssignedTo"] == DBNull.Value) ? string.Empty : (string)dataReader["AgentAssignedTo"]);
                        SPOobj.RecordIsActive = (dataReader["RecordIsActive"] == DBNull.Value) ? false : (bool)dataReader["RecordIsActive"];
                        SPOobj.CreatedOn = (dataReader["CreatedOn"] == DBNull.Value) ? string.Empty : (string)(dataReader["CreatedOn"]);

                        listGSItinerary.Add(SPOobj);
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("GlobalSearchdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listGSItinerary;

        }


        public List<GlobalSearchContact> GlobalSearchContact(string Fieldvalue, string wherecontains, string keyval)
        {
            List<GlobalSearchContact> listGSItinerary = new List<GlobalSearchContact>();

            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@Field", 100, Fieldvalue, SqlDbType.VarChar);
                SqlHelper.inputparams("@Where", 100, wherecontains, SqlDbType.VarChar);
                SqlHelper.inputparams("@Key", 100, keyval, SqlDbType.VarChar);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_SearchFilterContact", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {
                        GlobalSearchContact SPOobj = new GlobalSearchContact();
                        SPOobj.ContactId = ((dataReader["ContactId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ContactId"]).ToString();
                        SPOobj.ContactName = ((dataReader["Contactname"] == DBNull.Value) ? string.Empty : (string)dataReader["Contactname"]).ToString();
                        SPOobj.City = (dataReader["CityName"] == DBNull.Value) ? string.Empty : (string)dataReader["CityName"];
                        SPOobj.Country = (dataReader["CountryName"] == DBNull.Value) ? string.Empty : (string)dataReader["CountryName"];
                        SPOobj.PhoneWork = (dataReader["phonework"] == DBNull.Value) ? string.Empty : (string)dataReader["phonework"];
                        SPOobj.PhoneHome = ((dataReader["phonehome"] == DBNull.Value) ? string.Empty : (string)dataReader["phonehome"]);
                        SPOobj.Mobile = ((dataReader["mobile"] == DBNull.Value) ? string.Empty : (string)dataReader["mobile"]);
                        SPOobj.EmailOne = ((dataReader["EmailOne"] == DBNull.Value) ? string.Empty : (string)dataReader["EmailOne"]);
                        SPOobj.EmailTwo = ((dataReader["EmailTwo"] == DBNull.Value) ? string.Empty : (string)dataReader["EmailTwo"]).ToString();                       
                        SPOobj.RecordIsActive = (dataReader["RecordIsActive"] == DBNull.Value) ? false : (bool)dataReader["RecordIsActive"];
                        SPOobj.CreatedOn = (dataReader["CreatedOn"] == DBNull.Value) ? string.Empty : (string)(dataReader["CreatedOn"]);
                        SPOobj.Contactautoid = ((dataReader["Contactautoid"] == DBNull.Value) ? 0 : (long)(dataReader["Contactautoid"]));
                        listGSItinerary.Add(SPOobj);
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("GlobalSearchdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listGSItinerary;

        }

        public List<GlobalSearchfilters> GlobalSearchFilters(string Module)
        {
            List<GlobalSearchfilters> listGSfltr = new List<GlobalSearchfilters>();

            SqlHelper.parameters = null;
            try
            {
                GlobalSearchfilters SPOobj = new GlobalSearchfilters();
                SqlHelper.inputparams("@Module", 100, Module, SqlDbType.VarChar);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_SearchFilterModule", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {
                        
                        
                        if (Module.ToLower()== "itineraries")
                        {
                            SPOobj.filtersItineraries.Add(((dataReader["Split_Itineraries"] == DBNull.Value) ? string.Empty : (string)dataReader["Split_Itineraries"]).ToString());                            
                        }
                        if (Module.ToLower() == "suppliers")
                        {
                            SPOobj.filtersSuppliers.Add(((dataReader["Split_Suppliers"] == DBNull.Value) ? string.Empty : (string)dataReader["Split_Suppliers"]).ToString());
                        }
                        if (Module.ToLower() == "contacts")
                        {
                            SPOobj.filtersContacts.Add(((dataReader["Split_Contacts"] == DBNull.Value) ? string.Empty : (string)dataReader["Split_Contacts"]).ToString());
                        }
                        if (Module.ToLower() == "bookings")
                        {
                            SPOobj.filtersBookings.Add(((dataReader["Split_Bookings"] == DBNull.Value) ? string.Empty : (string)dataReader["Split_Bookings"]).ToString());
                        }



                        
                    }
                    listGSfltr.Add(SPOobj);
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("GlobalSearchdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listGSfltr;

        }

    }
}
