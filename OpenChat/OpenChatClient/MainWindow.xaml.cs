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
        public List<RoomDTO> data = new List<RoomDTO>();
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

            HubProxy.On<int>("Login", (valid) =>
            {
                Dispatcher.InvokeAsync(() =>
                {
                    this.LoginVisability(valid);
                });
            });

            connection.Error += ex => Console.WriteLine("SignalR error: {0}", ex.Message);

            HubProxy.On<List<RoomDTO>>("LoadUserRooms", (data) =>
            {
                Dispatcher.InvokeAsync(() =>
                {
                    this.data = data;
                });
            });

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

        public ChatClientInitalizer chatInit { get; set; }

        private void Connection_Closed()
        {
            this.Connection.Dispose();
        }

        private void Contacts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //TODO join room and room data load to chat
            //Todo on double click data loads to chatTextBox (if hw choose from rooms)
            RoomDTO room = (RoomDTO)Contacts.SelectedItem;
            HubProxy.Invoke("JoinGroup", this.userId, room.RoomId);
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

        private void LoginVisability(int Id)
        {
            this.userId = Id;
            if (Id != 0)
            {
                this.ErorValidatoin.Visibility = Visibility.Hidden;
                this.login.Visibility = Visibility.Hidden;
                Contacts.ItemsSource = HubProxy.Invoke<List<RoomDTO>>("LoadUserRooms", userId).Result;
            }
            else
            {
                this.ErorValidatoin.Visibility = Visibility.Visible;
                this.login.Visibility = Visibility.Visible;
            }
        }
    }
}