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
        private string username;
        private string myMessage;

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

            chatService.chatProxy.On("LeaveRoom", (roomName) =>
            {
                App.Current.Dispatcher.InvokeAsync(() =>
                {
                    Rooms.Remove(Rooms.Where(a => a.RoomName == roomName).FirstOrDefault());
                    Messages = null;
                });
            });

            chatService.chatProxy.On("Notify", (string RoomDTOName) =>
            {
                App.Current.Dispatcher.InvokeAsync(() =>
                {
                    if (SelectedRoom != null)
                        LoadMessages();

                    if (SelectedRoom.RoomName != RoomDTOName)
                        Rooms.First(a => a.RoomName == RoomDTOName).GotNewMessages = true;
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
        public RelayCommand LoadRoomMessagesCommand => new RelayCommand(LoadMessages);
        public RelayCommand LeaveRoomCommand => new RelayCommand(LeaveRoom);

        private void LeaveRoom()
        {
            chatService.chatProxy.Invoke("LeaveRoom", SelectedRoom.RoomName, Username);
        }

        public void LoadMessages()
        {
            chatService.chatProxy.Invoke<ObservableCollection<MessageDTO>>("LoadRoomMessages", SelectedRoom.RoomName, Username);
            Rooms.First(r => r.GotNewMessages == false);
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
            Messenger.Default.Send(new NotificationMessage("ShowCreateRoomView"));
            Messenger.Default.Send(new NotificationMessage<string>(Username, "token"));
        }
    }
}