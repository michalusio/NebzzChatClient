using NebzzClient;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NebzzClientFront
{
    public partial class LoginControl : UserControl
    {
        public event EventHandler LoggedEvent;

        public LoginControl()
        {
            InitializeComponent();
            textBoxUsername.Focus();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Connection.Instance.ConnectAndLoginAsync(new Uri(textBoxURL.Text), textBoxUsername.Text, textBoxPassword.Password, false);
                LoggedEvent?.Invoke(this, new EventArgs());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Connection.Instance.ConnectAndLoginAsync(new Uri(textBoxURL.Text), textBoxUsername.Text, textBoxPassword.Password, true);
                LoggedEvent?.Invoke(this, new EventArgs());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                e.Handled = true;
                Login_Click(null, null);
            }
        }
    }
}
