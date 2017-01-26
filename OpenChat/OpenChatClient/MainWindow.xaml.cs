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
        public RoomDTO SelectedRoomDTO = new RoomDTO();
        private string username;
        private string RoomDTOName;

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            try
            {
                Initalizer = new ChatClientInitalizer("http://localhost:11878/");
                Connection = Initalizer.connection;
                HubProxy = Initalizer.chatProxy;
            }
            catch (Exception)
            {
                ErrorValidatoin.Content = "Cannot connect to server. Contact your administrator";
                ErrorValidatoin.Visibility = Visibility.Visible;
            }

            HubProxy.On("Login", (valid) =>
            {
                Dispatcher.InvokeAsync(() =>
                {
                    this.LoginVisibility(valid);
                });
            });

            HubProxy.On("ReLoadRooms", (valid) =>
            {
                HubProxy.Invoke<List<RoomDTO>>("LoadRooms", username);
            });

            HubProxy.On("Notify", (string RoomDTOName) =>
            {
                var room = LoadedRooms.First(a => a.RoomName == RoomDTOName);
                Dispatcher.InvokeAsync(() =>
                {
                    SelectedRoomDTO = (RoomDTO)Contacts.SelectedItem;
                    room.GotNewMessages = true;
                    if (SelectedRoomDTO.RoomName == RoomDTOName)
                    {
                        ReloadMessageSource();
                    }
                });
            });

            HubProxy.On("LoadRoomMessages", (List<MessageDTO> msgs) =>
            {
                Dispatcher.InvokeAsync(() =>
                {
                    this.LoadedMessages = new ObservableCollection<MessageDTO>(msgs);
                    this.ChatView.ItemsSource = msgs;
                    ChatView.Items.MoveCurrentToLast();
                    ChatView.ScrollIntoView(ChatView.Items.CurrentItem);
                });
            });

            HubProxy.On("LoadRooms", (List<RoomDTO> rooms) =>
            {
                Dispatcher.InvokeAsync(() =>
                {
                    Contacts.ItemsSource = rooms;
                    this.LoadedRooms = new ObservableCollection<RoomDTO>(rooms);
                });
            });

            Connection.Error += ex => Console.WriteLine("SignalR error: {0}", ex.Message);

            Connection.Start();
        }

        public ChatClientInitalizer Initalizer { get; set; }
        public IHubProxy HubProxy { get; set; }

        public HubConnection Connection { get; set; }

        public ChatClientInitalizer chatInit { get; set; }

        public void ReloadMessageSource()
        {
            ChatView.ItemsSource = null;
            HubProxy.Invoke<ObservableCollection<MessageDTO>>("LoadRoomMessages", SelectedRoomDTO.RoomName, username);
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
            CreateRoomWindow win = new CreateRoomWindow(list, username);
            win.ShowDialog();
            if (win.DialogResult == true)
            {
                HubProxy.Invoke("CreateRoom", win.TempRoom);
            }
        }
    }
}