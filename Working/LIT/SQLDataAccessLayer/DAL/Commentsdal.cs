using Microsoft.VisualBasic;
using SQLDataAccessLayer.Models;
using SQLDataAccessLayer.SQLHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace SQLDataAccessLayer.DAL
{
    public class Commentsdal
    {
        Errorlog errobj = new Errorlog();

        #region "Comments Details SaveUpdate Start"
        public string SaveUpdateCommentsDetails(string Purpose, Commentsmodel objcomment)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                SqlHelper.inputparams("@Commentsid", 100, Guid.Parse(objcomment.Commentsid), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@BookingId", 100, objcomment.BookingId, SqlDbType.BigInt);
                SqlHelper.inputparams("@SupplierName", 500, objcomment.SupplierName, SqlDbType.VarChar);
                SqlHelper.inputparams("@SupplierRefNo", 500, objcomment.SupplierRefNo, SqlDbType.VarChar);
                SqlHelper.inputparams("@Comments", 1000, objcomment.Comments, SqlDbType.VarChar);
                SqlHelper.inputparams("@Itineraryid", 100, Guid.Parse(objcomment.Itineraryid), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@CreatedBy", 100, Guid.Parse(objcomment.CreatedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ModifiedBy", 100, Guid.Parse(objcomment.ModifiedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@IsDeleted", 100, objcomment.IsDeleted, SqlDbType.Bit);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(objcomment.DeletedBy), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@CommentedOn", 100, objcomment.CommentedOn, SqlDbType.DateTime);
                SqlHelper.inputparams("@CommentedBy", 100, Guid.Parse(objcomment.CommentedBy), SqlDbType.UniqueIdentifier);

                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_ItineraryCommentsSaveUpdate", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Commentsdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), objcomment.CreatedBy);
            }

            return retstr;

        }
        #endregion "Comments Details SaveUpdate End"

        #region "Comments Details Retrive Start"

        public List<Commentsmodel> RetriveCommentsDetails(Guid ItineraryID)
        {
            List<Commentsmodel> listPassdet = new List<Commentsmodel>();

            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@Itineraryid", 100, ItineraryID, SqlDbType.UniqueIdentifier);
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "SP_GetAllItineraryComments", SqlHelper.parameters))
                {
                    while (dataReader.Read())
                    {
                        Commentsmodel CommentsDetobj = new Commentsmodel();
                        CommentsDetobj.Commentsid = ((dataReader["Commentsid"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Commentsid"]).ToString();
                        CommentsDetobj.BookingId = ((dataReader["BookingId"] == DBNull.Value) ? 0 : (long)dataReader["BookingId"]);
                        CommentsDetobj.SupplierName = ((dataReader["SupplierName"] == DBNull.Value) ? string.Empty : (string)dataReader["SupplierName"]).ToString();
                        CommentsDetobj.SupplierRefNo = ((dataReader["SupplierRefNo"] == DBNull.Value) ? string.Empty : (string)dataReader["SupplierRefNo"]);
                        CommentsDetobj.Comments = (dataReader["Comments"] == DBNull.Value) ? string.Empty : (string)dataReader["Comments"];  
                        CommentsDetobj.CommentedOn = (dataReader["CommentedOn"] == DBNull.Value) ? DateTime.Now : Convert.ToDateTime(dataReader["CommentedOn"]);
                        CommentsDetobj.CommentedBy = ((dataReader["CommentedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["CommentedBy"]).ToString();
                        CommentsDetobj.Itineraryid = ((dataReader["Itineraryid"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["Itineraryid"]).ToString();
                        CommentsDetobj.CreatedBy = ((dataReader["CreatedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["CreatedBy"]).ToString();
                        CommentsDetobj.ModifiedBy = ((dataReader["ModifiedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["ModifiedBy"]).ToString();
                        CommentsDetobj.DeletedBy = ((dataReader["DeletedBy"] == DBNull.Value) ? Guid.Empty : (Guid)dataReader["DeletedBy"]).ToString();
                        CommentsDetobj.IsDeleted = (dataReader["IsDeleted"] == DBNull.Value) ? false : (bool)dataReader["IsDeleted"];
                        CommentsDetobj.Commentedname = (dataReader["Commentedname"] == DBNull.Value) ? string.Empty: (string)dataReader["Commentedname"];
                        
                        listPassdet.Add(CommentsDetobj);
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Commentsdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return listPassdet;

        }

        #endregion "Comments Details Retrive end"

        #region "Comments Details Delete"
        public string DeleteCommentsDetails(string Commentsid, string DeletedBy)
        {
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            try
            {
                SqlHelper.inputparams("@Commentsid", 100, Guid.Parse(Commentsid), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@DeletedBy", 100, Guid.Parse(DeletedBy), SqlDbType.UniqueIdentifier);

                int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_ItineraryCommentsDelete", SqlHelper.parameters);
                retstr = ret.ToString();
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Commentsdal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), DeletedBy);
            }

            return retstr;
        }
        #endregion "Comments Details Delete"
    }
}
