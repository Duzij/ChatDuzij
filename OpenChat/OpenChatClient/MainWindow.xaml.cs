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

namespace OpenChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool loginOverVisible;
        private bool errorValidationLabelVisibility;

        public MainWindow()
        {
            InitializeComponent();

            this.Users.AddRange(
                new List<Conversation>()
                {
                    new Conversation() { GotNewMessages = true, RoomName = "Vole"},
                    new Conversation() { GotNewMessages = false, RoomName = "SUka, tohle je proste nejdelsí"},
                    new Conversation() { GotNewMessages = true, RoomName = "Jetpacks room"}
                });

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

        public List<Conversation> Users { get; set; }

        private void Contacts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //TODO join room and room data load to chat
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