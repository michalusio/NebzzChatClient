using NebzzClient.Messages;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace NebzzClientFront
{
    public partial class MainControl : UserControl
    {
        public BindingList<NotifierMessage> Messages { get; set; } = new BindingList<NotifierMessage>();
        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();

        private readonly DispatcherTimer timer;

        public MainControl()
        {
            InitializeComponent();
            foreach (var user in MessageRepo.Instance.Users)
            {
                Users.Add(new User(user));
            }
            MessageRepo.Instance.MessageEvent += Instance_MessageEvent;
            MessageRepo.Instance.UserEvent += Instance_UserEvent;

            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(9) };
            timer.Tick += Timer_Tick;
            timer.Start();

            textBoxMessage.Focus();
        }

        public void StopTimer()
        {
            timer.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var eventArgs = new PropertyChangedEventArgs(nameof(NotifierMessage.MetaInfo));
            foreach (var message in Messages)
            {
                message.Notify(this, eventArgs);
            }
        }

        private void Instance_UserEvent(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                Users.Clear();
                foreach (var user in MessageRepo.Instance.Users)
                {
                    Users.Add(new User(user));
                }
            });
        }

        private void Instance_MessageEvent(object sender, EventArgs e)
        {
            var groupedMessages = MessageRepo.Instance.Messages
                    .GroupTogether((previous, current) => current.Author == previous.Author && current.Thumbnail == previous.Thumbnail && (current.UtcTime - previous.UtcTime).Minutes < 5)
                    .Select(msgGroup => new NotifierMessage(msgGroup.key.Author, msgGroup.key.UtcTime, string.Join(Environment.NewLine, msgGroup.group.Select(msg => msg.Text)), msgGroup.key.Thumbnail))
                    .ToList();
            Dispatcher.Invoke(() =>
            {
                Messages.Clear();
                foreach (var message in groupedMessages)
                {
                    Messages.Add(message);
                }
            });
        }

        private void TextBoxMessage_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                e.Handled = true;
                if (!string.IsNullOrWhiteSpace(textBoxMessage.Text))
                {
                    MessageRepo.Instance.SendMessage(textBoxMessage.Text.Trim());
                    textBoxMessage.Text = string.Empty;
                }
            }
        }
    }
}
