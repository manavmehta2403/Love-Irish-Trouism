using SQLDataAccessLayer.Models;
using SQLDataAccessLayer.SQLHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataAccessLayer.DAL
{
    public class Errorlog
    {
         
                public void writeLog(string strValue)
                {
                    try
                    {
                //Logfile
                //string path = var exePath = AppDomain.CurrentDomain.BaseDirectory;
                var exePath = AppDomain.CurrentDomain.BaseDirectory;
                var pagesFolder = Directory.GetParent(exePath);
                string path= pagesFolder+"\\apperror.log";
                StreamWriter sw;
                        if (!File.Exists(path))
                        { sw = File.CreateText(path); }
                        else
                        { sw = File.AppendText(path); }

                        LogWrite(strValue, sw);

                        sw.Flush();
                        sw.Close();
                    }
                    catch (Exception ex)
                    {

                    }
                }

                private static void LogWrite(string logMessage, StreamWriter w)
                {
                    w.WriteLine("{0}", logMessage);
                    w.WriteLine("----------------------------------------");
                } 


        public void WriteErrorLoginfo(string StrPageName,string StrFunctionName,string StrMessageDescription,string StrCreatedBy)
        {
            try
            {
                Errorlogobj errobj = new Errorlogobj()
                {
                    CreatedBy = StrCreatedBy,
                    PageName = StrPageName,
                    FunctionName = StrFunctionName,
                    MessageDescription = StrMessageDescription,
                    ErrorFrom = "Application"
                };
                WriteErrorLogDB(errobj);
            }
            catch (Exception ex)
            {
                writeLog(DateTime.Now.ToString() + " : Error : " + ex.Message.ToString());
            }
        }
        public void WriteErrorLogDB(Errorlogobj objerr)
        {
            
            SqlHelper.parameters = null;
            string retstr = string.Empty;
            SqlHelper.inputparams("@PageName", 500, objerr.PageName, SqlDbType.VarChar);
            SqlHelper.inputparams("@FunctionName", 500, objerr.FunctionName, SqlDbType.VarChar);
            SqlHelper.inputparams("@MessageDescription", 4000, objerr.MessageDescription, SqlDbType.VarChar);
            SqlHelper.inputparams("@CreatedBy", 100, (objerr.CreatedBy == null || objerr.CreatedBy == "") ? Guid.Empty : Guid.Parse(objerr.CreatedBy), SqlDbType.UniqueIdentifier);
            SqlHelper.inputparams("@ErrorFrom", 100, objerr.ErrorFrom, SqlDbType.VarChar);

            int ret = SqlHelper.ExecuteNonQuery(DBConfiguration.instance.ConnectionString, "SP_WriteErrorLog", SqlHelper.parameters);

        

        }

    }
}
