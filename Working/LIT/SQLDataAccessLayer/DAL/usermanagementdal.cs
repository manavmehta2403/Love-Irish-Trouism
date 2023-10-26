using SQLDataAccessLayer.SQLHelper;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SQLDataAccessLayer.DAL
{
    public class UserManagementDAL
    {
        #region "Login Start"
        // SQLHelper.SQLHelper sqlHelper = new SQLHelper.SQLHelper();

        Errorlog errobj = new Errorlog();
        public string UserloginVerify(string username, string password)
        {
            /* // SqlHelper.ExecuteNonQuery().
              string retnstr = string.Empty;
              if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password)) {
                  sqlHelper.inputparams("@Action", 100, "C", SqlDbType.VarChar);
                  sqlHelper.inputparams("@Username", 100,username, SqlDbType.VarChar);
                  sqlHelper.inputparams("@password", 100, password, SqlDbType.VarChar);
                  retnstr=sqlHelper.FetchSingleRecords("SP_UserChecklogin", string.Empty);
              }
              return retnstr;
              */

            var retnstr = new object();
            SqlHelper.parameters = null;
            try 
            {            
                SqlHelper.inputparams("@Action", 100, "C", SqlDbType.VarChar);
                SqlHelper.inputparams("@Username", 100, username, SqlDbType.VarChar);
                SqlHelper.inputparams("@password", 100, password, SqlDbType.VarChar);
                retnstr = SqlHelper.ExecuteScalar(DBConfiguration.instance.ConnectionString, "SP_UserChecklogin", SqlHelper.parameters);
            }             
            catch(Exception ex) 
            {
                errobj.WriteErrorLoginfo("LoadDropDownListValues", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return retnstr.ToString();
                
        }
        #endregion "Login End"

    }
}
