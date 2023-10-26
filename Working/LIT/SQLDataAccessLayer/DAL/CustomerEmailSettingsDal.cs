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

namespace SQLDataAccessLayer.DAL
{
    public class CustomerEmailSettingsDal
    {
        #region Get Data
        public List<EmailLogsSettingCollection> GetCustomerEmailSettings(string ItineraryID)
        {
            List<EmailLogsSettingCollection> emailSettings = new List<EmailLogsSettingCollection>();
            try
            {
                SqlHelper.parameters = null;
                SqlHelper.inputparams("@ItineraryID", 100, Guid.Parse(ItineraryID), SqlDbType.UniqueIdentifier);
                using (SqlDataReader reader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_GETCustomerEmailSetting", SqlHelper.parameters))
                {
                    while (reader.Read())
                    {
                        EmailLogsSettingCollection emailSetting = new EmailLogsSettingCollection
                        {
                            CustomerEmailSettingId = (long)reader["CustomerEmailSettingId"],
                            ItineraryID = reader["ItineraryID"] as Guid?,
                            SentDate = reader["SentDate"] as DateTime?,
                            TypeID = reader["TypID"] as Guid?,
                            Type = reader["Type"] as string,
                            FromAddress = reader["FromAddress"] as string,
                            RecipientEmail = reader["RecipientEmail"] as string,
                            Recipient = reader["Recipient"] as string,
                            EmailSubject = reader["EmailSubject"] as string,
                            AttachmentPDF = reader["AttachmentPDF"] as string,
                            AttachmentWord = reader["AttachmentWord"] as string,
                            EmailBodyContentTemplate = reader["EmailBodyContentTemplate"] as string,
                            EmailBodyContentPreview = reader["EmailBodyContentPreview"] as string,
                            SendStatus = reader["SendStatus"] as bool?,
                            PassengerId = reader["Passengerid"] as Guid?
                        };

                        emailSettings.Add(emailSetting);
                    }

                }

            }
            catch (Exception ex)
            {
                // Handle exceptions here
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return emailSettings;
        }
        #endregion

        #region Save Data
        public string SaveCustomerEmailSetting(string purpose, EmailLogsSettingCollection emailSetting)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@action", 10, purpose, SqlDbType.VarChar);
                SqlHelper.inputparams("@ItineraryID", 100, emailSetting.ItineraryID, SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@SentDate", 7, emailSetting.SentDate, SqlDbType.DateTime2);
                SqlHelper.inputparams("@TypeID", 100, emailSetting.TypeID, SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Type", 150, emailSetting.Type, SqlDbType.VarChar);
                SqlHelper.inputparams("@FromAddress", 1000, emailSetting.FromAddress, SqlDbType.NVarChar);
                SqlHelper.inputparams("@RecipientEmail", 1000, emailSetting.RecipientEmail, SqlDbType.NVarChar);
                SqlHelper.inputparams("@Recipient", 200, emailSetting.Recipient, SqlDbType.NVarChar);
                SqlHelper.inputparams("@EmailSubject", 2000, emailSetting.EmailSubject, SqlDbType.NVarChar);
                SqlHelper.inputparams("@AttachmentPDF", 2000, emailSetting.AttachmentPDF, SqlDbType.NVarChar);
                SqlHelper.inputparams("@AttachmentWord", 2000, emailSetting.AttachmentWord, SqlDbType.NVarChar);
                SqlHelper.inputparams("@EmailBodyContentTemplate", 2000, emailSetting.EmailBodyContentTemplate, SqlDbType.NVarChar);
                SqlHelper.inputparams("@EmailBodyContentPreview", -1, emailSetting.EmailBodyContentPreview, SqlDbType.NVarChar);
                SqlHelper.inputparams("@SendStatus", 1, emailSetting.SendStatus, SqlDbType.Bit);
                SqlHelper.inputparams("@CreatedBy", 100, emailSetting.CreatedBy, SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ModifiedBy", 100, emailSetting.ModifiedBy, SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Passengerid", 100, emailSetting.PassengerId, SqlDbType.UniqueIdentifier);

                int ret = 0;
                using (SqlConnection connection = new SqlConnection(DBConfiguration.instance.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("SP_CustomerEmailSettingSave", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    foreach (SqlParameter param in SqlHelper.parameters)
                    {
                        cmd.Parameters.Add(param);
                    }

                    cmd.ExecuteNonQuery();
                    // Retrieve any output or return values if needed
                    // ret = Convert.ToInt32(cmd.Parameters["@OutputParameterName"].Value);

                    connection.Close();
                }

                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return retstr;
        }

        #endregion
    }
}
