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
using OpenChatClient.Model;
using System.Collections.ObjectModel;

namespace OpenChatClient
{
    public partial class MainWindow : Window
    {
        private bool loginOverVisible;
        private bool errorValidationLabelVisibility;

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            data = new ObservableCollection<RoomDTO>()
                {
                    new RoomDTO() {Type=OpenChat.Models.RoomType.Group, GotNewMessages = true, RoomName = "Vole"},
                    new RoomDTO() { Type=OpenChat.Models.RoomType.Private, GotNewMessages = false, RoomName = "SUka, tohle je proste nejdelsí"},
                    new RoomDTO() { Type = OpenChat.Models.RoomType.Public, GotNewMessages = true, RoomName = "Jetpacks room"}
                };

            Contacts.ItemsSource = data;

            chatInit = new ChatClientInitalizer("http://localhost:11878/");

            var mychat = chatInit.connection;
            var mychatProxy = chatInit.chat;
            mychatProxy.On<bool>("login", (bool valid) =>
            {
                if (valid)
                    this.LoginOverVisible = false;
                else
                    this.ErrorValidationLabelVisibility = true;
            });
            mychatProxy.On<string>("send", Console.WriteLine);
            mychatProxy.On("send", () =>
            {
                Dispatcher.InvokeAsync(() =>
                    {
                        ChatView.Text += MessageTextBox.Text;
                        MessageTextBox.Text = "";
                    });
            });
            mychat.Start();
        }

        public bool ErrorValidationLabelVisibility
        {
            get { return errorValidationLabelVisibility; }
            set { errorValidationLabelVisibility = value; }
        }

        public bool LoginOverVisible
        {
            get { return loginOverVisible; }
            set { loginOverVisible = value; }
        }

        public ChatClientInitalizer chatInit { get; set; }

        ObservableCollection<RoomDTO> data = new ObservableCollection<RoomDTO>();

        public ObservableCollection<RoomDTO> Data
        {
            get
            {
                return data;
            }
        }



        private void Contacts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //TODO join room and room data load to chat
            //Todo on double click data loads to chatTextBox (if hw choose from rooms)
        }

        private async void sendbnn_Click(object sender, RoutedEventArgs e)
        {
            await chatInit.chat.Invoke("send", MessageTextBox.Text);
        }

        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            this.ErrorValidationLabelVisibility = false;
            await chatInit.chat.Invoke("login", LoginTextBox.Text, PasswordTextBox.Text);
        }
    }
}