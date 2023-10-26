using LIT.Core.Controls;
using LIT.Core.Mvvm;
using LIT.Old_LIT;
using LIT.ViewModels;
using LITModels.LITModels.Models;
using Prism.Commands;
using Prism.Regions;
using SQLDataAccessLayer.DAL;
using SQLDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LIT.Commands
{
    public class ContactsCommands : CrudOperations<ContactsCommands>
    {
        private readonly ContactsViewModel _viewModel;
        private readonly ContactInfoDal _emailDal;

        public DelegateCommand AddCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand RetrieveCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }

        public ContactsCommands(ContactsViewModel viewModel)
            : base("Retrieve")
        {
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            _emailDal = new ContactInfoDal();
            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
            SaveCommand = new DelegateCommand(ExecuteSave);
            RetrieveCommand = new DelegateCommand(ExecuteRetrieve);
            DeleteCommand = new DelegateCommand(ExecuteDelete);
        }

        public new event EventHandler CanExecuteChanged;

        protected override void ExecuteSave()
        {
            try
            {
                //if (string.IsNullOrEmpty(_viewModel.ContactFirstName))
                //{
                //    MessageBox.Show("ContactFirstName cannot be null or empty.");
                //    return;
                //}
                //if (_viewModel.ContactType == null)
                //{
                //    MessageBox.Show("Select User Type");
                //    return;
                //}                
                //if (_viewModel.ContactType.ContactTypename == "LIV User")
                //{
                //    if (_viewModel.ContactRole == null || _viewModel.ContactRole.ToString() == "00000000-0000-0000-0000-000000000000")
                //    {
                //        MessageBox.Show("select contact role");
                //        return;
                //    }
                //    if (!string.IsNullOrEmpty(_viewModel.UserName))
                //    {
                //        MessageBox.Show("Please provide the username");
                //        return;
                //    }
                //    if (string.IsNullOrEmpty(_viewModel.Password)) 
                //    {
                //        MessageBox.Show("Enter Password");
                //        return;
                //    }
                //    if (string.IsNullOrEmpty(_viewModel.ConfirmPassword))
                //    {
                //        MessageBox.Show("Enter Confirm Password");
                //        return;
                //    }
                //    if (!string.IsNullOrEmpty(_viewModel.Password) && (_viewModel.Password.ToString().Length<5))
                //    {
                //        MessageBox.Show("Please Provide maximum 5 character in password");
                //        return;
                //    }
                //    if (!string.IsNullOrEmpty(_viewModel.ConfirmPassword) && (_viewModel.Password.ToString().Length < 5))
                //    {
                //        MessageBox.Show("Please Provide maximum 5 character in Confirm Password");
                //        return;
                //    }
                //    if (!string.Equals(_viewModel.ConfirmPassword, _viewModel.Password))
                //    {
                //        MessageBox.Show("Confirm Password Should be same as Password");
                //        return;
                //    }
                //}
                ContactModel contact = new ContactModel
                {
                    ContactId = _viewModel.ContactId == Guid.Empty ? Guid.NewGuid() : _viewModel.ContactId,
                    ContactTypeID = Guid.Parse(_viewModel.ContactType.ContactTypeid),
                    ContactType = _viewModel.ContactType.ContactTypename ?? string.Empty,
                    ContactTitle = _viewModel?.ContactTitle ?? Guid.Empty,
                    ContactFirstName = _viewModel.ContactFirstName ?? string.Empty,
                    ContactLastName = _viewModel.ContactLastName ?? string.Empty,
                    ContactGender = _viewModel.genderIndex >= 0 ? _viewModel.ContactGenderList[_viewModel.genderIndex] : string.Empty,
                    PhoneWork = _viewModel.PhoneWork ?? string.Empty,
                    PhoneHome = _viewModel.PhoneHome ?? string.Empty,
                    Mobile = _viewModel.mobile ?? string.Empty,
                    Fax = _viewModel.Fax ?? string.Empty,
                    Address = _viewModel.Address ?? string.Empty,
                    City = _viewModel.City?.CityId ?? Guid.Empty,
                    Region = _viewModel.Region?.RegionId ?? Guid.Empty,
                    State = _viewModel.State?.StatesId ?? Guid.Empty,
                    Country = _viewModel.Country?.CountryId ?? Guid.Empty,
                    Postcode = _viewModel.Postcode ?? string.Empty,
                    CompanyName = _viewModel.CompanyName ?? string.Empty,
                    CompanyPosition = _viewModel.CompanyPosition ?? string.Empty,
                    CompanyDescription = _viewModel.CompanyDescription ?? string.Empty,
                    EmailOne = _viewModel.EmailOne ?? string.Empty,
                    EmailTwo = _viewModel.EmailTwo ?? string.Empty,
                    Website = _viewModel.Website ?? string.Empty,
                    Password = _viewModel.Password ?? string.Empty,
                    ConfrimPassword = _viewModel.ConfirmPassword ?? string.Empty,
                    CreatedBy = _viewModel.UserId,
                    ModifiedBy = _viewModel.UserId,
                    UserRoleId = _viewModel.ContactRole == null ? Guid.Empty : _viewModel.ContactRole,
                    UserName = _viewModel.UserName ?? string.Empty,

                };
                _emailDal.SaveUpdateContactDetails("I", contact);
                TreeviewAccordion tracc = new TreeviewAccordion();
                _viewModel.MainWindow.trviewcontact.ItemsSource = tracc.ContactTreeViewModel.SupplierItems;
                MessageBox.Show("Contact is saved.");
                _viewModel.IsFormSave = true;
            }

            catch(NullReferenceException) 
            {
                MessageBox.Show("Please enter the required values");
            }

        }
        protected override void ExecuteRetrieve()
        {
            var Contact = _emailDal.RetrieveContactById(_viewModel.ContactId);
            LoadDropDownListValues loadDropDownListValues = new LoadDropDownListValues();
            CommonValueCountrycity objCVCC = new CommonValueCountrycity();

            List<CommonValueCountrycity> ListofCountry = new List<CommonValueCountrycity>();
            ListofCountry = loadDropDownListValues.LoadCommonValuesCountry("Country", objCVCC);

            List<CommonValueCountrycity> ListofState = new List<CommonValueCountrycity>();
            objCVCC.CountryId = Contact.Country;
            ListofState = loadDropDownListValues.LoadCommonValuesState("State", objCVCC);

            List<CommonValueCountrycity> ListofRegion = new List<CommonValueCountrycity>();
            objCVCC.StatesId = Contact.State;
            ListofRegion = loadDropDownListValues.LoadCommonValuesRegion("Region", objCVCC);

            List<CommonValueCountrycity> ListofCity = new List<CommonValueCountrycity>();
            objCVCC.RegionId = Contact.Region;
            ListofCity = loadDropDownListValues.LoadCommonValuesCity("CitywithRegionid", objCVCC);

            List<contacttype> ListofContactType = new List<contacttype>();
            ListofContactType = _emailDal.RetrieveContactTypeId();
            _viewModel.ContactType = ListofContactType.Where(id => id.ContactTypeid == Contact.ContactTypeID.ToString()).FirstOrDefault();
            _viewModel.ContactTypeIndex = ListofContactType.IndexOf(_viewModel.ContactType);

            List<UserRole> UserRolesList = new List<UserRole>();
            UserRolesList = _emailDal.LoadUserRoles();


            _viewModel.ContactId = Contact.ContactId;
            _viewModel.ContactTitle = Contact.ContactTitle;
            _viewModel.ContactFirstName = Contact.ContactFirstName;
            _viewModel.UserName = Contact.UserName;
            _viewModel.Password = Contact.Password;
            _viewModel.ConfirmPassword = Contact.ConfrimPassword;
            _viewModel.ContactLastName = Contact.ContactLastName;
            if (Contact.ContactGender != null)
            {
                _viewModel.genderIndex = _viewModel.ContactGenderList.IndexOf(_viewModel.ContactGenderList.Where(x => x.Contains(Contact.ContactGender)).FirstOrDefault());
            }
            _viewModel.PhoneWork = Contact.PhoneWork;
            _viewModel.PhoneHome = Contact.PhoneHome;
            _viewModel.mobile = Contact.Mobile;
            _viewModel.Fax = Contact.Fax;
            _viewModel.Address = Contact.Address;
            _viewModel.Postcode = Contact.Postcode;
            _viewModel.CompanyName = Contact.CompanyName;
            _viewModel.CompanyPosition = Contact.CompanyPosition;
            _viewModel.CompanyDescription = Contact.CompanyDescription;
            _viewModel.EmailOne = Contact.EmailOne;
            _viewModel.EmailTwo = Contact.EmailTwo;
            _viewModel.Website = Contact.Website;
            _viewModel.ContactTypeId = Contact.ContactTypeID.ToString();
            _viewModel.ContactAutoid = Contact.Contactautoid;

            _viewModel.ContactRole = Contact.UserRoleId;
            if (_viewModel.ContactRole != null)
            { 
            _viewModel.ContactrRoleIndex = UserRolesList.IndexOf(UserRolesList.Where(id => id.UserRoldId == _viewModel.ContactRole).FirstOrDefault());
            }
            _viewModel.Country = ListofCountry.Where(id => id.CountryId == Contact.Country).FirstOrDefault();
            _viewModel.countryIndex = ListofCountry.IndexOf(_viewModel.Country);

            _viewModel.State = ListofState.Where(id => id.StatesId == Contact.State).FirstOrDefault();
            _viewModel.stateIndex = ListofState.IndexOf(_viewModel.State);


            LIT.Views.Contacts cc = new LIT.Views.Contacts();
            
            _viewModel.Region = ListofRegion.Where(id => id.RegionId == Contact.Region).FirstOrDefault();
            _viewModel.regionIndex = ListofRegion.IndexOf(_viewModel.Region);
            cc.CmbRegionLoad(Contact.Region.ToString());

            
            _viewModel.City = ListofCity.Where(id => id.CityId == Contact.City).FirstOrDefault();
            _viewModel.cityIndex = ListofCity.IndexOf(_viewModel.City);
            cc.CmbCityLoad(Contact.City.ToString());

        }
        protected override void ExecuteDelete()
        {
           
                _emailDal.DeleteContact(_viewModel.ContactId, _viewModel.UserId);
                TreeviewAccordion tracc = new TreeviewAccordion();
                _viewModel.MainWindow.trviewcontact.ItemsSource = tracc.ContactTreeViewModel.SupplierItems;
                MessageBox.Show("Contact is deleted.");
                _viewModel.IsFormSave = true;

        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }

}
