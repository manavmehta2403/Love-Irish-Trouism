using LIT.ViewModels;
using Prism.Commands;
using SQLDataAccessLayer.DAL;
using System;
using System.ComponentModel;
using System.Windows;

namespace LIT.Commands
{
    public class LoginCommand : DelegateCommandBase
    {
        private readonly UserManagementDAL _objuserdal;
        private readonly LoginViewModel _viewModel;
        private readonly Errorlog _erobj;

        public LoginCommand(LoginViewModel viewModel)
        {
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            _objuserdal = new UserManagementDAL();
            _erobj = new Errorlog();
            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        public new event EventHandler CanExecuteChanged;

        protected override bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_viewModel.UserName) && !string.IsNullOrEmpty(_viewModel.Password);
        }

        protected override void Execute(object parameter)
        {
            try
            {
                string retobj = _objuserdal.UserloginVerify(_viewModel.UserName, _viewModel.Password);
                if (retobj.ToUpper() == "EXISTS")
                {
                    LoadDropDownListValues loadDropDownListValues = new LoadDropDownListValues();
                    string loginuserid = loadDropDownListValues.Currentuseridinfo(_viewModel.UserName);
                    // Navigate to MainWindow
                    LIT.Old_LIT.MainWindow mainWindow = new(_viewModel.UserName);
                    mainWindow.Show();

                    // Close the Login window
                    Application.Current.MainWindow?.Close();
                    ((App)Application.Current).UserLoggedIn(Guid.Parse(loginuserid));
                }
                else if (retobj.ToUpper() == "NOT EXISTS")
                {
                    // Handle the case where login credentials are incorrect.
                    MessageBox.Show("Please provide valid Username & Password");
                }
            }
            catch (Exception ex)
            {
                _erobj.WriteErrorLoginfo("Login", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }
        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
