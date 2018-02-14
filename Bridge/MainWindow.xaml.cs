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
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace Bridge
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    public class oneChat:INotifyPropertyChanged
    {
        private BridgeClient enginee;
        public string id { get; set; }
        private string _name;
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("name");
            }
        }
        private string nick;
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
        public void updateIcon(BridgeClient e)
        {
            if (id.StartsWith("pm"))
            {
                if (image != e.map[nick].pict)
                    image = e.map[nick].pict;
                if (name != e.map[nick].name)
                    name = "/pm "+e.map[nick].name;
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
               /* if (_isnotread == false)
                    image = "resources/readmsg.png";
                else
                    image = "resources/newmsg.png";*/
                OnPropertyChanged("isNotRead");
            }
        }
        public oneChat(string id, string mylogin, BridgeClient e)
        {
            this.id = id;
            this.enginee = e;
            image = "resources/readmsg.png";
            if (id.StartsWith("gr") == true)
            {
                name = id.Remove(0, 2);
            }
            else
            {
                if (id.StartsWith("pm") == true)
                {
                    string newid = id.Remove(0, 2);
                    string[] qqq = newid.Split('|');
                    if (qqq[0] == mylogin)
                    {
                        name = "/pm " + qqq[1];
                        nick = qqq[1];
                        image = this.enginee.map[qqq[1]].pict;
                    }
                    else
                    {
                        name = "/pm " + qqq[0];
                        nick = qqq[0];
                        image = this.enginee.map[qqq[0]].pict;
                    }
                }
                else
                    name = id;

            }
           
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    [DataContract]
    public class OnePeople:INotifyPropertyChanged
    {
        private BridgeClient eng;

        [DataMember]
        public string id { get; set; }
        private string _name;
        [DataMember]
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("name");
            }
        }
        [DataMember]
        public string pict { get; set; }
        public OnePeople(BridgeClient e)
        {
            eng = e;
            id = null;
            name = null;
            pict = null;
        }
        public string Pict
        {
            get
            {
                return pict;
            }
            set
            {
                pict = value;
                OnPropertyChanged("pict");
            }
        }

        public void updateIcons(BridgeClient e)
        {
            if(Pict!= e.map[id].pict)
            {
                Pict = e.map[id].pict;
            }
            if (name != e.map[id].name)
                name = e.map[id].name;
        }

        public OnePeople(string idd, string namee, string pictt)
        {
            id = idd;
            name = namee;
            pict = pictt;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class OneUser:INotifyPropertyChanged
    {
        BridgeClient eng;
        public string id { get; set; }
        private string _name;
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("name");
            }
        }
        private string _pict;
        public string pict {
            get
            {
                return _pict;
            }
            set
            {
                _pict = value;
                OnPropertyChanged("pict");
            }
        }
        public OneUser(string id, BridgeClient e)
        {
            eng = e;
            this.id = id;
            name = this.id;
            _pict = eng.map[id].pict;
        }

        public void updateIcons(BridgeClient e)
        {
            if(pict != e.map[id].pict)
            {
                pict = e.map[id].pict;
            }
            if (name != e.map[id].name)
                name = e.map[id].name;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class oneMsg:INotifyPropertyChanged
    {
        BridgeClient eng;
        public string idfrom { get; set; }
        private string _namefrom;
        private string _time;
        public string time
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged("time");
            }
        }
        public string name
        {
            get
            {
                return _namefrom;
            }
            set
            {
                _namefrom = value;
                OnPropertyChanged("name");
            }
        }
        public string login { get; set; }
        public string textmsg { get; set; }
        public bool fromSelf { get; set; }
        private string _pict;
        public string pict
        {
            get
            {
                return _pict;
            }
            set
            {
                _pict = value;
                OnPropertyChanged("pict");
            }
        }
        public oneMsg(string id, string text, string ttime, string login, BridgeClient e)
        {
            string[] retime = ttime.Split(new char[] { ' ', ':' });
            time = " at " + retime[4] + ":" + retime[5] + " of " + retime[0] + " " + retime[2] + " " + retime[1] + " " + retime[3];
            eng = e;
            idfrom = id;
            name = id;
            this.login = login;
            textmsg = text;
            if (idfrom == login)
                fromSelf = true;
            else
                fromSelf = false;
            pict = this.eng.map[idfrom].pict;
        }

        public void updateIcons(BridgeClient e)
        {
            if(pict != e.map[idfrom].pict)
            {
                pict = e.map[idfrom].pict;
            }
            if (name != e.map[idfrom].name)
                name = e.map[idfrom].name;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
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
        public bool isSettingImageChaged = false;
        public ObservableCollection<OneUser> users { get; set; }
        public ObservableCollection<OnePeople> friends { get; set; }
        BridgeClient enginee;
        public MainWindow()
        {
            InitializeComponent();
            chates = new ObservableCollection<oneChat>();
            msges = new ObservableCollection<oneMsg>();
            friends = new ObservableCollection<OnePeople>();
            chatsList.ItemsSource = chates;
            msgsList.ItemsSource = msges;
            enginee = new BridgeClient(chates, msges, this);
            users = new ObservableCollection<OneUser>();
            lbUsersKek.ItemsSource = users;
            //lbFriends.Items.Clear();
            lbFriends.ItemsSource = friends;
            //chatsList.Focus();
            btnAddUser.Visibility = Visibility.Hidden;
            btnLeaveChat.Visibility = Visibility.Hidden;
            btnShowUsers.Visibility = Visibility.Hidden;
            msgsList.Visibility = Visibility.Hidden;
            btnSendMsg.Visibility = Visibility.Hidden;
            boxtomsg.Visibility = Visibility.Hidden;
            btnFriendsDelete.Visibility = Visibility.Hidden;
            btnFriendsSend.Visibility = Visibility.Hidden;
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
                if (one.id.StartsWith("gr"))
                    btnAddUser.Visibility = Visibility.Visible;
                else
                    btnAddUser.Visibility = Visibility.Hidden;
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
            if ((string)btnLeaveChat.Content == "Leave chat")
            {
                enginee._leaveCurrentChat();
            } else
            {
                enginee._delChatByMaker();
            }
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            chooseUser cu = new chooseUser(true, enginee, this, new Action<string>(re => {
                enginee._addPersonToCurrentChat(re);
            }));

            cu.Show();
            cu.Focus();
            cu.tbIncome.Focus();
            cu.Top = this.Top + 100;
            cu.Left = this.Left + 100;
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

        private void imgSettingPict_Drop(object sender, DragEventArgs e)
        {
            //imgSettingPict.Source = (ImageSource)e.Data.GetData(typeof(ImageSource));
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files[0].IndexOf(".jpeg") != -1 || files[0].IndexOf(".png") != -1 || files[0].IndexOf(".jpg") != -1 || files[0].IndexOf(".gif") != -1)
            {
                ImageSourceConverter conv = new ImageSourceConverter();
                imgSettingPict.Source = (ImageSource)conv.ConvertFromString(files[0]);
                this.isSettingImageChaged = true;
            }
        }

        private void imgSettingPict_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        private void imgSettingPict_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void btnSettingLoadFrom_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".png";
            dlg.Filter = "Pictures (*.jpeg,*.png,*.jpg,*.gif)|*.jpeg;*.png;*.jpg;*.gif";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                ImageSourceConverter conv = new ImageSourceConverter();
                imgSettingPict.Source = (ImageSource)conv.ConvertFromString(filename);
                this.isSettingImageChaged = true;
            }
        }

        private void btnSettingConfirmPicture_Click(object sender, RoutedEventArgs e)
        {
            if(this.isSettingImageChaged == true)
            {
                this.enginee._assignPicture(imgSettingPict.Source.ToString());
            }
        }

        private void lbFriends_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OnePeople onee = (OnePeople)lbFriends.SelectedItem;
            //friends.Add(onee);
            if(onee!=null)
            {
                btnFriendsSend.Visibility = Visibility.Visible;
                btnFriendsDelete.Visibility = Visibility.Visible;
            }
            else
            {
                btnFriendsSend.Visibility = Visibility.Hidden;
                btnFriendsDelete.Visibility = Visibility.Hidden;
            }
        }

        private void btnFriendAdd_Click(object sender, RoutedEventArgs e)
        {
            chooseUser cu = new chooseUser(false, enginee, this, new Action<string>(re => {
                enginee._newFriend(re);
            }));

            cu.Show();
            cu.Focus();
            cu.tbIncome.Focus();
            cu.Top = this.Top + 100;
            cu.Left = this.Left + 100;
        }

        private void btnFriendsDelete_Click(object sender, RoutedEventArgs e)
        {
            OnePeople onee = (OnePeople)lbFriends.SelectedItem;
            if (onee != null)
            {
                enginee._delFriend(onee.id);
            }
        }

        private void btnFriendsSend_Click(object sender, RoutedEventArgs e)
        {
            OnePeople onee = (OnePeople)lbFriends.SelectedItem;
            if (onee != null)
            {
                enginee._openPrivate(onee.id);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
        }

        private void tryLogOut()
        {
            chates = new ObservableCollection<oneChat>();
            msges = new ObservableCollection<oneMsg>();
            friends = new ObservableCollection<OnePeople>();
            chatsList.ItemsSource = chates;
            msgsList.ItemsSource = msges;
            enginee = new BridgeClient(chates, msges, this);
            users = new ObservableCollection<OneUser>();
            lbUsersKek.ItemsSource = users;
            //lbFriends.Items.Clear();
            lbFriends.ItemsSource = friends;
            //chatsList.Focus();
            btnAddUser.Visibility = Visibility.Hidden;
            btnLeaveChat.Visibility = Visibility.Hidden;
            btnShowUsers.Visibility = Visibility.Hidden;
            msgsList.Visibility = Visibility.Hidden;
            btnSendMsg.Visibility = Visibility.Hidden;
            boxtomsg.Visibility = Visibility.Hidden;
            btnFriendsDelete.Visibility = Visibility.Hidden;
            btnFriendsSend.Visibility = Visibility.Hidden;
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

        private void tbLogOut_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void tbLogOut_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void tbLogOut_MouseUp(object sender, MouseButtonEventArgs e)
        {
            tryLogOut();
            tabcontroll.SelectedItem = tabcontroll.Items[1];
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
                tabcontroll.SelectedIndex = 1;
        }
    }
}
