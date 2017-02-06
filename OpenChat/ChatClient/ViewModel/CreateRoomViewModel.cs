using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using OpenChatClient;
using OpenChatClient.Models;

namespace ChatClient.ViewModel
{
    public class CreateRoomViewModel : ViewModelBase
    {
        private CreateRoomDTO createdRoom = new CreateRoomDTO();
        private ObservableCollection<UserDTO> avalibleUsers = new ObservableCollection<UserDTO>();
        private string username = string.Empty;
        private string roomName = string.Empty;

        public CreateRoomViewModel(IChatClientService chatService)
        {
            ChatService = chatService;

            Messenger.Default.Register<NotificationMessage<string>>(this, (usernameMsg) =>
            {
                Username = usernameMsg.Content;
                AvalibleUsers.First(a => a.Username == Username).IsSelected = true;
            });

            AvalibleUsers = new ObservableCollection<UserDTO>(chatService.chatProxy.Invoke<List<UserDTO>>("LoadUsers", Username).Result);
        }

        public RelayCommand CreateRoomCommand => new RelayCommand(CreateRoom);

        public IChatClientService ChatService { get; set; }

        public string Username
        {
            get { return username; }
            set { Set(ref username, value); }
        }

        public string RoomName
        {
            get { return roomName; }
            set { Set(ref roomName, value); }
        }

        public CreateRoomDTO SelectedRoom
        {
            get { return createdRoom; }
            set { Set(ref createdRoom, value); }
        }

        public ObservableCollection<UserDTO> AvalibleUsers
        {
            get { return avalibleUsers; }
            set { Set(ref avalibleUsers, value); }
        }

        private void CreateRoom()
        {
            var list = avalibleUsers.Where(a => a.IsSelected).Select(b => b.Username).ToList();
            list.Add(Username);

            Messenger.Default.Send(new NotificationMessage<CreateRoomDTO>(new CreateRoomDTO()
            {
                Name = RoomName,
                Users = list
            }, "CrateRoomToken"));
        }
    }
}