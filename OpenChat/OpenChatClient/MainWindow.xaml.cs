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
using System.Windows.Automation.Peers;
using Microsoft.AspNet.SignalR.Hubs;
using OpenChat.Models;
using OpenChatClient.Model;

namespace OpenChatClient
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<RoomDTO> data = new ObservableCollection<RoomDTO>();
        public ObservableCollection<MessageDTO> chatMessages = new ObservableCollection<MessageDTO>();
        private string username;
        private string RoomDTOName;

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

            HubProxy.On<List<RoomDTO>>("LoadUserRoomDTOs", (data) =>
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
                    chatMessages.Add(new MessageDTO() { Text = MessageTextBox.Text, Author = username, Room = SelectedRoomDTO.RoomName });
                    MessageTextBox.Text = "";
                });
            });

            HubProxy.On<string>("Notify", (string RoomDTOName) =>
            {
                var room = data.First(a => a.RoomName == RoomDTOName);
                Dispatcher.InvokeAsync(() =>
                {
                    room.GotNewMessages = true;
                    if (SelectedRoomDTO.RoomName == RoomDTOName)
                        ReloadMessageSource();
                });
            });

            HubProxy.On<List<MessageDTO>>("ReloadGroupData", (List<MessageDTO> msgs) =>
            {
                Dispatcher.InvokeAsync(() =>
                {
                    this.ChatView.ItemsSource = msgs;
                    ChatView.Items.MoveCurrentToLast();
                    ChatView.ScrollIntoView(ChatView.Items.CurrentItem);
                });
            });

            connection.Start();
        }

        public RoomDTO SelectedRoomDTO { get; set; }
        public IHubProxy HubProxy { get; set; }

        public HubConnection Connection { get; set; }

        public ChatClientInitalizer chatInit { get; set; }

        public void ReloadMessageSource()
        {
            ChatView.ItemsSource = null;
            HubProxy.Invoke<ObservableCollection<MessageDTO>>("LoadGroupData", SelectedRoomDTO.RoomName, username);
        }

        private void Connection_Closed()
        {
            if (Connection != null)
                this.Connection.Dispose();
        }

        private void Contacts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectedRoomDTO = (RoomDTO)Contacts.SelectedItem;
            //chatMessages = HubProxy.Invoke<ObservableCollection<MessageDTO>>("LoadGroupData", SelectedRoomDTO.RoomName, username).Result;
            ReloadMessageSource();
        }

        private void sendbnn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectedRoomDTO = (RoomDTO)Contacts.SelectedItem;
                HubProxy.Invoke("SendMessageToGroup", SelectedRoomDTO.RoomName, MessageTextBox.Text, username);
                ReloadMessageSource();
                MessageTextBox.Text = "";
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
                await HubProxy.Invoke("Login", LoginTextBox.Text, PasswordTextBox.Password);
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
                var list = await HubProxy.Invoke<List<RoomDTO>>("LoadUserRooms", username);
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