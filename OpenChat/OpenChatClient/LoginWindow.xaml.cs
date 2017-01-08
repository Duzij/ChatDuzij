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
using System.Windows.Shapes;
using Microsoft.AspNet.SignalR.Client;
using System.Runtime.Remoting.Contexts;
using System.Threading;

namespace OpenChatClient
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        public HubConnection Connection { get; set; }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            //https://www.asp.net/signalr/overview/guide-to-the-api/hubs-api-guide-net-client
            //https://www.asp.net/signalr/overview/guide-to-the-api/hubs-api-guide-server#signalrurl

            var connection = new HubConnection("http://localhost:11878/");
            IHubProxy chat = connection.CreateHubProxy("Chat");

            chat.On<string>("send", Console.WriteLine);

            await connection.Start();

            await chat.Invoke("send", nickTextBox.Text);


        }
    }
}