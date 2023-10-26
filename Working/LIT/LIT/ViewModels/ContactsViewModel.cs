using LIT.Commands;
using LIT.Old_LIT;
using LITModels.LITModels.Models;
using Prism.Commands;
using Prism.Mvvm;
using SQLDataAccessLayer.DAL;
using SQLDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LIT.ViewModels
{
    public class ContactsViewModel : BindableBase
    {
        #region private field
        private ContactModel _contactInfo;
        private ContactsCommands _contactsCommands;
        private List<string> _contactGender;
        private Guid _userId;
        private Guid _contactId;
        MainWindow _parentWindow;
        private bool _isFormSave;
        #endregion

        #region ctor
        public ContactsViewModel(MainWindow ParentWindow,string Id = null)
        {
            _contactsCommands = new ContactsCommands(this);
            _contactGender = new List<string>();
            _contactGender.Add("Male");
            _contactGender.Add("Female");
            _contactGender.Add("Others");

            _parentWindow = ParentWindow;
            if(!string.IsNullOrEmpty(Id)) 
            {
                _contactId = Guid.Parse(Id);
                _contactsCommands.RetrieveCommand.Execute();
            }
        }
        #endregion

        #region public proptery

        public ContactsCommands ContactsCommands => _contactsCommands;

        public List<string> ContactGenderList
        {
            get { return _contactGender; }
            set
            {
                SetProperty(ref _contactGender, value);
                _contactsCommands.RaiseCanExecuteChanged();
            }
        }

        public ContactModel contactInfo
        {
            get { return _contactInfo; }
            set
            {
                SetProperty(ref _contactInfo, value);
                _contactsCommands.RaiseCanExecuteChanged();
            }
        }
        
        public Guid ContactId
        {
            get { return _contactId; }
            set
            {
                SetProperty(ref _contactId, value);
                _contactsCommands.RaiseCanExecuteChanged();
            }
        }
        
        public MainWindow MainWindow
        {
            get { return _parentWindow; }
            set
            {
                SetProperty(ref _parentWindow, value);
            }
        }
        
        public Guid UserId
        {
            get { return _userId; }
            set
            {
                SetProperty(ref _userId, value);
                _contactsCommands.RaiseCanExecuteChanged();
            }
        }
        public bool IsFormSave
        {
            get { return _isFormSave; }
            set
            {
                SetProperty(ref _isFormSave, value);
                _contactsCommands.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region model proptery

        private int _countryIndex;
        private int _stateIndex;
        private int _cityIndex;
        private int _regionIndex;
        private int _contactTypeIndex;
        private int _contactRoleIndex;

        public int countryIndex
        {
            get { return _countryIndex; }
            set
            {
                if (_countryIndex != value && value != -1)
                {
                    SetProperty(ref _countryIndex, value);
                    _contactsCommands.RaiseCanExecuteChanged();
                }
            }
        }

        public int stateIndex
        {
            get { return _stateIndex; }
            set
            {
                if (_stateIndex != value && value != -1)
                {
                    SetProperty(ref _stateIndex, value);
                    _contactsCommands.RaiseCanExecuteChanged();
                }
            }
        }

        public int cityIndex
        {
            get { return _cityIndex; }
            set
            {
                if (_cityIndex != value && value != -1)
                {
                    SetProperty(ref _cityIndex, value);
                    _contactsCommands.RaiseCanExecuteChanged();
                }
            }
        }

        public int regionIndex
        {
            get { return _regionIndex; }
            set
            {
                if (_regionIndex != value && value != -1)
                {
                    SetProperty(ref _regionIndex, value);
                    _contactsCommands.RaiseCanExecuteChanged();
                }
            }
        }
        
        public int ContactTypeIndex
        {
            get { return _contactTypeIndex; }
            set
            {
                if (_contactTypeIndex != value)
                {
                    _contactTypeIndex = value;
                    _contactsCommands.RaiseCanExecuteChanged();
                }
            }
        }
        
        public int ContactrRoleIndex
        {
            get { return _contactRoleIndex; }
            set
            {
                if (_contactRoleIndex != value)
                {
                    _contactRoleIndex = value;
                    _contactsCommands.RaiseCanExecuteChanged();
                }
            }
        }

        public int genderIndex { get; set; }    

        public Guid ContactTitle { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string UserName { get; set; }
        public string PhoneWork { get; set; }
        public string PhoneHome { get; set; }
        public string mobile { get; set; }
        public string Fax { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string CompanyName { get; set; }
        public string CompanyPosition { get; set; }
        public string CompanyDescription { get; set; }
        public string EmailOne { get; set; }
        public string EmailTwo { get; set; }
        public string Website { get; set; }
        public string ContactTypeId { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string ContactAutoid { get; set; }
        public contacttype ContactType { get; set; }
        public Guid ContactRole { get; set; }

        private CommonValueCountrycity _city;
        private CommonValueCountrycity _region;
        private CommonValueCountrycity _state;
        private CommonValueCountrycity _country;

        public CommonValueCountrycity City
        {
            get { return _city; }
            set
            {
                if (_city != value && value != null)
                {
                    SetProperty(ref _city, value);
                    _contactsCommands.RaiseCanExecuteChanged();
                }
            }
        }
        public CommonValueCountrycity Region
        {
            get { return _region; }
            set
            {
                if (_region != value && value != null)
                {
                    SetProperty(ref _region, value);
                    _contactsCommands.RaiseCanExecuteChanged();
                }
            }
        }
        public CommonValueCountrycity State
        {
            get { return _state; }
            set
            {
                if (_state != value && value != null)
                {
                    SetProperty(ref _state, value);
                    _contactsCommands.RaiseCanExecuteChanged();
                }
            }
        }
        public CommonValueCountrycity Country
        {
            get { return _country; }
            set
            {
                if (_country != value && value != null)
                {
                    SetProperty(ref _country, value);
                    _contactsCommands.RaiseCanExecuteChanged();
                }
            }
        }
        #endregion

    }
}
