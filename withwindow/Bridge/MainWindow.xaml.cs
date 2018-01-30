using System;
using System.Collections.Generic;
using System.Collections;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Bridge
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    public class oneChat:INotifyPropertyChanged
    {
        public string id { get; set; }
        public string name { get; set; }
        private string _image;
        public string image {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                OnPropertyChanged("image");
            }
        }
        private bool _isnotread = false;
        public bool isNotRead
        {
            get
            {
                return _isnotread;
            }
            set
            {
                _isnotread = value;
                if (_isnotread == false)
                    image = "resources/readmsg.png";
                else
                    image = "resources/newmsg.png";
                OnPropertyChanged("isNotRead");
            }
        }
        public oneChat(string id)
        {
            this.id = id;
            name = this.id;
            image = "resources/readmsg.png";
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class OneUser
    {
        public string id { get; set; }
        public string name { get; set; }
        public OneUser(string id)
        {
            this.id = id;
            name = this.id;
        }
    }

    public class oneMsg
    {
        public string idfrom { get; set; }
        public string namefrom { get; set; }
        public string login { get; set; }
        public string textmsg { get; set; }
        public bool fromSelf { get; set; }
        public oneMsg(string id, string text, string login)
        {
            idfrom = id;
            namefrom = id;
            this.login = login;
            textmsg = text;
            if (idfrom == login)
                fromSelf = true;
            else
                fromSelf = false;
        }
    }

    /*
     * <DataTemplate.Triggers>
                                    <DataTrigger Binding="{Binding fromSelf}" Value="true">
                                        <Setter TargetName="panelmsg" Property="DockPanel.Dock" Value="Right"></Setter>
                                        <Setter TargetName="panelmsg" Property="Background" Value="Red"/>
                                    </DataTrigger>
                                </DataTemplate.Triggers>

        <DockPanel>
                                    <StackPanel Name="panelmsg" DockPanel.Dock="Left" HorizontalAlignment="Stretch" Width="300" Background="Aqua">
                                        <TextBlock FontStyle="Italic" FontSize="10" Text="{Binding Path=namefrom}"></TextBlock>
                                        <TextBlock FontSize="16" Text="{Binding Path=textmsg}"></TextBlock>
                                    </StackPanel>
                                </DockPanel>
     * */

    public partial class MainWindow : Window
    {
        public ObservableCollection<oneChat> chates { get; set; }
        public ObservableCollection<oneMsg> msges { get; set; }
        public ObservableCollection<OneUser> users { get; set; }
        BridgeClient enginee;
        public MainWindow()
        {
            InitializeComponent();
            chates = new ObservableCollection<oneChat>();
            msges = new ObservableCollection<oneMsg>();
            chatsList.ItemsSource = chates;
            msgsList.ItemsSource = msges;
            enginee = new BridgeClient(chates, msges, this);
            users = new ObservableCollection<OneUser>();
            lbUsersKek.ItemsSource = users;
            //chatsList.Focus();
            btnAddUser.Visibility = Visibility.Hidden;
            btnLeaveChat.Visibility = Visibility.Hidden;
            btnShowUsers.Visibility = Visibility.Hidden;
            msgsList.Visibility = Visibility.Hidden;
            btnSendMsg.Visibility = Visibility.Hidden;
            boxtomsg.Visibility = Visibility.Hidden;
        }

        private void createChatKek(string s)
        {
            enginee._createNewChat(s);
        }

        private void chatsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            oneChat one = (oneChat)chatsList.SelectedItem;
            //MessageBox.Show(one.id);
            //msges.Clear();
            if (one != null)
            {
                enginee.setCurrentChat(one.id);
                one.isNotRead = false;
                btnAddUser.Visibility = Visibility.Visible;
                btnLeaveChat.Visibility = Visibility.Visible;
                btnShowUsers.Visibility = Visibility.Visible;
                msgsList.Visibility = Visibility.Visible;
                btnSendMsg.Visibility = Visibility.Visible;
                boxtomsg.Visibility = Visibility.Visible;

            }
            else
            {
                enginee.setCurrentChat("/none");
                btnAddUser.Visibility = Visibility.Hidden;
                btnLeaveChat.Visibility = Visibility.Hidden;
                btnShowUsers.Visibility = Visibility.Hidden;
                msgsList.Visibility = Visibility.Hidden;
                btnSendMsg.Visibility = Visibility.Hidden;
                boxtomsg.Visibility = Visibility.Hidden;
            }
        }

        private void buttontest_click(object sender, RoutedEventArgs e)
        {
            Window1 w1 = new Window1(enginee);
            w1.Owner = this;
            w1.Show();
            w1.Focus();
            w1.tbName.Focus();
            w1.Top = this.Top + 100;
            w1.Left = this.Left + 100;
           // chates.Add(new oneChat { id = "roflanebalo", name = "rofl", image = "resources/chat.png" });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            enginee._sendMsgFromClient(boxtomsg.Text);
            boxtomsg.Clear();
        }

        private void Button_KeyUp(object sender, KeyEventArgs e)
        {
            
        }

        private void boxtomsg_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                enginee._sendMsgFromClient(boxtomsg.Text);
                boxtomsg.Clear();
            }
        }

        private void btnLeaveChat_Click(object sender, RoutedEventArgs e)
        {
            enginee._leaveCurrentChat();
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            addUserWindow auw = new addUserWindow(enginee);
            auw.Owner = this;
            auw.Show();
            auw.Focus();
            auw.tbPerson.Focus();
            auw.Top = this.Top + 100;
            auw.Left = this.Left + 100;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            loginWindow lw = new loginWindow(enginee);
            lw.Top = this.Top + 100;
            lw.Left = this.Left + 100;
            lw.Owner = this;
            lw.Show();
            lw.Focus();
            lw.tbLogin.Focus();
            System.Windows.Forms.Timer tim = new System.Windows.Forms.Timer();
            tim.Tick += new EventHandler(enginee._sendEmpty);
            tim.Interval = 20000;
            tim.Start();

        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            
        }

        private void btnShowUsers_MouseEnter(object sender, MouseEventArgs e)
        {
            lbUsersKek.Visibility = Visibility.Visible;
        }

        private void btnShowUsers_MouseLeave(object sender, MouseEventArgs e)
        {
            lbUsersKek.Visibility = Visibility.Hidden;
        }

        private void btnShowUsers_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
