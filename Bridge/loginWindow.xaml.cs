using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bridge
{
    /// <summary>
    /// Логика взаимодействия для loginWindow.xaml
    /// </summary>
    public partial class loginWindow : Window
    {
        public BridgeClient eng;
        public loginWindow(BridgeClient e)
        {
            eng = e;
            InitializeComponent();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            eng._testLogIn(tbLogin.Text, tbPassword.Text, this);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (eng.authorized == false)
                this.Owner.Close();
            else
                this.Owner.Visibility = Visibility.Visible;
        }

        private void tbPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                eng._testLogIn(tbLogin.Text, tbPassword.Text, this);
        }

        private void tbLogin_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                tbPassword.Focus();
        }

        private void tbRegLogin_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                tbRegPass.Focus();
        }

        private void tbRegPass_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                tbRegMail.Focus();
        }

        private void tbRegMail_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if (tbRegLogin.Text == "" || tbRegPass.Text == "" || tbRegMail.Text == "" || IsValidEmail(tbRegMail.Text) == false || (tbRegLogin.Text.IndexOf(' ') >= 0) || (tbRegPass.Text.IndexOf(' ') >= 0))
                {
                    /*ToolTip toolTip = new ToolTip();
                    StackPanel toolTipPanel = new StackPanel();
                    toolTipPanel.Children.Add(new TextBlock { Text = "Введите корректные данные", FontSize = 16 });
                    toolTip.Content = toolTipPanel;
                    btnReg.ToolTip = toolTip;*/
                    return;
                }
                
                
                eng._regUserKek(tbRegLogin.Text, tbRegPass.Text, tbRegMail.Text);
                Tab1.SelectedIndex = 0;
                tbRegLogin.Clear();
                tbRegPass.Clear();
                tbRegMail.Clear();
            }
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            eng._regUserKek(tbRegLogin.Text, tbRegPass.Text, tbRegMail.Text);
            tbRegLogin.Clear();
            tbRegPass.Clear();
            tbRegMail.Clear();
        }

        private void tbLogin_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbLogin.Text == "login")
                tbLogin.Clear();
        }

        private void tbPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbPassword.Text == "password")
                tbPassword.Clear();
        }

        private void tbRegLogin_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbRegLogin.Text == "login")
                tbRegLogin.Clear();
        }

        private void tbRegPass_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbRegPass.Text == "password")
                tbRegPass.Clear();
        }

        private void tbRegMail_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbRegMail.Text == "email")
                tbRegMail.Clear();
        }
        public bool IsValidEmail(string email)
        {
            string pattern = @"^[-a-zA-Z0-9][-.a-zA-Z0-9]*@[-.a-zA-Z0-9]+(\.[-.a-zA-Z0-9]+)*\.
    (com|edu|info|gov|int|mil|net|org|biz|name|museum|coop|aero|pro|tv|[a-zA-Z]{2})$";

            Regex check = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);
            bool valid = false;

            if (string.IsNullOrEmpty(email)) valid = false;
            else valid = check.IsMatch(email);

            return valid;
        }
    }
}
