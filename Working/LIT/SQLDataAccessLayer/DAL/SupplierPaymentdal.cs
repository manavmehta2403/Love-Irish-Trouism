using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SQLDataAccessLayer.Models;
using SQLDataAccessLayer.SQLHelper;
using System.Data;

namespace SQLDataAccessLayer.DAL
{
    public class SupplierPaymentdal
    {
        Errorlog errobj = new Errorlog();

        #region "Supplier Payments Details SaveUpdate Start"
        public string SaveUpdateSupplierPaymentDetails(string Purpose, SupplierPayments objsupplpay)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                SqlHelper.inputparams("@SupplierPaymentId", 100, Guid.Parse(objsupplpay.SupplierPaymentId), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@BookingId", 100, objsupplpay.BookingId, SqlDbType.BigInt);
                SqlHelper.inputparams("@ItineraryId", 100, Guid.Parse(objsupplpay.ItineraryId), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@SupplierId", 100, Guid.Parse(objsupplpay.SupplierId), SqlDbType.UniqueIdentifier); 
                SqlHelper.inputparams("@InvoiceId", 100, objsupplpay.InvoiceId, SqlDbType.BigInt);
                SqlHelper.inputparams("@InvoiceNumber", 500, objsupplpay.InvoiceNumber, SqlDbType.VarChar);
                SqlHelper.inputparams("@InvoiceDate", 500, objsupplpay.InvoiceDate, SqlDbType.DateTime2);
                SqlHelper.inputparams("@InvoiceAmount", 1000, objsupplpay.InvoiceAmount, SqlDbType.Decimal);
                SqlHelper.inputparams("@InvoiceDueDate", 500, objsupplpay.InvoiceDueDate, SqlDbType.DateTime2);
                SqlHelper.inputparams("@PaymentType", 500, objsupplpay.PaymentType, SqlDbType.VarChar);
                SqlHelper.inputparams("@ExchangeRate", 500, objsupplpay.ExchangeRate, SqlDbType.Decimal);
                SqlHelper.inputparams("@PaymentAmount", 500, objsupplpay.PaymentAmount, SqlDbType.Decimal);
                SqlHelper.inputparams("@CurrencyCode", 500, objsupplpay.CurrencyCode, SqlDbType.VarChar);
                SqlHelper.inputparams("@CurrencyExchangeRate", 500, objsupplpay.CurrencyExchangeRate, SqlDbType.Decimal);
                SqlHelper.inputparams("@ConvertedAmount", 500, objsupplpay.ConvertedAmount, SqlDbType.Decimal);
                SqlHelper.inputparams("@PaymentDate", 500, objsupplpay.PaymentDate, SqlDbType.DateTime2);
                SqlHelper.inputparams("@TotalOutstanding", 500, objsupplpay.TotalOutstanding, SqlDbType.Decimal);
                SqlHelper.inputparams("@Notes", 500, objsupplpay.Notes, SqlDbType.VarChar);
                SqlHelper.inputparams("@CreatedBy", 100, Guid.Parse(objsupplpay.CreatedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ModifiedBy", 100, Guid.Parse(objsupplpay.ModifiedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@IsDeleted", 100, objsupplpay.IsDeleted, SqlDbType.Bit);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(objsupplpay.DeletedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@BookingIDIdentifier", 100, Guid.Parse(objsupplpay.BookingIDIdentifier), SqlDbType.UniqueIdentifier);

                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_SupplierPaymentRecordsSaveUpdate", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierPaymentdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), objsupplpay.CreatedBy);
            }

            return retstr;

        }
        #endregion "Supplier Payments SaveUpdate End"



        #region "Supplier Payments Retrive Start"


        public List<SupplierPayments> RetriveSupplierPaymentsDetails(Guid ItineraryID, long Bookingid, Guid BookingIDIdentifier)
        {
            List<SupplierPayments> listsuplpay = new List<SupplierPayments>();

            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@Itineraryid", 100, ItineraryID, SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Bookingid", 100, Bookingid, SqlDbType.BigInt);
                SqlHelper.inputparams("@BookingIDIdentifier", 100, BookingIDIdentifier, SqlDbType.UniqueIdentifier);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_GetSupplierPaymentRecords", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {
                        SupplierPayments suplpayobj = new SupplierPayments();
                        suplpayobj.SupplierPaymentId = ((dataReader["SupplierPaymentId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["SupplierPaymentId"]).ToString();
                        suplpayobj.BookingId = ((dataReader["BookingId"] == DBNull.Value) ? 0 : (long)dataReader["BookingId"]);
                        suplpayobj.ItineraryId = ((dataReader["ItineraryId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ItineraryId"]).ToString();
                        suplpayobj.SupplierId = ((dataReader["SupplierId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["SupplierId"]).ToString();
                        suplpayobj.InvoiceId = ((dataReader["InvoiceId"] == DBNull.Value) ? 0 : (long)dataReader["InvoiceId"]);

                        suplpayobj.InvoiceNumber = ((dataReader["InvoiceNumber"] == DBNull.Value) ? string.Empty : (string)dataReader["InvoiceNumber"]).ToString();
                        suplpayobj.InvoiceDate = ((dataReader["InvoiceDate"] == DBNull.Value) ? null : (DateTime)dataReader["InvoiceDate"]);
                        suplpayobj.InvoiceAmount = (dataReader["InvoiceAmount"] == DBNull.Value) ? 0 : (decimal)dataReader["InvoiceAmount"];
                        suplpayobj.PaymentAmount = (dataReader["PaymentAmount"] == DBNull.Value) ? 0 : (decimal)dataReader["PaymentAmount"];
                        suplpayobj.InvoiceDueDate = ((dataReader["InvoiceDueDate"] == DBNull.Value) ? null : (DateTime)dataReader["InvoiceDueDate"]);

                        suplpayobj.CurrencyCode = ((dataReader["CurrencyCode"] == DBNull.Value) ? string.Empty : (string)dataReader["CurrencyCode"]).ToString();
                        suplpayobj.CurrencyExchangeRate = ((dataReader["CurrencyExchangeRate"] == DBNull.Value) ? 0 : (decimal)dataReader["CurrencyExchangeRate"]);
                        suplpayobj.ConvertedAmount = (dataReader["ConvertedAmount"] == DBNull.Value) ? 0 : (decimal)dataReader["ConvertedAmount"];
                        suplpayobj.PaymentDate = ((dataReader["PaymentDate"] == DBNull.Value) ? null : (DateTime)dataReader["PaymentDate"]);
                        suplpayobj.TotalOutstanding = (dataReader["TotalOutstanding"] == DBNull.Value) ? 0 : (decimal)dataReader["TotalOutstanding"];
                        suplpayobj.Notes = ((dataReader["Notes"] == DBNull.Value) ? string.Empty : (string)dataReader["Notes"]).ToString();
                        suplpayobj.CreatedBy = ((dataReader["CreatedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["CreatedBy"]).ToString();
                        suplpayobj.ModifiedBy = ((dataReader["ModifiedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ModifiedBy"]).ToString();
                        suplpayobj.DeletedBy = ((dataReader["DeletedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["DeletedBy"]).ToString();
                        suplpayobj.IsDeleted = (dataReader["IsDeleted"] == DBNull.Value) ? false : (bool)dataReader["IsDeleted"];

                        listsuplpay.Add(suplpayobj);
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierPaymentdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listsuplpay;

        }


        #endregion "Supplier Payments Retrive end"


        #region "Supplier Payments Details Delete"
        public string DeleteSupplierPaymentsDetails(string SupplierPaymentId, string DeletedBy)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@SupplierPaymentId", 100, Guid.Parse(SupplierPaymentId), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(DeletedBy), SqlDbType.UniqueIdentifier);

                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_SupplierPaymentRecordsDelete", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierPaymentdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), DeletedBy);
            }

            return retstr;
        }
        #endregion "Supplier Payments Details Delete"
    }
}
