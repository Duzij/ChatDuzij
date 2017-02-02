using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Command;
using Microsoft.AspNet.SignalR.Client;
using System.Windows.Threading;
using ChatClient;
using Microsoft.AspNet.SignalR.Client.Hubs;
using OpenChatClient.Models;

namespace OpenChatClient.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IChatClientService init;
        public IHubProxy HubProxy { get; set; }
        public HubConnection Connection { get; set; }

        private string _username;

        private string _password;

        public LoginViewModel()
        {
            init = new ChatClientSevice("http://localhost:11878/");
            Connection = init.connection;
            HubProxy = init.chatProxy;

            HubProxy.On("Login", (valid) => {  this.Login(valid); });

            Connection.Start();
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

        public async void OnLoginCommand(object commandParameter)
        {
            try
            {
                await init.chatProxy.Invoke("Login", Username, ((PasswordBox)commandParameter).Password);
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Login(bool valid)
        {
            if (valid)
            {
                var win = new MainWindow();
                win.ShowDialog();
            }
            else
            {
                //todo validation label
            }
        }
    }
}