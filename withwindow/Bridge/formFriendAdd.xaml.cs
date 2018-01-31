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

namespace Bridge
{
    /// <summary>
    /// Логика взаимодействия для formFriendAdd.xaml
    /// </summary>
    public partial class formFriendAdd : Window
    {
        public BridgeClient eng;
        public formFriendAdd(BridgeClient e)
        {
            eng = e;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            eng._newFriend(tbName.Text);
            this.Close();
        }

        private void tbName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                eng._newFriend(tbName.Text);
                this.Close();
            }
        }
    }
}
