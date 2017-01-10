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
        public MainWindow()
        {
            InitializeComponent();

        }

        public async void InitalizeConnnection()
        {

            var connection = new HubConnection("http://localhost:11878/");
            IHubProxy chat = connection.CreateHubProxy("Chat");

            chat.On<string>("send", Console.WriteLine);

            chat.On("send", () =>
            {

            });

            await connection.Start();
            await chat.Invoke("send", MessageTextBox.Text);
        }

        private void Contacts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //TODO join room and room data load to chat
        }

        private void sendbnn_Click(object sender, RoutedEventArgs e)
        {

        

        }
    }
}