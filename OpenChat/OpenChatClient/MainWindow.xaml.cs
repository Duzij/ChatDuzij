using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.AspNet.SignalR.Client;
using System.Collections.ObjectModel;
using OpenChatClient.Models;

namespace OpenChatClient
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<RoomDTO> LoadedRooms = new ObservableCollection<RoomDTO>();
        public ObservableCollection<MessageDTO> LoadedMessages = new ObservableCollection<MessageDTO>();
        private string username;
        private string RoomDTOName;

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            try
            {
                var connection = new HubConnection("http://localhost:11878/");
                connection.Closed += Connection_Closed;
                HubProxy = connection.CreateHubProxy("chat");

                connection.TraceLevel = TraceLevels.All;
                connection.TraceWriter = Console.Out;

            }
            catch (Exception)
            {
                ErrorValidatoin.Content = "Cannot connect to server. Contact your administrator";
                ErrorValidatoin.Visibility = Visibility.Visible;
            }
           
            HubProxy.On<bool>("Login", (valid) =>
            {
                Dispatcher.InvokeAsync(() =>
                {
                    this.LoginVisibility(valid);
                });
            });

            HubProxy.On<string>("Notify", (string RoomDTOName) =>
            {
                var room = LoadedRooms.First(a => a.RoomName == RoomDTOName);
                Dispatcher.InvokeAsync(() =>
                {
                    room.GotNewMessages = true;
                    if (SelectedRoomDTO.RoomName == RoomDTOName)
                        ReloadMessageSource();
                });
            });

            HubProxy.On<List<MessageDTO>>("LoadRoomMessages", (List<MessageDTO> msgs) =>
            {
                Dispatcher.InvokeAsync(() =>
                {
                    this.LoadedMessages = new ObservableCollection<MessageDTO>(msgs);
                    this.ChatView.ItemsSource = msgs;
                    ChatView.Items.MoveCurrentToLast();
                    ChatView.ScrollIntoView(ChatView.Items.CurrentItem);
                });
            });

            HubProxy.On<List<RoomDTO>>("LoadRooms", (List<RoomDTO> rooms) =>
            {
                Dispatcher.InvokeAsync(() =>
                {
                    Contacts.ItemsSource = rooms;
                    this.LoadedRooms = new ObservableCollection<RoomDTO>(rooms);
                });
            });

            connection.Error += ex => Console.WriteLine("SignalR error: {0}", ex.Message);

            connection.Start();
        }

        public RoomDTO SelectedRoomDTO { get; set; }
        public IHubProxy HubProxy { get; set; }

        public HubConnection Connection { get; set; }

        public ChatClientInitalizer chatInit { get; set; }

        public void ReloadMessageSource()
        {
            ChatView.ItemsSource = null;
            HubProxy.Invoke<ObservableCollection<MessageDTO>>("LoadRoomMessages", SelectedRoomDTO.RoomName, username);
        }

        private void Connection_Closed()
        {
            if (Connection != null)
                this.Connection.Dispose();
        }

        private void Contacts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectedRoomDTO = (RoomDTO)Contacts.SelectedItem;
            //LoadedMessages = HubProxy.Invoke<ObservableCollection<MessageDTO>>("LoadGroupData", SelectedRoomDTO.RoomName, username).Result;
            ReloadMessageSource();
        }

        private void sendbnn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectedRoomDTO = (RoomDTO)Contacts.SelectedItem;
                HubProxy.Invoke("SendMessage", SelectedRoomDTO.RoomName, MessageTextBox.Text, username);
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

        private async void LoginVisibility(bool valid)
        {
            if (valid)
            {
                this.username = LoginTextBox.Text;
                this.ErrorValidatoin.Visibility = Visibility.Hidden;
                this.login.Visibility = Visibility.Hidden;
                await HubProxy.Invoke<List<RoomDTO>>("LoadRooms", username);
            }
            else
            {
                ErrorValidatoin.Content = "Incorrect username or password";
                this.ErrorValidatoin.Visibility = Visibility.Visible;
                this.login.Visibility = Visibility.Visible;
            }
        }

        private void AddRoom(object sender, RoutedEventArgs e)
        {
            var list = HubProxy.Invoke<List<UserDTO>>("LoadUsers", username).Result;
            CreateRoomWindow win = new CreateRoomWindow(list);
            win.ShowDialog();
            if (win.DialogResult == true)
            {
                List<string> selectedUsers = win.avalibleUsers.Where(b => b.IsSelected).ToList().ConvertAll(a => a.Username);
                HubProxy.Invoke<List<UserDTO>>("LoadUsers", username);
            }
        }
    }
}