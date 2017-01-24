using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OpenChatClient.Models;

namespace OpenChatClient
{
    /// <summary>
    /// Interaction logic for CreateRoomWindow.xaml
    /// </summary>
    public partial class CreateRoomWindow : Window
    {
        public ObservableCollection<UserDTO> avalibleUsers = new ObservableCollection<UserDTO>();

        public CreateRoomWindow(List<UserDTO> users)
        {
            InitializeComponent();
            avalibleUsers = new ObservableCollection<UserDTO>(users);
            usersDataGrid.ItemsSource = users;
        }

        public Room TempRoom { get; set; }

        private void AddRoomBtn_Click(object sender, RoutedEventArgs e)
        {
            this.TempRoom = new RoomDTO() { RoomName = RoomName.Text, Users = avalibleUsers };
            this.Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}