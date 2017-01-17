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
using System.Collections.ObjectModel;
using Microsoft.AspNet.SignalR.Hubs;
using OpenChat.Models;

namespace OpenChatClient
{
    public partial class MainWindow : Window
    {
        public List<Room> data = new List<Room>();
        public List<Message> chatMessages = new List<Message>();
        private string username;

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();

            var connection = new HubConnection("http://localhost:11878/");
            connection.Closed += Connection_Closed;
            HubProxy = connection.CreateHubProxy("chat");

            connection.TraceLevel = TraceLevels.All;
            connection.TraceWriter = Console.Out;

            HubProxy.On<string>("Login", (valid) =>
            {
                Dispatcher.InvokeAsync(() =>
                {
                    this.LoginVisability(valid);
                });
            });

            connection.Error += ex => Console.WriteLine("SignalR error: {0}", ex.Message);

            HubProxy.On<List<Room>>("LoadUserRooms", (data) =>
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
                        chatMessages.Add(new Message() { Text = MessageTextBox.Text, Author = username, Room = SelectedRoom.RoomName });
                        MessageTextBox.Text = "";
                    });
            });

            connection.Start();
        }

        public Room SelectedRoom { get; set; }
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
            SelectedRoom = (Room)Contacts.SelectedItem;
            chatMessages = HubProxy.Invoke<List<Message>>("JoinGroup", username, SelectedRoom.RoomName).Result;
        }

        private void sendbnn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HubProxy.Invoke("SendMessageToGroup", MessageTextBox.Text);
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

        private void LoginVisability(string username)
        {
            if (username != "404")
            {
                this.ErorValidatoin.Visibility = Visibility.Hidden;
                this.login.Visibility = Visibility.Hidden;
                Contacts.ItemsSource = HubProxy.Invoke<List<Room>>("LoadUserRooms", username).Result;
            }
            else
            {
                this.ErorValidatoin.Visibility = Visibility.Visible;
                this.login.Visibility = Visibility.Visible;
            }
        }
    }
}