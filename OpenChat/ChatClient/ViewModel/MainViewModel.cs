using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Messaging;
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
        private ObservableCollection<MessageDTO> messagers = new ObservableCollection<MessageDTO>();
        private ObservableCollection<RoomDTO> rooms = new ObservableCollection<RoomDTO>();
        private RoomDTO selectedRoom = new RoomDTO();
        private string username = string.Empty;
        private string myMessage = string.Empty;

        public MainViewModel(IChatClientService chatService)
        {
            this.chatService = chatService;

            Messenger.Default.Register<NotificationMessage<string>>(this, (usernameMsg) =>
            {
                Username = usernameMsg.Content;
                chatService.chatProxy.Invoke("LoadRooms", Username);
            });

            Messenger.Default.Register<NotificationMessage<CreateRoomDTO>>(this, (roomToCreate) =>
            {
                chatService.chatProxy.Invoke("CreateRoom", roomToCreate.Content);
            });

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
                        LoadMessages();
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
        }

        public ObservableCollection<RoomDTO> Rooms
        {
            get { return rooms; }
            set { Set(ref rooms, value); }
        }

        public ObservableCollection<MessageDTO> Messages
        {
            get { return messagers; }
            set { Set(ref messagers, value); }
        }

        public RoomDTO SelectedRoom
        {
            get { return selectedRoom; }
            set { Set(ref selectedRoom, value); }
        }

        public string Username
        {
            get { return username; }
            set { Set(ref username, value); }
        }

        public string MyMessage
        {
            get { return myMessage; }
            set { Set(ref myMessage, value); }
        }

        public RelayCommand SendMessageCommand => new RelayCommand(SendMessage);
        public RelayCommand AddRoomCommand => new RelayCommand(AddRoom);
        public RelayCommand LoadRoomMessages => new RelayCommand(LoadMessages);

        public void LoadMessages()
        {
            chatService.chatProxy.Invoke<ObservableCollection<MessageDTO>>("LoadRoomMessages", SelectedRoom.RoomName, Username);
        }

        private void SendMessage()
        {
            if (!string.IsNullOrWhiteSpace(MyMessage))
                chatService.chatProxy.Invoke("SendMessage", SelectedRoom.RoomName, MyMessage, Username);
        }

        private void AddRoom()
        {
            var win = new CreateRoomWindow();
            Messenger.Default.Send(new NotificationMessage<string>(Username, "token"));

            win.Show();
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

//

//
//}