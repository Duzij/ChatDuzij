using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Command;

namespace OpenChatClient.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private string _username;

        private string _password;

        public LoginViewModel()
        {
        }

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public RelayCommand<object> LoginCommand => new RelayCommand<object>(OnLoginCommand);
        public RelayCommand SendMsgCommand => new RelayCommand(SendMessageCommand);

        public void SendMessageCommand()
        {
            //TODO send
        }

        public void OnLoginCommand(object commandParameter)
        {
            var password = ((PasswordBox)commandParameter).Password;
            var username = Username;
        }
    }
}