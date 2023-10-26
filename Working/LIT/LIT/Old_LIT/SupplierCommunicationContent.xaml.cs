using Microsoft.EntityFrameworkCore.Migrations.Operations;
using SQLDataAccessLayer.DAL;
using SQLDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
    /// Interaction logic for SupplierCommunicationContent.xaml
    /// </summary>
    public partial class SupplierCommunicationContent : Window
    {
        string contentnamefor=string.Empty;
        SQLDataAccessLayer.DAL.SupplierDAL objsupdal = new SQLDataAccessLayer.DAL.SupplierDAL();
        LoadDropDownListValues loadDropDownListValues = new LoadDropDownListValues();
        string loginusername = string.Empty;
        string loginuserid = string.Empty;
        string recordmode = string.Empty;
        Errorlog errobj = new Errorlog();
        string supplieridval=string.Empty;
        string serviceidval = string.Empty;
        Supplier supwindow;
        public SupplierCommunicationContent()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public SupplierCommunicationContent(string contentname,string supplierid,string serviceid,Supplier supwn)
        {            
            InitializeComponent();
            this.DataContext = this;
            contentnamefor = contentname;
            txtcontentname.Text = contentname;
            supplieridval= supplierid;
            supwindow= supwn;
            serviceidval=serviceid;
            var observablecollection = new ObservableCollection<SupplierCommunicationContentdata>(supwindow.SuppCommContentDatainfo.Distinct().ToList());            
            SuppCommContenDt = observablecollection;
            LoadContentDetails(supplieridval);
        }
        private ObservableCollection<SupplierCommunicationContentdata> _SuppCommContenDt;
        public ObservableCollection<SupplierCommunicationContentdata> SuppCommContenDt
        {
            get { return _SuppCommContenDt ?? (_SuppCommContenDt = new ObservableCollection<SupplierCommunicationContentdata>()); }
            set
            {
                _SuppCommContenDt = value;
            }
        }
        private void LoadContentDetails(string SupplierIdval)
        {
           
             try
             {
                if (SupplierIdval != "")
                {
                        List<SupplierCommunicationContentdata> lstssm = new List<SupplierCommunicationContentdata>();
                        lstssm = objsupdal.SupplierCommunicationContentRetrive(Guid.Parse(SupplierIdval));
                        if (lstssm != null && lstssm.Count > 0)
                        {                            
                            foreach (SupplierCommunicationContentdata scn in lstssm)
                            {
                                if (SuppCommContenDt.Where(x => x.ContentID == scn.ContentID).Count() == 0)
                                {                                    
                                    SuppCommContenDt.Add(scn);                                   
                                }
                            }
                        }
                    DgSuppCommContent.ItemsSource = SuppCommContenDt;
                }
             }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierCommunicationContent", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

            

            //throw new NotImplementedException();
        }

        private void BtnAddSuppCommContent_Click(object sender, RoutedEventArgs e)
        {
            AddItemforCommunicationcontent();
            //supwindow.SuppCommContentDatainfo = SuppCommContenDt;
            supwindow.ReteriveSupplierCommunicationContent();            
            this.Close();
        }

        private void AddItemforCommunicationcontent()
        {
            SupplierCommunicationContentdata sccd;
            sccd = new SupplierCommunicationContentdata();
            sccd.SupplierID = supplieridval;
            sccd.ContentFor = contentnamefor;
            sccd.ServiceID= serviceidval;
            sccd.ContentName = "";// "New Content" + " (" + (SuppCommContenDt.Count + 1) + ")";
            supwindow.txtcontentnamevalue.Text = "";//"New Content" + " (" + (SuppCommContenDt.Count + 1) + ")";
            sccd.ContentID = (Guid.NewGuid()).ToString();
            sccd.IsLastadded = true;
            supwindow.SuppCommContentDatainfo.Add(sccd);            
        }

        

        private void btncommcontcancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DgSuppCommContentDataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CommunicationcontentSelect();
        }

        private void CommunicationcontentSelect()
        {
            try
            {

                SupplierCommunicationContentdata ssmobj = DgSuppCommContent.SelectedItem as SupplierCommunicationContentdata;
                if (ssmobj.ContentID != null)
                {
                    
                    ssmobj.IsselectedContent = true;
                    SupplierCommunicationContentdata sccd;
                    sccd = new SupplierCommunicationContentdata();
                    sccd.SupplierID = ssmobj.SupplierID;
                    sccd.IsselectedContent = true;
                    sccd.ContentID = ssmobj.ContentID;
                    sccd.ContentName = ssmobj.ContentName;
                    sccd.ContentFor = ssmobj.ContentFor;
                    sccd.BodyHtml = ssmobj.BodyHtml;
                    sccd.ContentType = ssmobj.ContentType;
                    sccd.Heading = ssmobj.Heading;
                    sccd.OnlineImage = ssmobj.OnlineImage;
                    sccd.ReportImage = ssmobj.ReportImage;
                    sccd.ServiceID = ssmobj.ServiceID;
                    sccd.SelectedItemcontentype = ssmobj.SelectedItemcontentype;
                    sccd.IsLastadded = true;
                    supwindow.SuppCommContentDatainfo.Add(sccd);
                    supwindow.ReteriveSupplierCommunicationContent();
                    this.Close();
                    // SuppCommContenDt.Where(x => x.ContentID == ssmobj.ContentID).FirstOrDefault().IsselectedContent = true;
                    //AddItemforCommunicationcontent();
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierCommunicationContent", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }
    }
}
