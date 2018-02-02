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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bridge
{
    /// <summary>
    /// Логика взаимодействия для chooseUser.xaml
    /// </summary>
    public partial class chooseUser : Window
    {
        public ObservableCollection<OnePeople> full { get; set; }
        public ObservableCollection<OnePeople> friend { get; set; }
        public ObservableCollection<OnePeople> other { get; set; }

        private bool useFriends;
        BridgeClient eng;
        MainWindow mw;
        Action<string> act;
        public chooseUser(bool b, BridgeClient e, MainWindow mm, Action<string> a)
        {
            full = new ObservableCollection<OnePeople>();
            friend = new ObservableCollection<OnePeople>();
            other = new ObservableCollection<OnePeople>();
            act = a;
            useFriends = b;
            eng = e;
            mw = mm;
            InitializeComponent();
            clearThis();
        }

        private void searchText()
        {
            clearThis();
            if (useFriends == true)
                lbsecond.ItemsSource = friend;

            string income = tbIncome.Text;
            if (income == null || income.Replace(" ", "") == "")
            {
                clearThis();
                tbIncome.Text = "";
                return;
            }

            income = income.Replace(" ", "");

            List<string> re = new List<string>();
            int countFull = 0, countFriend = 0, countOther = 0;
            foreach (var item in eng.map)
            {
                if (item.Value.id == income)
                {
                    re.Add(item.Key);
                    countFull++;
                    full.Add(new OnePeople(item.Value.id, item.Value.name, item.Value.Pict));
                }
            }
            foreach (var item in mw.friends)
            {
                if (item.id.Contains(income))
                {
                    re.Add(item.id);
                    countFriend++;
                    friend.Add(new OnePeople(eng.map[item.id].id, eng.map[item.id].name, eng.map[item.id].Pict));
                }
            }
            foreach(var item in eng.map)
            {
                if (item.Value.id.Contains(income) && re.Contains(item.Value.id) == false)
                {
                    re.Add(item.Key);
                    countOther++;
                    other.Add(new OnePeople(item.Value.id, item.Value.name, item.Value.Pict));
                }
            }



        }

        private void tbIncome_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                searchText();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            searchText();
        }

        private void clearThis()
        {
            full.Clear();
            lbfirst.ItemsSource = full;

            lbsecond.ItemsSource = this.mw.friends;
            if (this.useFriends)
            {
                lbsecond.IsEnabled = true;
            }
            else
            {
                lbsecond.IsEnabled = false;
            }
            friend.Clear();

            other.Clear();
            lbthird.ItemsSource = other;

            //tbIncome.Text = "";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            clearThis();
            tbIncome.Text = "";
        }

        private void confirm()
        {
            if (lbfirst.SelectedItem == null && lbsecond.SelectedItem == null && lbthird.SelectedItem == null)
                this.Close();
            else
            {
                string re = "";
                if (lbfirst.SelectedItem != null)
                    re = ((OnePeople)lbfirst.SelectedItem).id;
                if (lbsecond.SelectedItem != null)
                    re = ((OnePeople)lbsecond.SelectedItem).id;
                if (lbthird.SelectedItem != null)
                    re = ((OnePeople)lbthird.SelectedItem).id;
                act.Invoke(re);
                this.Close();
            }
        }

        private void lbfirst_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            confirm();
        }

        private void lbsecond_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            confirm();
        }

        private void lbthird_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            confirm();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            confirm();
        }

        private void lbfirst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbfirst.SelectedItem != null)
            {
                lbsecond.SelectedIndex = -1;
                lbthird.SelectedIndex = -1;
            }
        }

        private void lbsecond_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbsecond.SelectedItem != null)
            {
                lbfirst.SelectedIndex = -1;
                lbthird.SelectedIndex = -1;
            }
        }

        private void lbthird_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbthird.SelectedItem != null)
            {
                lbfirst.SelectedIndex = -1;
                lbsecond.SelectedIndex = -1;
            }
        }
    }


}
