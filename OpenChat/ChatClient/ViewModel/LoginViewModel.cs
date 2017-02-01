using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Command;
using Microsoft.AspNet.SignalR.Client;
using System.Windows.Threading;

namespace OpenChatClient.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IChatClientService init;

        private string _username;

        private string _password;

        public LoginViewModel()
        {
            init = new ChatClientSevice("http://localhost:11878/");

            init.chatProxy.On<bool>("Login", (valid) =>
                {
                    Dispatcher.CurrentDispatcher.InvokeAsync(() =>
                    {
                        Login(valid);
                    });
                });
        }

        private void Login(bool valid)
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