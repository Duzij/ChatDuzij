using GalaSoft.MvvmLight;
using ChatClient.Model;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using OpenChatClient;
using OpenChatClient.Models;

namespace ChatClient.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IChatClientService chatService;
        private ObservableCollection<RoomDTO> rooms;
        private ObservableCollection<MessageDTO> messages;
        private RoomDTO selectedRoom;
        private string username;

        public MainViewModel(IChatClientService chatService, string username)
        {
            this.chatService = chatService;
            Username = MessengerInstance.;

            chatService.chatProxy.On("ReLoadRooms", (valid) =>
            {
                chatService.chatProxy.Invoke<List<RoomDTO>>("LoadRooms", Username);
            });

            chatService.chatProxy.On("Notify", (string RoomDTOName) =>
            {
                var room = Rooms.First(a => a.RoomName == RoomDTOName);
                App.Current.Dispatcher.InvokeAsync(() =>
                {
                    room.GotNewMessages = true;
                    if (SelectedRoom.RoomName == RoomDTOName)
                    {
                        ReloadMessageSource();
                    }
                });
            });

            chatService.chatProxy.On("LoadRoomMessages", (List<MessageDTO> msgs) =>
            {
                App.Current.Dispatcher.InvokeAsync(() =>
                {
                    Messages = new ObservableCollection<MessageDTO>(msgs);
                });
            });

            chatService.chatProxy.On("LoadRooms", (List<RoomDTO> rooms) =>
            {
                Rooms = new ObservableCollection<RoomDTO>(rooms);
            });

            chatService.chatProxy.Invoke("LoadRooms", Username);
        }

        public ObservableCollection<MessageDTO> Messages
        {
            get { return messages; }
            set { messages = value; }
        }

        public ObservableCollection<RoomDTO> Rooms
        {
            get { return rooms; }
            set { rooms = value; }
        }

        public RoomDTO SelectedRoom
        {
            get { return selectedRoom; }
            set { selectedRoom = value; }
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public RelayCommand AddRoomCommand => new RelayCommand(AddRoom);

        public void ReloadMessageSource()
        {
            chatService.chatProxy.Invoke<ObservableCollection<MessageDTO>>("LoadRoomMessages", SelectedRoom.RoomName, Username);
        }

        private void AddRoom()
        {
            //TODO add room
        }
    }
}

//public partial class MainWindow : Window
//{
//    private string username;

//    private string RoomDTOName;

//    public ChatClientInitalizer Initalizer { get; set; }
//    public IHubProxy HubProxy { get; set; }

//    public HubConnection Connection { get; set; }

//    public ChatClientInitalizer chatInit { get; set; }

//    public void ReloadMessageSource()
//    {
//        ChatView.ItemsSource = null;
//        HubProxy.Invoke<ObservableCollection<MessageDTO>>("LoadRoomMessages", SelectedRoomDTO.RoomName, username);
//    }

//    private void Contacts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
//    {
//        SelectedRoomDTO = (RoomDTO)Contacts.SelectedItem;
//        //LoadedMessages = HubProxy.Invoke<ObservableCollection<MessageDTO>>("LoadGroupData", SelectedRoomDTO.RoomName, username).Result;
//        ReloadMessageSource();
//    }

//    private void sendbnn_Click(object sender, RoutedEventArgs e)
//    {
//        try
//        {
//            SelectedRoomDTO = (RoomDTO)Contacts.SelectedItem;
//            HubProxy.Invoke("SendMessage", SelectedRoomDTO.RoomName, MessageTextBox.Text, username);
//            ReloadMessageSource();
//            MessageTextBox.Text = "";
//        }
//        catch (NullReferenceException ex)
//        {
//            MessageBox.Show(ex.Message);
//        }
//    }

//    private async void LoginBtn_Click(object sender, RoutedEventArgs e)
//    {
//        try
//        {
//            //await HubProxy.Invoke("Login", LoginTextBox.Text, PasswordTextBox.Password);
//        }
//        catch (NullReferenceException ex)
//        {
//            MessageBox.Show(ex.Message);
//        }
//    }

//    private async void LoginVisibility(bool valid)
//    {
//        if (valid)
//        {
//            //username = LoginTextBox.Text;
//            //username_lbl.Content = $"Logged as {username}.";
//            //ErrorValidatoin.Visibility = Visibility.Hidden;
//            //login.Visibility = Visibility.Hidden;
//
//        }
//        else
//        {
//            //ErrorValidatoin.Content = "User is already connected";
//            //ErrorValidatoin.Visibility = Visibility.Visible;
//            //login.Visibility = Visibility.Visible;
//        }
//    }

//    private void AddRoom(object sender, RoutedEventArgs e)
//    {
//        var list = HubProxy.Invoke<List<UserDTO>>("LoadUsers", username).Result;
//        CreateRoomWindow win = new CreateRoomWindow(list, username);
//        win.ShowDialog();
//        if (win.DialogResult == true)
//        {
//            HubProxy.Invoke("CreateRoom", win.TempRoom);
//        }
//    }

//
//}