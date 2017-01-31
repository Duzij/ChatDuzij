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

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for CreateRoomWindow.xaml
    /// </summary>
    public partial class CreateRoomWindow
    {
        public CreateRoomWindow()
        {
            InitializeComponent();
        }
    }
}







//public partial class CreateRoomWindow : Window
//{
//    public ObservableCollection<UserDTO> avalibleUsers = new ObservableCollection<UserDTO>();
//    public string AuthorUser { get; set; }

//    public CreateRoomWindow(List<UserDTO> users, string author)
//    {
//        InitializeComponent();
//        DataContext = this;
//        this.AuthorUser = author;
//        avalibleUsers = new ObservableCollection<UserDTO>(users);
//        usersDataGrid.ItemsSource = avalibleUsers;
//    }

//    public CreateRoomDTO TempRoom { get; set; }

//    private void AddRoomBtn_Click(object sender, RoutedEventArgs e)
//    {
//        this.avalibleUsers.Where(a => a.Username == AuthorUser).First().IsSelected = true;
//        this.TempRoom = new CreateRoomDTO() { Name = RoomName.Text, Users = avalibleUsers.Select(a => a.Username).ToList() };
//        this.DialogResult = true;
//        this.Close();
//    }

//    private void CancelBtn_Click(object sender, RoutedEventArgs e)
//    {
//        this.Close();
//    }
//}