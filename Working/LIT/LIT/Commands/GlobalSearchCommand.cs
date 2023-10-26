using LIT.Core.Mvvm;
using LIT.Modules.TabControl.Commands;
using LIT.Modules.TabControl.ViewModels;
using LIT.ViewModels;
using Prism.Commands;
using SQLDataAccessLayer.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using SQLDataAccessLayer.DAL; 
using System.Collections.ObjectModel;
using SQLDataAccessLayer.Models;

namespace LIT.Commands
{
    public class GlobalSearchCommand: CrudOperations<GlobalSearchCommand>
    {

        private readonly GlobalSearchViewModel _viewModel;
        private readonly GlobalSearchdal _GlobalSearchdal;
        public DelegateCommand RetrieveCommand { get; set; }
        public DelegateCommand ListSelectionChangedCommand { get; set; }
        public GlobalSearchCommand(GlobalSearchViewModel viewModel)
            : base("Add", "Delete", "Retrieve", "Save")
        {
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            _GlobalSearchdal = new GlobalSearchdal();

            _viewModel.PropertyChanged += ViewModel_PropertyChanged;

           
            RetrieveCommand = new DelegateCommand(ExecuteRetrieve);
           // ListSelectionChangedCommand = new DelegateCommand(Executelistchange);

        }

        public new event EventHandler CanExecuteChanged;

        protected override void ExecuteRetrieve()
        { 
            if (!string.IsNullOrEmpty(_viewModel.Listselecteditem))
            {
                string SearchFieldnospace = string.Empty;

                if (_viewModel.SearchField.Contains(" "))
                {
                    SearchFieldnospace = _viewModel.SearchField.Replace(" ", "");
                }
                else
                {
                    SearchFieldnospace = _viewModel.SearchField;
                }
                if (_viewModel.Listselecteditem.ToLower() == "itineraries")
                {
                    if(SearchFieldnospace.ToLower()== "itineraryid")
                    {
                        SearchFieldnospace = "ItineraryAutoID";
                    }
                     var ItineraryCollection = _GlobalSearchdal.GlobalSearchItinerary(SearchFieldnospace, _viewModel.SearchWhere, _viewModel.SearchKeyText);
                    _viewModel.GlobalSearchItineraryCollection.Clear();
                    _viewModel.GlobalSearchItineraryCollection.AddRange(ItineraryCollection);
                }
                if (_viewModel.Listselecteditem.ToLower() == "suppliers")
                {
                    if (SearchFieldnospace.ToLower() == "supplierid")
                    {
                        SearchFieldnospace = "SupplierAutoId";
                    }
                    var SuppliersCollection = _GlobalSearchdal.GlobalSearchSupplier(SearchFieldnospace, _viewModel.SearchWhere, _viewModel.SearchKeyText);
                    _viewModel.GlobalSearchSupplierCollection.Clear();
                    _viewModel.GlobalSearchSupplierCollection.AddRange(SuppliersCollection);
                }
                if (_viewModel.Listselecteditem.ToLower() == "contacts")
                {
                    var ContactsCollection = _GlobalSearchdal.GlobalSearchContact(SearchFieldnospace, _viewModel.SearchWhere, _viewModel.SearchKeyText);
                    _viewModel.GlobalSearchContactCollection.Clear();
                    _viewModel.GlobalSearchContactCollection.AddRange(ContactsCollection);
                }
                if (_viewModel.Listselecteditem.ToLower() == "bookings")
                {
                    var bookingsCollection = _GlobalSearchdal.GlobalSearchBooking(SearchFieldnospace, _viewModel.SearchWhere, _viewModel.SearchKeyText);
                    _viewModel.GlobalSearchBookingCollection.Clear();
                    _viewModel.GlobalSearchBookingCollection.AddRange(bookingsCollection);
                }
                
            }
        }
        //protected void Executelistchange()
        //{
        //    Guid itineraryId = Guid.Parse(_viewModel.ItineraryId);
        //    List<GlobalSearchfilters> objlist = new List<GlobalSearchfilters>();
        //    if (itineraryId != Guid.Empty)
        //    {
        //        objlist = _GlobalSearchdal.GlobalSearchFilters(_viewModel.Listselecteditem);
        //        // Use the Dispatcher to update the UI collection on the UI thread
        //        _viewModel.GlobalSearchFiltercollection.AddRange(objlist);

        //    }
        //}

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

    }
}
