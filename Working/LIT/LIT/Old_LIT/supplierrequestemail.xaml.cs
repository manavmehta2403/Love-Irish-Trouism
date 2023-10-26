//using mshtml;
using SQLDataAccessLayer.DAL;
using SQLDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
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

namespace LIT.Old_LIT
{
    /// <summary>
    /// Interaction logic for SupplierRequestEmail.xaml
    /// </summary>
    public partial class SupplierRequestEmail : Window
    {
        SQLDataAccessLayer.DAL.ItineraryDAL objitdal = new SQLDataAccessLayer.DAL.ItineraryDAL();
        LoadDropDownListValues loadDropDownListValues = new LoadDropDownListValues();
        EmailDal EmailDalobj = new EmailDal();
        List<Userdetails> ListUserdet = new List<Userdetails>();
        DataSet dsFolder = new DataSet();
        string loginusername = string.Empty;
        string loginuserid = string.Empty;
        string recordmode = string.Empty;
        string Emailoption = string.Empty;
        string templateFullPath = string.Empty;
        Errorlog errobj = new Errorlog();
        //  public List<string> Emailcontent = new List<string>();
        public MailCredentials mailSetting = null;

        BookingEmail ObjBE = new BookingEmail();

        ItineraryWindow iwparwindow;
        string validmsg = string.Empty;
        public SupplierRequestEmail()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public SupplierRequestEmail(string username, string PEmailoption, BookingEmail RobjBE = null, ItineraryWindow iwpar = null)
        {
            ObservableCollection<BookingItems> observablecollection = new ObservableCollection<BookingItems>();
            InitializeComponent();
            this.DataContext = this;
            loginusername = username;
            loginuserid = loadDropDownListValues.Currentuseridinfo(loginusername);
            Emailoption = PEmailoption;
            if (RobjBE != null)
            {
                ObjBE = RobjBE;
                txtsubject.Text = ObjBE.Subject;
                txtBcc.Text = ObjBE.BccEmail;
            }

            LoadTemplate();
            mailSetting = EmailDalobj.GetMailCredentials();
            if (mailSetting != null)
            {
                txtfrom.Text = mailSetting.FromEmail;
                txtfrom.IsReadOnly = true;
            }
            if (iwpar != null)
            {
                iwparwindow = iwpar;
                BookingItemsitinemail = null;
                BookingItemsitinemail = iwparwindow.BookingItemsitin;
                ObjBE.Subject = "Booking request for " + iwparwindow.TxtItineraryName.Text.Trim();
                txtsubject.Text = ObjBE.Subject;
                if (!string.IsNullOrEmpty(Emailoption))
                {
                    if (Emailoption.Contains(","))
                    {
                        //long bkid = Convert.ToInt64(Emailoption.Split(",")[1].ToString());
                        string[] ids = Emailoption.Split(","); //select new BookingItems { BookingID };
                                                               //var booklist = from a in iwparwindow.BookingItemsitin join b in ids on a.BookingID equals b.i select new BookingItems { BookingID = b.BookingID };


                        observablecollection = new ObservableCollection<BookingItems>(iwparwindow.BookingItemsitin.Where(x => ids.Contains(x.BookingID.ToString())).ToList());
                        BookingItemsitinemail = observablecollection;
                        ChangeTemplateasEmailContent(BookingItemsitinemail);
                    }
                    else
                    {
                        ChangeTemplateasEmailContent(BookingItemsitinemail);
                    }
                }

                ChckBxPrices.Checked += (sender, e) => ChangeTemplateasEmailContent(BookingItemsitinemail);
                ChckBxPrices.Unchecked += (sender, e) => ChangeTemplateasEmailContent(BookingItemsitinemail);
            }




        }

        private ObservableCollection<BookingItems> _BookingItemsitinemail;
        public ObservableCollection<BookingItems> BookingItemsitinemail
        {
            get { return _BookingItemsitinemail ?? (_BookingItemsitinemail = new ObservableCollection<BookingItems>()); }
            set
            {
                _BookingItemsitinemail = value;
            }
        }

        private ObservableCollection<BookingEmailRequestDetails> _BookingReqDet;
        public ObservableCollection<BookingEmailRequestDetails> BookingReqDet
        {
            get { return _BookingReqDet ?? (_BookingReqDet = new ObservableCollection<BookingEmailRequestDetails>()); }
            set
            {
                _BookingReqDet = value;
            }
        }




        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            
            ObjBE.templatepath = templateFullPath;
            ObjBE.FromEmail = txtfrom.Text.Trim();
            ObjBE.BccEmail = txtBcc.Text.Trim();
            ObjBE.Subject = txtsubject.Text.Trim();
            ObjBE.attach = null;
            validmsg = ObjBE.Validdateemail(txtfrom.Text.Trim(), "from");
            if (!string.IsNullOrEmpty(validmsg))
            {
                MessageBox.Show(validmsg);
                txtfrom.Focus();
                return;
            }
            if (txtBcc.Text.Split(";").Length > 0)
            {
                foreach (string s in txtBcc.Text.Split(";"))
                {
                    validmsg = ObjBE.Validdateemail(s, "bcc");
                    if (!string.IsNullOrEmpty(validmsg))
                    {
                        MessageBox.Show(validmsg);
                        txtBcc.Focus();
                        return;
                    }
                }
            }
            else
            {
                validmsg = ObjBE.Validdateemail(txtBcc.Text.Trim(), "bcc");
            }
            if (!string.IsNullOrEmpty(validmsg))
            {
                MessageBox.Show(validmsg);
                txtBcc.Focus();
                return;
            }
            if (!string.IsNullOrEmpty(Emailoption))
            {
                if (Emailoption.Contains(","))
                {
                    string[] ids = Emailoption.Split(","); //select new BookingItems { BookingID };
                                                           //var booklist = from a in iwparwindow.BookingItemsitin join b in ids on a.BookingID equals b.i select new BookingItems { BookingID = b.BookingID };


                    var observablecollection = new ObservableCollection<BookingItems>(iwparwindow.BookingItemsitin.Where(x => ids.Contains(x.BookingID.ToString())).ToList());
                   // long bkid = Convert.ToInt64(Emailoption.Split(",")[1].ToString());
                   // var observablecollection = new ObservableCollection<BookingItems>(iwparwindow.BookingItemsitin.Where(x => x.BookingID == bkid).ToList());
                    BookingItemsitinemail = observablecollection;
                    ChangeTemplateasEmailContent(BookingItemsitinemail);
                }
                else
                {
                    ChangeTemplateasEmailContent(BookingItemsitinemail);
                }
            }
            SupplierRequestEmailEdit wnEmSend = new SupplierRequestEmailEdit(loginusername, Emailoption, "ReqEmail", ObjBE, this, iwparwindow);
            this.Close();
            wnEmSend.ShowDialog();
        }

        private bool LoadTemplate()
        { // Load HTML document as a stream  
          // Uri uri = new Uri(@"pack://application:,,,/TourWriter.Email.BookingRequest.html", UriKind.Absolute);

            /*Uri urival=new Uri(String.Format("Email Templates/{0}.html", "Email.BookingRequest"), UriKind.Relative);
            Stream source = Application.GetContentStream(urival).Stream;
            txtfileupload.Text = urival.AbsolutePath.ToString();
            ObjBE.templatepath = urival.AbsolutePath.ToString();
          // Navigate to HTML document stream  
          this.webBrowsercontent.NavigateToStream(source);
            */

            try
            {
                var exePath = AppDomain.CurrentDomain.BaseDirectory;
                //var exePathbfbin = exePath.Split("bin")[0].ToString();
                var pagesFolder = Directory.GetParent(exePath);//.Parent.Parent;
                                                               //var pagesFolder = Directory.GetDirectories(exePathbfbin+ "\\Email Templates\\Email.BookingRequest.html");
                templateFullPath = pagesFolder.FullName + "\\Email Templates\\Email.BookingRequest.html";
                // templateFullPath = pagesFolder[0];
                txtfileupload.Text = templateFullPath;
                ObjBE.templatepath = templateFullPath;

                webBrowsercontent.Source = new Uri(templateFullPath);
                //IHTMLDocument2 doc = webBrowsercontent.Document as IHTMLDocument2;
                //doc.designMode = "Off";
                return true;
            }
            catch (Exception ex)
            {

                errobj.WriteErrorLoginfo("SupplierRequestEmail", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
                return false;
            }

        }

        private void ChangeTemplateasEmailContent(ObservableCollection<BookingItems> objbkitm)
        {
            BookingReqDet.Clear();

            string strMarkers = String.Empty;
            string filePath = ObjBE.templatepath;

            string priceString = @"<tr><td><p>Price</p></td><td><p>##ItemPrice##</p></td></tr><tr><td><p>Total Net</p></td><td><p>##ItemTotal##</p></td></tr>";

            if (ChckBxPrices.IsChecked == true)
            {
                strMarkers = AddNewTableRow(filePath, priceString);
            }

            if(ChckBxPrices.IsChecked == false) 
            {
                strMarkers = RemoveTableRow(filePath, priceString);
            }

            string EmailStrMarkers = string.Empty;
            List<string> CoreMarkers = EmailDalobj.GetMarkervalues(strMarkers, "##", "##");
            List<string> RepeatedMarkers = EmailDalobj.GetMarkervalues(strMarkers, "##BookingsStart##", "##BookingsEnd##");
            int i = 0;

            foreach (BookingItems Bkit in objbkitm)
            {

                if (BookingReqDet.Where(x => x.BookingID == Bkit.BookingID.ToString()).Count() == 0)
                {
                    EmailStrMarkers = strMarkers;

                    Tuple<string, string> trobj = null;
                    trobj = EmailDalobj.GetSupplierEmailandName(Bkit.SupplierID);
                    BookingEmailRequestDetails _marker = new BookingEmailRequestDetails();
                    _marker.DivStart = "<div contenteditable=\"true\" id=\"Div_bkid_##BookingID##\">";
                    _marker.DivEnd = "</div>";
                    _marker.HostName = (trobj.Item1 != null) ? trobj.Item1.ToString() : string.Empty;
                    _marker.BookingsStart = string.Empty;
                    _marker.ClientNotes = string.Empty;
                    string stremailsign = EmailDalobj.GetUserEmailsign(loginuserid);
                    if (!string.IsNullOrEmpty(stremailsign))
                    {
                        _marker.EmailSignature = stremailsign;
                    }
                    //foreach (var _repeatedMarker in RepeatedMarkers)
                    //{
                    _marker.BookingID = (Bkit.BookingID != null) ? Bkit.BookingID.ToString() : string.Empty;
                    _marker.BookingAutoID = (Bkit.BookingAutoID != null) ? Bkit.BookingAutoID.ToString() : string.Empty;
                    _marker.UserEmail = (trobj.Item2 != null) ? trobj.Item2.ToString() : string.Empty;
                    _marker.BookingDetailStart = string.Empty;
                    _marker.ItemCount = (i + 1).ToString();
                    _marker.SupplierName = (trobj.Item1 != null) ? trobj.Item1.ToString() : string.Empty;
                    _marker.ItemName = iwparwindow.TxtItineraryName.Text;
                    _marker.ItemDesc = (Bkit.ItemDescription != null) ? Bkit.ItemDescription : string.Empty;
                    _marker.ItemStartText = "Check In";
                    _marker.ItemStartDate = (Bkit.StartDate != null) ? Bkit.StartDate.ToShortDateString() : string.Empty;
                    _marker.ItemEndDateStart = string.Empty;

                    _marker.ItemEndText = "Check Out";
                    _marker.ItemEndDate = (Bkit.Enddate != null) ? Bkit.Enddate.ToShortDateString() : string.Empty;
                    _marker.ItemLengthText = "Nights";
                    _marker.ItemLength = (Bkit.NtsDays != null) ? Bkit.NtsDays.ToString() : string.Empty;
                    _marker.ItemEndDateEnd = string.Empty;
                    _marker.ItemQuantity = (Bkit.Qty != null) ? Bkit.Qty.ToString() : string.Empty;
                    _marker.ItemReferenceStart = string.Empty;
                    _marker.ItemReference = Bkit.Ref;
                    _marker.ItemReferenceEnd = string.Empty;
                    _marker.ItemPriceStart = string.Empty;
                    string itemprice = string.Empty;
                    string ItemTotal = string.Empty;
                    if (Bkit.BkgCurrencyName.ToLower() == "euro")
                    {
                        itemprice=(Bkit.Netunit != null) ? "&#8364" + " " + Bkit.Netunit.ToString() : string.Empty;
                    }
                    if (Bkit.BkgCurrencyName.ToLower() == "pound sterling")
                    {
                        itemprice = (Bkit.Netunit != null) ? "&#163" + " " + Bkit.Netunit.ToString() : string.Empty;
                    }
                    if (Bkit.ItinCurrency.ToLower() == "euro")
                    {
                        ItemTotal = (Bkit.Nettotal != null) ? "&#8364" + " " + Bkit.Nettotal.ToString() : string.Empty;
                    }
                    if (Bkit.ItinCurrency.ToLower() == "pound sterling")
                    {
                        ItemTotal = (Bkit.Nettotal != null) ? "&#163" + " " + Bkit.Nettotal.ToString() : string.Empty;
                    }
                    _marker.ItemPrice = itemprice;
                    _marker.ItemTotal = ItemTotal;// (Bkit.Nettotal != null) ? Bkit.ItinCurDisplayFormat + " " + Bkit.Nettotal.ToString("0.00") : string.Empty;
                    _marker.ItemPriceEnd = string.Empty;
                    _marker.BookingDetailEnd = string.Empty;
                    _marker.BookingNotes = (Bkit.BookingNote != null) ? Bkit.BookingNote.ToString() : string.Empty;
                    _marker.BookingsEnd = string.Empty;
                    _marker.Auth = (Bkit.BookingidIdentifier != null) ? Bkit.BookingidIdentifier.ToString() : string.Empty;
                    _marker.starttime = (Bkit.StartTime != null) ? Bkit.StartTime.ToString() : string.Empty;
                    _marker.endtime = (Bkit.EndTime != null) ? Bkit.EndTime.ToString() : string.Empty;

                    //   }

                    foreach (var _CoreMarkers in CoreMarkers)
                    {

                        switch (_CoreMarkers)
                        {
                            case "HostName":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.HostName);
                                break;
                            case "BookingsStart":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.BookingsStart);
                                break;
                            case "ClientNotes":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.ClientNotes);
                                break;
                            case "EmailSignature":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.EmailSignature);
                                break;
                            case "BookingID":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.BookingID);
                                break;
                            case "BookingAutoID":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.BookingAutoID);
                                break;
                            case "UserEmail":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.UserEmail);
                                break;
                            case "BookingDetailStart":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.BookingDetailStart);
                                break;
                            case "ItemCount":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.ItemCount);
                                break;
                            case "SupplierName":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.SupplierName);
                                break;
                            case "ItemName":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.ItemName);
                                break;
                            case "ItemDesc":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.ItemDesc);
                                break;
                            case "ItemStartText":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.ItemStartText);
                                break;
                            case "ItemStartDate":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.ItemStartDate);
                                break;
                            case "ItemEndDateStart":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.ItemEndDateStart);
                                break;
                            case "ItemEndText":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.ItemEndText);
                                break;
                            case "ItemEndDate":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.ItemEndDate);
                                break;
                            case "ItemLengthText":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.ItemLengthText);
                                break;
                            case "ItemLength":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.ItemLength);
                                break;
                            case "ItemEndDateEnd":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.ItemEndDateEnd);
                                break;
                            case "ItemQuantity":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.ItemQuantity);
                                break;
                            case "ItemReferenceStart":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.ItemReferenceStart);
                                break;
                            case "ItemReference":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.ItemReference);
                                break;
                            case "ItemReferenceEnd":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.ItemReferenceEnd);
                                break;
                            case "ItemPriceStart":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.ItemPriceStart);
                                break;
                            case "ItemPrice":
                                    EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.ItemPrice);
                                break;
                            case "ItemTotal":
                                    EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.ItemTotal);                   
                                break;
                            case "ItemPriceEnd":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.ItemPriceEnd);
                                break;
                            case "BookingDetailEnd":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.BookingDetailEnd);
                                break;
                            case "BookingNotes":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.BookingNotes);
                                break;
                            case "BookingsEnd":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.BookingsEnd);
                                break;
                            case "DivStart":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.DivStart);
                                break;
                            case "DivEnd":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.DivEnd);
                                break;
                            case "Auth":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.Auth);
                                break;
                            case "starttime":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.starttime);
                                break;
                            case "endtime":
                                EmailStrMarkers = EmailStrMarkers.Replace("##" + _CoreMarkers + "##", _marker.endtime);
                                break;


                        }
                    }
                    // Emailcontent.Add(EmailStrMarkers);
                    _marker.FinalEmailContent = EmailStrMarkers;
                    _marker.ItineraryID = Bkit.ItineraryID;
                    BookingReqDet.Add(_marker);

                }
            }
        }

        static string RemoveLineBreaksAndIndentation(string input)
        {
            // Remove line breaks and indentation (preserving spaces)
            return Regex.Replace(input, @"[\r\n\t]+", "");
        }

        private string AddNewTableRow(string filePath, string addString)
        {
            try
            {
                // Read the content of the file
                string fileContent = File.ReadAllText(filePath);

                // Remove whitespace, line breaks, and indentation from both strings
                fileContent = RemoveLineBreaksAndIndentation(fileContent);
                addString = RemoveLineBreaksAndIndentation(addString);

                // Check if the file contains the specified string
                if (!fileContent.Contains(addString))
                {
                    int position = fileContent.IndexOf("</table>");

                    if (position != -1)
                    {
                        // Insert the new content just before the </table> tag
                        StringBuilder sb = new StringBuilder(fileContent);
                        sb.Insert(position, addString);

                        // Write the modified content back to the file
                        File.WriteAllText(filePath, sb.ToString());

                        Console.WriteLine("New content added.");
                    }
                }

                LoadTemplate();
                // Return the updated content
                return File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Handle any errors that may occur during file manipulation
                return null; // Return null in case of an error
            }
        }

        private string RemoveTableRow(string filePath, string removeString)
        {
            try
            {
                // Read the content of the file
                string fileContent = File.ReadAllText(filePath);

                // Remove whitespace, line breaks, and indentation from both strings
                fileContent = RemoveLineBreaksAndIndentation(fileContent);
                removeString = RemoveLineBreaksAndIndentation(removeString);

                // Check if the file contains the specified string
                if (fileContent.Contains(removeString))
                {
                    // Remove the specified string from the content
                    fileContent = fileContent.Replace(removeString, "");

                    // Write the modified content back to the file
                    File.WriteAllText(filePath, fileContent);

                    Console.WriteLine("Content removed.");
                }

                LoadTemplate();
                // Return the updated content
                return File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Handle any errors that may occur during file manipulation
                return null; // Return null in case of an error
            }
        }

    }

}
