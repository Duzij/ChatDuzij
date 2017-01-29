using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OpenChatClient.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private string _username;

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        private string _login;

        public string Login
        {
            get { return _login; }
            set { _login = value; }
        }

        private void OnLoginCommand(object commandParameter)
        {
            var password = ((PasswordBox)commandParameter).Password;
        }

    }
}
