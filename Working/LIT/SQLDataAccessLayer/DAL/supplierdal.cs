using SQLDataAccessLayer.Models;
using SQLDataAccessLayer.SQLHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SQLDataAccessLayer.DAL
{
    public class SupplierDAL
    {
        Errorlog errobj = new Errorlog();

        #region "Supplier SaveUpdate Start"
        public string SaveUpdateSupplier(string Purpose, SupplierModels objsupp)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                SqlHelper.inputparams("@SupplierId", 100, Guid.Parse(objsupp.SupplierId), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@SupplierName", 500, objsupp.SupplierName, SqlDbType.VarChar);
                SqlHelper.inputparams("@SupplierAutoId", 500, objsupp.SupplierAutoId, SqlDbType.BigInt);
                SqlHelper.inputparams("@Hosts", 100, objsupp.Hosts, SqlDbType.VarChar);
                SqlHelper.inputparams("@IsSupplierActive", 100, objsupp.IsSupplierActive, SqlDbType.Bit);
                SqlHelper.inputparams("@CustomCode", 100, objsupp.CustomCode, SqlDbType.VarChar);
                SqlHelper.inputparams("@Street", 100, objsupp.Street, SqlDbType.VarChar);
                SqlHelper.inputparams("@City", 100, Guid.Parse(objsupp.City), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Region", 100, Guid.Parse(objsupp.Region), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@State", 100, Guid.Parse(objsupp.State), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Country ", 100, Guid.Parse(objsupp.Country), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Postcode", 500, objsupp.Postcode, SqlDbType.VarChar);
                SqlHelper.inputparams("@Phone", 500, objsupp.Phone, SqlDbType.VarChar);
                SqlHelper.inputparams("@Mobile", 500, objsupp.Mobile, SqlDbType.VarChar);
                SqlHelper.inputparams("@FreePh", 500, objsupp.FreePh, SqlDbType.VarChar);
                SqlHelper.inputparams("@Fax", 500, objsupp.Fax, SqlDbType.VarChar);
                SqlHelper.inputparams("@Email", 500, objsupp.Email, SqlDbType.VarChar);
                SqlHelper.inputparams("@Website", 500, objsupp.Website, SqlDbType.VarChar);
                SqlHelper.inputparams("@PostalAddress", 1000, objsupp.PostalAddress, SqlDbType.VarChar);
                SqlHelper.inputparams("@SupplierComments", 2000, objsupp.SupplierComments, SqlDbType.VarChar);
                SqlHelper.inputparams("@SupplierDescription", 2000, objsupp.SupplierDescription, SqlDbType.VarChar);
                SqlHelper.inputparams("@SupplierFolderPath", 2000, objsupp.SupplierFolderPath, SqlDbType.VarChar);
                SqlHelper.inputparams("@SupplierFolderInfoId", 2000, objsupp.SupplierfolderinfoId, SqlDbType.BigInt);
                SqlHelper.inputparams("@CreatedBy", 100, Guid.Parse(objsupp.CreatedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ModifiedBy", 100, Guid.Parse(objsupp.ModifiedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@IsDeleted", 100, objsupp.IsDeleted, SqlDbType.Bit);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(objsupp.DeletedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@SupplierPaymentTermsindays", 100, objsupp.SupplierPaymentTermsindays, SqlDbType.Int);
                SqlHelper.inputparams("@SupplierPaymentDepositAmount", 100, objsupp.SupplierPaymentDepositAmount, SqlDbType.Decimal);
                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_SupplierSaveUpdate", SqlHelper.parameters);
                retstr=ret.ToString();  


            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), objsupp.CreatedBy);
            }
            return retstr;

        }
        #endregion "Supplier SaveUpdate End"

        #region "Supplier Retrive Start"
        public List<SupplierModels> SupplierRetrive(string Purpose, Guid SupplierID)
        {
            List<SupplierModels> listsupplier = new List<SupplierModels>();

            SqlHelper.parameters = null;
            try { 
            if (!string.IsNullOrEmpty(Purpose))
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                SqlHelper.inputparams("@SupplierID", 100, SupplierID, SqlDbType.UniqueIdentifier);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_SupplierReterive", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {
                        SupplierModels smobj=new SupplierModels();
                        smobj.SupplierId = ((dataReader["SupplierId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["SupplierId"]).ToString();                        
                        smobj.SupplierName = (dataReader["SupplierName"] == DBNull.Value) ? string.Empty : (string)dataReader["SupplierName"];
                        smobj.SupplierAutoId = ((dataReader["SupplierAutoId"] == DBNull.Value) ? 0 : (long)(dataReader["SupplierAutoId"]));
                        smobj.Hosts = (dataReader["Hosts"] == DBNull.Value) ? string.Empty : (string)dataReader["Hosts"];
                        smobj.IsSupplierActive = (dataReader["IsSupplierActive"] == DBNull.Value) ? false : (bool)dataReader["IsSupplierActive"];
                        smobj.CustomCode = (dataReader["CustomCode"] == DBNull.Value) ? string.Empty : (string)dataReader["CustomCode"];
                        smobj.Street = (dataReader["Street"] == DBNull.Value) ? string.Empty : (string)dataReader["Street"];
                        smobj.City = ((dataReader["City"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["City"]).ToString();
                        smobj.Region = ((dataReader["Region"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Region"]).ToString();
                        smobj.State = ((dataReader["State"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["State"]).ToString();
                        smobj.Country = ((dataReader["Country"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Country"]).ToString();
                        smobj.Postcode = (dataReader["Postcode"] == DBNull.Value) ? string.Empty : (string)dataReader["Postcode"];
                        smobj.Phone = (dataReader["Phone"] == DBNull.Value) ? string.Empty : (string)dataReader["Phone"];
                        smobj.Mobile = (dataReader["Mobile"] == DBNull.Value) ? string.Empty : (string)dataReader["Mobile"];
                        smobj.FreePh = (dataReader["FreePh"] == DBNull.Value) ? string.Empty : (string)dataReader["FreePh"];
                        smobj.Fax = (dataReader["Fax"] == DBNull.Value) ? string.Empty : (string)dataReader["Fax"];
                        smobj.Email = (dataReader["Email"] == DBNull.Value) ? string.Empty : (string)dataReader["Email"];
                        smobj.Website = (dataReader["Website"] == DBNull.Value) ? string.Empty : (string)dataReader["Website"];
                        smobj.PostalAddress = (dataReader["PostalAddress"] == DBNull.Value) ? string.Empty : (string)dataReader["PostalAddress"];
                        smobj.SupplierComments = (dataReader["SupplierComments"] == DBNull.Value) ? string.Empty : (string)dataReader["SupplierComments"];
                        smobj.SupplierDescription = (dataReader["SupplierDescription"] == DBNull.Value) ? string.Empty : (string)dataReader["SupplierDescription"];
                        smobj.SupplierFolderPath = (dataReader["SupplierFolderPath"] == DBNull.Value) ? string.Empty : (string)dataReader["SupplierFolderPath"];
                        smobj.SupplierfolderinfoId = (dataReader["SupplierfolderinfoId"] == DBNull.Value) ? string.Empty : (string)dataReader["SupplierfolderinfoId"];
                        smobj.CreatedBy = ((dataReader["CreatedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["CreatedBy"]).ToString();
                        smobj.ModifiedBy = ((dataReader["ModifiedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ModifiedBy"]).ToString();
                        smobj.DeletedBy = ((dataReader["DeletedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["DeletedBy"]).ToString();
                        smobj.IsDeleted = (dataReader["IsDeleted"] == DBNull.Value) ? false : (bool)dataReader["IsDeleted"];
                        smobj.SupplierPaymentTermsindays = (dataReader["SupplierPaymentTermsindays"] == DBNull.Value) ? 0 : (int)dataReader["SupplierPaymentTermsindays"];
                        smobj.SupplierPaymentDepositAmount = (dataReader["SupplierPaymentDepositAmount"] == DBNull.Value) ? 0 : (decimal)dataReader["SupplierPaymentDepositAmount"]; 
                        listsupplier.Add(smobj);                            
                    }
                }
            }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listsupplier;

        }

        public List<SupplierModels> SupplierRetriveID(string Purpose, Guid SupplierID)
        {
            List<SupplierModels> listsupplier = new List<SupplierModels>();

            SqlHelper.parameters = null;
            try { 
            if (!string.IsNullOrEmpty(Purpose))
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                SqlHelper.inputparams("@SupplierID", 100, SupplierID, SqlDbType.UniqueIdentifier);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_SupplierReterive", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {
                        SupplierModels smobj = new SupplierModels();
                        smobj.SupplierId = ((dataReader["SupplierId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["SupplierId"]).ToString();                        
                        listsupplier.Add(smobj);
                    }
                }
            }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listsupplier;

        }

        public List<SupplierModels> SupplierRetriveFolderinfo(string Purpose, Guid SupplierID)
        {
            List<SupplierModels> listsupplier = new List<SupplierModels>();

            SqlHelper.parameters = null;
            try 
            { 
                if (!string.IsNullOrEmpty(Purpose))
                {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                SqlHelper.inputparams("@SupplierID", 100, SupplierID, SqlDbType.UniqueIdentifier);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_SupplierReterive", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {
                        SupplierModels smobj = new SupplierModels();
                        smobj.SupplierId = ((dataReader["SupplierId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["SupplierId"]).ToString();                       
                        smobj.SupplierName = (dataReader["SupplierName"] == DBNull.Value) ? string.Empty : (string)dataReader["SupplierName"];
                        smobj.SupplierFolderPath = (dataReader["SupplierFolderPath"] == DBNull.Value) ? string.Empty : (string)dataReader["SupplierFolderPath"];
                        smobj.SupplierfolderinfoId = (dataReader["SupplierfolderinfoId"] == DBNull.Value) ? string.Empty : (string)dataReader["SupplierfolderinfoId"];
                        smobj.SupplierAutoId = ((dataReader["SupplierAutoId"] == DBNull.Value) ? 0 : (long)(dataReader["SupplierAutoId"]));
                        listsupplier.Add(smobj);
                    }
                }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listsupplier;

        }



        public List<BookingSupplierServiceModels> BookingSupplierServiceRetrive(string Suppliername, Guid ServiceTypeID, Guid CityID, Guid RegionID, Guid SupplierID)
        {
            List<BookingSupplierServiceModels> listBkgSupplierService = new List<BookingSupplierServiceModels>();

            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@Suppliername", 500, Suppliername, SqlDbType.VarChar);
                SqlHelper.inputparams("@ServiceTypeID", 100, ServiceTypeID, SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@RegionID", 100, RegionID, SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@CityID", 100, CityID, SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@SupplierID", 100, SupplierID, SqlDbType.UniqueIdentifier);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "GetSupplierServiceforBooking", SqlHelper.parameters))
                    {
                        while (dataReader.Read())
                        {
                            BookingSupplierServiceModels smobj = new BookingSupplierServiceModels();
                            smobj.SupplierID = ((dataReader["SupplierId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["SupplierId"]).ToString();
                            smobj.SupplierName = (dataReader["SupplierName"] == DBNull.Value) ? string.Empty : (string)dataReader["SupplierName"];
                            smobj.ServiceID = ((dataReader["ServiceId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ServiceId"]).ToString();
                            smobj.ServiceName = (dataReader["ServiceName"] == DBNull.Value) ? string.Empty : (string)dataReader["ServiceName"];
                            smobj.ServiceTypeID = ((dataReader["ServiceTypeID"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ServiceTypeID"]).ToString();
                            smobj.CityID = ((dataReader["CityID"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["CityID"]).ToString();
                            smobj.RegionID = ((dataReader["RegionID"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["RegionID"]).ToString();
                            smobj.CityName = ((dataReader["CityName"] == DBNull.Value) ? string.Empty : (string)dataReader["CityName"]).ToString();
                            smobj.RegionName = ((dataReader["RegionName"] == DBNull.Value) ? string.Empty : (string)dataReader["RegionName"]).ToString();
                            listBkgSupplierService.Add(smobj);
                        }
                    }
                
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listBkgSupplierService;

        }
        #endregion "Supplier Retrive end"

        public List<string> distinctSupplierFolderlist()
        {
            List<string> listsupplierfolder = new List<string>();

            SqlHelper.parameters = null;
            try { 
            // SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
            // SqlHelper.inputparams("@SupplierID", 100, SupplierID, SqlDbType.UniqueIdentifier);
            using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "GetSupplierDistinctFolderPath", SqlHelper.parameters))
            {
                while (dataReader.Read())
                {
                    listsupplierfolder.Add((dataReader["SupplierFolderPath"] == DBNull.Value) ? string.Empty : (string)dataReader["SupplierFolderPath"]);
                }
            }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listsupplierfolder;

        }


        


        #region "Supplier Service Start"

        public string SaveUpdateSupplierService(string Purpose, SupplierServiceModels objsuppser)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                SqlHelper.inputparams("@ServiceId", 100, Guid.Parse(objsuppser.ServiceId), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ServiceName", 500, objsuppser.ServiceName, SqlDbType.VarChar);
                SqlHelper.inputparams("@Type", 100, Guid.Parse(objsuppser.Type), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@SupplierId", 100, Guid.Parse(objsuppser.SupplierId), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@CreatedBy", 100, Guid.Parse(objsuppser.CreatedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ModifiedBy", 100, Guid.Parse(objsuppser.ModifiedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@IsDeleted", 100, objsuppser.IsDeleted, SqlDbType.Bit);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(objsuppser.DeletedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@IsActive", 100, objsuppser.IsActive, SqlDbType.Bit);
                SqlHelper.inputparams("@Currency", 100, Guid.Parse(objsuppser.Currency), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Groupinfo", 100, Guid.Parse(objsuppser.Groupinfo), SqlDbType.UniqueIdentifier);
                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_SupplierServiceSaveUpdate", SqlHelper.parameters);
                retstr=ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), objsuppser.CreatedBy);
            }
            return retstr;

        }

        public List<SupplierServiceModels> SupplierServiceRetrive(Guid SupplierID)
        {
            List<SupplierServiceModels> listsupplier = new List<SupplierServiceModels>();

            SqlHelper.parameters = null;
            try { 
            SqlHelper.inputparams("@SupplierID", 100, SupplierID, SqlDbType.UniqueIdentifier);
            using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "GetSupplierServices", SqlHelper.parameters))
            {
                while (dataReader.Read())
                {
                    SupplierServiceModels SSMobj = new SupplierServiceModels();
                    SSMobj.ServiceId = ((dataReader["ServiceId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ServiceId"]).ToString();
                    SSMobj.ServiceName = (dataReader["ServiceName"] == DBNull.Value) ? string.Empty : (string)dataReader["ServiceName"];                        
                    SSMobj.Type = ((dataReader["Type"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Type"]).ToString();
                    SSMobj.SupplierId = ((dataReader["SupplierId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["SupplierId"]).ToString();                        
                    SSMobj.CreatedBy = ((dataReader["CreatedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["CreatedBy"]).ToString();
                    SSMobj.ModifiedBy = ((dataReader["ModifiedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ModifiedBy"]).ToString();
                    SSMobj.DeletedBy = ((dataReader["DeletedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["DeletedBy"]).ToString();
                    SSMobj.IsDeleted = (dataReader["IsDeleted"] == DBNull.Value) ? false : (bool)dataReader["IsDeleted"];
                    SSMobj.IsActive = (dataReader["IsActive"] == DBNull.Value) ? false : (bool)dataReader["IsActive"];
                    SSMobj.Currency = ((dataReader["Currency"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Currency"]).ToString();
                    SSMobj.Groupinfo = ((dataReader["Groupinfo"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Groupinfo"]).ToString();
                        listsupplier.Add(SSMobj);
                }
            }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listsupplier;

        }


        public string DeleteSupplierService(SupplierServiceModels objsuppser)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty; 
            try 
            { 
                SqlHelper.inputparams("@ServiceId", 100, Guid.Parse(objsuppser.ServiceId), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@SupplierId", 100, Guid.Parse(objsuppser.SupplierId), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(objsuppser.DeletedBy), SqlDbType.UniqueIdentifier);
                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_SupplierServiceDelete", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), objsuppser.DeletedBy);
            }
            return retstr;

        }

        #endregion "Supplier Service end"


        #region "Supplier Service Rate/Date Details Start"

        public string SaveUpdateSupplierServiceRateDt(string Purpose, SupplierServiceRatesDt objsuppserRate)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                SqlHelper.inputparams("@ValidFrom", 100, objsuppserRate.ValidFrom, SqlDbType.DateTime2);
                SqlHelper.inputparams("@ValidTo", 100, objsuppserRate.ValidTo, SqlDbType.DateTime2);
                SqlHelper.inputparams("@SupplierServiceDetailsRateId", 500, Guid.Parse(objsuppserRate.SupplierServiceDetailsRateId), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@SupplierServiceId", 100, Guid.Parse(objsuppserRate.SupplierServiceId), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@CreatedBy", 100, Guid.Parse(objsuppserRate.CreatedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ModifiedBy", 100, Guid.Parse(objsuppserRate.ModifiedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@IsDeleted", 100, objsuppserRate.IsDeleted, SqlDbType.Bit);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(objsuppserRate.DeletedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@IsActive", 100, objsuppserRate.IsActive, SqlDbType.Bit);
                SqlHelper.inputparams("@IsExpired", 100, objsuppserRate.IsExpired, SqlDbType.Bit);
                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_SupplierServiceRatesDtSaveUpdate", SqlHelper.parameters);
                retstr = ret.ToString();
                    
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), objsuppserRate.CreatedBy);
            }
            return retstr;

        }

        public List<SupplierServiceRatesDt> SupplierServiceRateDtRetrive(Guid SupplierServiceId)
        {
            List<SupplierServiceRatesDt> listsupplier = new List<SupplierServiceRatesDt>();
            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@SupplierServiceId", 100, SupplierServiceId, SqlDbType.UniqueIdentifier);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "GetSupplierServicesRatesDt", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {
                        SupplierServiceRatesDt SSRDobj = new SupplierServiceRatesDt();
                        SSRDobj.ValidFrom = (dataReader["ValidFrom"] == DBNull.Value) ? DateTime.Now : Convert.ToDateTime(dataReader["ValidFrom"].ToString());
                        SSRDobj.ValidTo = (dataReader["ValidTo"] == DBNull.Value) ? DateTime.Now : Convert.ToDateTime(dataReader["ValidTo"]);
                        SSRDobj.SupplierServiceDetailsRateId = ((dataReader["SupplierServiceDetailsRateId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["SupplierServiceDetailsRateId"]).ToString();
                        SSRDobj.SupplierServiceId = ((dataReader["SupplierServiceId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["SupplierServiceId"]).ToString();
                        SSRDobj.CreatedBy = ((dataReader["CreatedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["CreatedBy"]).ToString();
                        SSRDobj.ModifiedBy = ((dataReader["ModifiedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ModifiedBy"]).ToString();
                        SSRDobj.DeletedBy = ((dataReader["DeletedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["DeletedBy"]).ToString();
                        SSRDobj.IsDeleted = (dataReader["IsDeleted"] == DBNull.Value) ? false : (bool)dataReader["IsDeleted"];
                        SSRDobj.IsExpired = (dataReader["IsExpired"] == DBNull.Value) ? false : (bool)dataReader["IsExpired"];
                        SSRDobj.IsActive = (dataReader["IsActive"] == DBNull.Value) ? false : (bool)dataReader["IsActive"];

                        listsupplier.Add(SSRDobj);
                    }
                }
            
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

            return listsupplier;

        }


        public string DeleteSupplierServiceRateDt(SupplierServiceRatesDt objsuppserrate)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try 
            { 
                SqlHelper.inputparams("@SupplierServiceDetailsRateId", 100, Guid.Parse(objsuppserrate.SupplierServiceDetailsRateId), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@SupplierServiceId", 100, Guid.Parse(objsuppserrate.SupplierServiceId), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(objsuppserrate.DeletedBy), SqlDbType.UniqueIdentifier);
                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_SupplierServiceRatesDtDelete", SqlHelper.parameters);
                retstr= ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), objsuppserrate.DeletedBy);
            }
            return retstr;

        }

        #endregion "Supplier Service Rate/Date Details end"


        #region "Supplier Service Warning Details Start"

        public string SaveUpdateSupplierServiceWarning(string Purpose, SupplierServiceWarning objsuppserWar)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                SqlHelper.inputparams("@ValidFrom", 100, objsuppserWar.ValidFromwarning, SqlDbType.DateTime2);
                SqlHelper.inputparams("@ValidTo", 100, objsuppserWar.ValidTowarning, SqlDbType.DateTime2);
                SqlHelper.inputparams("@SupplierServiceDetailsWarningID", 500, Guid.Parse(objsuppserWar.SupplierServiceDetailsWarningID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@SupplierServiceId", 100, Guid.Parse(objsuppserWar.SupplierServiceId), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Description", 4000, objsuppserWar.WarDescription, SqlDbType.VarChar);
                SqlHelper.inputparams("@Messagefor", 100, objsuppserWar.Messagefor, SqlDbType.VarChar);
                SqlHelper.inputparams("@IsActive", 100, objsuppserWar.IsActive, SqlDbType.Bit);
                SqlHelper.inputparams("@IsExpired", 100, objsuppserWar.IsExpired, SqlDbType.Bit);
                SqlHelper.inputparams("@CreatedBy", 100, Guid.Parse(objsuppserWar.CreatedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ModifiedBy", 100, Guid.Parse(objsuppserWar.ModifiedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@IsDeleted", 100, objsuppserWar.IsDeleted, SqlDbType.Bit);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(objsuppserWar.DeletedBy), SqlDbType.UniqueIdentifier);
                
                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_SupplierServiceWarningSaveUpdate", SqlHelper.parameters);
                retstr = ret.ToString();

            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), objsuppserWar.CreatedBy);
            }
            return retstr;

        }

        public List<SupplierServiceWarning> SupplierServiceWarningRetrive(Guid SupplierServiceId)
        {
            List<SupplierServiceWarning> listsupplier = new List<SupplierServiceWarning>();
            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@SupplierServiceId", 100, SupplierServiceId, SqlDbType.UniqueIdentifier);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "GetSupplierServicesWarning", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {                        
                        SupplierServiceWarning SSMobj = new SupplierServiceWarning();
                        SSMobj.ValidFromwarning = (dataReader["ValidFrom"] == DBNull.Value) ? DateTime.Now : Convert.ToDateTime(dataReader["ValidFrom"]);
                        SSMobj.ValidTowarning = (dataReader["ValidTo"] == DBNull.Value) ? DateTime.Now : Convert.ToDateTime(dataReader["ValidTo"]);
                        SSMobj.SupplierServiceDetailsWarningID = ((dataReader["SupplierServiceDetailsWarningID"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["SupplierServiceDetailsWarningID"]).ToString();
                        SSMobj.SupplierServiceId = ((dataReader["SupplierServiceId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["SupplierServiceId"]).ToString();
                        SSMobj.WarDescription = (dataReader["Description"] == DBNull.Value) ? string.Empty : (string)dataReader["Description"];
                        SSMobj.Messagefor = (dataReader["Messagefor"] == DBNull.Value) ? string.Empty : (string)dataReader["Messagefor"];                       
                        SSMobj.CreatedBy = ((dataReader["CreatedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["CreatedBy"]).ToString();
                        SSMobj.ModifiedBy = ((dataReader["ModifiedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ModifiedBy"]).ToString();
                        SSMobj.DeletedBy = ((dataReader["DeletedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["DeletedBy"]).ToString();
                        SSMobj.IsDeleted = (dataReader["IsDeleted"] == DBNull.Value) ? false : (bool)dataReader["IsDeleted"];
                        SSMobj.IsExpired = (dataReader["IsExpired"] == DBNull.Value) ? false : (bool)dataReader["IsExpired"];
                        SSMobj.IsActive = (dataReader["IsActive"] == DBNull.Value) ? false : (bool)dataReader["IsActive"];

                        listsupplier.Add(SSMobj);
                    }
                }

            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

            return listsupplier;

        }

        public List<SupplierServiceWarning> SupplServiceWarningdtRetrive(Guid SupplierServiceId,DateTime WarningDtfrom,DateTime WarningDtto)
        {
            List<SupplierServiceWarning> listsupplier = new List<SupplierServiceWarning>();
            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@SupplierServiceId", 100, SupplierServiceId, SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@WarningDtfrom", 100, WarningDtfrom, SqlDbType.DateTime2);
                SqlHelper.inputparams("@WarningDtto", 100, WarningDtto, SqlDbType.DateTime2);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "GetSupplierServicesWarningdt", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {
                        SupplierServiceWarning SSMobj = new SupplierServiceWarning();
                        SSMobj.ValidFromwarning = (dataReader["ValidFrom"] == DBNull.Value) ? DateTime.Now : Convert.ToDateTime(dataReader["ValidFrom"]);
                        SSMobj.ValidTowarning = (dataReader["ValidTo"] == DBNull.Value) ? DateTime.Now : Convert.ToDateTime(dataReader["ValidTo"]);
                        SSMobj.SupplierServiceDetailsWarningID = ((dataReader["SupplierServiceDetailsWarningID"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["SupplierServiceDetailsWarningID"]).ToString();
                        SSMobj.SupplierServiceId = ((dataReader["SupplierServiceId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["SupplierServiceId"]).ToString();
                        SSMobj.WarDescription = (dataReader["Description"] == DBNull.Value) ? string.Empty : (string)dataReader["Description"];
                        SSMobj.Messagefor = (dataReader["Messagefor"] == DBNull.Value) ? string.Empty : (string)dataReader["Messagefor"];
                        listsupplier.Add(SSMobj);
                    }
                }

            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

            return listsupplier;

        }



        public string DeleteSupplierServiceWarning(SupplierServiceWarning objsuppwar)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@SupplierServiceDetailsWarningID", 100, Guid.Parse(objsuppwar.SupplierServiceDetailsWarningID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@SupplierServiceId", 100, Guid.Parse(objsuppwar.SupplierServiceId), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(objsuppwar.DeletedBy), SqlDbType.UniqueIdentifier);
                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_SupplierServiceWarningDelete", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), objsuppwar.DeletedBy);
            }
            return retstr;

        }

        #endregion "Supplier Service Rate/Date Details end"


        #region "Supplier Pricing Option Start"

        public string SaveUpdatePricingoption(string Purpose, SupplierPricingOption objsuppprice)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                SqlHelper.inputparams("@PricingOptionId", 100, Guid.Parse(objsuppprice.PricingOptionId), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@PricingOptionName", 500, objsuppprice.PricingOptionName, SqlDbType.VarChar);
                SqlHelper.inputparams("@NetPrice", 100, (objsuppprice.NetPrice), SqlDbType.Decimal);
                SqlHelper.inputparams("@MarkupPercentage", 100, objsuppprice.MarkupPercentage, SqlDbType.Decimal);
                SqlHelper.inputparams("@GrossPrice", 100, (objsuppprice.GrossPrice), SqlDbType.Decimal);
                SqlHelper.inputparams("@CommissionPercentage", 100, objsuppprice.CommissionPercentage, SqlDbType.Decimal);
                SqlHelper.inputparams("@Type", 100, Guid.Parse(objsuppprice.PriceType), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@IsDefault", 100, (objsuppprice.PriceIsDefault), SqlDbType.Bit);
                SqlHelper.inputparams("@SupplierServiceDetailsRateId", 100, Guid.Parse(objsuppprice.SupplierServiceDetailsRateId), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@SupplierServiceId", 100, Guid.Parse(objsuppprice.SupplierServiceId), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@CreatedBy", 100, Guid.Parse(objsuppprice.CreatedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ModifiedBy", 100, Guid.Parse(objsuppprice.ModifiedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@IsDeleted", 100, objsuppprice.IsDeleted, SqlDbType.Bit);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(objsuppprice.DeletedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@IsActive", 100, objsuppprice.PriceIsActive, SqlDbType.Bit);
                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_PricingOptionSaveUpdate", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), objsuppprice.CreatedBy);
            }
            return retstr;

        }
        public List<SupplierPricingOption> PricingOptionRetrive(Guid SupplierServiceId,Guid SupplierServiceDetailsRateId)
        {
            List<SupplierPricingOption> listPricingoption = new List<SupplierPricingOption>();

            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@SupplierServiceId", 100, SupplierServiceId, SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@SupplierServiceDetailsRateId", 100, SupplierServiceDetailsRateId, SqlDbType.UniqueIdentifier);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "GetPricingoption", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {
                        SupplierPricingOption SPOobj = new SupplierPricingOption();
                        SPOobj.PricingOptionId = ((dataReader["PricingOptionId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["PricingOptionId"]).ToString();
                        SPOobj.PricingOptionName = (dataReader["PricingOptionName"] == DBNull.Value) ? string.Empty : (string)dataReader["PricingOptionName"];
                        SPOobj.NetPrice = (dataReader["NetPrice"] == DBNull.Value) ? 0 : (decimal)dataReader["NetPrice"];
                        SPOobj.MarkupPercentage = (dataReader["MarkupPercentage"] == DBNull.Value) ? 0 : (decimal)dataReader["MarkupPercentage"];
                        SPOobj.GrossPrice = (dataReader["GrossPrice"] == DBNull.Value) ? 0 : (decimal)dataReader["GrossPrice"];
                        SPOobj.CommissionPercentage = (dataReader["CommissionPercentage"] == DBNull.Value) ? 0 : (decimal)dataReader["CommissionPercentage"];
                        SPOobj.SupplierServiceDetailsRateId = ((dataReader["SupplierServiceDetailsRateId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["SupplierServiceDetailsRateId"]).ToString();
                        SPOobj.SupplierServiceId = ((dataReader["SupplierServiceId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["SupplierServiceId"]).ToString();
                        SPOobj.PriceType = ((dataReader["Type"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Type"]).ToString();
                        SPOobj.PriceIsDefault = ((dataReader["IsDefault"] == DBNull.Value) ? false : (bool)dataReader["IsDefault"]);
                        SPOobj.CreatedBy = ((dataReader["CreatedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["CreatedBy"]).ToString();
                        SPOobj.ModifiedBy = ((dataReader["ModifiedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ModifiedBy"]).ToString();
                        SPOobj.DeletedBy = ((dataReader["DeletedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["DeletedBy"]).ToString();
                        SPOobj.IsDeleted = (dataReader["IsDeleted"] == DBNull.Value) ? false : (bool)dataReader["IsDeleted"];
                        SPOobj.PriceIsActive = (dataReader["IsActive"] == DBNull.Value) ? false : (bool)dataReader["IsActive"];

                        listPricingoption.Add(SPOobj);
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listPricingoption;

        }
        public string DeletePricingOption(SupplierPricingOption objSPO)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@PricingOptionId", 100, Guid.Parse(objSPO.PricingOptionId), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@SupplierServiceDetailsRateId", 100, Guid.Parse(objSPO.SupplierServiceDetailsRateId), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@SupplierServiceId", 100, Guid.Parse(objSPO.SupplierServiceId), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(objSPO.DeletedBy), SqlDbType.UniqueIdentifier);
                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_PricingOptionDelete", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), objSPO.DeletedBy);
            }
            return retstr;

        }

        #endregion "Supplier Pricing Option end"


        #region "Supplier Price Edit Rate Start"

        public string SaveUpdatePriceEditRate(string Purpose, SupplierPriceRateEdit objPriceEditRate)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                SqlHelper.inputparams("@PriceEditRateId", 100, Guid.Parse(objPriceEditRate.PriceEditRateId), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@PricingOptionId", 500, Guid.Parse(objPriceEditRate.PricingOptionId), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ChooseEditOption", 100, (objPriceEditRate.ChooseEditOption), SqlDbType.Int);
                SqlHelper.inputparams("@NetPrice", 100, objPriceEditRate.NetPrice, SqlDbType.Decimal);
                SqlHelper.inputparams("@MarkupPercentage", 100, (objPriceEditRate.MarkupPercentage), SqlDbType.Decimal);
                SqlHelper.inputparams("@GrossPrice", 100, (objPriceEditRate.GrossPrice), SqlDbType.Decimal);
                SqlHelper.inputparams("@CommissionPercentage", 100, objPriceEditRate.CommissionPercentage, SqlDbType.Decimal);
                SqlHelper.inputparams("@SupplierServiceId", 100, Guid.Parse(objPriceEditRate.SupplierServiceId), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Monday", 100, objPriceEditRate.Monday, SqlDbType.Bit);
                SqlHelper.inputparams("@Tuesday", 100, objPriceEditRate.Tuesday, SqlDbType.Bit);
                SqlHelper.inputparams("@Wednesday", 100, objPriceEditRate.Wednesday, SqlDbType.Bit);
                SqlHelper.inputparams("@Thursday", 100, objPriceEditRate.Thursday, SqlDbType.Bit);
                SqlHelper.inputparams("@Friday", 100, objPriceEditRate.Friday, SqlDbType.Bit);
                SqlHelper.inputparams("@Saturday", 100, objPriceEditRate.Saturday, SqlDbType.Bit);
                SqlHelper.inputparams("@Sunday", 100, objPriceEditRate.Sunday, SqlDbType.Bit);
                //SqlHelper.inputparams("@RatevalidFromDay", 100, (objPriceEditRate.RatevalidFromDay), SqlDbType.Int);
               // SqlHelper.inputparams("@RatevalidToDay", 100, (objPriceEditRate.RatevalidToDay), SqlDbType.Int);
                SqlHelper.inputparams("@Rounding", 100, (objPriceEditRate.Rounding), SqlDbType.Int);
                SqlHelper.inputparams("@IsActive", 100, objPriceEditRate.PriceEditIsActive, SqlDbType.Bit);
                SqlHelper.inputparams("@CreatedBy", 100, Guid.Parse(objPriceEditRate.CreatedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ModifiedBy", 100, Guid.Parse(objPriceEditRate.ModifiedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@IsDeleted", 100, objPriceEditRate.IsDeleted, SqlDbType.Bit);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(objPriceEditRate.DeletedBy), SqlDbType.UniqueIdentifier);
                
                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_PriceEditRateSaveUpdate", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), objPriceEditRate.CreatedBy);
            }
            return retstr;

        }
        public List<SupplierPriceRateEdit> PriceEditRateRetrive(Guid SupplierServiceId, Guid PricingOptionId,bool Isdeletedflag=false)
        {
            List<SupplierPriceRateEdit> listPricingoption = new List<SupplierPriceRateEdit>();

            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@SupplierServiceId", 100, SupplierServiceId, SqlDbType.UniqueIdentifier);
//                SqlHelper.inputparams("@PriceEditRateId", 100, PriceEditRateId, SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@PricingOptionId", 100, PricingOptionId, SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@IsDeleted", 100, Isdeletedflag, SqlDbType.Bit);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "GetPriceEditRate", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {
                        
                        SupplierPriceRateEdit SPOobj = new SupplierPriceRateEdit();
                        SPOobj.PriceEditRateId = ((dataReader["PriceEditRateId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["PriceEditRateId"]).ToString();
                        SPOobj.PricingOptionId = ((dataReader["PricingOptionId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["PricingOptionId"]).ToString();
                        SPOobj.ChooseEditOption = (dataReader["ChooseEditOption"] == DBNull.Value) ? 0 : (int)dataReader["ChooseEditOption"];
                        SPOobj.NetPrice = (dataReader["NetPrice"] == DBNull.Value) ? 0 : (decimal)dataReader["NetPrice"];
                        SPOobj.MarkupPercentage = (dataReader["MarkupPercentage"] == DBNull.Value) ? 0 : (decimal)dataReader["MarkupPercentage"];
                        SPOobj.GrossPrice = (dataReader["GrossPrice"] == DBNull.Value) ? 0 : (decimal)dataReader["GrossPrice"];
                        SPOobj.CommissionPercentage = (dataReader["CommissionPercentage"] == DBNull.Value) ? 0 : (decimal)dataReader["CommissionPercentage"];                        
                        SPOobj.SupplierServiceId = ((dataReader["SupplierServiceId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["SupplierServiceId"]).ToString();
                        SPOobj.Monday = (dataReader["Monday"] == DBNull.Value) ? false : (bool)dataReader["Monday"];
                        SPOobj.Tuesday = (dataReader["Tuesday"] == DBNull.Value) ? false : (bool)dataReader["Tuesday"];
                        SPOobj.Wednesday = (dataReader["Wednesday"] == DBNull.Value) ? false : (bool)dataReader["Wednesday"];
                        SPOobj.Thursday = (dataReader["Thursday"] == DBNull.Value) ? false : (bool)dataReader["Thursday"];
                        SPOobj.Friday = (dataReader["Friday"] == DBNull.Value) ? false : (bool)dataReader["Friday"];
                        SPOobj.Saturday = (dataReader["Saturday"] == DBNull.Value) ? false : (bool)dataReader["Saturday"];
                        SPOobj.Sunday = (dataReader["Sunday"] == DBNull.Value) ? false : (bool)dataReader["Sunday"];
                        //SPOobj.RatevalidFromDay = (dataReader["RatevalidFromDay"] == DBNull.Value) ? 0 : (int)dataReader["RatevalidFromDay"];
                        //SPOobj.RatevalidToDay = (dataReader["RatevalidToDay"] == DBNull.Value) ? 0 : (int)dataReader["RatevalidToDay"];
                        SPOobj.Rounding = (dataReader["Rounding"] == DBNull.Value) ? 0 : (int)dataReader["Rounding"];
                        SPOobj.CreatedBy = ((dataReader["CreatedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["CreatedBy"]).ToString();
                        SPOobj.ModifiedBy = ((dataReader["ModifiedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ModifiedBy"]).ToString();
                        SPOobj.DeletedBy = ((dataReader["DeletedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["DeletedBy"]).ToString();
                        SPOobj.IsDeleted = (dataReader["IsDeleted"] == DBNull.Value) ? false : (bool)dataReader["IsDeleted"];
                        SPOobj.PriceEditIsActive = (dataReader["IsActive"] == DBNull.Value) ? false : (bool)dataReader["IsActive"];
                        
                        listPricingoption.Add(SPOobj);
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listPricingoption;

        }
        public string DeletePriceEditRate(SupplierPriceRateEdit objSPO)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@PricingOptionId", 100, Guid.Parse(objSPO.PricingOptionId), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@PriceEditRateId", 100, Guid.Parse(objSPO.PriceEditRateId), SqlDbType.UniqueIdentifier);                
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(objSPO.DeletedBy), SqlDbType.UniqueIdentifier);
                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_PriceEditRateDelete", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), objSPO.DeletedBy);
            }
            return retstr;

        }

        #endregion "Supplier Price Edit Rate End"

        #region "Supplier Voucher Notes Start"
        public string SaveUpdateSupplierCommunicationNotes(string Purpose, SupplierCommunicationNotes objSCN)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                SqlHelper.inputparams("@NoteID", 100, Guid.Parse(objSCN.NotesinfoID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Notes", 500, objSCN.Notesinfo, SqlDbType.VarChar);
                SqlHelper.inputparams("@SupplierID", 100, Guid.Parse(objSCN.SupplierId), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@IsAutoSelected", 100, (objSCN.Autoselected), SqlDbType.Bit);
                SqlHelper.inputparams("@CreatedBy", 100, Guid.Parse(objSCN.CreatedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ModifiedBy", 100, Guid.Parse(objSCN.ModifiedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@IsDeleted", 100, objSCN.IsDeleted, SqlDbType.Bit);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(objSCN.DeletedBy), SqlDbType.UniqueIdentifier);

                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_CommunicationNotesSaveUpdate", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), objSCN.CreatedBy);
            }
            return retstr;

        }
        public List<SupplierCommunicationNotes> SupplierCommunicationNoteRetrive(Guid SupplierId)
        {
            List<SupplierCommunicationNotes> listSupplierCommunicationNotes = new List<SupplierCommunicationNotes>();

            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@SupplierID", 100, SupplierId, SqlDbType.UniqueIdentifier);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_GetCommunicationNotes", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {

                        SupplierCommunicationNotes SPOobj = new SupplierCommunicationNotes();
                        SPOobj.NotesinfoID = ((dataReader["NoteID"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["NoteID"]).ToString();
                        SPOobj.Notesinfo = ((dataReader["Notes"] == DBNull.Value) ? string.Empty : (string)dataReader["Notes"]).ToString();
                        SPOobj.SupplierId = ((dataReader["SupplierID"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["SupplierID"]).ToString();
                        SPOobj.Autoselected = (dataReader["IsAutoSelected"] == DBNull.Value) ? false : (bool)dataReader["IsAutoSelected"]; 
                        SPOobj.CreatedBy = ((dataReader["CreatedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["CreatedBy"]).ToString();
                        SPOobj.ModifiedBy = ((dataReader["ModifiedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ModifiedBy"]).ToString();
                        SPOobj.DeletedBy = ((dataReader["DeletedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["DeletedBy"]).ToString();
                        listSupplierCommunicationNotes.Add(SPOobj);
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listSupplierCommunicationNotes;

        }
        public string DeleteSupplierCommunicationNotes(SupplierCommunicationNotes objSCN)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@NoteID", 100, Guid.Parse(objSCN.NotesinfoID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(objSCN.DeletedBy), SqlDbType.UniqueIdentifier);
                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_CommunicationNotesDelete", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), objSCN.DeletedBy);
            }
            return retstr;

        }
        #endregion "Supplier Voucher Notes End"

        #region "Supplier Communication Content Start"
        public string SaveUpdateSupplierCommunicationContent(string Purpose, SupplierCommunicationContentdata objSCN)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                SqlHelper.inputparams("@ContentID", 100, Guid.Parse(objSCN.ContentID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ContentName", 500, objSCN.ContentName, SqlDbType.VarChar);
                SqlHelper.inputparams("@ContentFor", 500, objSCN.ContentFor, SqlDbType.VarChar);
                SqlHelper.inputparams("@ContentType", 100, Guid.Parse(objSCN.ContentType), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@SupplierID", 100, Guid.Parse(objSCN.SupplierID), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Heading", 500, objSCN.Heading, SqlDbType.VarChar);
                SqlHelper.inputparams("@ReportImage", 500, objSCN.ReportImage, SqlDbType.VarChar);
                SqlHelper.inputparams("@OnlineImage", 500, objSCN.OnlineImage, SqlDbType.VarChar);
                SqlHelper.inputparams("@BodyHtml", 10000, objSCN.BodyHtml, SqlDbType.VarChar);                
                SqlHelper.inputparams("@CreatedBy", 100, Guid.Parse(objSCN.CreatedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ModifiedBy", 100, Guid.Parse(objSCN.ModifiedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@IsDeleted", 100, objSCN.IsDeleted, SqlDbType.Bit);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(objSCN.DeletedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ServiceID", 100, Guid.Parse(objSCN.ServiceID), SqlDbType.UniqueIdentifier);

                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_SupplierCommunication", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), objSCN.CreatedBy);
            }
            return retstr;

        }
        public List<SupplierCommunicationContentdata> SupplierCommunicationContentRetrive(Guid SupplierServiceId)
        {
            List<SupplierCommunicationContentdata> listCommunicationcontent = new List<SupplierCommunicationContentdata>();

            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@SupplierID", 100, SupplierServiceId, SqlDbType.UniqueIdentifier);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_GetSupplierCommunication", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {

                        SupplierCommunicationContentdata SPOobj = new SupplierCommunicationContentdata();
                        SPOobj.ContentID = ((dataReader["ContentID"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ContentID"]).ToString();
                        SPOobj.ContentName = ((dataReader["ContentName"] == DBNull.Value) ? string.Empty : (string)dataReader["ContentName"]).ToString();
                        SPOobj.ContentFor = (dataReader["ContentFor"] == DBNull.Value) ? string.Empty : dataReader["ContentFor"].ToString();
                        SPOobj.ContentType = ((dataReader["ContentType"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ContentType"]).ToString();
                        SPOobj.SupplierID = ((dataReader["SupplierID"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["SupplierID"]).ToString();
                        SPOobj.Heading = (dataReader["Heading"] == DBNull.Value) ? string.Empty : (string)dataReader["Heading"];
                        SPOobj.ReportImage = (dataReader["ReportImage"] == DBNull.Value) ? string.Empty : (string)dataReader["ReportImage"];
                        SPOobj.OnlineImage = (dataReader["OnlineImage"] == DBNull.Value) ? string.Empty : (string)dataReader["OnlineImage"];
                        SPOobj.BodyHtml = (dataReader["BodyHtml"] == DBNull.Value) ? string.Empty : (string)dataReader["BodyHtml"];
                        SPOobj.IsActivated = (dataReader["IsActivated"] == DBNull.Value) ? false : (bool)dataReader["IsActivated"];
                        SPOobj.CreatedBy = ((dataReader["CreatedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["CreatedBy"]).ToString();
                        SPOobj.ModifiedBy = ((dataReader["ModifiedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ModifiedBy"]).ToString();
                        SPOobj.DeletedBy = ((dataReader["DeletedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["DeletedBy"]).ToString();
                        SPOobj.IsDeleted = (dataReader["IsDeleted"] == DBNull.Value) ? false : (bool)dataReader["IsDeleted"];
                        SPOobj.ServiceID = ((dataReader["Serviceid"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Serviceid"]).ToString();


                        listCommunicationcontent.Add(SPOobj);
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listCommunicationcontent;

        }
        public string DeleteSupplierCommunicationContent(SupplierCommunicationContentdata objSCN)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@ContentID", 100, Guid.Parse(objSCN.ContentID), SqlDbType.UniqueIdentifier);                
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(objSCN.DeletedBy), SqlDbType.UniqueIdentifier);
                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_SupplierCommunicationDelete", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), objSCN.DeletedBy);
            }
            return retstr;

        }

        public List<supplierservicemenulist> SupplierServiceMenuRetrive(Guid SupplierServiceId)
        {
            List<supplierservicemenulist> listofSSmenu = new List<supplierservicemenulist>();

            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@SupplierId", 100, SupplierServiceId, SqlDbType.UniqueIdentifier);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "GetSupplierServicesName", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {

                        supplierservicemenulist SSMobj = new supplierservicemenulist();
                        SSMobj.SupplierServiceID = ((dataReader["SupplierServiceID"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["SupplierServiceID"]).ToString();
                        SSMobj.ServiceName = ((dataReader["ServiceName"] == DBNull.Value) ? string.Empty : (string)dataReader["ServiceName"]).ToString();
                        SSMobj.ID = (dataReader["ID"] == DBNull.Value) ? string.Empty : dataReader["ID"].ToString();

                        listofSSmenu.Add(SSMobj);
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listofSSmenu;

        }
        #endregion "Supplier Communication Content End"

    }
}
