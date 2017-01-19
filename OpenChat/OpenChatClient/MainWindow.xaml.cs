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
using OpenChatClient.Model;

namespace OpenChatClient
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<RoomDTO> data = new ObservableCollection<RoomDTO>();
        public List<Message> chatMessages = new List<Message>();
        private string username;
        private string roomName;

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();

            var connection = new HubConnection("http://localhost:11878/");
            connection.Closed += Connection_Closed;
            HubProxy = connection.CreateHubProxy("chat");

            connection.TraceLevel = TraceLevels.All;
            connection.TraceWriter = Console.Out;

            HubProxy.On<bool>("Login", (valid) =>
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
                    this.data = new ObservableCollection<RoomDTO>(data);
                });
            });

            HubProxy.On("SendToGroup", () =>
            {
                Dispatcher.InvokeAsync(() =>
                    {
                        chatMessages.Add(new Message() { Text = MessageTextBox.Text, Author = username, Room = SelectedRoom.RoomName });
                        MessageTextBox.Text = "";
                    });
            });

            HubProxy.On<string>("Notify", (string RoomName) =>
            {
                Dispatcher.InvokeAsync(() =>
                {
                    data.FirstOrDefault(a => a.RoomName == RoomName).GotNewMessages = true;
                    if (SelectedRoom.RoomName == RoomName)
                        chatMessages = HubProxy.Invoke<List<Message>>("LoadGroupData", username, SelectedRoom.RoomName).Result;
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
            SelectedRoom = (Room)Contacts.SelectedItem;
            chatMessages = HubProxy.Invoke<List<Message>>("LoadGroupData", username, SelectedRoom.RoomName).Result;
        }

        private void sendbnn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectedRoom = (Room)Contacts.SelectedItem;
                HubProxy.Invoke("SendMessageToGroup", SelectedRoom.RoomName, MessageTextBox.Text);
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await HubProxy.Invoke("Login", LoginTextBox.Text, PasswordTextBox.Text);
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void LoginVisability(bool valid)
        {
            if (valid)
            {
                this.username = LoginTextBox.Text;
                this.ErorValidatoin.Visibility = Visibility.Hidden;
                this.login.Visibility = Visibility.Hidden;

                var list = await HubProxy.Invoke<List<Room>>("LoadUserRooms", username);
                Contacts.ItemsSource = list;
            }
            else
            {
                this.ErorValidatoin.Visibility = Visibility.Visible;
                this.login.Visibility = Visibility.Visible;
            }
        }
    }
}