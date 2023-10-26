using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SQLDataAccessLayer.Models
{
    public class EmailModel
    {

    }
    public class BookingEmailRequestDetails : INotifyPropertyChanged
    {
        public string HostName { get; set; }
        public string BookingsStart { get; set; }
        public string BookingDetailStart { get; set; }
        public string BookingID { get; set; }
        public string UserEmail { get; set; }
        public string ItemCount { get; set; }
        public string SupplierName { get; set; }
        public string ItemName { get; set; }
        public string ItemDesc { get; set; }
        public string ItemStartText { get; set; }
        public string ItemStartDate { get; set; }
        public string ItemEndDateStart { get; set; }
        public string ItemEndText { get; set; }
        public string ItemEndDate { get; set; }
        public string ItemLengthText { get; set; }
        public string ItemLength { get; set; }
        public string ItemEndDateEnd { get; set; }
        public string ItemQuantity { get; set; }



        public string ItemReferenceStart { get; set; }
        public string ItemReference { get; set; }
        public string ItemReferenceEnd { get; set; }
        public string ItemPriceStart { get; set; }
        public string ItemPrice { get; set; }

        public string ItemTotal { get; set; }
        public string ItemPriceEnd { get; set; }
        public string BookingDetailEnd { get; set; }
        public string BookingNotes { get; set; }

        public string BookingsEnd { get; set; }
        public string ClientNotes { get; set; }
        public string EmailSignature { get; set; }

        public string FinalEmailContent { get; set; }
        public string FileAttachName { get; set; }

        public string ItineraryID { get; set; }

        private string _FileAttachPath;
        public string FileAttachPath
        {
            get { return _FileAttachPath; }
            set
            {
                _FileAttachPath = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("FileAttachPath"));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                //  handler(this, new PropertyChangedEventArgs(e));
                this.PropertyChanged(this, e);
            }
        }

        public string Emailsubject { get; set; }
        public string DivStart { get; set; }

        public string DivEnd { get; set; }

        public string Auth { get; set; }

        public string BookingAutoID { get; set; }

        public string starttime { get; set; }
        public string endtime { get; set; } 

    }

    public class BookingEmailSend:INotifyPropertyChanged
    {
        public string ItineraryID { get; set; }
        
        public long BookingID { get; set; }
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }        

        public bool Selected { get; set; }
        private string _Status;
        public string Status
        {
            get { return _Status; }
            set
            {
                _Status = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("Status"));
            }
        }
        public string Subject { get; set; }
        public string FileAttachName { get; set; }
        

        private string _Error;
        public string Error
        {
            get { return _Error; }
            set
            {
                _Error = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("Error"));
            }
        }
        public string Message { get; set; }

        
        private string _FileAttachPath;
        public string FileAttachPath
        {
            get { return _FileAttachPath; }
            set
            {
                _FileAttachPath = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("FileAttachPath"));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                //  handler(this, new PropertyChangedEventArgs(e));
                this.PropertyChanged(this, e);
            }
        }

    }

    public class BookingEmail
    {

        public string FromEmail { get; set; }
        public string BccEmail { get; set; }
        public string Subject { get; set; }
        public string To { get; set; }
        public string status { get; set; }
        public string isattachavailable { get; set; }
        public string attach { get; set; }
        public string error { get; set; }
        public string templatepath { get; set; }


        public string Validdateemail(string emailaddress, string cntrlname)
        {
            string retn = string.Empty;
            if (emailaddress.Length == 0)
            {
                if (cntrlname.ToString().ToLower() == "from")
                {
                    retn = "Please provide an From Email Address";
                }
                if (cntrlname.ToString().ToLower() == "bcc")
                {
                    retn = "Please provide an Bcc Email Address";
                }
                if (cntrlname.ToString().ToLower() == "to")
                {
                    retn = "Please provide an To Email Address";
                }
                return retn;
            }

            if (emailaddress.Length > 0)
            {
                if (cntrlname.ToString().ToLower() == "to")
                {
                    if (emailaddress.Contains("<"))
                    {
                        emailaddress = emailaddress.Split("<")[1].Replace(">","");
                    }
                }

                if ((!Regex.IsMatch(emailaddress, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$")))
                {
                    if (cntrlname.ToString().ToLower() == "from")
                    {
                        retn = "Please provide a valid From Email Address";
                    }
                    if (cntrlname.ToString().ToLower() == "bcc")
                    {
                        retn = "Please provide a valid Bcc Email Address";
                    }
                    if (cntrlname.ToString().ToLower() == "to")
                    {
                        retn = "Please provide a valid To Email Address";
                    }

                    return retn;
                }
            }
            return retn;
        }

    }

    public class MailCredentials
    {        
        public string SMTPServer { get; set; }
        public int SMTPPort { get; set; }
        public bool IsSSLRequired { get; set; }
        public string FromEmail { get; set; }
        public string FromEmailName { get; set; }
        public string FromEmailPassword { get; set; }
        public string BCCEmail { get; set; }
    }

    
}
