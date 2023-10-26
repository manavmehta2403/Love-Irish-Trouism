using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SQLDataAccessLayer.DAL;
using System.Collections.ObjectModel;
using SQLDataAccessLayer.Models;
using LIT.ViewModels;
using System.Reflection;
using LIT.Commands;
using LIT.Old_LIT;
using System.Text.RegularExpressions;

namespace LIT.Views
{
    /// <summary>
    /// Interaction logic for GlobalSearch.xaml
    /// </summary>
    public partial class GlobalSearch : UserControl
    {
        private readonly GlobalSearchdal _GlobalSearchdal;
        private readonly GlobalSearchViewModel _viewModel;
        private readonly GlobalSearchCommand _GlobalSearchcmd;
        private ContactsViewModel _viewModelcontact;        
        private LIT.Old_LIT.MainWindow _parentWindow;
        Errorlog errobj = new Errorlog();
        public GlobalSearch()
        {
            InitializeComponent();
        }
        public GlobalSearch(LIT.Old_LIT.MainWindow ParentWindow)
        {
            
            _GlobalSearchdal = new GlobalSearchdal();
            _viewModel = new GlobalSearchViewModel();
            _GlobalSearchcmd = new GlobalSearchCommand(_viewModel);
            InitializeComponent();            
            _parentWindow=ParentWindow;
            cmblistval.SelectedIndex = 0;
            cmbfiltervalues.SelectedIndex = 0;

        }

        private void cmblistval_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel != null && _GlobalSearchdal != null)
            {
                if (cmblistval.SelectedValue != null)
                {
                    List<GlobalSearchfilters> objlist = new List<GlobalSearchfilters>();
                    string moduleval = ((System.Windows.Controls.ContentControl)cmblistval.SelectedValue).Content.ToString();
                    objlist = _GlobalSearchdal.GlobalSearchFilters(moduleval);
                    // Use the Dispatcher to update the UI collection on the UI thread
                    string defaultselected = string.Empty;
                    if (moduleval.ToLower() == "itineraries")
                    {
                        cmbSearchField.ItemsSource = objlist[0].filtersItineraries.ToList();
                        defaultselected = objlist[0].filtersItineraries.ToList().Where(x => x.Contains("Itinerary Name")).FirstOrDefault();
                        if(!string.IsNullOrEmpty(defaultselected))
                        cmbSearchField.SelectedValue = defaultselected.ToString();


                    }
                    if (moduleval.ToLower() == "suppliers")
                    {
                        cmbSearchField.ItemsSource = objlist[0].filtersSuppliers.ToList();
                        defaultselected = objlist[0].filtersSuppliers.ToList().Where(x => x.Contains("Supplier Name")).FirstOrDefault();
                        if (!string.IsNullOrEmpty(defaultselected))
                            cmbSearchField.SelectedValue = defaultselected.ToString();
                    }
                    if (moduleval.ToLower() == "contacts")
                    {
                        cmbSearchField.ItemsSource = objlist[0].filtersContacts.ToList();
                        defaultselected = objlist[0].filtersContacts.ToList().Where(x => x.Contains("Contact Name")).FirstOrDefault();
                        if (!string.IsNullOrEmpty(defaultselected))
                            cmbSearchField.SelectedValue = defaultselected.ToString();
                    }
                    if (moduleval.ToLower() == "bookings")
                    {
                        cmbSearchField.ItemsSource = objlist[0].filtersBookings.ToList();
                        defaultselected = objlist[0].filtersBookings.ToList().Where(x => x.Contains("Booking Name")).FirstOrDefault();
                        if (!string.IsNullOrEmpty(defaultselected))
                            cmbSearchField.SelectedValue = defaultselected.ToString();
                    }

                    // _viewModel.GlobalSearchFiltercollection.AddRange(objlist);

                }
            }
        }

        private void btnsearchgs_Click(object sender, RoutedEventArgs e)
        {
            GlobalSearchresult();
        }

        private void GlobalSearchresult()
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            if (!string.IsNullOrEmpty(TxtSearchval.Text))
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                try
                {
                    if (TxtSearchval.Text.Length < 0)
                    {
                        MessageBox.Show("Please provide atleast one character to search...!");
                        TxtSearchval.Focus();
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                        return;
                    }
                    //if (Regex.IsMatch(TxtSearchval.Text.ToString(), @"[\#^*|\"":<>[\]{}`\\()';@&$]"))
                    //{
                    //    if (cmbSearchField.SelectedValue.ToString().ToLower() == "email")
                    //    {

                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show("No special characters on the Phrase are allowed");
                    //        TxtSearchval.Focus();
                    //        Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                    //        return;
                    //    }
                    //}
                    //else
                    //{
                        if (cmblistval.SelectedValue != null)
                        {
                            string moduleval = ((System.Windows.Controls.ContentControl)cmblistval.SelectedValue).Content.ToString();
                            string SearchField = cmbSearchField.SelectedValue.ToString();
                            string SearchWhere = ((System.Windows.Controls.ContentControl)cmbfiltervalues.SelectedValue).Content.ToString();
                            string SearchKeyText = TxtSearchval.Text.ToString();
                            if (SearchKeyText.Contains("'"))
                            {
                                SearchKeyText = SearchKeyText.Replace("'", "''");
                            }
                            _viewModel.Listselecteditem = moduleval.Trim();
                            _viewModel.SearchField = SearchField.Trim();
                            _viewModel.SearchWhere = SearchWhere.Trim();
                            _viewModel.SearchKeyText = SearchKeyText.Trim();
                            if (moduleval.ToLower() == "itineraries")
                            {
                                _GlobalSearchcmd.RetrieveCommand.Execute();
                                DGItinearyresult.ItemsSource = _viewModel.GlobalSearchItineraryCollection.ToList();
                                ItineraryBorder.Visibility = Visibility.Visible;
                                SupplierBorder.Visibility = Visibility.Collapsed;
                                ContactBorder.Visibility = Visibility.Collapsed;
                                BookingBorder.Visibility = Visibility.Collapsed;
                            }
                            if (moduleval.ToLower() == "suppliers")
                            {
                                _GlobalSearchcmd.RetrieveCommand.Execute();
                                DGSupplierresult.ItemsSource = _viewModel.GlobalSearchSupplierCollection.ToList();
                                SupplierBorder.Visibility = Visibility.Visible;
                                ItineraryBorder.Visibility = Visibility.Collapsed;
                                ContactBorder.Visibility = Visibility.Collapsed;
                                BookingBorder.Visibility = Visibility.Collapsed;
                            }
                            if (moduleval.ToLower() == "contacts")
                            {
                                _GlobalSearchcmd.RetrieveCommand.Execute();
                                DGContactresult.ItemsSource = _viewModel.GlobalSearchContactCollection.ToList();
                                ContactBorder.Visibility = Visibility.Visible;
                                ItineraryBorder.Visibility = Visibility.Collapsed;
                                SupplierBorder.Visibility = Visibility.Collapsed;
                                BookingBorder.Visibility = Visibility.Collapsed;
                            }
                            if (moduleval.ToLower() == "bookings")
                            {
                                _GlobalSearchcmd.RetrieveCommand.Execute();
                                DGBookingresult.ItemsSource = _viewModel.GlobalSearchBookingCollection.ToList();
                                BookingBorder.Visibility = Visibility.Visible;
                                ItineraryBorder.Visibility = Visibility.Collapsed;
                                SupplierBorder.Visibility = Visibility.Collapsed;
                                ContactBorder.Visibility = Visibility.Collapsed;

                            }

                            // _viewModel.GlobalSearchFiltercollection.AddRange(objlist);

                        }
                   // }
                }
                catch(Exception ex)
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                    errobj.WriteErrorLoginfo("GlobalSearch", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
                }
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }
            else
            {
                MessageBox.Show("Please provide the phrase to search...!");
                TxtSearchval.Focus();
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                return;
            }
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }
        private void btnclear_Click(object sender, RoutedEventArgs e)
        {
            cmblistval.SelectedIndex = 0;
            cmbfiltervalues.SelectedIndex = 0;
            TxtSearchval.Text = string.Empty;
            BookingBorder.Visibility = Visibility.Collapsed;
            ItineraryBorder.Visibility = Visibility.Collapsed;
            SupplierBorder.Visibility = Visibility.Collapsed;
            ContactBorder.Visibility = Visibility.Collapsed;
        }

        private void DGItinearyresultCell_SelectedClick(object sender, RoutedEventArgs e)
        {
            var dataGridCellTarget = (DataGridCell)sender;

            GlobalSearchItinerary objGSI = DGItinearyresult.SelectedItem as GlobalSearchItinerary;

            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            try
            {
                if (objGSI != null)
                {
                    if (!string.IsNullOrEmpty(objGSI.ItineraryID))
                    {
                        TreeviewAccordion tracc = new TreeviewAccordion();
                        NodeViewModel snvm = tracc.ItineraryTreeViewModelTr.Items.FirstOrDefault();
                        if (snvm != null)
                        {
                            snvm.IsSelectedItinerary = true;
                            snvm.IsExpandedItinerary = true;
                            if (snvm.Children.Where(x => x.Id == objGSI.ItineraryID).ToList().Count > 0)
                            {
                                snvm.Children.Where(x => x.Id == objGSI.ItineraryID).FirstOrDefault().IsSelectedItinerary = true;
                                snvm.Children.Where(x => x.Id == objGSI.ItineraryID).FirstOrDefault().IsExpandedItinerary = true;
                            }
                            _parentWindow.trview.ItemsSource = tracc.ItineraryTreeViewModelTr.Items;
                            _parentWindow.AddItineraryTabDetails(objGSI.ItineraryID, "GS");
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                errobj.WriteErrorLoginfo("GlobalSearch", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            DGItinearyresult.SelectedItem = null;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        private void DGBookingresultCell_SelectedClick(object sender, RoutedEventArgs e)
        {
            var dataGridCellTarget = (DataGridCell)sender;

            GlobalSearchBooking objGSI = DGBookingresult.SelectedItem as GlobalSearchBooking;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            try
            {
                if (objGSI != null)
                {
                    if (!string.IsNullOrEmpty(objGSI.ItineraryId))
                    {
                        TreeviewAccordion tracc = new TreeviewAccordion();
                        NodeViewModel snvm = tracc.ItineraryTreeViewModelTr.Items.FirstOrDefault();
                        if (snvm != null)
                        {
                            snvm.IsSelectedItinerary = true;
                            snvm.IsExpandedItinerary = true;
                            if (snvm.Children.Where(x => x.Id == objGSI.ItineraryId).ToList().Count > 0)
                            {
                                snvm.Children.Where(x => x.Id == objGSI.ItineraryId).FirstOrDefault().IsSelectedItinerary = true;
                                snvm.Children.Where(x => x.Id == objGSI.ItineraryId).FirstOrDefault().IsExpandedItinerary = true;
                            }
                            _parentWindow.trview.ItemsSource = tracc.ItineraryTreeViewModelTr.Items;
                            _parentWindow.AddItineraryTabDetails(objGSI.ItineraryId, "GS");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                errobj.WriteErrorLoginfo("GlobalSearch", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            DGBookingresult.SelectedItem = null;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        private void DGSupplierresultCell_SelectedClick(object sender, RoutedEventArgs e)
        {
            var dataGridCellTarget = (DataGridCell)sender;

            GlobalSearchSupplier objGSS = DGSupplierresult.SelectedItem as GlobalSearchSupplier;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            try { 
            if (objGSS != null)
            {
                if (!string.IsNullOrEmpty(objGSS.SupplierId))
                {
                    TreeviewAccordion tracc = new TreeviewAccordion();
                    SupplierNodeViewModel snvm = tracc.SupplierTreeViewModel.SupplierItems.FirstOrDefault();
                    if (snvm != null)
                    {
                        snvm.IsSelectedsupplier = true;
                        snvm.IsExpandedsupplier = true;
                        if (snvm.SupplierChildren.Where(x => x.SupplierId == objGSS.SupplierId).ToList().Count > 0)
                        {
                            snvm.SupplierChildren.Where(x => x.SupplierId == objGSS.SupplierId).FirstOrDefault().IsSelectedsupplier = true;
                            snvm.SupplierChildren.Where(x => x.SupplierId == objGSS.SupplierId).FirstOrDefault().IsExpandedsupplier = true;
                        }
                        _parentWindow.trviewsupplier.ItemsSource = tracc.SupplierTreeViewModel.SupplierItems;                        
                        _parentWindow.AddSupplierTabDetails(objGSS.SupplierId, "GS");
                    }


                }
            }
            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                errobj.WriteErrorLoginfo("GlobalSearch", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            DGSupplierresult.SelectedItem = null;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }
        
        private void DGContactresultCell_SelectedClick(object sender, RoutedEventArgs e)
        {
            var dataGridCellTarget = (DataGridCell)sender;            
            
            GlobalSearchContact objGSCC= DGContactresult.SelectedItem as GlobalSearchContact;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

            
            

            try
            {
                if (objGSCC != null)
                {
                    if (!string.IsNullOrEmpty(objGSCC.ContactId))
                    {
                        Contacts contacts = new Contacts();
                        ContactsViewModel viewModel = new ContactsViewModel(_parentWindow, objGSCC.ContactId);
                        contacts.DataContext = viewModel;
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                        contacts.ShowDialog();
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                errobj.WriteErrorLoginfo("GlobalSearch", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        private void TxtSearchval_KeyUp(object sender, KeyEventArgs e)
        {
          if (e.Key != System.Windows.Input.Key.Enter) return;
            e.Handled = true;
            if (e.Key == Key.Enter)
            {
                GlobalSearchresult();
            }
        }
    }
}
