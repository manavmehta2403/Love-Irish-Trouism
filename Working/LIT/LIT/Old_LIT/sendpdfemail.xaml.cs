using DocumentFormat.OpenXml.Office2010.Excel;
using LITModels;
using Microsoft.Win32;
using SQLDataAccessLayer.DAL;
using SQLDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static LIT.Old_LIT.ItineraryWindow;

namespace LIT.Old_LIT
{
    /// <summary>
    /// Interaction logic for sendpdfemail.xaml
    /// </summary>
    public partial class Sendpdfemail : Window
    {
        private string attachmentPath = "";
        public MailCredentials mailSetting = null;
        EmailDal EmailDalobj = new EmailDal();
        private List<string> attachmentPaths = new List<string>();
        private string _type;
        private string _Id;
        string loginusername = string.Empty;
        string loginuserid = string.Empty;
        LoadDropDownListValues loadDropDownListValues = new LoadDropDownListValues();
        List<PDFFilelist> LstPdffiles=new List<PDFFilelist>();
        int emailcnt = 0;
        int Currentemailcnt = 0;
        DBConnectionEF DBconnEF = new DBConnectionEF();
        public string Imagepdfhtmlfolderpathval
        {
            get; set;
        }

        //  public Sendpdfemail(string OutputFileName, string sub,string ToEmail,string type, string Itineraryid)
        public Sendpdfemail(List<PDFFilelist> Pdffileslist,string username)
        {
            InitializeComponent();
            this.DataContext = this;
            Imagepdfhtmlfolderpathval = DBconnEF.GetImagePDFTHtmlFolderPath();
            loginusername = username;
            loginuserid = loadDropDownListValues.Currentuseridinfo(loginusername);
            LstPdffiles = Pdffileslist;
            emailcnt = Pdffileslist.Count;
            Currentemailcnt = 1;
            _type = Pdffileslist[0].Selectionoption;
            _Id = Pdffileslist[0].Itineraryid;
            mailSetting = EmailDalobj.GetMailCredentials();
            txtfrom.Text = mailSetting.FromEmail;
            txtsubject.Text = Pdffileslist[0].ItineraryName; 
            txtTo.Text = Pdffileslist[0].EmailID;
            string defaultAttachmentPath = System.IO.Path.Combine(Imagepdfhtmlfolderpathval+"\\", "Pdf", Pdffileslist[0].OutputFilesname);
            SavePdfChecked.IsChecked = true;

            if (System.IO.File.Exists(defaultAttachmentPath))
            {
                attachmentPath = defaultAttachmentPath;
                txtfileupload.AppendText(System.IO.Path.GetFileName(attachmentPath));
                //attachmentPaths.Add(attachmentPath);
                Pdffileslist[0].Attachmentfile= txtfileupload.Text;
            }
            if (emailcnt == 1)
            {
                btnNextCustEmailid.IsEnabled = false;
                btnPreviousCustEmailid.IsEnabled = false;
            }
            else
            {
                btnNextCustEmailid.IsEnabled = true;
                btnPreviousCustEmailid.IsEnabled = true;
            }
        }

        public Sendpdfemail()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                int sntcnt = 0;

                foreach (PDFFilelist pdflst in LstPdffiles)
                {
                    Clienttabdal clienttabdal = new Clienttabdal();
                    List<PassengerDetails> passengerDetails = clienttabdal.RetrivePassengerDetails(Guid.Parse(_Id));
                    pdflst.passengerName =  passengerDetails.FirstOrDefault(id => id.Passengerid == pdflst.Passengerid).DisplayName;               
                        // Create a MailMessage with To, CC, Subject, and Body
                    var mailmsg = new MailMessage();

                    // Split and add multiple "To" recipients
                    string[] toRecipients = pdflst.EmailID.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries); //txtTo.Text.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string toRecipient in toRecipients)
                    {
                        mailmsg.To.Add(toRecipient.Trim());
                    }

                    mailmsg.Subject = pdflst.ItineraryName; //txtsubject.Text;
                    mailmsg.Body = pdflst.Bodyhtml;//webbrowserEmailEdit.Text;

                    // Attach selected files
                    //foreach (string filePath in attachmentPaths)
                    //{
                    //    Attachment attachment = new Attachment(filePath);
                    //    mailmsg.Attachments.Add(attachment);
                    //}

                    string defaultAttachmentPath = System.IO.Path.Combine(Imagepdfhtmlfolderpathval+"\\", "Pdf", pdflst.OutputFilesname);

                    if (System.IO.File.Exists(defaultAttachmentPath))
                    {
                        Attachment attachment = new Attachment(defaultAttachmentPath);
                        mailmsg.Attachments.Add(attachment);

                    }
                    SmtpClient smtpClient = new SmtpClient(mailSetting.SMTPServer, mailSetting.SMTPPort);

                    if (!string.IsNullOrWhiteSpace(mailSetting.FromEmail) && !string.IsNullOrWhiteSpace(mailSetting.FromEmailPassword))
                    {
                        smtpClient.Credentials = new NetworkCredential(mailSetting.FromEmail, mailSetting.FromEmailPassword);
                    }

                    smtpClient.Timeout = 300000;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.EnableSsl = mailSetting.IsSSLRequired;
                    mailmsg.From = new MailAddress(txtfrom.Text.Trim(), mailSetting.FromEmailName);

                    smtpClient.Send(mailmsg);
                    if (SavePdfChecked.IsChecked == true)
                    {
                        SavePdf(mailmsg , pdflst);
                    }
                    mailmsg.Dispose();
                    sntcnt = sntcnt + 1;
                }

                if (LstPdffiles.Count== sntcnt)
                {
                    MessageBox.Show("Email sent successfully.");
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void SavePdf(MailMessage mailMessage , PDFFilelist pdflist)
        {
            CustomerEmailSettingsDal customerEmail = new CustomerEmailSettingsDal();
                EmailLogsSettingCollection emailLogs = new EmailLogsSettingCollection
                {
                    CustomerEmailSettingId = new Random().Next(1000, 9999),
                    ItineraryID = Guid.Parse(_Id),
                    SentDate = DateTime.Now,
                    TypeID = Guid.NewGuid(),
                    Type = _type,
                    FromAddress = mailMessage.From.ToString(),
                    RecipientEmail = pdflist.EmailID,
                    Recipient = pdflist.passengerName,
                    PassengerId = Guid.Parse(pdflist.Passengerid),
                    EmailSubject = mailMessage.Subject,
                    AttachmentPDF = $"{Imagepdfhtmlfolderpathval}\\Pdf\\{pdflist.Attachmentfile}",
                    AttachmentWord = "sample.docx",
                    EmailBodyContentTemplate = "sample.docx",
                    EmailBodyContentPreview = $"<html><body><div>{pdflist.Bodyhtml}</div></body></html>",
                    SendStatus = true,
                    CreatedOn = DateTime.Now,
                    CreatedBy = Guid.Parse(loginuserid),
                    ModifiedOn = DateTime.Now,
                    ModifiedBy = Guid.Parse(loginuserid),
                    IsDeleted = false,
                    DeletedOn = DateTime.Now, // Set to null or specific value
                    DeletedBy = Guid.Parse(loginuserid) // Set to null or specific value
                };

                customerEmail.SaveCustomerEmailSetting("I", emailLogs);

        }
        private void AttachButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true; // Allow multiple file selection

            if (openFileDialog.ShowDialog() == true)
            {
                txtfileupload.AppendText("; ");
                foreach (string filePath in openFileDialog.FileNames)
                {
                    txtfileupload.AppendText(System.IO.Path.GetFileName(filePath) + "; ");

                    // Add the file path to the list or array
                    attachmentPaths.Add(filePath);
                }
            }
        }

        private void btnPreviousCustEmailid_Click(object sender, RoutedEventArgs e)
        {
            if (emailcnt > 1 && emailcnt >= Currentemailcnt && Currentemailcnt > 1)
            {
                string toaddress = string.Empty;
                dynamic doc1 = webbrowserEmailEdit.Text;
                LstPdffiles[Currentemailcnt - 1].Bodyhtml = doc1;
                LstPdffiles[Currentemailcnt - 1].EmailID = txtTo.Text;
                LstPdffiles[Currentemailcnt - 1].Attachmentfile = txtfileupload.Text;
                //if (txtTo.Text.Trim().Contains("<"))
                //{
                //    toaddress = txtTo.Text.Trim().Split("<")[1].Replace(">", "");
                //    LstPdffiles[Currentemailcnt - 1].EmailID = toaddress;
                //    LstPdffiles[Currentemailcnt - 1].Emailsubject = txtsubject.Text;
                //}
                //else
                //{
                //    toaddress = txtTo.Text.Trim();
                //    BookingReqDetedit[Currentemailcnt - 1].UserEmail = toaddress;
                //    BookingReqDetedit[Currentemailcnt - 1].Emailsubject = txtsubject.Text;
                //}

                Currentemailcnt = Currentemailcnt - 1;

                
                txtTo.Text = LstPdffiles[Currentemailcnt - 1].EmailID;
                txtsubject.Text = LstPdffiles[Currentemailcnt - 1].ItineraryName;
                string defaultAttachmentPath = System.IO.Path.Combine(Imagepdfhtmlfolderpathval+"\\", "Pdf", LstPdffiles[Currentemailcnt - 1].OutputFilesname);


                if (System.IO.File.Exists(defaultAttachmentPath))
                {
                    attachmentPath = defaultAttachmentPath;
                    //txtfileupload.AppendText(System.IO.Path.GetFileName(attachmentPath));
                    txtfileupload.Text= (System.IO.Path.GetFileName(attachmentPath));
                    attachmentPaths.Add(attachmentPath);
                    LstPdffiles[Currentemailcnt - 1].Attachmentfile = txtfileupload.Text;
                }
                // txtfileupload.Text = (LstPdffiles[Currentemailcnt - 1].Attachmentfile != null) ? LstPdffiles[Currentemailcnt - 1].Attachmentfile : string.Empty;
                if (LstPdffiles[Currentemailcnt - 1].Bodyhtml != null)
                {
                    webbrowserEmailEdit.Text = (LstPdffiles[Currentemailcnt - 1].Bodyhtml.ToString());
                }
                else
                {
                    webbrowserEmailEdit.Text = string.Empty;
                }
        }
        }

        private void btnNextCustEmailid_Click(object sender, RoutedEventArgs e)
        {
            if (LstPdffiles.Count > 1 && emailcnt > Currentemailcnt)
            {
                dynamic doc1 = webbrowserEmailEdit.Text;
                LstPdffiles[Currentemailcnt - 1].Bodyhtml = doc1;
                LstPdffiles[Currentemailcnt - 1].EmailID = txtTo.Text;
                LstPdffiles[Currentemailcnt - 1].Attachmentfile = txtfileupload.Text;
                string toaddress = string.Empty;
                //if (txtTo.Text.Trim().Contains("<"))
                //{
                //    toaddress = txtTo.Text.Trim().Split("<")[1].Replace(">", "");
                //    LstPdffiles[Currentemailcnt - 1].EmailID = toaddress;
                //    LstPdffiles[Currentemailcnt - 1].Emailsubject = txtsubject.Text;
                //}
                //else
                //{
                //    toaddress = txtTo.Text.Trim();
                //    BookingReqDetedit[Currentemailcnt - 1].UserEmail = toaddress;
                //    BookingReqDetedit[Currentemailcnt - 1].Emailsubject = txtsubject.Text;
                //}

                Currentemailcnt = Currentemailcnt + 1;
                txtTo.Text = LstPdffiles[Currentemailcnt - 1].EmailID;
                txtsubject.Text = LstPdffiles[Currentemailcnt - 1].ItineraryName;
                string defaultAttachmentPath = System.IO.Path.Combine(Imagepdfhtmlfolderpathval+"\\", "Pdf", LstPdffiles[Currentemailcnt - 1].OutputFilesname);


                if (System.IO.File.Exists(defaultAttachmentPath))
                {
                    attachmentPath = defaultAttachmentPath;
                    //txtfileupload.AppendText(System.IO.Path.GetFileName(attachmentPath));
                    txtfileupload.Text=(System.IO.Path.GetFileName(attachmentPath));
                    attachmentPaths.Add(attachmentPath);
                    LstPdffiles[Currentemailcnt - 1].Attachmentfile = txtfileupload.Text;
                }
                // txtfileupload.Text = (LstPdffiles[Currentemailcnt - 1].Attachmentfile != null) ? LstPdffiles[Currentemailcnt - 1].Attachmentfile : string.Empty;
                if (LstPdffiles[Currentemailcnt - 1].Bodyhtml != null)
                {
                    webbrowserEmailEdit.Text = (LstPdffiles[Currentemailcnt - 1].Bodyhtml.ToString());
                }
                else
                {
                    webbrowserEmailEdit.Text = string.Empty;
                }
                
            }
        }

        private void webbrowserEmailEdit_LostFocus(object sender, RoutedEventArgs e)
        {
            dynamic doc1 = webbrowserEmailEdit.Text;
            LstPdffiles[Currentemailcnt - 1].Bodyhtml = doc1;           
        }

        private void txtTo_LostFocus(object sender, RoutedEventArgs e)
        {
            LstPdffiles[Currentemailcnt - 1].EmailID = txtTo.Text;
        }
    }
}
