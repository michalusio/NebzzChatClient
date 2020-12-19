using NebzzClient;
using NebzzClient.Handlers;
using NebzzClient.Messages;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace NebzzClientFront
{
    public partial class MainWindow : Window
    {
        internal ObservableCollection<Message> Messages { get; private set; } = new ObservableCollection<Message>();
        internal ObservableCollection<User> Users { get; private set; } = new ObservableCollection<User>();

        public MainWindow()
        {
            Connection.Instance.AddHandler<WarnHandler>();
            Connection.Instance.AddHandler<ErrorHandler>();
            Connection.Instance.AddHandler<MotdHandler>();
            Connection.Instance.AddHandler<RegisterHandler>();
            Connection.Instance.AddHandler<LoginHandler>();
            Connection.Instance.AddHandler<InfoHandler>();
            Connection.Instance.AddHandler<BroadcastLoginHandler>();
            Connection.Instance.AddHandler(new BroadcastMessageHandler(new WindowInteropHelper(this).Handle));
            InitializeComponent();

            var login = new LoginControl();
            login.LoggedEvent += Login_LoggedEvent;
            ContentArea.Content = login;
        }

        private void Login_LoggedEvent(object sender, EventArgs e)
        {
            ContentArea.Content = new MainControl();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ContentArea.Content is MainControl mc)
            {
                try
                {
                    mc.StopTimer();
                    Console.WriteLine("Disconnecting");
                    Task.Run(async () => await Connection.Instance.DisconnectAsync()).Wait();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}
