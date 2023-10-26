using LIT.Commands;
using LIT.Modules.TabControl.Commands;
using Prism.Mvvm;
using SQLDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIT.ViewModels
{
    public class GlobalSearchViewModel: BindableBase
    {

        #region Private field
        private readonly GlobalSearchCommand _GlobalSearchCommands;

        private ObservableCollection<GlobalSearchItinerary> _GlobalSearchItineraryCollection;

        private ObservableCollection<GlobalSearchfilters> _GlobalSearchFiltercollection;
        private ObservableCollection<GlobalSearchContact> _GlobalSearchContactCollection;
        private ObservableCollection<GlobalSearchBooking> _GlobalSearchBookingCollection;
        private ObservableCollection<GlobalSearchSupplier> _GlobalSearchSupplierCollection;

        private GlobalSearchItinerary _GlobalSearchItinerarymodel;

        private string _itineraryId;

        private string _loginId;

        private string _userName;

        private string _Listselecteditem;

        private string _SearchField;
        private string _SearchWhere;
        private string _SearchKeyText;
        #endregion

        #region ctor

        public GlobalSearchViewModel()
        {
            _GlobalSearchCommands = new GlobalSearchCommand(this);
            _GlobalSearchItineraryCollection = new ObservableCollection<GlobalSearchItinerary>();
            _GlobalSearchSupplierCollection = new ObservableCollection<GlobalSearchSupplier>();
            _GlobalSearchBookingCollection = new ObservableCollection<GlobalSearchBooking>();
            _GlobalSearchContactCollection = new ObservableCollection<GlobalSearchContact>();
        }
        #endregion

        #region public propterty

        public GlobalSearchCommand GlobalSearchCommands => _GlobalSearchCommands;
        public ObservableCollection<GlobalSearchItinerary> GlobalSearchItineraryCollection
        {
            get { return _GlobalSearchItineraryCollection; }
            set
            {
                SetProperty(ref _GlobalSearchItineraryCollection, value);
                _GlobalSearchCommands.RaiseCanExecuteChanged();
            }
        }
        public ObservableCollection<GlobalSearchSupplier> GlobalSearchSupplierCollection
        {
            get { return _GlobalSearchSupplierCollection; }
            set
            {
                SetProperty(ref _GlobalSearchSupplierCollection, value);
                _GlobalSearchCommands.RaiseCanExecuteChanged();
            }
        }      
        public ObservableCollection<GlobalSearchBooking> GlobalSearchBookingCollection
        {
            get { return _GlobalSearchBookingCollection; }
            set
            {
                SetProperty(ref _GlobalSearchBookingCollection, value);
                _GlobalSearchCommands.RaiseCanExecuteChanged();
            }
        }
        public ObservableCollection<GlobalSearchContact> GlobalSearchContactCollection
        {
            get { return _GlobalSearchContactCollection; }
            set
            {
                SetProperty(ref _GlobalSearchContactCollection, value);
                _GlobalSearchCommands.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<GlobalSearchfilters> GlobalSearchFiltercollection
        {
            get { return _GlobalSearchFiltercollection; }
            set
            {
                SetProperty(ref _GlobalSearchFiltercollection, value);
                _GlobalSearchCommands.RaiseCanExecuteChanged();
            }
        }

        public GlobalSearchItinerary GlobalSearchItinerarymodel
        {
            get { return _GlobalSearchItinerarymodel; }
            set
            {
                SetProperty(ref _GlobalSearchItinerarymodel, value);
                _GlobalSearchCommands.RaiseCanExecuteChanged();
            }
        }

        public string LoginId
        {
            get { return _loginId; }
            set
            {
                SetProperty(ref _loginId, value);
                _GlobalSearchCommands.RaiseCanExecuteChanged();
            }

        }

        public string ItineraryId
        {
            get { return _itineraryId; }
            set
            {
                SetProperty(ref _itineraryId, value);
                _GlobalSearchCommands.RaiseCanExecuteChanged();
                _GlobalSearchCommands.RetrieveCommand.Execute();
            }
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                SetProperty(ref _userName, value);
                _GlobalSearchCommands.RaiseCanExecuteChanged();
                //_commentsTabCommands.RetrieveCommand.Execute();
            }
        }
        public string SearchField
        {
            get { return _SearchField; }
            set
            {
                SetProperty(ref _SearchField, value);
                _GlobalSearchCommands.RaiseCanExecuteChanged();
                //_commentsTabCommands.RetrieveCommand.Execute();
            }
        }        
        public string SearchWhere
        {
            get { return _SearchWhere; }
            set
            {
                SetProperty(ref _SearchWhere, value);
                _GlobalSearchCommands.RaiseCanExecuteChanged();
                //_commentsTabCommands.RetrieveCommand.Execute();
            }
        }
        public string SearchKeyText
        {
            get { return _SearchKeyText; }
            set
            {
                SetProperty(ref _SearchKeyText, value);
                _GlobalSearchCommands.RaiseCanExecuteChanged();
                //_commentsTabCommands.RetrieveCommand.Execute();
            }
        }
        public string Listselecteditem
        {
            get { return _Listselecteditem; }
            set
            {
                SetProperty(ref _Listselecteditem, value);
                _GlobalSearchCommands.RaiseCanExecuteChanged();
            }

        }

        #endregion

    }

}
