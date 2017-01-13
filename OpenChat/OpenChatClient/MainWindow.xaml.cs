using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.AspNet.SignalR.Client;
using System.Windows.Shapes;
using OpenChatClient.Model;
using System.Collections.ObjectModel;
using Microsoft.AspNet.SignalR.Hubs;

namespace OpenChatClient
{
    public partial class MainWindow : Window
    {
        private bool loginOverVisible;
        private bool errorValidationLabelVisibility;

        private List<RoomDTO> data = new List<RoomDTO>();
        private int userId;

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();

            var connection = new HubConnection("http://localhost:11878/");
            connection.Closed += Connection_Closed;
            HubProxy = connection.CreateHubProxy("chat");

            connection.TraceLevel = TraceLevels.All;
            connection.TraceWriter = Console.Out;

            HubProxy.On<int>("LoginUser", (valid) =>
            {
                Dispatcher.InvokeAsync(() =>
                {
                    this.LoginVisability(valid);
                });
            });

            connection.Error += ex => Console.WriteLine("SignalR error: {0}", ex.Message);

            HubProxy.On("send", () =>
            {
                Dispatcher.InvokeAsync(() =>
                    {
                        ChatView.Text += MessageTextBox.Text;
                        MessageTextBox.Text = "";
                    });
            });

            connection.Start();
        }

        public IHubProxy HubProxy { get; set; }

        public HubConnection Connection { get; set; }

        public bool ErrorValidationLabelVisibility
        {
            get { return errorValidationLabelVisibility; }
            set { errorValidationLabelVisibility = value; }
        }

        public bool LoginOverVisible
        {
            get { return loginOverVisible; }
            set { loginOverVisible = value; }
        }

        public ChatClientInitalizer chatInit { get; set; }

        public List<RoomDTO> Data
        {
            get
            {
                return data;
            }
        }

        private void Connection_Closed()
        {
            this.Connection.Dispose();
        }

        private void Contacts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //TODO join room and room data load to chat
            //Todo on double click data loads to chatTextBox (if hw choose from rooms)
        }

        private void sendbnn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HubProxy.Invoke("send", MessageTextBox.Text);
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HubProxy.Invoke("Login", LoginTextBox.Text, PasswordTextBox.Text);
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoginVisability(int userId)
        {
            if (userId != 0)
            {
                this.ErorValidatoin.Visibility = Visibility.Hidden;
                this.login.Visibility = Visibility.Hidden;
                data = HubProxy.Invoke<List<RoomDTO>>("LoadUserRooms", userId).Result;
            }
            else
            {
                this.ErorValidatoin.Visibility = Visibility.Visible;
                this.login.Visibility = Visibility.Visible;
            }
        }
    }
}