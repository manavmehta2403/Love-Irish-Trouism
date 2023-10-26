using LIT.ViewModels;
using LITModels.LITModels.Models;
using Prism.Regions;
using SQLDataAccessLayer.DAL;
using SQLDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LIT.Views
{
    /// <summary>
    /// Interaction logic for Contacts.xaml
    /// </summary>
    public partial class Contacts : Window
    {
        List<CommonValueCountrycity> ListofCountry = new List<CommonValueCountrycity>();
        List<CommonValueCountrycity> ListofCity = new List<CommonValueCountrycity>();
        List<CommonValueCountrycity> ListofState = new List<CommonValueCountrycity>();
        List<CommonValueCountrycity> ListofRegion = new List<CommonValueCountrycity>();
        private ContactsViewModel viewModel { get; set; }
        private bool IsNew { get; set; }

        public Contacts()
        {
            InitializeComponent();
            UpdateList();
            
        }

        public Contacts(bool isNew)
        {
            this.IsNew = isNew;
            InitializeComponent();
            UpdateList();

        }

        private void UpdateList()
        {

            List<contacttype> ListofContactType = new List<contacttype>();
            CommonValueCountrycity objCVCC = new CommonValueCountrycity();
            LoadDropDownListValues loadDropDownListValues = new LoadDropDownListValues();
            ContactInfoDal emailDal = new ContactInfoDal();
            List<Salutation> ListofSalutation = new List<Salutation>();
            List<UserRole> UserRolesList = new List<UserRole>();

            UserRolesList = emailDal.LoadUserRoles();
            if (UserRolesList != null && UserRolesList.Count > 0)
            {
                ConactRole.ItemsSource = UserRolesList;
                ConactRole.SelectedValuePath = "UserRoldId";
                ConactRole.DisplayMemberPath = "UserRole1";
            }
            ListofSalutation = loadDropDownListValues.Loadsalutation();
            if (ListofSalutation != null && ListofSalutation.Count > 0)
            {
                conactTitle.ItemsSource = ListofSalutation;
                conactTitle.SelectedValuePath = "SalutationId";
                conactTitle.DisplayMemberPath = "SalutationName";
            }

            ListofContactType = emailDal.RetrieveContactTypeId();
            ConactType.ItemsSource = ListofContactType;
            ConactType.SelectedValuePath = "ContactTypeid";
            ConactType.DisplayMemberPath = "ContactTypename";

            ListofCountry = loadDropDownListValues.LoadCommonValuesCountry("Country", objCVCC);
            conactCountry.ItemsSource = ListofCountry;
            conactCountry.SelectedValuePath = "CountryId";
            conactCountry.DisplayMemberPath = "CountryName";

           /* objCVCC.CountryId = ListofCountry[0].CountryId;
            ListofState = loadDropDownListValues.LoadCommonValuesState("State", objCVCC);
            if (ListofCity.Count > 0)
            {
                conactState.ItemsSource = ListofState;
                conactState.SelectedValuePath = "StatesId";
                conactState.DisplayMemberPath = "StatesName";
            }
            else
            {
                conactState.IsEnabled = false;
                conactRegion.IsEnabled = false;
                conactCity.IsEnabled = false;

            }


            if (ListofState.Count > 0)
            {
                objCVCC.StatesId = ListofState[0].StatesId;
                ListofRegion = loadDropDownListValues.LoadCommonValuesRegion("Region", objCVCC);
                if (ListofRegion.Count > 0)
                {
                    conactRegion.ItemsSource = ListofRegion;
                    conactRegion.SelectedValuePath = "RegionId";
                    conactRegion.DisplayMemberPath = "RegionName";

                }
                else
                {
                    conactRegion.IsEnabled = false;
                }

            }


            if (ListofRegion.Count > 0)
            {
                objCVCC.RegionId = ListofRegion[0].RegionId;
                ListofCity = loadDropDownListValues.LoadCommonValuesCity("CitywithRegionid", objCVCC);
                if (ListofCity.Count > 0)
                {
                    conactCity.ItemsSource = ListofCity;
                    conactCity.SelectedValuePath = "CityId";
                    conactCity.DisplayMemberPath = "CityName";

                }
                else
                {
                    conactCity.IsEnabled = false;
                }

            }
           */
        }       

        private void conactCountry_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var country = conactCountry.SelectedItem as CommonValueCountrycity;
            if (country != null)
            {
                CmbStateLoad(country.CountryId.ToString());
            }

        }

        private void conactState_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // var state = conactState.SelectedItem as CommonValueCountrycity;
            ContactsViewModel viewModel = (ContactsViewModel)this.DataContext;


            if (viewModel.State != null && viewModel.State.StatesId != Guid.Empty)
            {
                CmbRegionLoad(viewModel.State.StatesId.ToString());
                //conactState.SelectedValue = viewModel.State.StatesId;
                if(!IsNew)
                {
                    conactState.SelectedItem = viewModel.State;
                    conactState.SelectedValue = viewModel.State.StatesId;
                }

                //conactState.SelectedIndex = ListofState.IndexOf(viewModel.State);
            }
        }

        private void conactRegion_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // var region = conactRegion.SelectedItem as CommonValueCountrycity;
            ContactsViewModel viewModel = (ContactsViewModel)this.DataContext;

            if (viewModel.Region != null && viewModel.Region.RegionId!=Guid.Empty)
            {
                CmbCityLoad(viewModel.Region.RegionId.ToString());
                if(!IsNew)
                {
                    conactRegion.SelectedItem = viewModel.Region;
                    conactRegion.SelectedValue = viewModel.Region.RegionId;
                }

            }
            
        }

        private void conactCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var city = conactCity.SelectedItem as CommonValueCountrycity;
            /*  viewModel = DataContext as ContactsViewModel;
              if (viewModel.Region != null )
              {
                  if (viewModel.Region.RegionId != Guid.Empty)
                  {
                      conactRegion.SelectedItem = viewModel.Region;
                      conactRegion.SelectedIndex = ListofRegion.FindIndex(x => x.RegionId == viewModel.Region.RegionId);
                      //conactRegion.SelectedValue = viewModel.Region.RegionId;
                  }             

              }
              if (viewModel.City != null && viewModel.City.CityId != Guid.Empty)
              {
                  conactCity.SelectedValue = viewModel.City.CityId;
              }*/
            ContactsViewModel viewModel = (ContactsViewModel)this.DataContext;
            if (city != null)
            {
                if(!IsNew)
                {
                    conactCity.SelectedItem = viewModel.City;
                    conactCity.SelectedValue = viewModel.City.CityId;

                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
                this.Close();
        }
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            ContactsViewModel viewModel = (ContactsViewModel)this.DataContext;
            string valmsg = validation(viewModel);
            if (valmsg == string.Empty)
            {
                viewModel.ContactsCommands.SaveCommand.Execute();
                if (viewModel.IsFormSave)
                {
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show(valmsg);
                return;
            }
        }
        private string validation(ContactsViewModel _viewModel)
        {
            string retnmsg = string.Empty;
            if (string.IsNullOrEmpty(_viewModel.ContactFirstName))
            {
                retnmsg="ContactFirstName cannot be null or empty.";
               
                return retnmsg;
            }
            if (_viewModel.ContactType == null)
            {
                retnmsg="Select User Type";
                return retnmsg;
            }
            if (_viewModel.ContactType.ContactTypename == "LIV User")
            {
                
                if (string.IsNullOrEmpty(_viewModel.UserName))
                {
                    retnmsg="Please provide the username";
                    Username.Focus();
                    return retnmsg;
                }
                if (string.IsNullOrEmpty(_viewModel.Password))
                {
                    retnmsg="Enter Password";
                    conactPassword.Focus();
                    return retnmsg;
                }
                if (string.IsNullOrEmpty(_viewModel.ConfirmPassword))
                {
                    retnmsg="Enter Confirm Password";
                    conactConfirmPassword.Focus();
                    return retnmsg;
                }
                if (!string.IsNullOrEmpty(_viewModel.Password) && (_viewModel.Password.ToString().Length < 5))
                {
                    retnmsg="Please Provide maximum 5 character in password";
                    conactPassword.Focus();
                    return retnmsg;
                }
                if (!string.IsNullOrEmpty(_viewModel.ConfirmPassword) && (_viewModel.Password.ToString().Length < 5))
                {
                    retnmsg="Please Provide maximum 5 character in Confirm Password";
                    conactConfirmPassword.Focus();
                    return retnmsg;
                }
                if (!string.Equals(_viewModel.ConfirmPassword, _viewModel.Password))
                {
                    retnmsg="Confirm Password Should be same as Password";
                    conactConfirmPassword.Focus();
                    return retnmsg;
                }
                if (_viewModel.ContactRole == null || _viewModel.ContactRole.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    retnmsg = "select contact role";
                    ConactRole.Focus();
                    return retnmsg;
                }
            }
            return retnmsg;
        }
        public void CmbStateLoad(string countryid)
        {

            CommonValueCountrycity objCVCC = new CommonValueCountrycity();
            LoadDropDownListValues loadDropDownListValues = new LoadDropDownListValues();


            objCVCC.CountryId = Guid.Parse(countryid);
            ListofState = loadDropDownListValues.LoadCommonValuesState("State", objCVCC);
            if (ListofState.Count > 0)
            {
                conactState.ItemsSource = ListofState;
                conactState.SelectedValuePath = "StatesId";
                conactState.DisplayMemberPath = "StatesName";
                conactState.IsEnabled = true;
            }
        }

        public void CmbRegionLoad(string stateid)
        {
            CommonValueCountrycity objCVCC = new CommonValueCountrycity();
            LoadDropDownListValues loadDropDownListValues = new LoadDropDownListValues();

            objCVCC.StatesId = Guid.Parse(stateid);
            ListofRegion = loadDropDownListValues.LoadCommonValuesRegion("Region", objCVCC);
            if (ListofRegion.Count > 0)
            {
                conactRegion.ItemsSource = ListofRegion;
                conactRegion.SelectedValuePath = "RegionId";
                conactRegion.DisplayMemberPath = "RegionName";
                conactRegion.IsEnabled = true;

            }
        }

        public void CmbCityLoad(string regionid)
        {
            CommonValueCountrycity objCVCC = new CommonValueCountrycity();
            LoadDropDownListValues loadDropDownListValues = new LoadDropDownListValues();

            objCVCC.RegionId = Guid.Parse(regionid);
            ListofCity = loadDropDownListValues.LoadCommonValuesCity("CitywithRegionid", objCVCC);
            if (ListofCity.Count > 0)
            {
                conactCity.ItemsSource = ListofCity;
                conactCity.SelectedValuePath = "CityId";
                conactCity.DisplayMemberPath = "CityName";
                conactCity.IsEnabled = true;

            }

        }

        private void Btncontdelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this item?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                ContactsViewModel viewModel = (ContactsViewModel)this.DataContext;
                viewModel.ContactsCommands.DeleteCommand.Execute();
                if (viewModel.IsFormSave)
                {
                    this.Close();
                }
            }
        }
    }
}
