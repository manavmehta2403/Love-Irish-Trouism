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
    public class NotificationDal
    {
        Errorlog errobj = new Errorlog();

        public List<Notification> GetNotifications(Guid userId)
        {
            List<Notification> Notifications = new List<Notification>();
            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@UserId", 100, userId, SqlDbType.UniqueIdentifier);
                using (SqlDataReader reader = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "GetNotification", SqlHelper.parameters))
                {
                    while (reader.Read())
                    {
                        // Add a condition to filter notifications for the specific user
                        if ((Guid)reader["TargetUserId"] == userId)
                        {
                            Notification notification = new Notification
                            {
                                BubbleNotificationId = (Guid)reader["BubbleNotificationId"],
                                NotificationType = (int)reader["NotificationType"],
                                NotificationTitle = reader["NotificationTitle"] as string ?? string.Empty,
                                Comments = reader["Comments"] as string ?? string.Empty,
                                TargetUserId = (Guid)reader["TargetUserId"],
                                IsRead = (bool)reader["IsRead"],
                                ReadOn = reader["ReadOn"] as DateTime?,
                                ItineraryId = reader["ItineraryId"] as Guid?,
                                BookingId = reader["BookingId"] as string ?? string.Empty,
                                SupplierId = reader["SupplierId"] as Guid?,
                                SupplierRefNo = reader["SupplierRefNo"] as string ?? string.Empty,
                                IsPriceChanged = reader["IsPriceChanged"] as bool?,
                                UpdatedPrice = reader["UpdatedPrice"] as decimal?,
                                CreatedOn = (DateTime)reader["CreatedOn"],
                                CreatedBy = reader.GetGuid(reader.GetOrdinal("CreatedBy")),
                                ModifiedOn = reader["ModifiedOn"] as DateTime?,
                                ModifiedBy = reader.GetGuid(reader.GetOrdinal("ModifiedBy")),
                                IsDeleted = (bool)reader["IsDeleted"],
                                BookingName = reader["BookingName"] as string ?? string.Empty,
                                SupplierName = reader["SupplierName"] as string ?? string.Empty,
                            };

                            Notifications.Add(notification);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("NotificationDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return Notifications;
        }


        public void MarkNotificationAsRead(Guid bubbleNotificationId, Guid userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DBConfiguration.instance.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("SP_UpdateBubbleNotificationReadStatus", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the SqlCommand
                        cmd.Parameters.Add(new SqlParameter("@BubbleNotificationId", bubbleNotificationId));
                        cmd.Parameters.Add(new SqlParameter("@UserID", userId));

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here, log them, or take appropriate action
                errobj.WriteErrorLoginfo("NotificationDal", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }

    }
}
