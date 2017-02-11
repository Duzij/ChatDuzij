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
        private RoomDTO selectedRoom;
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
            if (!string.IsNullOrWhiteSpace(MyMessage) && SelectedRoom != null)
            {
                chatService.chatProxy.Invoke("SendMessage", SelectedRoom.RoomName, MyMessage, Username);
                MyMessage = "";
            }
        }

        private void AddRoom()
        {
            var win = new CreateRoomWindow();
            Messenger.Default.Send(new NotificationMessage<string>(Username, "token"));

            win.Show();
        }
    }
}