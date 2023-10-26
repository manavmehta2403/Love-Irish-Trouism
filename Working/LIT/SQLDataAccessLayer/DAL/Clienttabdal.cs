using LITModels.LITModels.Models;
using SQLDataAccessLayer.Models;
using SQLDataAccessLayer.SQLHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace SQLDataAccessLayer.DAL
{
    public class Clienttabdal
    {
        Errorlog errobj = new Errorlog();

        #region "Pax SaveUpdate Start"
        public string SaveUpdatePaxinformation(string Purpose, Paxinformationdata objpax)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                SqlHelper.inputparams("@Paxid", 100, Guid.Parse(objpax.Paxid), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@PaxNumbers", 500, objpax.PaxNumbers, SqlDbType.Int);
                SqlHelper.inputparams("@Bookingid", 500, objpax.Bookingid, SqlDbType.Int);
                SqlHelper.inputparams("@ItineraryID", 100, Guid.Parse(objpax.ItineraryID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@CreatedBy", 100, Guid.Parse(objpax.CreatedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ModifiedBy", 100, Guid.Parse(objpax.ModifiedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@IsDeleted", 100, objpax.IsDeleted, SqlDbType.Bit);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(objpax.DeletedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Groupoption", 100,objpax.Groupoption, SqlDbType.Bit);
                SqlHelper.inputparams("@Individualoption", 100, objpax.Individualoption, SqlDbType.Bit);
                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_PaxinformationSaveUpdate", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Clienttabdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), objpax.CreatedBy);
            }

            return retstr;

        }
        #endregion "Pax SaveUpdate End"

        #region "Pax Retrive Start"
        public List<Paxinformationdata> RetrivePaxinformation(Guid ItineraryID)
        {
            List<Paxinformationdata> listPax = new List<Paxinformationdata>();

            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@Itineraryid", 100, ItineraryID, SqlDbType.UniqueIdentifier);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_GetPaxinformation", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {
                        Paxinformationdata Paxobj = new Paxinformationdata();
                        Paxobj.Paxid = ((dataReader["Paxid"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Paxid"]).ToString();
                        Paxobj.PaxNumbers = ((dataReader["PaxNumbers"] == DBNull.Value) ? 0 : (Int32)dataReader["PaxNumbers"]);
                        Paxobj.Bookingid = ((dataReader["Bookingid"] == DBNull.Value) ? 0 : (Int32)dataReader["Bookingid"]);
                        Paxobj.ItineraryID = ((dataReader["Itineraryid"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Itineraryid"]).ToString();
                        Paxobj.CreatedBy = ((dataReader["CreatedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["CreatedBy"]).ToString();
                        Paxobj.ModifiedBy = ((dataReader["ModifiedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ModifiedBy"]).ToString();
                        Paxobj.DeletedBy = ((dataReader["DeletedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["DeletedBy"]).ToString();
                        Paxobj.IsDeleted = (dataReader["IsDeleted"] == DBNull.Value) ? false : (bool)dataReader["IsDeleted"];
                        Paxobj.Groupoption = (dataReader["Groupoption"] == DBNull.Value) ? false : (bool)dataReader["Groupoption"];
                        Paxobj.Individualoption = (dataReader["Individualoption"] == DBNull.Value) ? false : (bool)dataReader["Individualoption"];


                        listPax.Add(Paxobj);
                    }
                }

            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("ItineraryDAL", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listPax;

        }
        #endregion "Pax Retrive end"


        #region "Passenger Details SaveUpdate Start"
        public string SaveUpdatePassengerDetails(string Purpose, PassengerDetails objpassenger)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                SqlHelper.inputparams("@Passengerid", 100, Guid.Parse(objpassenger.Passengerid), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Age", 500, objpassenger.Age, SqlDbType.Int);
                SqlHelper.inputparams("@AgeGroup", 100, Guid.Parse(objpassenger.AgeGroup), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Agent", 100, Guid.Parse(objpassenger.Agent), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@AgentNet", 1500, objpassenger.AgentNet, SqlDbType.Decimal);
                SqlHelper.inputparams("@CmmOvrd", 100, objpassenger.CommissionOverride, SqlDbType.Decimal);
                SqlHelper.inputparams("@CommissionPercentage", 100, objpassenger.CommissionPercentage, SqlDbType.Decimal);
                SqlHelper.inputparams("@Comments", 100, objpassenger.Comments, SqlDbType.VarChar);
                SqlHelper.inputparams("@CompanyName", 100, objpassenger.CompanyName, SqlDbType.VarChar);
                SqlHelper.inputparams("@Country", 100, Guid.Parse(objpassenger.Country), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@DefaultPrice", 100, objpassenger.DefaultPrice, SqlDbType.Decimal);
                SqlHelper.inputparams("@DisplayName", 100, objpassenger.DisplayName, SqlDbType.VarChar);
                SqlHelper.inputparams("@Email", 100, objpassenger.Email, SqlDbType.VarChar);
                SqlHelper.inputparams("@FirstName", 100, objpassenger.FirstName, SqlDbType.VarChar);
                SqlHelper.inputparams("@LastName", 100, objpassenger.LastName, SqlDbType.VarChar);
                SqlHelper.inputparams("@PassengerStatus", 100, Guid.Parse(objpassenger.PassengerStatus), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@PassengerType", 100, Guid.Parse(objpassenger.PassengerType), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Payee", 100, Guid.Parse(objpassenger.Payee), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@PayingPax", 100, objpassenger.PayingPax, SqlDbType.Bit);
                SqlHelper.inputparams("@PriceOverride", 100, objpassenger.PriceOverride, SqlDbType.Decimal);
                SqlHelper.inputparams("@Room", 100, objpassenger.Room, SqlDbType.VarChar);
                SqlHelper.inputparams("@Rommtype", 100, Guid.Parse(objpassenger.Roomtype), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Saledate", 100, objpassenger.Saledate, SqlDbType.DateTime2);
                SqlHelper.inputparams("@Title", 100, Guid.Parse(objpassenger.SalutationID), SqlDbType.VarChar);
                SqlHelper.inputparams("@ItineraryID", 100, Guid.Parse(objpassenger.ItineraryID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@CreatedBy", 100, Guid.Parse(objpassenger.CreatedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ModifiedBy", 100, Guid.Parse(objpassenger.ModifiedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@IsDeleted", 100, objpassenger.IsDeleted, SqlDbType.Bit);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(objpassenger.DeletedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Totalpassenger", 100, objpassenger.Totalpassenger, SqlDbType.BigInt);
                SqlHelper.inputparams("@LeadPassenger", 100, objpassenger.LeadPassenger, SqlDbType.Bit);
                SqlHelper.inputparams("@Address", 100, objpassenger.Address, SqlDbType.VarChar);
                SqlHelper.inputparams("@State", 100, Guid.Parse(objpassenger.State), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@City", 100, Guid.Parse(objpassenger.City), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Postcode", 100, objpassenger.Postcode, SqlDbType.VarChar);
                SqlHelper.inputparams("@Region", 100, Guid.Parse(objpassenger.Region), SqlDbType.UniqueIdentifier);

                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_PassengerDetailsSaveUpdate", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Clienttabdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), objpassenger.CreatedBy);
            }

            return retstr;

        }
        #endregion "Passenger Details SaveUpdate End"

        #region "Passenger Details Retrive Start"


        public List<PassengerDetails> RetrivePassengerDetails(Guid ItineraryID)
        {
            List<PassengerDetails> listPassdet = new List<PassengerDetails>();

            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@ItineraryID", 100, ItineraryID, SqlDbType.UniqueIdentifier);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_GetPassengerDetails", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {

                        PassengerDetails PassDetobj = new PassengerDetails();
                        PassDetobj.Passengerid = ((dataReader["Passengerid"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Passengerid"]).ToString();
                        PassDetobj.Age = ((dataReader["Age"] == DBNull.Value) ? 0 : (Int32)dataReader["Age"]);
                        PassDetobj.AgeGroup = ((dataReader["AgeGroup"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["AgeGroup"]).ToString();
                        PassDetobj.Agent = ((dataReader["Agent"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Agent"]).ToString();
                        PassDetobj.AgentNet = (dataReader["AgentNet"] == DBNull.Value) ? 0 : (decimal)dataReader["AgentNet"];
                        PassDetobj.CommissionOverride = ((dataReader["CmmOvrd"] == DBNull.Value) ? 0 : (decimal)dataReader["CmmOvrd"]);
                        PassDetobj.CommissionPercentage = ((dataReader["CommissionPercentage"] == DBNull.Value) ? 0 : (decimal)dataReader["CommissionPercentage"]);
                        PassDetobj.Comments = (dataReader["Comments"] == DBNull.Value) ? string.Empty : (string)dataReader["Comments"];
                        PassDetobj.CompanyName = (dataReader["CompanyName"] == DBNull.Value) ? string.Empty : (string)dataReader["CompanyName"];
                        PassDetobj.Country = ((dataReader["Country"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Country"]).ToString();
                        PassDetobj.DefaultPrice = ((dataReader["DefaultPrice"] == DBNull.Value) ? 0 : (decimal)dataReader["DefaultPrice"]);
                        PassDetobj.DisplayName = (dataReader["DisplayName"] == DBNull.Value) ? string.Empty : (string)dataReader["DisplayName"];
                        PassDetobj.Email = (dataReader["Email"] == DBNull.Value) ? string.Empty : (string)dataReader["Email"];
                        PassDetobj.FirstName = (dataReader["FirstName"] == DBNull.Value) ? string.Empty : (string)dataReader["FirstName"];
                        PassDetobj.LastName = (dataReader["LastName"] == DBNull.Value) ? string.Empty : (string)dataReader["LastName"];
                        PassDetobj.PassengerStatus = ((dataReader["PassengerStatus"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["PassengerStatus"]).ToString();
                        PassDetobj.PassengerType = ((dataReader["PassengerType"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["PassengerType"]).ToString();
                        PassDetobj.Payee = ((dataReader["Payee"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Payee"]).ToString();
                        PassDetobj.PayingPax = (dataReader["PayingPax"] == DBNull.Value) ? false : (bool)dataReader["PayingPax"];
                        PassDetobj.PriceOverride = (dataReader["PriceOverride"] == DBNull.Value) ? 0 : (decimal)dataReader["PriceOverride"];
                        PassDetobj.Room = ((dataReader["Room"] == DBNull.Value) ? string.Empty : (string)dataReader["Room"]).ToString();
                        PassDetobj.Roomtype = ((dataReader["Rommtype"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Rommtype"]).ToString();
                        PassDetobj.Saledate = ((dataReader["Saledate"] == DBNull.Value) ? DateTime.Now : Convert.ToDateTime(dataReader["Saledate"]));
                        PassDetobj.SalutationID = ((dataReader["Title"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Title"]).ToString();
                        PassDetobj.ItineraryID = ((dataReader["ItineraryID"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ItineraryID"]).ToString();
                        PassDetobj.CreatedBy = ((dataReader["CreatedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["CreatedBy"]).ToString();
                        PassDetobj.ModifiedBy = ((dataReader["ModifiedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ModifiedBy"]).ToString();
                        PassDetobj.DeletedBy = ((dataReader["DeletedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["DeletedBy"]).ToString();
                        PassDetobj.IsDeleted = (dataReader["IsDeleted"] == DBNull.Value) ? false : (bool)dataReader["IsDeleted"];                        
                        PassDetobj.Totalpassenger = ((dataReader["Totalpassenger"] == DBNull.Value) ? 0 : (Int64)dataReader["Totalpassenger"]);
                        PassDetobj.LeadPassenger = ((dataReader["LeadPassenger"] == DBNull.Value) ? false : (bool)dataReader["LeadPassenger"]);
                        PassDetobj.Address = ((dataReader["Address"] == DBNull.Value) ? string.Empty : (string)dataReader["Address"]).ToString();
                        PassDetobj.State = ((dataReader["State"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["State"]).ToString();
                        PassDetobj.City = ((dataReader["City"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["City"]).ToString();
                        PassDetobj.Postcode = ((dataReader["Postcode"] == DBNull.Value) ? string.Empty : (string)dataReader["Postcode"]).ToString();
                        PassDetobj.Region = ((dataReader["Region"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Region"]).ToString();
                        listPassdet.Add(PassDetobj);
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Clienttabdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listPassdet;

        }


        #endregion "Passenger Details Retrive end"


        #region "Passenger Details Delete"
        public string DeletePassengerDetails(string Passengerid, string DeletedBy)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@Passengerid", 100, Guid.Parse(Passengerid), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(DeletedBy), SqlDbType.UniqueIdentifier);

                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_PassengerDetailsDelete", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Clienttabdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), DeletedBy);
            }

            return retstr;
        }
        #endregion "Passenger Details Delete"


        #region "Payment Details SaveUpdate Start"
        public string SaveUpdatePaymentDetails(string Purpose, PaymentDetails objpayment)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                SqlHelper.inputparams("@PaymentID", 100, Guid.Parse(objpayment.PaymentID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Amount", 100, objpayment.Amount, SqlDbType.Decimal);
                SqlHelper.inputparams("@CurrencyCode", 100, Guid.Parse(objpayment.CurrencyCode), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@DateofPayment", 100, objpayment.DateofPayment, SqlDbType.DateTime2);
                SqlHelper.inputparams("@Details", 1000, objpayment.Details, SqlDbType.VarChar);
                SqlHelper.inputparams("@ExchangeRate", 100, objpayment.ExchangeRate, SqlDbType.Decimal);
                SqlHelper.inputparams("@Fee", 100, objpayment.Fee, SqlDbType.Decimal);
                SqlHelper.inputparams("@FeePercent", 100, objpayment.FeePercent, SqlDbType.Decimal);
                SqlHelper.inputparams("@FeeType", 100, Guid.Parse(objpayment.FeeType), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Inclusive", 100, objpayment.Inclusive, SqlDbType.Bit);
                SqlHelper.inputparams("@PaymentAmount", 100, objpayment.PaymentAmount, SqlDbType.Decimal);
                SqlHelper.inputparams("@PaymentTypeID", 100, Guid.Parse(objpayment.PaymentTypeID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Personname", 100, objpayment.Personname, SqlDbType.VarChar);
                SqlHelper.inputparams("@Sale", 100, objpayment.Sale, SqlDbType.Decimal);
                SqlHelper.inputparams("@ItineraryID", 100, Guid.Parse(objpayment.ItineraryID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@CreatedBy", 100, Guid.Parse(objpayment.CreatedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ModifiedBy", 100, Guid.Parse(objpayment.ModifiedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@IsDeleted", 100, objpayment.IsDeleted, SqlDbType.Bit);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(objpayment.DeletedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@RefundPaymentTotalAmount", 100, objpayment.RefundPaymentTotalAmount, SqlDbType.Decimal);
                SqlHelper.inputparams("@Passengerid", 100, Guid.Parse(objpayment.Passengerid), SqlDbType.UniqueIdentifier);
                

                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_PaymentDetailsSaveUpdate", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Clienttabdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), objpayment.CreatedBy);
            }

            return retstr;

        }
        #endregion "Payment Details SaveUpdate End"

        #region "Payment Details Retrive Start"


        public List<PaymentDetails> RetrivePaymentDetails(Guid ItineraryID)
        {
            List<PaymentDetails> listPassdet = new List<PaymentDetails>();

            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@ItineraryID", 100, ItineraryID, SqlDbType.UniqueIdentifier);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_GetPaymentDetails", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {
                        PaymentDetails PaymntDetobj = new PaymentDetails();
                        PaymntDetobj.PaymentID = ((dataReader["PaymentID"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["PaymentID"]).ToString();
                        PaymntDetobj.Amount = ((dataReader["Amount"] == DBNull.Value) ? 0 : (decimal)dataReader["Amount"]);
                        PaymntDetobj.CurrencyCode = ((dataReader["CurrencyCode"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["CurrencyCode"]).ToString();
                        PaymntDetobj.DateofPayment = ((dataReader["DateofPayment"] == DBNull.Value) ? DateTime.Now : (DateTime)dataReader["DateofPayment"]);
                        PaymntDetobj.Details = (dataReader["Details"] == DBNull.Value) ? string.Empty : (string)dataReader["Details"];
                        PaymntDetobj.ExchangeRate = ((dataReader["ExchangeRate"] == DBNull.Value) ? 0 : (decimal)dataReader["ExchangeRate"]);
                        PaymntDetobj.Fee = ((dataReader["Fee"] == DBNull.Value) ? 0 : (decimal)dataReader["Fee"]);
                        PaymntDetobj.FeePercent = (dataReader["FeePercent"] == DBNull.Value) ? 0 : (decimal)dataReader["FeePercent"];
                        PaymntDetobj.FeeType = ((dataReader["FeeType"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["FeeType"]).ToString();
                        PaymntDetobj.Inclusive = ((dataReader["Inclusive"] == DBNull.Value) ? false : (bool)dataReader["Inclusive"]);
                        PaymntDetobj.PaymentAmount = ((dataReader["PaymentAmount"] == DBNull.Value) ? 0 : (decimal)dataReader["PaymentAmount"]);
                        PaymntDetobj.PaymentTypeID = ((dataReader["PaymentTypeID"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["PaymentTypeID"]).ToString();
                        PaymntDetobj.Personname = (dataReader["Personname"] == DBNull.Value) ? string.Empty : (string)dataReader["Personname"];
                        PaymntDetobj.Sale = (dataReader["Sale"] == DBNull.Value) ? 0 : (decimal)dataReader["Sale"];
                        PaymntDetobj.ItineraryID = ((dataReader["ItineraryID"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ItineraryID"]).ToString();
                        PaymntDetobj.CreatedBy = ((dataReader["CreatedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["CreatedBy"]).ToString();
                        PaymntDetobj.ModifiedBy = ((dataReader["ModifiedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ModifiedBy"]).ToString();
                        PaymntDetobj.DeletedBy = ((dataReader["DeletedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["DeletedBy"]).ToString();
                        PaymntDetobj.IsDeleted = (dataReader["IsDeleted"] == DBNull.Value) ? false : (bool)dataReader["IsDeleted"];
                        PaymntDetobj.RefundPaymentTotalAmount = ((dataReader["RefundPaymentTotalAmount"] == DBNull.Value) ? 0 : (decimal)dataReader["RefundPaymentTotalAmount"]);
                        PaymntDetobj.Passengerid = (((dataReader["Passengerid"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Passengerid"])).ToString();
                        
                        listPassdet.Add(PaymntDetobj);
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Clienttabdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listPassdet;

        }


        #endregion "Payment Details Retrive end"


        #region "Payment Details Delete"
        public string DeletePaymentDetails(string PaymentID, string DeletedBy)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@PaymentID", 100, Guid.Parse(PaymentID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(DeletedBy), SqlDbType.UniqueIdentifier);

                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_PaymentDetailsDelete", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Clienttabdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), DeletedBy);
            }

            return retstr;
        }
        #endregion "Payment Details Delete"


        #region "Room type client tab Details SaveUpdate Start"
        public string SaveUpdateRoomTypeClientTab(string Purpose, RoomTypesClienttab objRoomtypect)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                SqlHelper.inputparams("@Roomtypesid", 100, Guid.Parse(objRoomtypect.RoomtypeID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@OptionTypeRoomid", 100, Guid.Parse(objRoomtypect.OptionTypeRoomid), SqlDbType.Decimal);
                SqlHelper.inputparams("@RmsBkd", 100, objRoomtypect.RmsBkd, SqlDbType.Int);
                SqlHelper.inputparams("@PaxBkd", 100, objRoomtypect.PaxBkd, SqlDbType.Int);
                SqlHelper.inputparams("@RmsSold", 100, objRoomtypect.RmsSold, SqlDbType.Int);
                SqlHelper.inputparams("@PaxSold", 100, objRoomtypect.PaxSold, SqlDbType.Int);
                SqlHelper.inputparams("@SellPrice", 100, objRoomtypect.@SellPrice, SqlDbType.Decimal);
                SqlHelper.inputparams("@ItineraryID", 100, Guid.Parse(objRoomtypect.ItineraryID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@CreatedBy", 100, Guid.Parse(objRoomtypect.CreatedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ModifiedBy", 100, Guid.Parse(objRoomtypect.ModifiedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@IsDeleted", 100, objRoomtypect.IsDeleted, SqlDbType.Bit);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(objRoomtypect.DeletedBy), SqlDbType.UniqueIdentifier);

                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_RoomTypesSaveUpdate", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Clienttabdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), objRoomtypect.CreatedBy);
            }

            return retstr;

        }
        #endregion "Room type client tab Details SaveUpdate End"

        #region "Room type client tab Details Retrive Start"


        public List<RoomTypesClienttab> RetriveRoomtypeclienttab(Guid ItineraryID)
        {
            List<RoomTypesClienttab> listroomtypect = new List<RoomTypesClienttab>();

            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@ItineraryID", 100, ItineraryID, SqlDbType.UniqueIdentifier);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_GETRoomTypesClientTab", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {
                        RoomTypesClienttab PaymntDetobj = new RoomTypesClienttab();
                        PaymntDetobj.RoomtypeID = ((dataReader["Roomtypesid"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Roomtypesid"]).ToString();
                        PaymntDetobj.OptionTypeRoomid = ((dataReader["OptionTypeRoomid"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["OptionTypeRoomid"]).ToString();
                        PaymntDetobj.RmsBkd = ((dataReader["RmsBkd"] == DBNull.Value) ? 0 : (Int32)dataReader["RmsBkd"]);
                        PaymntDetobj.PaxBkd = ((dataReader["PaxBkd"] == DBNull.Value) ? 0 : (Int32)dataReader["PaxBkd"]);
                        PaymntDetobj.RmsSold = ((dataReader["RmsSold"] == DBNull.Value) ? 0 : (Int32)dataReader["RmsSold"]);
                        PaymntDetobj.PaxSold = ((dataReader["PaxSold"] == DBNull.Value) ? 0 : (Int32)dataReader["PaxSold"]);
                        PaymntDetobj.SellPrice = (dataReader["SellPrice"] == DBNull.Value) ? 0 : (decimal)dataReader["SellPrice"];
                        PaymntDetobj.ItineraryID = ((dataReader["ItineraryID"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ItineraryID"]).ToString();
                        PaymntDetobj.CreatedBy = ((dataReader["CreatedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["CreatedBy"]).ToString();
                        PaymntDetobj.ModifiedBy = ((dataReader["ModifiedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ModifiedBy"]).ToString();
                        PaymntDetobj.DeletedBy = ((dataReader["DeletedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["DeletedBy"]).ToString();
                        PaymntDetobj.IsDeleted = (dataReader["IsDeleted"] == DBNull.Value) ? false : (bool)dataReader["IsDeleted"];

                        listroomtypect.Add(PaymntDetobj);
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Clienttabdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listroomtypect;

        }


        #endregion "Room type client tab Details Retrive end"


        #region "Room type client tab Details Delete"
        public string DeleteRoomtypeclienttab(string Roomtypesid, string DeletedBy)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@Roomtypesid", 100, Guid.Parse(Roomtypesid), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(DeletedBy), SqlDbType.UniqueIdentifier);

                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_RoomTypesDelete", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Clienttabdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), DeletedBy);
            }

            return retstr;
        }
        #endregion "Room type client tab Details Delete"


        #region "Get Option room list"
        public List<OptionforRoomtypes> RetriveRoomtypes()
        {
            List<OptionforRoomtypes> listRoomlist = new List<OptionforRoomtypes>();

            SqlHelper.parameters = null;
            try
            {

                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "GetOptionTypeofRooms", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {
                        OptionforRoomtypes PaymntDetobj = new OptionforRoomtypes();
                        PaymntDetobj.OptionTypeRoomid = ((dataReader["OptionTypeRoomid"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["OptionTypeRoomid"]).ToString();
                        PaymntDetobj.OptionTypesName = ((dataReader["OptionTypesName"] == DBNull.Value) ? string.Empty : (string)dataReader["OptionTypesName"]);
                        PaymntDetobj.Divisor = ((dataReader["Divisor"] == DBNull.Value) ? 0 : (Int32)dataReader["Divisor"]).ToString();

                        listRoomlist.Add(PaymntDetobj);
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Clienttabdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listRoomlist;

        }

        #endregion "Get Option room list"

        #region "Get Option PassengerType"

        public List<PassengerTypeValues> RetrivePassengerTypeValues()
        {
            List<PassengerTypeValues> listPasstype = new List<PassengerTypeValues>();

            SqlHelper.parameters = null;
            try
            {

                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_GetPassengerTypDropdown", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {
                        PassengerTypeValues Passtypeobj = new PassengerTypeValues();
                        Passtypeobj.PassengerTypeid = ((dataReader["PassengerTypeid"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["PassengerTypeid"]).ToString();
                        Passtypeobj.PassengerTypename = ((dataReader["PassengerTypename"] == DBNull.Value) ? string.Empty : (string)dataReader["PassengerTypename"]);
                        listPasstype.Add(Passtypeobj);
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Clienttabdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listPasstype;

        }
        #endregion "Get Option PassengerType"


        #region "Get Option PaymentType"

        public List<PaymenttypeValues> RetrivePaymentTypeValues()
        {
            List<PaymenttypeValues> listPaymenttype = new List<PaymenttypeValues>();

            SqlHelper.parameters = null;
            try
            {

                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_GetPaymenttypesDropdown", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {
                        PaymenttypeValues Paymenttypeobj = new PaymenttypeValues();
                        Paymenttypeobj.Paymenttypesid = ((dataReader["Paymenttypesid"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Paymenttypesid"]).ToString();
                        Paymenttypeobj.Paymenttypesname = ((dataReader["Paymenttypesname"] == DBNull.Value) ? string.Empty : (string)dataReader["Paymenttypesname"]);
                        listPaymenttype.Add(Paymenttypeobj);
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Clienttabdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listPaymenttype;

        }
        #endregion "Get Option PaymentType"

        #region "Get Option AgeGroups"

        public List<AgeGroupValues> RetriveAgegroupValues()
        {
            List<AgeGroupValues> listAgeGroup = new List<AgeGroupValues>();

            SqlHelper.parameters = null;
            try
            {

                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_GetAgeGroupDropdown", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {
                        AgeGroupValues AgeGroupobj = new AgeGroupValues();
                        AgeGroupobj.AgeGroupsid = ((dataReader["AgeGroupsid"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["AgeGroupsid"]).ToString();
                        AgeGroupobj.AgeGroupsname = ((dataReader["AgeGroupsname"] == DBNull.Value) ? string.Empty : (string)dataReader["AgeGroupsname"]);
                        listAgeGroup.Add(AgeGroupobj);
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Clienttabdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listAgeGroup;

        }
        #endregion "Get Option AgeGroups"

        #region "Get Option Passengergroup"

        public List<PassengergroupValues> RetrivePassengergroupValues()
        {
            List<PassengergroupValues> listPassengergroup = new List<PassengergroupValues>();

            SqlHelper.parameters = null;
            try
            {

                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_GetPassengerGroupTypeDropdown", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {
                        PassengergroupValues PassGroupobj = new PassengergroupValues();
                        PassGroupobj.Passengergroupid = ((dataReader["Passengergroupid"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Passengergroupid"]).ToString();
                        PassGroupobj.Passengergroupname = ((dataReader["Passengergroupname"] == DBNull.Value) ? string.Empty : (string)dataReader["Passengergroupname"]);
                        listPassengergroup.Add(PassGroupobj);
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Clienttabdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listPassengergroup;

        }
        #endregion "Get Option AgeGroups"
    }
}
