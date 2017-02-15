using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Input;
using ChatClient.ViewModel;
using GalaSoft.MvvmLight.Messaging;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();

            ((INotifyCollectionChanged)ChatView.Items).CollectionChanged += new NotifyCollectionChangedEventHandler(ChatViewSourceChanged);

            Messenger.Default.Register<NotificationMessage>(this, CreateRoomViewModelMessage);
        }

        private void CreateRoomViewModelMessage(NotificationMessage msg)
        {
            if (msg.Notification == "ShowCreateRoomView")
            {
                var CreateRoom = new CreateRoomWindow();
                CreateRoom.Show();
            }
        }

        private void ChatViewSourceChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                ChatView.Items.MoveCurrentToLast();
                ChatView.ScrollIntoView(ChatView.Items.CurrentItem);
            }
        }

        private void Contacts_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var viewModel = (MainViewModel)DataContext;
            if (viewModel.LoadRoomMessagesCommand.CanExecute(null))
                viewModel.LoadRoomMessagesCommand.Execute(null);
        }
    }
}