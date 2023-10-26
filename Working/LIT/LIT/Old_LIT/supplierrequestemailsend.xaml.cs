using SQLDataAccessLayer.DAL;
using SQLDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using LITModels.LITModels.Models;

namespace LIT.Old_LIT
{
    /// <summary>
    /// Interaction logic for SupplierRequestEmail.xaml
    /// </summary>
    public partial class SupplierRequestEmailSend : Window
    {
        SQLDataAccessLayer.DAL.ItineraryDAL objitdal = new SQLDataAccessLayer.DAL.ItineraryDAL();
        LoadDropDownListValues loadDropDownListValues = new LoadDropDownListValues();
        List<Userdetails> ListUserdet = new List<Userdetails>();
        DataSet dsFolder = new DataSet();
        string loginusername = string.Empty;
        string loginuserid = string.Empty;
        string recordmode = string.Empty;
        string Emailoption = string.Empty;
        Errorlog errobj = new Errorlog();
        public MailCredentials mailSetting = null;
        EmailDal EmailDalobj = new EmailDal();
        BookingEmail ParobjBE;
        SupplierRequestEmailEdit ParobjSREE;
        SupplierRequestEmail ParSReqE;
        //List<BookingEmailSend> lstBkEmailSend = new List<BookingEmailSend>();
        List<BkRequestStatus> ListofRequestStatus = new List<BkRequestStatus>();
        
        ItineraryWindow Pariw;
        public SupplierRequestEmailSend()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public SupplierRequestEmailSend(string username,string PEmailoption, 
            SupplierRequestEmailEdit PSREdit, SupplierRequestEmail PSReqE, ItineraryWindow Piw, BookingEmail PObjBE=null)
        {
            InitializeComponent();
            this.DataContext = this;
            loginusername =username;
            Emailoption = PEmailoption;
            mailSetting = EmailDalobj.GetMailCredentials();
            if(Piw != null)
            {
                Pariw = Piw;
            }
            if (PSREdit != null)
            {
                ParobjSREE= PSREdit;
                BookingReqDetsend = null;
                BookingReqDetsend = ParobjSREE.BookingReqDetedit;
            }
            if (PObjBE != null)
            {
                ParobjBE = PObjBE;
            }
            if (PSReqE != null)
            {
                ParSReqE = PSReqE;
            }
            loginuserid = loadDropDownListValues.Currentuseridinfo(loginusername);
            LoadStatus();
            LoadSendEmailgrid();

        }
        private ObservableCollection<BookingEmailRequestDetails> _BookingReqDetsend;
        public ObservableCollection<BookingEmailRequestDetails> BookingReqDetsend
        {
            get { return _BookingReqDetsend ?? (_BookingReqDetsend = new ObservableCollection<BookingEmailRequestDetails>()); }
            set
            {
                _BookingReqDetsend = value;                
            }
        }

        private ObservableCollection<BookingEmailSend> _Bookingsend;
        public ObservableCollection<BookingEmailSend> Bookingsend
        {
            get { return _Bookingsend ?? (_Bookingsend = new ObservableCollection<BookingEmailSend>()); }
            set
            {
                _Bookingsend = value;
            }
        }

        private void LoadStatus()
        {
            ListofRequestStatus = loadDropDownListValues.LoadRequestStatus();
            if (ListofRequestStatus != null && ListofRequestStatus.Count > 0)
            {
                CmbEmailStatus.ItemsSource = ListofRequestStatus;
                CmbEmailStatus.SelectedValuePath = "RequestStatusId";
                CmbEmailStatus.DisplayMemberPath = "RequestStatusName";
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        } 

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {

            //BackgroundWorker worker = new BackgroundWorker();
            //worker.WorkerReportsProgress = true;
            //worker.DoWork += worker_DoWork;
            //worker.ProgressChanged += worker_ProgressChanged;
            //worker.RunWorkerCompleted += worker_completed;
            //worker.RunWorkerAsync();
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            txtbemailstatus.Text = "Email send in progress";
            EventHandler evthdl = worker_DoWork1;
            if (evthdl != null)
                evthdl(sender, e);


        }
         

        public event EventHandler worker_DoWork;
        private void worker_completed(object? sender, RunWorkerCompletedEventArgs e)
        {
           // emailprogess.Value = 0;
            txtbemailstatus.Text =string.Empty;

        }

        private void worker_DoWork1(object sender, EventArgs e)
        {
             
                string result = string.Empty;
                btnSend.IsEnabled = false;
                result = SendEMail();
                if (result != string.Empty)
                {
                    if(result.Contains(","))
                    {
                    System.Windows.MessageBox.Show("Selected Email sent successfully...");
                    txtbemailstatus.Text = "";
                    txtbemailstatus.Visibility = Visibility.Collapsed;
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                    
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Email sent successfully...");
                        txtbemailstatus.Text = "";
                        txtbemailstatus.Visibility = Visibility.Collapsed;
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                        this.Close();
                    }
                    
                }
            
        }
        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //emailprogess.Value = e.ProgressPercentage;
            txtbemailstatus.Text = (string)e.UserState;
        }
        
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            SupplierRequestEmailEdit wnEmSend = new SupplierRequestEmailEdit(loginusername, Emailoption, "ReqSendEmail", ParobjBE, ParSReqE, Pariw);
            this.Close();
            wnEmSend.ShowDialog();
        }
        private void LoadSendEmailgrid() 
        {
            if (ParobjSREE != null && ParobjBE != null)
            {
                foreach (var item in BookingReqDetsend)
                {
                    BookingEmailSend objBES = new BookingEmailSend();
                    objBES.FromEmail = ParobjBE.FromEmail;
                    objBES.ToEmail = item.UserEmail; 
                    objBES.Selected = true;
                    objBES.Status = string.Empty;
                    objBES.Subject = item.Emailsubject;
                    objBES.FileAttachName = item.FileAttachName;
                    objBES.Error = string.Empty;
                    objBES.Message = item.FinalEmailContent;
                    objBES.FileAttachPath = item.FileAttachPath;
                    objBES.BookingID =(!string.IsNullOrEmpty(item.BookingID))?Convert.ToInt64(item.BookingID):0;
                    objBES.ItineraryID = item.ItineraryID;

                    Bookingsend.Add(objBES);
                }
                dgBookingemailsend.ItemsSource= Bookingsend;
            }
        }

        public string SendEMail()
        {
            string result = string.Empty;
            try
            {
                //txtbemailstatus.Visibility = Visibility.Visible;
                //txtbemailstatus.Text = "Email send in progress";
                string strFromName, strFromEmail, strToEmail, strMailSubject, strMailContent;
                string attachfilename = string.Empty;
                bool isBodyHtml = true;
                string[] stringSeparators = new string[] { ";" };
                string mMailDecryptedPassword = string.Empty;
                string[] ToAddresses;
                string[] BccAddresses=null;

                int selectcnt = Bookingsend.Where(x => x.Selected == true).Count();
                int k = 0;
                foreach (BookingEmailSend item in Bookingsend.ToList())
               {
                    if (item.Selected == true)
                    {
                        if (ParobjBE != null)
                        {
                            strToEmail = string.Empty; strFromEmail = string.Empty; strFromName = string.Empty; strMailSubject = string.Empty; strMailContent = string.Empty;
                            SupplierEmailSetting Suppemailset = new SupplierEmailSetting();

                            strToEmail = item.ToEmail; 
                            strFromEmail = item.FromEmail;
                            strMailSubject = item.Subject;
                            strMailContent = item.Message;
                            ToAddresses = strToEmail.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                            if (!string.IsNullOrWhiteSpace(ParobjBE.BccEmail))
                            {
                                BccAddresses = ParobjBE.BccEmail.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                            }

                            strFromEmail = mailSetting.FromEmail;
                            strFromName = mailSetting.FromEmailName;

                            mailSetting.FromEmail = strFromEmail;
                            mailSetting.FromEmailName = strFromName;
                            attachfilename = item.FileAttachPath;

                            MailMessage mailmsg = new MailMessage();

                            SmtpClient smtpClient = new SmtpClient(mailSetting.SMTPServer, mailSetting.SMTPPort);

                            if (!string.IsNullOrWhiteSpace(mailSetting.FromEmail) && !string.IsNullOrWhiteSpace(mailSetting.FromEmailPassword))
                            {
                                // ManageEncryptionDetails EncryptionDetails = new ManageEncryptionDetails();
                                // mMailDecryptedPassword = EncryptionDetails.GetDecryptedString(mailSetting.FromEmailPassword);
                                // smtpClient.Credentials = new NetworkCredential(mailSetting.FromEmail, mMailDecryptedPassword);

                                smtpClient.Credentials = new NetworkCredential(mailSetting.FromEmail, mailSetting.FromEmailPassword);
                            }
                            smtpClient.Timeout = 300000;
                            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtpClient.EnableSsl = mailSetting.IsSSLRequired;
                            mailmsg.From = new MailAddress(mailSetting.FromEmail, mailSetting.FromEmailName);

                            foreach (string email in ToAddresses)
                            {
                                mailmsg.To.Add(email.Trim());
                            }
                            foreach (string bcc in BccAddresses)
                            {
                                mailmsg.Bcc.Add(bcc.Trim());                                
                            }
                            mailmsg.Subject = strMailSubject;
                            mailmsg.SubjectEncoding = System.Text.Encoding.UTF8;
                            mailmsg.Body = strMailContent;
                            mailmsg.BodyEncoding = System.Text.Encoding.UTF8;
                            mailmsg.IsBodyHtml = isBodyHtml;


                            //EmailAttachments


                            if (attachfilename != null)
                            {
                                string filename = System.IO.Path.GetFileName(attachfilename);
                                string extensionname = filename.Substring(filename.LastIndexOf("."));
                                //filename = filename.Replace(filename.Substring(filename.LastIndexOf("_")), "") + extensionname;

                                var stream = new WebClient().OpenRead(attachfilename);
                                Attachment attachment = new Attachment(stream, filename);
                                mailmsg.Attachments.Add(attachment);
                            }

                            Guid? Suppid=Guid.Empty;

                            if (Pariw.BookingItemsitin.Where(x => x.BookingID == item.BookingID && x.ItineraryID == item.ItineraryID).FirstOrDefault() != null)
                            {
                                if ((Pariw.BookingItemsitin.Where(x => x.BookingID == item.BookingID && x.ItineraryID == item.ItineraryID).FirstOrDefault().SupplierID) != null)
                                {
                                    Suppid = Guid.Parse(Pariw.BookingItemsitin.Where(x => x.BookingID ==(long)item.BookingID && x.ItineraryID == item.ItineraryID).FirstOrDefault().SupplierID);
                                }
                            }
                            Suppemailset.SupplierId=Suppid;
                            Suppemailset.SupplierName = BookingReqDetsend[0].SupplierName;
                            Suppemailset.SupplierEmailSettingsid = 0;
                            Suppemailset.FromAddress = strFromEmail;
                            Suppemailset.Bcc = String.Join(";", BccAddresses);
                            Suppemailset.EmailSubject = strMailSubject;
                            Suppemailset.EmailBodyContentTemplate = string.Empty;
                            Suppemailset.EmailTemplate = ParobjBE.templatepath;
                            Suppemailset.ToAddress = strToEmail;
                            Suppemailset.EmailBodyContentPreview = strMailContent;
                            Suppemailset.Attachment = (attachfilename!=null)? attachfilename:string.Empty;
                            Suppemailset.SendStatus = false;
                            Suppemailset.Error = string.Empty;
                            Suppemailset.SetbookingStatusAfterSuccessfulSend = (CmbEmailStatus.SelectedItem != null) ? Guid.Parse(((SQLDataAccessLayer.Models.BkRequestStatus)CmbEmailStatus.SelectedItem).RequestStatusID.ToString()) : Guid.Empty;
                            Suppemailset.Skipthispage = false;
                            Suppemailset.Saveacopyofthesentemail = this.ParSReqE.ChckBxSaveCopy.IsChecked;
                            Suppemailset.Bcctosendersemailaddress = false;
                            Suppemailset.Showpricedetailsforbooking = false;
                            Suppemailset.Includeareadreceipt = false;
                            Suppemailset.GroupbookingsbySupplieremail = false;
                            Suppemailset.CreatedBy = (!string.IsNullOrEmpty(loginuserid)) ? Guid.Parse(loginuserid) : Guid.Empty;
                            Suppemailset.ModifiedBy = (!string.IsNullOrEmpty(loginuserid)) ? Guid.Parse(loginuserid) : Guid.Empty;
                            long  supSavesettingid =SaveSupplierEmaildetails("I",Suppemailset);


                            try
                            {
                               // emailprogess.IsIndeterminate = true;
                                smtpClient.Send(mailmsg);
                                mailmsg.Dispose();

                                item.Status = "Sent";

                                k = k + 1;

                                if (CmbEmailStatus.SelectedItem != null)
                                {
                                    BookingItems bkitm;
                                    bkitm = Pariw.BookingItemsitin.Where(x => x.BookingID == item.BookingID && x.ItineraryID == item.ItineraryID).FirstOrDefault();
                                    int i = Pariw.BookingItemsitin.IndexOf(bkitm);
                                    Pariw.BookingItemsitin[i].SelectedItemRequstStatus = CmbEmailStatus.SelectedItem;
                                    Pariw.BookingItemsitin[i].Status = ((SQLDataAccessLayer.Models.BkRequestStatus)CmbEmailStatus.SelectedItem).RequestStatusID.ToString();
                                    Pariw.ReteriveBookingItems();
                                }

                                Bookingsend.Remove(item);
                                dgBookingemailsend.ItemsSource = Bookingsend;
                                // emailprogess.IsIndeterminate = false;

                                if(supSavesettingid>0)
                                {
                                    Suppemailset.ModifiedBy = (!string.IsNullOrEmpty(loginuserid)) ? Guid.Parse(loginuserid) : Guid.Empty;
                                    Suppemailset.SendStatus = true;
                                    Suppemailset.Error = string.Empty;
                                    Suppemailset.SupplierEmailSettingsid = supSavesettingid;
                                    long supSavesettingidre = SaveSupplierEmaildetails("U",Suppemailset);

                                }



                            }
                            catch (Exception ex)
                            {
                                item.Status = "Failed";
                                item.Error = ex.Message.ToString(); 
                                Bookingsend.Where(x => x.BookingID == item.BookingID).FirstOrDefault().Status=item.Status;
                                Bookingsend.Where(x => x.BookingID == item.BookingID).FirstOrDefault().Error = item.Error;

                                

                                if (supSavesettingid > 0)
                                {
                                    Suppemailset.ModifiedBy = (!string.IsNullOrEmpty(loginuserid)) ? Guid.Parse(loginuserid) : Guid.Empty;
                                    Suppemailset.SendStatus = false;
                                    Suppemailset.Error = ex.Message.ToString();
                                    Suppemailset.SupplierEmailSettingsid = supSavesettingid;
                                    long supSavesettingidre = SaveSupplierEmaildetails("U",Suppemailset);

                                }
                                // Bookingsend.Remove(item);
                                // Bookingsend.Add(item);
                                dgBookingemailsend.ItemsSource = Bookingsend;
                                errobj.WriteErrorLoginfo("SupplierRequestEmailSend", System.Reflection.MethodBase.GetCurrentMethod().Name, "Mail Send Failed: To:" + strToEmail + " Subject:" + strMailSubject + "Error:" + ex.Message.ToString(), "");
                               // emailprogess.IsIndeterminate = false;
                            }
                            if (Bookingsend.Count == 0)
                            {
                                result = "success";
                            }
                            if (selectcnt==k)
                            {
                                result = "success,selected";
                            }
                            
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                errobj.WriteErrorLoginfo("SupplierRequestEmailSend", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            return result;
        }

        public long SaveSupplierEmaildetails(string Purpose,SupplierEmailSetting Suppemailset)
        {
            EmailDal dalobj = new EmailDal();
            string rest=dalobj.SaveSupplierEmails(Purpose,Suppemailset);
            return (!string.IsNullOrEmpty(rest)) ? Convert.ToInt64(rest) : 0;

        }

    }
}
