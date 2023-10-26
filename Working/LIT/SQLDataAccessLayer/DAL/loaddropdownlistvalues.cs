using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLDataAccessLayer.Models;
using System.Data.SqlClient;
using SQLDataAccessLayer.SQLHelper;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;

namespace SQLDataAccessLayer.DAL
{
    public class LoadDropDownListValues
    {
        Errorlog errobj = new Errorlog();
        public List<CommonValueList> LoadCommonValues(string Purpose)
        {

            List<CommonValueList> commonValueLists = new List<CommonValueList>();
            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                using (SqlDataReader rdr = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_LoadCommonValues", SqlHelper.parameters))
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            CommonValueList CVLObj = new CommonValueList();
                            CVLObj.ValueField = ((rdr["ValueField"] == DBNull.Value) ? Guid.Empty : (Guid)(rdr["ValueField"]));
                            CVLObj.TextField = (rdr["TextField"] == DBNull.Value) ? string.Empty : (string)rdr["TextField"];
                            commonValueLists.Add(CVLObj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("LoadDropDownListValues", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return commonValueLists;//GetDbvalue(Purpose, "SP_LoadCommonValues");

        }

        public List<CommonValueList> LoadCommonValuesWithDefault(string Purpose)
        {

            List<CommonValueList> commonValueLists = new List<CommonValueList>();
            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                using (SqlDataReader rdr = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_LoadCommonValues", SqlHelper.parameters))
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            CommonValueList CVLObj = new CommonValueList();
                            CVLObj.IsDefault = ((rdr["IsDefault"] == DBNull.Value) ? false : (bool)rdr["IsDefault"]);
                            CVLObj.ValueField = ((rdr["ValueField"] == DBNull.Value) ? Guid.Empty : (Guid)(rdr["ValueField"]));
                            CVLObj.TextField = (rdr["TextField"] == DBNull.Value) ? string.Empty : (string)rdr["TextField"];
                            commonValueLists.Add(CVLObj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("LoadDropDownListValues", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return commonValueLists;//GetDbvalue(Purpose, "SP_LoadCommonValues");

        }

        public string LoadFolderName(string Purpose)
        {

            string retnstr = string.Empty;
            object objfolder = new object();
            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                objfolder = SqlHelper.ExecuteScalar(DBConfiguration.instance.ConnectionString, "SP_LoadCommonValues", SqlHelper.parameters);
                if (objfolder != null)
                {
                    return objfolder.ToString();
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("LoadDropDownListValues", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return retnstr;//GetDbvalue(Purpose, "SP_LoadCommonValues");

        }


        public List<Userdetails> LoadUserDropDownlist(string Purpose)
        {

            List<Userdetails> commonValueLists = new List<Userdetails>();
            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                using (SqlDataReader rdr = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_LoadCommonValues", SqlHelper.parameters))
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            Userdetails UMObj = new Userdetails();
                            UMObj.Fullname = ((rdr["FullName"] == DBNull.Value) ? string.Empty : (string)rdr["FullName"]);
                            UMObj.Userid = ((rdr["UserId"] == DBNull.Value) ? Guid.Empty : (Guid)(rdr["UserId"]));
                            UMObj.UserName = (rdr["UserName"] == DBNull.Value) ? string.Empty : (string)rdr["UserName"];
                            UMObj.UserRoldID = (rdr["UserRoldID"] == DBNull.Value) ? Guid.Empty : (Guid)rdr["UserRoldID"];
                            UMObj.Userroles = (rdr["UserRole"] == DBNull.Value) ? string.Empty : (string)rdr["UserRole"];
                            UMObj.EmailAddress = (rdr["EmailAddress"] == DBNull.Value) ? string.Empty : (string)rdr["EmailAddress"];
                            commonValueLists.Add(UMObj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("LoadDropDownListValues", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return commonValueLists;//GetDbvalue(Purpose, "SP_LoadCommonValues");

        }

        public string Currentuseridinfo(string loginusername)
        {
            string loginuserid = string.Empty;
            List<Userdetails> ListUserdet = new List<Userdetails>();
            try
            {
                ListUserdet = LoadUserDropDownlist("User");
                if (ListUserdet != null && ListUserdet.Count > 0)
                {
                    Guid Userid = Guid.Empty;
                    Userid = ListUserdet.Where(x => x.UserName.ToString().ToUpper().Trim() == loginusername.ToString().ToUpper().Trim()).FirstOrDefault().Userid;
                    loginuserid = Userid.ToString();

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("LoadDropDownListValues", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return loginuserid;
        }

        public string CurrentUserRole(string loginusername)
        {
            string loginuserrole = string.Empty;
            List<Userdetails> ListUserdet = new List<Userdetails>();
            try
            {
                ListUserdet = LoadUserDropDownlist("User");
                if (ListUserdet != null && ListUserdet.Count > 0)
                {
                    loginuserrole = ListUserdet.Where(x => x.UserName.ToString().ToUpper().Trim() == loginusername.ToString().ToUpper().Trim()).FirstOrDefault().Userroles;
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("LoadDropDownListValues", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return loginuserrole;
        }

        public List<CommonValueCountrycity> LoadCommonValuesCountry(string Purpose, CommonValueCountrycity objCvc)
        {

            List<CommonValueCountrycity> commonValueLists = new List<CommonValueCountrycity>();
            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                using (SqlDataReader rdr = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_LoadCommonValuesCountry", SqlHelper.parameters))
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            CommonValueCountrycity objcvcinfo = new CommonValueCountrycity();
                            objcvcinfo.CountryId = ((rdr["CountryId"] == DBNull.Value) ? Guid.Empty : (Guid)(rdr["CountryId"]));
                            objcvcinfo.CountryName = (rdr["CountryName"] == DBNull.Value) ? string.Empty : (string)rdr["CountryName"];
                            commonValueLists.Add(objcvcinfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("LoadDropDownListValues", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return commonValueLists;//GetDbvalue(Purpose, "SP_LoadCommonValues");

        }

        public List<CommonValueCountrycity> LoadCommonValuesCity(string Purpose, CommonValueCountrycity objCvc)
        {

            List<CommonValueCountrycity> commonValueLists = new List<CommonValueCountrycity>();
            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                SqlHelper.inputparams("@RegionID", 100, objCvc.RegionId, SqlDbType.UniqueIdentifier);
                using (SqlDataReader rdr = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_LoadCommonValuesCity", SqlHelper.parameters))
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            CommonValueCountrycity objcvcinfo = new CommonValueCountrycity();
                            objcvcinfo.CityId = ((rdr["CityId"] == DBNull.Value) ? Guid.Empty : (Guid)(rdr["CityId"]));
                            objcvcinfo.CityName = (rdr["CityName"] == DBNull.Value) ? string.Empty : (string)rdr["CityName"];
                            commonValueLists.Add(objcvcinfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("LoadDropDownListValues", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return commonValueLists;//GetDbvalue(Purpose, "SP_LoadCommonValues");

        }


        public List<CommonValueCountrycity> LoadCommonValuesState(string Purpose, CommonValueCountrycity objCvc)
        {

            List<CommonValueCountrycity> commonValueLists = new List<CommonValueCountrycity>();
            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                SqlHelper.inputparams("@CountryID", 100, objCvc.CountryId, SqlDbType.UniqueIdentifier);
                using (SqlDataReader rdr = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_LoadCommonValuesState", SqlHelper.parameters))
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            CommonValueCountrycity objcvcinfo = new CommonValueCountrycity();
                            objcvcinfo.StatesId = ((rdr["StatesId"] == DBNull.Value) ? Guid.Empty : (Guid)(rdr["StatesId"]));
                            objcvcinfo.StatesName = (rdr["StatesName"] == DBNull.Value) ? string.Empty : (string)rdr["StatesName"];
                            commonValueLists.Add(objcvcinfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("LoadDropDownListValues", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return commonValueLists;//GetDbvalue(Purpose, "SP_LoadCommonValues");

        }


        public List<CommonValueCountrycity> LoadCommonValuesRegion(string Purpose, CommonValueCountrycity objCvc)
        {

            List<CommonValueCountrycity> commonValueLists = new List<CommonValueCountrycity>();
            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                SqlHelper.inputparams("@StatesID", 100, objCvc.StatesId, SqlDbType.UniqueIdentifier);
                using (SqlDataReader rdr = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_LoadCommonValuesRegion", SqlHelper.parameters))
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            CommonValueCountrycity objcvcinfo = new CommonValueCountrycity();
                            objcvcinfo.RegionId = ((rdr["RegionId"] == DBNull.Value) ? Guid.Empty : (Guid)(rdr["RegionId"]));
                            objcvcinfo.RegionName = (rdr["RegionName"] == DBNull.Value) ? string.Empty : (string)rdr["RegionName"];
                            commonValueLists.Add(objcvcinfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("LoadDropDownListValues", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return commonValueLists;//GetDbvalue(Purpose, "SP_LoadCommonValues");

        }

        public List<SupplierServiceType> LoadSupplierServiceTypes()
        {

            List<SupplierServiceType> commonValueLists = new List<SupplierServiceType>();
            SqlHelper.parameters = null;
            try
            {
                using (SqlDataReader rdr = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "GetSupplierServiceType", SqlHelper.parameters))
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            SupplierServiceType objcvcinfo = new SupplierServiceType();
                            objcvcinfo.ServiceTypeID = ((rdr["ServiceTypeID"] == DBNull.Value) ? Guid.Empty : (Guid)(rdr["ServiceTypeID"])).ToString();
                            objcvcinfo.ServiceTypeName = (rdr["ServiceTypeName"] == DBNull.Value) ? string.Empty : (string)rdr["ServiceTypeName"];
                            objcvcinfo.IsDefault = (rdr["IsDefault"] == DBNull.Value) ? false : (bool)rdr["IsDefault"];
                            commonValueLists.Add(objcvcinfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("LoadDropDownListValues", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return commonValueLists;//GetDbvalue(Purpose, "SP_LoadCommonValues");

        }


        public string LoadItineraryAutoNumber()
        {
            object ItineraryAutoNumber = new object();
            SqlHelper.parameters = null;
            try
            {
                ItineraryAutoNumber = SqlHelper.ExecuteScalar(DBConfiguration.instance.ConnectionString, "GetItineraryAutoNumber", SqlHelper.parameters);
                if (ItineraryAutoNumber != null)
                {
                    return ItineraryAutoNumber.ToString();//GetDbvalue(Purpose, "SP_LoadCommonValues");
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("LoadDropDownListValues", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return string.Empty;
        }
        public string LoadSupplierAutoNumber()
        {
            object ItineraryAutoNumber = new object();
            SqlHelper.parameters = null;
            try
            {
                ItineraryAutoNumber = SqlHelper.ExecuteScalar(DBConfiguration.instance.ConnectionString, "GetSupplierAutoNumber", SqlHelper.parameters);
                if (ItineraryAutoNumber != null)
                {
                    return ItineraryAutoNumber.ToString();//GetDbvalue(Purpose, "SP_LoadCommonValues");
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("LoadDropDownListValues", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return string.Empty;
        }


        public List<CommonValueList> LoadGroupinfo(string Purpose)
        {

            List<CommonValueList> commonValueLists = new List<CommonValueList>();
            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                using (SqlDataReader rdr = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "GetGroupinfo", SqlHelper.parameters))
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            CommonValueList CVLObj = new CommonValueList();
                            CVLObj.ValueField = ((rdr["ValueField"] == DBNull.Value) ? Guid.Empty : (Guid)(rdr["ValueField"]));
                            CVLObj.TextField = (rdr["TextField"] == DBNull.Value) ? string.Empty : (string)rdr["TextField"];
                            commonValueLists.Add(CVLObj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("LoadDropDownListValues", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return commonValueLists;//GetDbvalue(Purpose, "SP_LoadCommonValues");

        }

        public List<Currencydetails> LoadCurrencyDetails(string serviceid = null)
        {

            List<Currencydetails> CurrencyValueLists = new List<Currencydetails>();
            SqlHelper.parameters = null;
            string spname = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(serviceid))
                {
                    SqlHelper.inputparams("@ServiceID", 100, Guid.Parse(serviceid), SqlDbType.UniqueIdentifier);
                    spname = "GetServiceCurrencyInfo";
                }
                else
                {
                    spname = "GetCurrencyDetails";
                }
                using (SqlDataReader rdr = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, spname, SqlHelper.parameters))
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            Currencydetails CurObj = new Currencydetails();
                            CurObj.CurrencyID = ((rdr["CurrencyID"] == DBNull.Value) ? Guid.Empty : (Guid)(rdr["CurrencyID"])).ToString();
                            CurObj.CurrencyName = (rdr["CurrencyName"] == DBNull.Value) ? string.Empty : (string)rdr["CurrencyName"];
                            CurObj.CurrencyCode = (rdr["CurrencyCode"] == DBNull.Value) ? string.Empty : (string)rdr["CurrencyCode"];
                            CurObj.Isenable = (rdr["Isenable"] == DBNull.Value) ? false : (bool)rdr["Isenable"];
                            CurObj.DisplayFormat = (rdr["DisplayFormat"] == DBNull.Value) ? string.Empty : (string)rdr["DisplayFormat"];
                            CurObj.CurrencyCulture = (rdr["CurrencyCulture"] == DBNull.Value) ? string.Empty : (string)rdr["CurrencyCulture"];
                            CurrencyValueLists.Add(CurObj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("LoadDropDownListValues", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return CurrencyValueLists;//GetDbvalue(Purpose, "SP_LoadCommonValues");

        }

        public List<BkRequestStatus> LoadRequestStatus()
        {

            List<BkRequestStatus> commonValueLists = new List<BkRequestStatus>();
            SqlHelper.parameters = null;
            try
            {
                using (SqlDataReader rdr = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "GetRequestStatus", SqlHelper.parameters))
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            BkRequestStatus objcvcinfo = new BkRequestStatus();
                            objcvcinfo.RequestStatusID = ((rdr["RequestStatusId"] == DBNull.Value) ? Guid.Empty : (Guid)(rdr["RequestStatusId"])).ToString();
                            objcvcinfo.RequestStatusName = (rdr["RequestStatusName"] == DBNull.Value) ? string.Empty : (string)rdr["RequestStatusName"];
                            commonValueLists.Add(objcvcinfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("LoadDropDownListValues", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return commonValueLists;//GetDbvalue(Purpose, "SP_LoadCommonValues");

        }

        public List<Currencydetails> CurrencyinfoReterive(string serviceid)
        {
            List<Currencydetails> ListofCurrencyinfo = new List<Currencydetails>();
            ListofCurrencyinfo = LoadCurrencyDetails(serviceid);
            if (ListofCurrencyinfo != null && ListofCurrencyinfo.Count > 0)
            {
                return ListofCurrencyinfo;

            }
            return ListofCurrencyinfo;
        }

        public List<CommunicationTypeStatus> LoadCommunicationTypes()
        {

            List<CommunicationTypeStatus> commonValueLists = new List<CommunicationTypeStatus>();
            SqlHelper.parameters = null;
            try
            {
                using (SqlDataReader rdr = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_GetCommunicationTypes", SqlHelper.parameters))
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            CommunicationTypeStatus objcts = new CommunicationTypeStatus();
                            objcts.TypeID = ((rdr["TypeID"] == DBNull.Value) ? Guid.Empty : (Guid)(rdr["TypeID"])).ToString();
                            objcts.TypeName = (rdr["TypeName"] == DBNull.Value) ? string.Empty : (string)rdr["TypeName"];
                            commonValueLists.Add(objcts);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("LoadDropDownListValues", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return commonValueLists;//GetDbvalue(Purpose, "SP_LoadCommonValues");

        }



        #region "Pickup/Drop Location start"
        public List<Pickupdroplocation> Loadpickupdroplocation()
        {

            List<Pickupdroplocation> PDLocLists = new List<Pickupdroplocation>();
            SqlHelper.parameters = null;
            try
            {
                using (SqlDataReader rdr = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_GetPickupDropLocation", SqlHelper.parameters))
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            Pickupdroplocation objcts = new Pickupdroplocation();
                            objcts.PickupDropLocationId = ((rdr["PickupDropLocationId"] == DBNull.Value) ? Guid.Empty : (Guid)(rdr["PickupDropLocationId"])).ToString();
                            objcts.LocationName = (rdr["LocationName"] == DBNull.Value) ? string.Empty : (string)rdr["LocationName"];
                            PDLocLists.Add(objcts);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("LoadDropDownListValues", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return PDLocLists;//GetDbvalue(Purpose, "SP_LoadCommonValues");

        }

        #endregion "Pickup/Drop Location end"

        #region Salutation start
        public List<Salutation> Loadsalutation()
        {

            List<Salutation> SalutationLists = new List<Salutation>();
            SqlHelper.parameters = null;
            try
            {
                using (SqlDataReader rdr = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_GetSalutation", SqlHelper.parameters))
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            Salutation objcts = new Salutation();
                            objcts.SalutationId = ((rdr["SalutationId"] == DBNull.Value) ? Guid.Empty : (Guid)(rdr["SalutationId"])).ToString();
                            objcts.SalutationName = (rdr["SalutationName"] == DBNull.Value) ? string.Empty : (string)rdr["SalutationName"];
                            SalutationLists.Add(objcts);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("LoadDropDownListValues", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return SalutationLists;//GetDbvalue(Purpose, "SP_LoadCommonValues");

        }
        #endregion Salutation end

        #region Salutation start
        public List<TourList> LoadTourList()
        {

            List<TourList> TourlistnameLists = new List<TourList>();
            SqlHelper.parameters = null;
            try
            {
                using (SqlDataReader rdr = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "GetTourlist", SqlHelper.parameters))
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            TourList objcts = new TourList();
                            objcts.Tourlistid = ((rdr["Tourlistid"] == DBNull.Value) ? Guid.Empty : (Guid)(rdr["Tourlistid"])).ToString();
                            objcts.Tourlistname = (rdr["Tourlistname"] == DBNull.Value) ? string.Empty : (string)rdr["Tourlistname"];
                            TourlistnameLists.Add(objcts);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("LoadDropDownListValues", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return TourlistnameLists;//GetDbvalue(Purpose, "SP_LoadCommonValues");

        }
        #endregion Salutation end

    }


}
