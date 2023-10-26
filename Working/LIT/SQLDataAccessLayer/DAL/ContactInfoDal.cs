using LITModels.LITModels.Models;
using SQLDataAccessLayer.Models;
using SQLDataAccessLayer.SQLHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataAccessLayer.DAL
{
    public class ContactInfoDal
    {
        Errorlog errobj = new Errorlog();

        #region save and update
        public string SaveUpdateContactDetails(string Purpose, ContactModel objContact)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                SqlHelper.inputparams("@ContactId", 100, objContact.ContactId, SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ContactTypeId", 100, objContact.ContactTypeID, SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ContactType", 50, objContact.ContactType, SqlDbType.NVarChar);
                
                SqlHelper.inputparams("@ContactTitle", 150, objContact.ContactTitle, SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ContactFirstName", 250, objContact.ContactFirstName, SqlDbType.NVarChar);
                SqlHelper.inputparams("@ContactLastName", 250, objContact.ContactLastName, SqlDbType.NVarChar);
                SqlHelper.inputparams("@ContactGender", 150, objContact.ContactGender, SqlDbType.NVarChar);
                
                SqlHelper.inputparams("@PhoneWork", 20, objContact.PhoneWork, SqlDbType.NVarChar);
                SqlHelper.inputparams("@Phone", 20, objContact.PhoneWork, SqlDbType.NVarChar);
                SqlHelper.inputparams("@PhoneHome", 20, objContact.PhoneHome, SqlDbType.NVarChar);
                SqlHelper.inputparams("@Mobile", 20, objContact.Mobile, SqlDbType.NVarChar);
                SqlHelper.inputparams("@Fax", 30, objContact.Fax, SqlDbType.NVarChar);
                
                SqlHelper.inputparams("@Address", 250, objContact.Address, SqlDbType.NVarChar);
                SqlHelper.inputparams("@City", 100, objContact.City, SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Region", 100, objContact.Region, SqlDbType.UniqueIdentifier);
                
                SqlHelper.inputparams("@State", 100, objContact.State, SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Country", 100, objContact.Country, SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@Postcode", 20, objContact.Postcode, SqlDbType.NVarChar);
                
                SqlHelper.inputparams("@CompanyName", 500, objContact.CompanyName, SqlDbType.NVarChar);
                SqlHelper.inputparams("@CompanyPosition", 500, objContact.CompanyPosition, SqlDbType.NVarChar);
                SqlHelper.inputparams("@CompanyDescription", 2000, objContact.CompanyDescription, SqlDbType.NVarChar);

                SqlHelper.inputparams("@EmailOne", 150, objContact.EmailOne, SqlDbType.NVarChar);
                SqlHelper.inputparams("@EmailTwo", 150, objContact.EmailTwo, SqlDbType.NVarChar);
                SqlHelper.inputparams("@Website", 150, objContact.Website, SqlDbType.NVarChar);
                
                SqlHelper.inputparams("@CreatedBy", 100, objContact.CreatedBy, SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ModifiedBy", 100, objContact.ModifiedBy, SqlDbType.UniqueIdentifier);

                SqlHelper.inputparams("@UserRoles", 150, objContact.UserRoleId, SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@UserName ", 100, objContact.UserName, SqlDbType.NVarChar);
                SqlHelper.inputparams("@Password", 200, objContact.Password, SqlDbType.NVarChar);
                SqlHelper.inputparams("@ConfirmPassword", 200, objContact.ConfrimPassword, SqlDbType.NVarChar);

                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_ContactInformationSaveUpdate", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Contactdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), objContact.ContactFirstName);
            }

            return retstr;
        }
        #endregion

        #region Retrive by contact id
        public ContactModel RetrieveContactById(Guid contactId)
        {
            ContactModel contact = new ContactModel();
            SqlHelper.parameters = null;

            try
            {
                SqlHelper.inputparams("@ContactId", 100, contactId, SqlDbType.UniqueIdentifier);

                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_GetContactInformation", SqlHelper.parameters))
                {
                    if (dataReader.Read())
                    {
                       contact = new ContactModel
                        {
                            ContactId = (dataReader["ContactId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ContactId"],
                            ContactTypeID = (dataReader["ContactTypeId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ContactTypeId"],
                            ContactType = (dataReader["ContactType"] == DBNull.Value) ? string.Empty : (string)dataReader["ContactType"],
                            ContactTitle = ((dataReader["ContactTitle"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ContactTitle"]),
                            ContactFirstName = ((dataReader["ContactFirstName"] == DBNull.Value) ? string.Empty : (string)dataReader["ContactFirstName"]),
                            ContactLastName = ((dataReader["ContactLastName"] == DBNull.Value) ? string.Empty : (string)dataReader["ContactLastName"]),
                            ContactGender = ((dataReader["ContactGender"] == DBNull.Value) ? string.Empty : (string)dataReader["ContactGender"]),
                            PhoneWork = ((dataReader["PhoneWork"] == DBNull.Value) ? string.Empty : (string)dataReader["PhoneWork"]),
                            PhoneHome = ((dataReader["PhoneHome"] == DBNull.Value) ? string.Empty : (string)dataReader["PhoneHome"]),
                            Mobile = ((dataReader["Mobile"] == DBNull.Value) ? string.Empty : (string)dataReader["Mobile"]),
                            Fax = ((dataReader["Fax"] == DBNull.Value) ? string.Empty : (string)dataReader["Fax"]),
                            Address = ((dataReader["Address"] == DBNull.Value) ? string.Empty : (string)dataReader["Address"]),
                            City = (dataReader["City"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["City"],
                            Region = (dataReader["Region"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Region"],
                            State = ((dataReader["State"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["State"]),
                            Country = ((dataReader["Country"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Country"]),
                            Postcode = ((dataReader["Postcode"] == DBNull.Value) ? string.Empty : (string)dataReader["Postcode"]),
                            CompanyName = ((dataReader["CompanyName"] == DBNull.Value) ? string.Empty : (string)dataReader["CompanyName"]),
                            CompanyPosition = ((dataReader["CompanyPosition"] == DBNull.Value) ? string.Empty : (string)dataReader["CompanyPosition"]),
                            CompanyDescription = ((dataReader["CompanyDescription"] == DBNull.Value) ? string.Empty : (string)dataReader["CompanyDescription"]),
                            EmailOne = ((dataReader["EmailOne"] == DBNull.Value) ? string.Empty : (string)dataReader["EmailOne"]),
                            EmailTwo = ((dataReader["EmailTwo"] == DBNull.Value) ? string.Empty : (string)dataReader["EmailTwo"]),
                            Website = ((dataReader["Website"] == DBNull.Value) ? string.Empty : (string)dataReader["Website"]),
                            CreatedBy = ((dataReader["CreatedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["CreatedBy"]),
                            ModifiedBy = ((dataReader["ModifiedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ModifiedBy"]),
                            Contactautoid = ((dataReader["Contactautoid"] == DBNull.Value) ? 0 : (long)dataReader["Contactautoid"]).ToString(),
                           UserRoleId = (dataReader["UserRoles"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["UserRoles"],
                           UserName = ((dataReader["UserName"] == DBNull.Value) ? string.Empty : (string)dataReader["UserName"]),
                           Password = ((dataReader["Password"] == DBNull.Value) ? string.Empty : (string)dataReader["Password"]),
                           ConfrimPassword = ((dataReader["ConfirmPassword"] == DBNull.Value) ? string.Empty : (string)dataReader["ConfirmPassword"])

                       };
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Contactdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

            return contact;
        }

        #endregion

        #region Retrive by contact type id
        public ContactModel RetrieveContactsByContactTypeId(Guid contactTypeId)
        {
            ContactModel contact = new ContactModel();
            SqlHelper.parameters = null;

            try
            {
                SqlHelper.inputparams("@ContactTypeId", 100, contactTypeId, SqlDbType.UniqueIdentifier);

                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_GetContactInformationByType", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {
                        contact = new ContactModel
                        {
                            ContactId = (dataReader["ContactId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ContactId"],
                            ContactTypeID = (dataReader["ContactTypeId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ContactTypeId"],
                            ContactType = (dataReader["ContactType"] == DBNull.Value) ? string.Empty : (string)dataReader["ContactType"],
                            ContactTitle = ((dataReader["ContactTitle"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ContactTitle"]),
                            ContactFirstName = ((dataReader["ContactFirstName"] == DBNull.Value) ? string.Empty : (string)dataReader["ContactFirstName"]),
                            ContactLastName = ((dataReader["ContactLastName"] == DBNull.Value) ? string.Empty : (string)dataReader["ContactLastName"]),
                            ContactGender = ((dataReader["ContactGender"] == DBNull.Value) ? string.Empty : (string)dataReader["ContactGender"]),
                            PhoneWork = ((dataReader["PhoneWork"] == DBNull.Value) ? string.Empty : (string)dataReader["PhoneWork"]),
                            PhoneHome = ((dataReader["PhoneHome"] == DBNull.Value) ? string.Empty : (string)dataReader["PhoneHome"]),
                            Mobile = ((dataReader["Mobile"] == DBNull.Value) ? string.Empty : (string)dataReader["Mobile"]),
                            Fax = ((dataReader["Fax"] == DBNull.Value) ? string.Empty : (string)dataReader["Fax"]),
                            Address = ((dataReader["Address"] == DBNull.Value) ? string.Empty : (string)dataReader["Address"]),
                            City = (dataReader["City"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["City"],
                            Region = (dataReader["Region"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Region"],
                            State = ((dataReader["State"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["State"]),
                            Country = ((dataReader["Country"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Country"]),
                            Postcode = ((dataReader["Postcode"] == DBNull.Value) ? string.Empty : (string)dataReader["Postcode"]),
                            CompanyName = ((dataReader["CompanyName"] == DBNull.Value) ? string.Empty : (string)dataReader["CompanyName"]),
                            CompanyPosition = ((dataReader["CompanyPosition"] == DBNull.Value) ? string.Empty : (string)dataReader["CompanyPosition"]),
                            CompanyDescription = ((dataReader["CompanyDescription"] == DBNull.Value) ? string.Empty : (string)dataReader["CompanyDescription"]),
                            EmailOne = ((dataReader["EmailOne"] == DBNull.Value) ? string.Empty : (string)dataReader["EmailOne"]),
                            EmailTwo = ((dataReader["EmailTwo"] == DBNull.Value) ? string.Empty : (string)dataReader["EmailTwo"]),
                            Website = ((dataReader["Website"] == DBNull.Value) ? string.Empty : (string)dataReader["Website"]),
                            CreatedBy = ((dataReader["CreatedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["CreatedBy"]),
                            ModifiedBy = ((dataReader["ModifiedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ModifiedBy"]),
                            UserRoleId = (dataReader["UserRoles"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["UserRoles"],
                            UserName = ((dataReader["UserName"] == DBNull.Value) ? string.Empty : (string)dataReader["UserName"]),
                            Password = ((dataReader["Password"] == DBNull.Value) ? string.Empty : (string)dataReader["Password"]),
                            ConfrimPassword = ((dataReader["ConfirmPassword"] == DBNull.Value) ? string.Empty : (string)dataReader["ConfirmPassword"])
                        };

                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Contactdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

            return contact;
        }

        #endregion

        #region Retrive contact
        public List<ContactModel> RetrieveContacts()
        {
            ContactModel contact = new ContactModel();
            SqlHelper.parameters = null;
            List<ContactModel> contactsList = new List<ContactModel>();
            try
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_GetAllContactInformation", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {
                        contact = new ContactModel
                        {
                            ContactId = (dataReader["ContactId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ContactId"],
                            ContactTypeID = (dataReader["ContactTypeId"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ContactTypeId"],
                            ContactType = (dataReader["ContactType"] == DBNull.Value) ? string.Empty : (string)dataReader["ContactType"],
                            ContactTitle = ((dataReader["ContactTitle"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ContactTitle"]),
                            ContactFirstName = ((dataReader["ContactFirstName"] == DBNull.Value) ? string.Empty : (string)dataReader["ContactFirstName"]),
                            ContactLastName = ((dataReader["ContactLastName"] == DBNull.Value) ? string.Empty : (string)dataReader["ContactLastName"]),
                            ContactGender = ((dataReader["ContactGender"] == DBNull.Value) ? string.Empty : (string)dataReader["ContactGender"]),
                            PhoneWork = ((dataReader["PhoneWork"] == DBNull.Value) ? string.Empty : (string)dataReader["PhoneWork"]),
                            PhoneHome = ((dataReader["PhoneHome"] == DBNull.Value) ? string.Empty : (string)dataReader["PhoneHome"]),
                            Mobile = ((dataReader["Mobile"] == DBNull.Value) ? string.Empty : (string)dataReader["Mobile"]),
                            Fax = ((dataReader["Fax"] == DBNull.Value) ? string.Empty : (string)dataReader["Fax"]),
                            Address = ((dataReader["Address"] == DBNull.Value) ? string.Empty : (string)dataReader["Address"]),
                            City = (dataReader["City"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["City"],
                            Region = (dataReader["Region"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Region"],
                            State = ((dataReader["State"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["State"]),
                            Country = ((dataReader["Country"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Country"]),
                            Postcode = ((dataReader["Postcode"] == DBNull.Value) ? string.Empty : (string)dataReader["Postcode"]),
                            CompanyName = ((dataReader["CompanyName"] == DBNull.Value) ? string.Empty : (string)dataReader["CompanyName"]),
                            CompanyPosition = ((dataReader["CompanyPosition"] == DBNull.Value) ? string.Empty : (string)dataReader["CompanyPosition"]),
                            CompanyDescription = ((dataReader["CompanyDescription"] == DBNull.Value) ? string.Empty : (string)dataReader["CompanyDescription"]),
                            EmailOne = ((dataReader["EmailOne"] == DBNull.Value) ? string.Empty : (string)dataReader["EmailOne"]),
                            EmailTwo = ((dataReader["EmailTwo"] == DBNull.Value) ? string.Empty : (string)dataReader["EmailTwo"]),
                            Website = ((dataReader["Website"] == DBNull.Value) ? string.Empty : (string)dataReader["Website"]),
                            CreatedBy = ((dataReader["CreatedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["CreatedBy"]),
                            ModifiedBy = ((dataReader["ModifiedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ModifiedBy"]),
                            Contactautoid = ((dataReader["Contactautoid"] == DBNull.Value) ? 0 : (long)dataReader["Contactautoid"]).ToString(),
                            UserRoleId = (dataReader["UserRoles"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["UserRoles"],
                            UserName = ((dataReader["UserName"] == DBNull.Value) ? string.Empty : (string)dataReader["UserName"]),
                            Password = ((dataReader["Password"] == DBNull.Value) ? string.Empty : (string)dataReader["Password"]),
                            ConfrimPassword = ((dataReader["ConfirmPassword"] == DBNull.Value) ? string.Empty : (string)dataReader["ConfirmPassword"])

                        };

                        contactsList.Add(contact);

                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Contactdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

            return contactsList;
        }

        #endregion

        #region delete
        public string DeleteContact(Guid contactId, Guid deletedBy)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;

            try
            {
                SqlHelper.inputparams("@ContactId", 100, contactId, SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@DeletedBy", 100, deletedBy, SqlDbType.UniqueIdentifier);

                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_ContactInformationDelete", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Contactdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), deletedBy.ToString());
            }

            return retstr;
        }

        #endregion

        #region Retrive contact type id
        public List<contacttype> RetrieveContactTypeId()
        {
            List<contacttype> contact = new List<contacttype>();
            contacttype contactobj = null;
            SqlHelper.parameters = null;

            try
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "GetContacttype", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {
                        contactobj = new contacttype
                        {
                            ContactTypeid = ((dataReader["ContactTypeid"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ContactTypeid"]).ToString(),
                            ContactTypename = (dataReader["ContactTypename"] == DBNull.Value) ? string.Empty : (string)dataReader["ContactTypename"]
                        };

                        contact.Add(contactobj);
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Contactdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

            return contact;
        }

        #endregion

        public List<UserRole> LoadUserRoles()
        {
            List<UserRole> UserRolesList = new List<UserRole>();
            try
            {
                using (SqlDataReader rdr = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_GetUserRoles", SqlHelper.parameters))
                {
                    while (rdr.Read())
                    {
                        UserRole userRole = new UserRole();
                        userRole.UserRoldId = (rdr["UserRoldID"] is DBNull) ? (Guid?)null : (Guid)rdr["UserRoldID"];
                        userRole.UserRole1 = (rdr["UserRole"] is DBNull) ? string.Empty : (string)rdr["UserRole"];
                        UserRolesList.Add(userRole);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                errobj.WriteErrorLoginfo("LoadUserRoles", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

            return UserRolesList;
        }
    }
}
