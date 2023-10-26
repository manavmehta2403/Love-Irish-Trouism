using LIT.Commands;
using Prism.Mvvm;

namespace LIT.ViewModels
{
    public class LoginViewModel : BindableBase
    {

        private string _userName;
        private string _password;
        private readonly LoginCommand _loginCommand;

        public LoginViewModel()
        {
            _loginCommand = new LoginCommand(this);
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                SetProperty(ref _userName, value);
                _loginCommand.RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                SetProperty(ref _password, value);
                _loginCommand.RaiseCanExecuteChanged();
            }
        }

        public LoginCommand LoginCommand => _loginCommand;
    }
}
