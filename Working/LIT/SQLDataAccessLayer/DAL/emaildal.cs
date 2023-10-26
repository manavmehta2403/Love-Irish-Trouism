using SQLDataAccessLayer.Models;
using SQLDataAccessLayer.SQLHelper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using LITModels.LITModels.Models;
using System.Reflection.PortableExecutable;

namespace SQLDataAccessLayer.DAL
{
    public class EmailDal
    {
        Errorlog errobj = new Errorlog();
        public List<string> GetMarkervalues(String Text, string Startmarker, String Endmarker)
        {
            List<string> result = new List<string>();           
           
            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@Text", 10000, Text, SqlDbType.VarChar);
                SqlHelper.inputparams("@StartMarker", 100, Startmarker, SqlDbType.VarChar);
                SqlHelper.inputparams("@EndMarker", 100, Endmarker, SqlDbType.VarChar);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "WEB_GetHtmlBookmarks", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {

                        string MarkerName = (dataReader["MarkerName"] == DBNull.Value) ? string.Empty : (string)dataReader["MarkerName"];
                       // string OriginalText = (dataReader["OriginalText"] == DBNull.Value) ? string.Empty : (string)dataReader["OriginalText"];

                        result.Add(MarkerName);
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("EmailDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            
            return result;
        }

        public Tuple<string,string> GetSupplierEmailandName(String Supplierid)
        {
            string SupplierName = string.Empty;
            string Email = string.Empty;
            Tuple<string, string> trobj = null;
            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@Supplierid", 100, Guid.Parse(Supplierid), SqlDbType.UniqueIdentifier);
                
     
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "GetSuppliernameemail", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {

                        SupplierName = (dataReader["SupplierName"] == DBNull.Value) ? string.Empty : (string)dataReader["SupplierName"];
                        Email = (dataReader["Email"] == DBNull.Value) ? string.Empty : (string)dataReader["Email"];
                        trobj=new Tuple<string, string>(SupplierName,Email);

                        
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("EmailDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return trobj;
            
        }

        public MailCredentials GetMailCredentials()
        {
            MailCredentials mailobj=null;
            SqlHelper.parameters = null;
            try
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "GetEmailSettings", SqlHelper.parameters))
                {
                    mailobj = new MailCredentials();
                    while (dataReader.Read())
                    {
                        mailobj.SMTPServer = (dataReader["SMTPServer"] == DBNull.Value) ? string.Empty : (string)dataReader["SMTPServer"];
                        mailobj.SMTPPort =   (dataReader["SMTPPort"] == DBNull.Value) ? 0 : Convert.ToInt32(dataReader["SMTPPort"]);
                        mailobj.IsSSLRequired = (dataReader["IsSSLRequired"] == DBNull.Value) ? false : (bool)dataReader["IsSSLRequired"];
                        mailobj.FromEmail = (dataReader["FromEmail"] == DBNull.Value) ? string.Empty : (string)dataReader["FromEmail"];
                        mailobj.FromEmailName = (dataReader["FromEmailName"] == DBNull.Value) ? string.Empty : (string)dataReader["FromEmailName"];
                        mailobj.FromEmailPassword = (dataReader["FromEmailPassword"] == DBNull.Value) ? string.Empty : (string)dataReader["FromEmailPassword"];
                        mailobj.BCCEmail = (dataReader["BCCEmail"] == DBNull.Value) ? string.Empty : (string)dataReader["BCCEmail"];                       
                        
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("EmailDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

            return mailobj;
        }


        public string GetUserEmailsign(String userid)
        {
            
            string Emailsign = string.Empty;
            
            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@userid", 100, Guid.Parse(userid), SqlDbType.UniqueIdentifier);


                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "GetEmailSign", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {

                        Emailsign = (dataReader["EmailSignature"] == DBNull.Value) ? string.Empty : (string)dataReader["EmailSignature"];
                       

                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("EmailDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return Emailsign;

        }

        public string SaveSupplierEmails(string purpose,SupplierEmailSetting objset)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@action", 500, purpose, SqlDbType.VarChar);
                SqlHelper.inputparams("@SupplierEmailSettingsid", 500, objset.SupplierEmailSettingsid, SqlDbType.BigInt);
                SqlHelper.inputparams("@FromAddress", 500, objset.FromAddress, SqlDbType.VarChar);
                SqlHelper.inputparams("@Bcc", 500, objset.Bcc, SqlDbType.VarChar);
                SqlHelper.inputparams("@EmailSubject", 500, objset.EmailSubject, SqlDbType.VarChar);
                SqlHelper.inputparams("@EmailBodyContentTemplate", 10000, objset.EmailBodyContentTemplate, SqlDbType.VarChar);
                SqlHelper.inputparams("@EmailTemplate", 5000, objset.EmailTemplate, SqlDbType.VarChar);
                SqlHelper.inputparams("@ToAddress", 500, objset.ToAddress, SqlDbType.VarChar);
                SqlHelper.inputparams("@EmailBodyContentPreview", 5000, objset.EmailBodyContentPreview, SqlDbType.VarChar);
                SqlHelper.inputparams("@Attachment", 500, objset.Attachment, SqlDbType.VarChar);
                SqlHelper.inputparams("@SendStatus", 10, objset.SendStatus, SqlDbType.Bit);
                SqlHelper.inputparams("@Error", 500, objset.Error, SqlDbType.VarChar);
                SqlHelper.inputparams("@SetbookingStatusAfterSuccessfulSend", 250, objset.SetbookingStatusAfterSuccessfulSend, SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Skipthispage", 10, objset.Skipthispage, SqlDbType.Bit);
                SqlHelper.inputparams("@Saveacopyofthesentemail", 10, objset.Saveacopyofthesentemail, SqlDbType.Bit);
                SqlHelper.inputparams("@Bcctosendersemailaddress", 10, objset.Bcctosendersemailaddress, SqlDbType.Bit);
                SqlHelper.inputparams("@Showpricedetailsforbooking", 10, objset.Showpricedetailsforbooking, SqlDbType.Bit);
                SqlHelper.inputparams("@Includeareadreceipt", 10, objset.Includeareadreceipt, SqlDbType.Bit);
                SqlHelper.inputparams("@GroupbookingsbySupplieremail", 10, objset.GroupbookingsbySupplieremail, SqlDbType.Bit);
                SqlHelper.inputparams("@CreatedBy", 100, objset.CreatedBy, SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ModifiedBy", 100, objset.ModifiedBy, SqlDbType.UniqueIdentifier);
                SqlHelper.outparams("@LastSupplierEmailSettingsid", 100, SqlDbType.BigInt);                
                SqlHelper.inputparams("@Supplierid", 500, objset.SupplierId, SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@SupplierName", 500, objset.SupplierName, SqlDbType.VarChar);

                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_SaveSupplierEmail", SqlHelper.parameters);
                retstr = ret.ToString();

                //int ret = 0;
                //using (
                //    SqlConnection connection = new SqlConnection(DBConfiguration.instance.ConnectionString))
                //    using(
                //    SqlCommand cmd = new SqlCommand("SP_SaveSupplierEmail", connection))
                //        {

                //    cmd.CommandType = CommandType.StoredProcedure;
                //    connection.Open();
                //    foreach (SqlParameter param in SqlHelper.parameters)
                //    {
                //        cmd.Parameters.Add(param);
                //    }
                //    cmd.ExecuteNonQuery();
                //     ret = Convert.ToInt32(cmd.Parameters["@LastSupplierEmailSettingsid"].Value);
                //    connection.Close();
                //
               // int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_SaveSupplierEmail", SqlHelper.parameters);
               //retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("EmailDAL", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), objset.CreatedBy.ToString());
            }

            return retstr;

        }

        public List<SupplierEmailSetting> GetSavedEmailSettings()
        {
            List<SupplierEmailSetting> emailSettings = new List<SupplierEmailSetting>();
            SqlHelper.parameters = null;
            try
            {
                using (SqlDataReader reader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_GetEmailLogs", SqlHelper.parameters))
                {
                    while (reader.Read())
                    {
                        SupplierEmailSetting emailSetting = new SupplierEmailSetting
                        {
                            SupplierEmailSettingsid = ((reader["SupplierEmailSettingsid"] == DBNull.Value) ? 0 : Convert.ToInt32(reader["SupplierEmailSettingsid"])),
                            FromAddress = ((reader["FromAddress"] == DBNull.Value) ? string.Empty : reader["FromAddress"].ToString()),
                            Bcc = ((reader["Bcc"] == DBNull.Value) ? string.Empty : reader["Bcc"].ToString()),
                            EmailSubject = ((reader["EmailSubject"] == DBNull.Value) ? string.Empty : reader["EmailSubject"].ToString()),
                            EmailBodyContentTemplate = (reader["EmailBodyContentTemplate"] != DBNull.Value) ? reader["EmailBodyContentTemplate"].ToString() : string.Empty,
                            EmailTemplate = (reader["EmailTemplate"] != DBNull.Value) ? reader["EmailTemplate"].ToString() : string.Empty,
                            ToAddress = (reader["ToAddress"] != DBNull.Value) ? reader["ToAddress"].ToString() : string.Empty,
                            EmailBodyContentPreview = (reader["EmailBodyContentPreview"] != DBNull.Value) ? reader["EmailBodyContentPreview"].ToString() : string.Empty,
                            Attachment = (reader["Attachment"] != DBNull.Value) ? reader["Attachment"].ToString() : string.Empty,
                            SendStatus = (reader["SendStatus"] != DBNull.Value) ? (bool)reader["SendStatus"] : false,
                            //Error = (reader["Error"] != DBNull.Value) ? reader["Error"].ToString() : string.Empty,
                            //SetbookingStatusAfterSuccessfulSend = (reader["SetbookingStatusAfterSuccessfulSend"] != DBNull.Value) ? (Guid)reader["SetbookingStatusAfterSuccessfulSend"] : Guid.Empty,
                            //Skipthispage = (reader["Skipthispage"] != DBNull.Value) ? Convert.ToBoolean(reader["Skipthispage"]) : false,
                            Saveacopyofthesentemail = (reader["Saveacopyofthesentemail"] != DBNull.Value) ? Convert.ToBoolean(reader["Saveacopyofthesentemail"]) : false,
                            Bcctosendersemailaddress = (reader["Bcctosendersemailaddress"] != DBNull.Value) ? Convert.ToBoolean(reader["Bcctosendersemailaddress"]) : false,
                            Showpricedetailsforbooking = (reader["Showpricedetailsforbooking"] != DBNull.Value) ? Convert.ToBoolean(reader["Showpricedetailsforbooking"]) : false,
                            //Includeareadreceipt = (reader["Includeareadreceipt"] != DBNull.Value) ? Convert.ToBoolean(reader["Includeareadreceipt"]) : false,
                            //GroupbookingsbySupplieremail = (reader["GroupbookingsbySupplieremail"] != DBNull.Value) ? Convert.ToBoolean(reader["GroupbookingsbySupplieremail"]) : false,
                            CreatedOn = (reader["CreatedOn"] != DBNull.Value) ? Convert.ToDateTime(reader["CreatedOn"]) : DateTime.MinValue,
                           // CreatedBy = (reader["CreatedBy"] != DBNull.Value) ? (Guid)reader["CreatedBy"] : Guid.Empty,
                            //ModifiedOn = (reader["ModifiedOn"] != DBNull.Value) ? Convert.ToDateTime(reader["ModifiedOn"]) : DateTime.MinValue,
                           // ModifiedBy = (reader["ModifiedBy"] != DBNull.Value) ? (Guid)reader["ModifiedBy"] : Guid.Empty,
                            //IsDeleted = (reader["IsDeleted"] != DBNull.Value) ? Convert.ToBoolean(reader["IsDeleted"]) : false,
                            //DeletedOn = (reader["DeletedOn"] != DBNull.Value) ? Convert.ToDateTime(reader["DeletedOn"]) : (DateTime?)null,
                            //DeletedBy = (reader["DeletedBy"] != DBNull.Value) ? (Guid)reader["DeletedBy"] : Guid.Empty,
                            SupplierId = (reader["Supplierid"] != DBNull.Value) ? (Guid)reader["Supplierid"] : Guid.Empty,
                            SupplierName = ((reader["SupplierName"] == DBNull.Value) ? string.Empty : reader["SupplierName"].ToString())
                        };

                        emailSettings.Add(emailSetting);
                    }

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("EmailDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return emailSettings;

            return emailSettings;
        }

    }

}
