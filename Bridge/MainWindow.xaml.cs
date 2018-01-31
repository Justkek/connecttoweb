﻿using System;
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
        public string name { get; set; }
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
        public void updateIcon()
        {
            if (id.StartsWith("pm"))
            {
                if (image != this.enginee.map[nick].pict)
                    image = this.enginee.map[nick].pict;
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
                        nick = qqq[2];
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
    public class OnePeople
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string pict { get; set; }
        public OnePeople()
        {
            id = null;
            name = null;
            pict = null;
        }
        public OnePeople(string idd, string namee, string pictt)
        {
            id = idd;
            name = namee;
            pict = pictt;
        }
    }

    public class OneUser:INotifyPropertyChanged
    {
        BridgeClient eng;
        public string id { get; set; }
        public string name { get; set; }
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

        public void updateIcons()
        {
            if(pict != eng.map[id].pict)
            {
                pict = eng.map[id].pict;
            }
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
        public string namefrom { get; set; }
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
        public oneMsg(string id, string text, string login, BridgeClient e)
        {
            eng = e;
            idfrom = id;
            namefrom = id;
            this.login = login;
            textmsg = text;
            if (idfrom == login)
                fromSelf = true;
            else
                fromSelf = false;
            pict = this.eng.map[idfrom].pict;
        }

        public void updateIcons()
        {
            if(pict != eng.map[idfrom].pict)
            {
                pict = eng.map[idfrom].pict;
            }
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

        private void imgSettingPict_Drop(object sender, DragEventArgs e)
        {
            //imgSettingPict.Source = (ImageSource)e.Data.GetData(typeof(ImageSource));
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files[0].IndexOf(".jpeg") != -1 || files[0].IndexOf(".png") != -1 || files[0].IndexOf(".jpg") != -1 || files[0].IndexOf(".png") != -1)
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
            formFriendAdd auw = new formFriendAdd(enginee);
            auw.Owner = this;
            auw.Show();
            auw.Focus();
            auw.tbName.Focus();
            auw.Top = this.Top + 100;
            auw.Left = this.Left + 100;
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
    }
}
