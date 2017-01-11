using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace OpenChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ChatClientInitalizer chatInit { get; set; }
        public bool LoginOverVisible = true;
        public bool ErrorValidationLabelVisibility = false;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

             chatInit = new ChatClientInitalizer("http://localhost:11878/");

            var mychat = chatInit.connection;
            var mychatProxy = chatInit.chat;
            mychatProxy.On<bool>("login", (bool valid) => {
                if (valid)
                    this.LoginOverVisible = false;
                else
                    this.ErrorValidationLabelVisibility = true;
            });
            mychatProxy.On<string>("send", Console.WriteLine);
            mychatProxy.On("send", () =>
                {
                    ChatView.Text += MessageTextBox.Text;
                    MessageTextBox.Text = "";
                });
            mychat.Start();
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