using SQLDataAccessLayer.DAL;
using SQLDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xaml;
using System.Runtime.InteropServices;
using Microsoft.Win32;
//using mshtml;
using System.Net.Mail;


namespace LIT.Old_LIT
{
    /// <summary>
    /// Interaction logic for SupplierRequestEmail.xaml
    /// </summary>
    public partial class SupplierRequestEmailEdit : Window
    {
        SQLDataAccessLayer.DAL.ItineraryDAL objitdal = new SQLDataAccessLayer.DAL.ItineraryDAL();
        LoadDropDownListValues loadDropDownListValues = new LoadDropDownListValues();
        List<Userdetails> ListUserdet = new List<Userdetails>();
        DataSet dsFolder = new DataSet();
        string loginusername = string.Empty;
        string loginuserid = string.Empty;
        string recordmode = string.Empty;
        string Emailoption = string.Empty;
        string FromEmail = string.Empty;
        string BccEmail = string.Empty;
        string Subject = string.Empty;
        BookingEmail ParobjBE;
        string validmsg = string.Empty;

        SupplierRequestEmail PobjSre;
        ItineraryWindow Piw;
        int emailcnt = 0;
        int Currentemailcnt = 0;

        [ComImport]
        [Guid("3E68D4BD-7135-4D10-8018-9FB6D9F33FA1")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IInitializeWithWindow
        {
            void Initialize(IntPtr hwnd);
        }
        public SupplierRequestEmailEdit()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public SupplierRequestEmailEdit(string username, string PEmailoption, string navfrom, BookingEmail PObjBE, SupplierRequestEmail obSRE = null, ItineraryWindow objiw = null)
        {
            InitializeComponent();
            this.DataContext = this;
            loginusername = username;
            Emailoption = PEmailoption;
            if (objiw != null)
            {
                Piw = objiw;
            }
            if (obSRE != null)
            {
                PobjSre = obSRE;
                if (obSRE.BookingReqDet != null)
                {
                    emailcnt = obSRE.BookingReqDet.Count;
                    lblbookingemail.Content = "Booking 1 of " + emailcnt;
                    //txtTemplateContents.Text = obSRE.Emailcontent[0].ToString();

                    BookingReqDetedit = null;
                    BookingReqDetedit = obSRE.BookingReqDet;
                    txtTo.Text = BookingReqDetedit[0].SupplierName + " <" + BookingReqDetedit[0].UserEmail + ">";
                    Currentemailcnt = 1;
                    webbrowserEmailEdit.NavigateToString(obSRE.BookingReqDet[0].FinalEmailContent.ToString());
                    //txtTemplateContents.Text = obSRE.BookingReqDet[0].FinalEmailContent.ToString();

                    //  txtrchTemplateContents.AppendText(obSRE.BookingReqDet[0].FinalEmailContent.ToString());
                    // await mywebview.EnsureCoreWebView2Async();
                  //  load(obSRE.BookingReqDet[0].FinalEmailContent.ToString());

                    if (emailcnt == 1)
                    {
                        btnNextEmailid.IsEnabled = false;
                        btnPreviousEmailid.IsEnabled = false;
                    }
                    else
                    {
                        btnNextEmailid.IsEnabled = true;
                        btnPreviousEmailid.IsEnabled = true;
                    }
                }
            }
            if (PObjBE != null)
            {
                ParobjBE = PObjBE;
                
                if (navfrom.ToLower() == "reqemail")
                {
                    txtsubject.Text = ParobjBE.Subject;
                    BookingReqDetedit.ToList().ForEach(x => { x.Emailsubject = txtsubject.Text; });
                }
                else
                {
                    txtsubject.Text = BookingReqDetedit[0].Emailsubject;
                }
                txtfileupload.Text = (ParobjBE.attach != null) ? System.IO.Path.GetFileName(ParobjBE.attach) : string.Empty;
            }
            loginuserid = loadDropDownListValues.Currentuseridinfo(loginusername);
        }

     /*   private async void load(string htmstr)
        {
            await mywebview.EnsureCoreWebView2Async();
            mywebview.NavigateToString(htmstr);
        }

        private async Task<string> editedEmailContent()
        {
        //    NetworkProgressBar.IsEnabled = true;
        //    NetworkProgressBar.Visibility = Visibility.Visible;

            string str = await mywebview.ExecuteScriptAsync("document.documentElement.outerHTML");
            
           // NetworkProgressBar.IsEnabled = true;
           // NetworkProgressBar.Visibility = Visibility.Visible;

            return str;
        }

        */
        private ObservableCollection<BookingEmailRequestDetails> _BookingReqDetedit;
        public ObservableCollection<BookingEmailRequestDetails> BookingReqDetedit
        {
            get { return _BookingReqDetedit ?? (_BookingReqDetedit = new ObservableCollection<BookingEmailRequestDetails>()); }
            set
            {
                _BookingReqDetedit = value;
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            // Task<string> str= editedEmailContent();

            if (txtTo.Text.Split(";").Length > 0)
            {
                foreach (string s in txtTo.Text.Trim().Split(";"))
                {
                    validmsg = ParobjBE.Validdateemail(s, "to");
                    if (!string.IsNullOrEmpty(validmsg))
                    {
                        MessageBox.Show(validmsg);
                        txtTo.Focus();
                        return;
                    }
                }
            }
            else
            {
                validmsg = ParobjBE.Validdateemail(txtTo.Text.Trim(), "to");
            }
            dynamic doc1 = webbrowserEmailEdit.Document;
            var htmlText = doc1.documentElement.InnerHtml;
            BookingReqDetedit[Currentemailcnt-1].FinalEmailContent = htmlText;
            string toaddress=string.Empty;
            if (txtTo.Text.Trim().Contains("<"))
            {
               
                toaddress = txtTo.Text.Trim().Split("<")[1].Replace(">", "");                
                BookingReqDetedit[Currentemailcnt - 1].UserEmail = toaddress;
                BookingReqDetedit[Currentemailcnt - 1].Emailsubject = txtsubject.Text;
            }
            else 
            {
                toaddress = txtTo.Text.Trim();
                BookingReqDetedit[Currentemailcnt - 1].UserEmail = toaddress;
                BookingReqDetedit[Currentemailcnt - 1].Emailsubject = txtsubject.Text;
            }

            if (!string.IsNullOrEmpty(validmsg))
            {
                MessageBox.Show(validmsg);
                txtTo.Focus();
                return;
            }
            SupplierRequestEmailSend wnEmSend = new SupplierRequestEmailSend(loginusername, Emailoption, this, PobjSre,Piw, ParobjBE);
            this.Close();
            wnEmSend.ShowDialog();

        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            ParobjBE.Subject = txtsubject.Text.Trim();
            ParobjBE.To = txtTo.Text.Trim();

            validmsg = ParobjBE.Validdateemail(txtTo.Text.Trim(), "to");
            if (!string.IsNullOrEmpty(validmsg))
            {
                MessageBox.Show(validmsg);
                txtTo.Focus();
            }
            SupplierRequestEmail wnEm = new SupplierRequestEmail(loginusername, Emailoption, ParobjBE,Piw);
            this.Close();
            wnEm.ShowDialog();
        }

        private void btnPreviousEmailid_Click(object sender, RoutedEventArgs e)
        {
            if (emailcnt > 1 && emailcnt >= Currentemailcnt && Currentemailcnt>1)
            {
                dynamic doc1 = webbrowserEmailEdit.Document;
                var htmlText = doc1.documentElement.InnerHtml;
                BookingReqDetedit[Currentemailcnt-1].FinalEmailContent = htmlText;
                string toaddress = string.Empty;
                if (txtTo.Text.Trim().Contains("<"))
                {
                    toaddress = txtTo.Text.Trim().Split("<")[1].Replace(">", "");
                    BookingReqDetedit[Currentemailcnt - 1].UserEmail = toaddress;
                    BookingReqDetedit[Currentemailcnt - 1].Emailsubject = txtsubject.Text;
                }
                else
                {
                    toaddress = txtTo.Text.Trim();
                    BookingReqDetedit[Currentemailcnt - 1].UserEmail = toaddress;
                    BookingReqDetedit[Currentemailcnt - 1].Emailsubject = txtsubject.Text;
                    
                }

                Currentemailcnt = Currentemailcnt - 1;
                lblbookingemail.Content = "Booking "+ Currentemailcnt.ToString()+ " of " + emailcnt.ToString();
                txtTo.Text = BookingReqDetedit[Currentemailcnt - 1].SupplierName+" <"+BookingReqDetedit[Currentemailcnt-1].UserEmail+">";
                txtsubject.Text = BookingReqDetedit[Currentemailcnt - 1].Emailsubject;
                txtfileupload.Text = (BookingReqDetedit[Currentemailcnt - 1].FileAttachName != null) ? BookingReqDetedit[Currentemailcnt - 1].FileAttachName : string.Empty;
                webbrowserEmailEdit.NavigateToString(BookingReqDetedit[Currentemailcnt-1].FinalEmailContent.ToString());
                
                //txtTemplateContents.Text= BookingReqDetedit[Currentemailcnt - 1].FinalEmailContent.ToString();
                //  txtrchTemplateContents.AppendText(BookingReqDetedit[Currentemailcnt - 1].FinalEmailContent.ToString());
            }
        }

        private void btnNextEmailid_Click(object sender, RoutedEventArgs e)
        {
            if (emailcnt > 1 && emailcnt>Currentemailcnt)
            {
                dynamic doc1 = webbrowserEmailEdit.Document;
                var htmlText = doc1.documentElement.InnerHtml;
                BookingReqDetedit[Currentemailcnt-1].FinalEmailContent = htmlText;
                string toaddress = string.Empty;
                if (txtTo.Text.Trim().Contains("<"))
                {
                    toaddress = txtTo.Text.Trim().Split("<")[1].Replace(">", "");
                    BookingReqDetedit[Currentemailcnt - 1].UserEmail = toaddress;
                    BookingReqDetedit[Currentemailcnt - 1].Emailsubject = txtsubject.Text;
                }
                else
                {
                    toaddress = txtTo.Text.Trim();
                    BookingReqDetedit[Currentemailcnt - 1].UserEmail = toaddress;
                    BookingReqDetedit[Currentemailcnt - 1].Emailsubject = txtsubject.Text;
                }

                Currentemailcnt = Currentemailcnt + 1;                
                lblbookingemail.Content = "Booking " + Currentemailcnt.ToString() + " of " + emailcnt.ToString();
                txtTo.Text = BookingReqDetedit[Currentemailcnt - 1].SupplierName + " <" + BookingReqDetedit[Currentemailcnt-1].UserEmail+">";
                txtsubject.Text=BookingReqDetedit[Currentemailcnt - 1].Emailsubject;
                txtfileupload.Text = (BookingReqDetedit[Currentemailcnt - 1].FileAttachName!=null)? BookingReqDetedit[Currentemailcnt - 1].FileAttachName:string.Empty; 
                webbrowserEmailEdit.NavigateToString(BookingReqDetedit[Currentemailcnt-1].FinalEmailContent.ToString());
                //txtTemplateContents.Text = BookingReqDetedit[Currentemailcnt - 1].FinalEmailContent.ToString();

                //  txtrchTemplateContents.AppendText(BookingReqDetedit[Currentemailcnt - 1].FinalEmailContent.ToString());
               // webview.Source = BookingReqDetedit[Currentemailcnt - 1].FinalEmailContent.ToString();
            }            
        }

       
        //private void btnupload_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog fileDialog = new OpenFileDialog();
        //   // fileDialog.DefaultExt = ".png"; // Required file extension 
        //   // fileDialog.Filter = "All files (*.*)|*.*\\"; // Optional file extensions           
        //   // Filter = "TXT Files(*.txt;)|*.txt;|Image Files(*.jpg;*.jpeg;*.bmp)|*.jpg;*.jpeg;.bmp;"
        //    bool? res = fileDialog.ShowDialog(); 
        //    if (res.HasValue && res.Value)                
        //    {
        //        System.IO.StreamReader sr = new System.IO.StreamReader(fileDialog.FileName);
        //        // MessageBox.Show(sr.ReadToEnd());
        //        string selBkid=BookingReqDetedit[Currentemailcnt - 1].BookingID.ToString();
        //        txtfileupload.Text = System.IO.Path.GetFileName(fileDialog.FileName);
        //        fullfilepath.Text = fileDialog.FileName;
        //        ParobjBE.attach= System.IO.Path.GetFileName(fileDialog.FileName);
                

        //        sr.Close();
        //    }
        //}
        private void mnucurrent_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            bool? res = fileDialog.ShowDialog();
            if (res.HasValue && res.Value)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(fileDialog.FileName);
                // MessageBox.Show(sr.ReadToEnd());
                string selBkid = BookingReqDetedit[Currentemailcnt - 1].BookingID.ToString();
                txtfileupload.Text = System.IO.Path.GetFileName(fileDialog.FileName);
                fullfilepath.Text = fileDialog.FileName;
                ParobjBE.attach = System.IO.Path.GetFileName(fileDialog.FileName);
                BookingReqDetedit[Currentemailcnt - 1].FileAttachPath = fullfilepath.Text;
                BookingReqDetedit[Currentemailcnt - 1].FileAttachName = txtfileupload.Text;
                sr.Close();
            }
           
            //BookingReqDetedit.ToList().ForEach(x => { x.FileAttachPath = fileDialog.FileName; x.FileAttachName = System.IO.Path.GetFileName(fileDialog.FileName); });
        }

        private void mnuall_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            bool? res = fileDialog.ShowDialog();
            if (res.HasValue && res.Value)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(fileDialog.FileName);
                // MessageBox.Show(sr.ReadToEnd());
                string selBkid = BookingReqDetedit[Currentemailcnt - 1].BookingID.ToString();
                txtfileupload.Text = System.IO.Path.GetFileName(fileDialog.FileName);
                fullfilepath.Text = fileDialog.FileName;
                ParobjBE.attach = System.IO.Path.GetFileName(fileDialog.FileName); BookingReqDetedit.ToList().ForEach(x => { x.FileAttachPath = fullfilepath.Text; x.FileAttachName = System.IO.Path.GetFileName(txtfileupload.Text); });
                sr.Close();
            }
            
        }
    }
}
