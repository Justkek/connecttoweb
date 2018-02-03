using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SuperSocket;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using WebSocket4Net;
using System.IO;
using System.Windows.Forms;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Collections;
using System.Windows.Media;

namespace Bridge
{
    static class Util
    {
        public static void logTime()
        {
            Console.WriteLine(DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
        }
    }
    //some data classes
    [DataContract]
    class typemsg
    {

        [DataMember]
        public string msg;
        [DataMember]
        public string from;
        [DataMember]
        public string time;
        public void display()
        {
            Console.WriteLine($"\t\t{from}: {msg}");
        }
    }

    [DataContract]
    class typegetpeople
    {
        [DataMember]
        public string name;
        [DataMember]
        public string id;
        [DataMember]
        public string pict;
    }

    [DataContract]
    class typegetdata
    {
        [DataMember]
        public string name;
        public void display()
        {
            Console.WriteLine($"\t\t{name}");
        }
    }


    [DataContract]
    class typedata
    {
        [DataMember]
        public string command;
        [DataMember]
        public string msg;
        [DataMember]
        public string[] data;
        [DataMember]
        public string target;
        public void display()
        {
            Console.WriteLine("\tdata:");
            Console.WriteLine($"\tcommand:{this.command}");
            if (msg != null)
            {
                Console.WriteLine($"\tmsg:{this.msg}");
            }
            for (int i = 0; i < data.Length; i++)
            {
                if (this.command == "upddata")
                {
                    typegetdata getdata = new typegetdata();
                    getdata = (typegetdata)primeJSON.DeserializeObject(data[i], getdata.GetType());
                    getdata.display();
                }
                else if (this.command == "updmsg")
                {
                    typemsg tmsg = new typemsg();
                    tmsg = (typemsg)primeJSON.DeserializeObject(data[i], tmsg.GetType());
                    tmsg.display();
                }
                else
                    Console.WriteLine($"\tdata {i}:{this.data[i]}");
            }
            if (target != null)
            {
                Console.WriteLine($"\tmsg:{this.target}");
            }

            Console.WriteLine("\tEndofdata");
        }
    }

    //class for deserialize
    static class primeJSON
    {
        public static string SerializeObject(object target)
        {
            var serializer = new DataContractJsonSerializer(target.GetType());
            string result;
            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, target);
                result = Encoding.UTF8.GetString(ms.ToArray());
            };
            return result;
        }
        public static object DeserializeObject(string json, Type type)
        {
            var serializer = new DataContractJsonSerializer(type);
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return serializer.ReadObject(ms);
            };
        }
    }

    //main enginee
    public class BridgeClient
    {
        public Dictionary<string, OnePeople> map = new Dictionary<string, OnePeople>();

        WebSocket conn = new WebSocket("ws://gndlfserverbd.herokuapp.com");
        public MainWindow win;
        ObservableCollection<oneChat> listOfChates;
        ObservableCollection<oneMsg> listOfMsg;
        private string login = "unknown";
        private string currentGroup = "none";
        public bool authorized = false;
        loginWindow loginW = null;

        public BridgeClient(ObservableCollection<oneChat> chatstr, ObservableCollection<oneMsg> msgstr,MainWindow mw)
        {
            conn.Opened += new EventHandler(conn_opened);
            conn.MessageReceived += new EventHandler<MessageReceivedEventArgs>(conn_message);
            conn.Open();
            listOfChates = chatstr;
            win = mw;
            listOfMsg = msgstr;
        }

        public void _getFriends(string login, string forwhat)
        {
            typedata td = new typedata();
            td.command = "sendfr";
            td.target = login;
            td.msg = forwhat;
            sendDataToServer(td);
        }

        public void _assignPicture(string path)
        {
            Account account = new Account("gndlf", "322892712586893", "bdIeZOB0zvr6oWFKY0L5hu7fjU8");
            Cloudinary cloud = new Cloudinary(account);

            var uploadparams = new ImageUploadParams()
            {
                File = new FileDescription(new Uri(path).LocalPath)
            };

            var uploadresult = cloud.Upload(uploadparams);
            typedata td = new typedata();
            td.command = "assignpict";
            td.target = this.login;
            td.msg = "https://res.cloudinary.com"+uploadresult.Uri.AbsolutePath;
            sendDataToServer(td);
        }

        public void _assignNmae(string newname)
        {
            typedata td = new typedata();
            td.command = "assignname";
            td.target = this.login;
            td.msg = newname;
            sendDataToServer(td);
        }

        public void _regUserKek(string login, string pass, string mail)
        {

            if(login!=null && pass!=null && mail!=null)
            {
                login = login.Replace(" ", string.Empty);
                pass = pass.Replace(" ", string.Empty);
                mail = mail.Replace(" ", string.Empty);
                if (login != "" && pass != "" && mail != "" && (mail.IndexOf('@')>=0))
                {
                    typedata td = new typedata();
                    td.command = "reguser";
                    td.data = new string[3] { login, pass, mail };
                    sendDataToServer(td);
                }
            }
        }

        private string inputBoxx(string msg)
        {
            Console.Write(msg);
            return Console.ReadLine();
        }

        private void _signUp(typedata td)
        {
            td.command = "reguser";
            td.data = new string[2];
            td.data[0] = inputBoxx("login:    ");
            td.data[1] = inputBoxx("password: ");
            sendDataToServer(td);
        }

        public void _testLogIn(string log, string pass, loginWindow lw1)
        {
            System.Threading.Thread.Sleep(900);
            typedata td = new typedata();
            td.command = "authuser";
            td.data = new string[2] { log, pass };
            sendDataToServer(td);
            this.login = log;
            this.loginW = lw1;
        }

        public void _newFriend(string logg)
        {
            if(authorized == true)
            {
                typedata td = new typedata();
                td.command = "makefr";
                td.msg = this.login;
                td.target = logg;
                sendDataToServer(td);
            }
        }

        public void _delFriend(string logg)
        {
            if (authorized == true)
            {
                typedata td = new typedata();
                td.command = "delfr";
                td.msg = this.login;
                td.target = logg;
                sendDataToServer(td);
            }
        }

        private void authComplete()//after auth we need to get list of chats
        {
            //loginW.Owner.Visibility = System.Windows.Visibility.Visible;
            //loginW.Close();
            loginW.Dispatcher.Invoke(new Action(() => {
                loginW.Close();
            }));
            typedata td = new typedata();
            td.command = "sendgru";
            td.target = login;
            sendDataToServer(td);
            _getFriends(login, "main");
        }

        private void _logIn(typedata td)
        {
            td.command = "authuser";
            td.data = new string[2];
            td.data[0] = inputBoxx("login:    ");
            td.data[1] = inputBoxx("password: ");
            sendDataToServer(td);
            //Util.logTime();
            login = td.data[0];
        }

        private void _makeChat(typedata td)
        {
            if (this.authorized == true)
            {
                td.command = "makegr";
                td.msg = login;
                td.target = inputBoxx("chat:     ");
                sendDataToServer(td);
            }
            else
            {
                Console.WriteLine("Unknown users cannot do this");
            }
        }

        private void _joinChat(typedata td)
        {
            if (this.authorized == true)
            {
                td.command = "joingr";
                td.msg = login;
                td.target = inputBoxx("chat:     ");
                sendDataToServer(td);
            }
            else
            {
                Console.WriteLine("Unknown users cannot do this");
            }
        }

        private void _leaveChat(typedata td)
        {
            if (this.authorized == true)
            {
                td.command = "leavegr";
                td.msg = login;
                td.target = inputBoxx("chat:     ");
                sendDataToServer(td);
            }
            else
            {
                Console.WriteLine("Unknown users cannot do this");
            }
        }

        private void _logOut(typedata td)
        {
            if (this.authorized == true)
            {
                td.command = "enduser";
                td.msg = login;
                sendDataToServer(td);
            }
            else
            {
                Console.WriteLine("Unknown users cannot do this");
            }
        }

        private void _sendMsg(typedata td)
        {
            if (this.authorized == true)
            {
                if (currentGroup == "none")
                    Console.WriteLine("Select chat first");
                else
                {
                    td.command = "sendmsg";
                    td.msg = inputBoxx("message:  ");
                    td.data = new string[1];
                    td.data[0] = login;
                    td.target = currentGroup;
                    sendDataToServer(td);
                }
            }
            else
            {
                Console.WriteLine("Unknown users cannot do this");
            }
        }

        private void _updateMsg(typedata td)
        {
            if (this.authorized == true)
            {
                if (currentGroup == "none")
                    Console.WriteLine("Select chat first");
                else
                {
                    td.command = "updmsg";
                    td.target = currentGroup;
                    sendDataToServer(td);
                }
            }
            else
            {
                Console.WriteLine("Unknown users cannot do this");
            }
        }

        private void _showChats(typedata td)
        {
            if (this.authorized == true)
            {
                td.command = "sendgru";
                td.target = login;
                sendDataToServer(td);
            }
            else
            {
                Console.WriteLine("Unknown users cannot do this");
            }
        }

        private void _showUsers(typedata td)
        {
            if (this.authorized == true)
            {
                if (currentGroup == "none")
                    Console.WriteLine("Select chat first");
                else
                {
                    td.command = "sendugr";
                    td.target = currentGroup;
                    sendDataToServer(td);
                }
            }
            else
            {
                Console.WriteLine("Unknown users cannot do this");
            }
        }

        public void setCurrentChat(string chatid)
        {
            if (chatid == "/none")
                currentGroup = "none";
            else
            {
                currentGroup = chatid;
                _sendRefreshMsg(null);
                _updateUsersInCurrent();
            }

        }

        private void _changeCurrren(typedata td)
        {
            if (this.authorized == true)
            {
                currentGroup = inputBoxx("chat:     ");//UNSAFE!!!
            }
            else
            {
                Console.WriteLine("Unknown users cannot do this");
            }
        }

        private void _leaveCurrent(typedata td)
        {
            if (this.authorized == true)
            {
                currentGroup = "none";
            }
            else
            {
                Console.WriteLine("Unknown users cannot do this");
            }
        }

        private void _exit()
        {
            if (this.authorized == false)
            {
                Environment.Exit(0);
            }
        }

        public void _addPersonToCurrentChat(string idS)
        {
            if (currentGroup != "none")
            {
                typedata td = new typedata();
                td.command = "joingr";
                td.msg = idS;
                td.target = currentGroup;
                sendDataToServer(td);
            }
        }

        private typedata inputMessage()
        {
            typedata td = new typedata();
            string nowlogin;
            if (this.authorized == false)
                nowlogin = "unknown";
            else
                nowlogin = login;
            string startcommand = inputBoxx($"[{nowlogin},{currentGroup}]~");
            if (startcommand == "sign up")//registration
            {
                _signUp(td);
            }
            else
            if (startcommand == "log in")//log in
            {
                _logIn(td);
            }
            else
            if (startcommand == "make chat")
            {
                _makeChat(td);
            }
            else
            if (startcommand == "join chat")
            {
                _joinChat(td);
            }
            else
            if (startcommand == "leave chat")//UNSAFE
            {
                _leaveChat(td);
            }
            else
            if (startcommand == "log out")
            {
                _logOut(td);
            }
            else
            if (startcommand == "send msg")
            {
                _sendMsg(td);
            }
            else
            if (startcommand == "update msg")
            {
                _updateMsg(td);
            }
            else
            if (startcommand == "show chats")
            {
                _showChats(td);
            }
            else
            if (startcommand == "show users")
            {
                _showUsers(td);
            }
            else
            if (startcommand == "change current chat")
            {
                _changeCurrren(td);
            }
            else
            if (startcommand == "leave current chat")
            {
                _leaveCurrent(td);
            }
            if (startcommand == "exit")
            {
                _exit();
            }
            return td;
        }

        public void _leaveCurrentChat()
        {
            if (currentGroup != "none")
            {
                typedata td = new typedata();
                td.command = "leavegr";
                td.msg = this.login;
                td.target = this.currentGroup;
                sendDataToServer(td);
            }
        }

        private void sendDataToServer(object td)
        {
            conn.Send(primeJSON.SerializeObject(td));
        }

        public void _sendEmpty(object sender, EventArgs e)
        {
            conn.Send("empty");
            //MessageBox.Show("send empty", "notification", MessageBoxButtons.OK);
        }

        private void conn_message(object sender, WebSocket4Net.MessageReceivedEventArgs msg)
        {
            if (msg.Message != "empty")
            {
                typedata income = new typedata();
                income = (typedata)primeJSON.DeserializeObject(msg.Message, income.GetType());
                doMessage(income);
            }
        }

        public void _updateUsersInCurrent()
        {
            if (currentGroup != "none")
            {
                typedata td = new typedata();
                td.command = "sendugr";
                td.target = currentGroup;
                sendDataToServer(td);
            }
        }

        public void _openPrivate(string logg)
        {
            typedata td = new typedata();
            td.command = "openPrivate";
            td.msg = login;
            td.target = logg;
            sendDataToServer(td);
        }

        public void _createNewChat(string name)
        {
            typedata td = new typedata();
            td.command = "makegr";
            td.msg = this.login;
            td.target = name;
            sendDataToServer(td);
        }

        public void _sendMsgFromClient(string msg)
        {
            //MessageBox.Show("send empty", "notification", MessageBoxButtons.OK);
            typedata td = new typedata();
            td.command = "sendmsg";
            td.msg = msg;
            td.data = new string[1];
            td.data[0] = this.login;
            td.target = this.currentGroup;
            sendDataToServer(td);
        }

        private void doMessage(typedata income)
        {
            //Util.logTime();
            if(income.command == "usrdata")
            {
                for(int i=0; i<income.data.Length; i++)
                {
                    OnePeople onee = new OnePeople(this);
                    onee = (OnePeople)primeJSON.DeserializeObject(income.data[i], typeof(OnePeople));
                    map[onee.id] = onee;
                    if(onee.id == this.login)
                    {
                        this.win.Dispatcher.Invoke(new Action(() => {
                            ImageSourceConverter conv = new ImageSourceConverter();
                            this.win.imgInfoPict.Source = (ImageSource)conv.ConvertFrom(onee.Pict);
                            this.win.lbInfoId.Content = onee.id;
                        }));
                    }
                }
              this.win.Dispatcher.Invoke(new Action(() =>
              {
                  foreach (oneChat item in this.win.chates)
                  {
                      item.updateIcon(this);
                  }
                  foreach (OneUser item in this.win.users)
                  {
                      item.updateIcons(this);
                  }
                  foreach (oneMsg item in this.win.msges)
                  {
                      item.updateIcons(this);
                  }
                  foreach (OnePeople item in this.win.friends)
                  {
                      item.updateIcons(this);
                  }

              }));
            }
            else
            if (income.command == "answ")
            {
                if (income.data[0] == "no")
                {
                    Console.Write("\tWhoops! ");
                    if (income.data.Length > 1)
                        Console.WriteLine(income.data[1]);
                }
                else if (income.data[0] == "yes")
                {
                    Console.WriteLine("\tAccept!");
                    if (income.msg == "authuser")
                    {
                        this.authorized = true;
                        this.currentGroup = "none";
                        authComplete();
                    }
                    else if (income.msg == "enduser")
                    {
                        this.authorized = false;
                        this.currentGroup = "none";
                    }
                    else if (income.msg == "leavegr")
                    {
                        //this.currentGroup = "none";
                        this.win.Dispatcher.Invoke(new Action(() => {
                            oneChat pos = null;
                            foreach (oneChat item in this.win.chatsList.Items)
                            {
                                if(item.id == this.currentGroup)
                                {
                                    pos = item;
                                    break;
                                }
                            }
                            //this.win.chatsList.Items.Remove(pos);
                            this.win.chates.Remove(pos);
                            authComplete();
                            this.win.msges.Clear();
                        }));
                    }
                    else if (income.msg == "makegr")
                    {

                    }

                }
            }
            else
            if (income.command == "switch")
            {
                this.win.Dispatcher.Invoke(new Action(() =>
               {
                   this.win.tabcontroll.SelectedIndex = 1;
                   foreach (oneChat item in this.win.chates)
                   {
                       if (item.id == income.target)
                       {
                           this.win.chatsList.SelectedItem = item;
                           break;
                       }
                   }
               }));
            }

            else
            if (income.command == "upddata")
            {
                if (income.msg == "sendgru")
                {this.win.Dispatcher.Invoke(new Action( () =>
                                {
                    this.listOfChates.Clear();
                    for (int i = 0; i < income.data.Length; i++)
                    {
                        typegetdata getdata = new typegetdata();
                        getdata = (typegetdata)primeJSON.DeserializeObject(income.data[i], getdata.GetType());
                        oneChat newchat = new oneChat(getdata.name, this.login, this);
                        getdata.display();
                        
                                    this.listOfChates.Add(newchat);
                                
                        
                    }}
                            ));
                }
                else if (income.msg == "sendugr")
                {
                    if(income.target == currentGroup)
                    {
                        this.win.Dispatcher.Invoke(new Action(() =>
                        {
                            this.win.users.Clear();
                            for(int i=0; i<income.data.Length; i++)
                            {
                                typegetdata getdata = new typegetdata();
                                getdata = (typegetdata)primeJSON.DeserializeObject(income.data[i], getdata.GetType());
                                OneUser us = new OneUser(getdata.name, this);
                                this.win.users.Add(us);
                            }
                        }));
                    }
                }
                else if(income.msg == "sendgruws")
                {
                    this.win.Dispatcher.Invoke(new Action(() =>
                    {
                        this.listOfChates.Clear();
                        for (int i = 0; i < income.data.Length; i++)
                        {
                            typegetdata getdata = new typegetdata();
                            getdata = (typegetdata)primeJSON.DeserializeObject(income.data[i], getdata.GetType());
                            oneChat newchat = new oneChat(getdata.name, this.login, this);
                            getdata.display();

                            this.listOfChates.Add(newchat);


                        }

                        this.win.tabcontroll.SelectedIndex = 1;
                        foreach (oneChat item in this.win.chates)
                        {
                            if(item.id == income.target)
                            {
                                this.win.chatsList.SelectedItem = item;
                                break;
                            }
                        }

                    }
                            ));
                }
            }
            else
            if (income.command == "updmsg")
            {this.win.Dispatcher.Invoke(new Action(() =>
                   {
                       this.listOfMsg.Clear();
                for (int i = 0; i < income.data.Length; i++)
                {
                    typemsg tmsg = new typemsg();
                    tmsg = (typemsg)primeJSON.DeserializeObject(income.data[i], tmsg.GetType());
                           oneMsg newmsg = new oneMsg(tmsg.from, tmsg.msg, tmsg.time, this.login, this);
                    
                     this.listOfMsg.Add(newmsg);
                     
                     //win.msges.
                    //tmsg.display();
                }
                        if(this.win.msgsList.Items.Count>0)
                       this.win.msgsList.ScrollIntoView(this.win.msgsList.Items[this.win.msgsList.Items.Count - 1]);
                   }));
            }
            else
            if (income.command == "notification")
            {
                if (income.data[0] == "newmsg")
                // Console.WriteLine($"\t\t{income.msg}");
                {
                    //MessageBox.Show(income.msg, "notification", MessageBoxButtons.OK);
                    if (income.msg == currentGroup)
                        _sendRefreshMsg(null);
                    else
                    {
                        this.win.Dispatcher.Invoke(new Action(() =>
                       {
                           int index = -1;
                           for(int i=0; i<this.win.chates.Count; i++)
                           {
                               if (this.win.chates[i].id == income.msg)
                                   index = i;
                           }
                           if (index != -1)
                           {
                               this.win.chates[index].isNotRead = true;
                           }
                       }));
                    }

                  this.win.Dispatcher.Invoke(new Action(() =>
                  {
                      int ind = -1;
                      object sel = this.win.chatsList.SelectedItem;
                      for(int i=0; i<this.win.chates.Count; i++)
                      {
                          if (this.win.chates[i].id == income.msg)
                              ind = i;
                      }
                      if(ind != -1)
                      {
                          oneChat forSwap = this.win.chates[ind];
                          this.win.chates.Remove(forSwap);
                          this.win.chates.Insert(0, forSwap);
                          this.win.chatsList.SelectedItem = sel;
                          
                      }
                  }));
                }
                else if(income.data[0] == "updateusers")
                {
                    if (income.msg == currentGroup)
                        _updateUsersInCurrent();
                }
                //income.display();
            }
            else
            if(income.command == "friends")
            {
               this.win.Dispatcher.Invoke(new Action(() =>
               {
                   this.win.friends.Clear();

                   for(int i=0; i<income.data.Length; i++)
                   {
                       OnePeople onee = new OnePeople(this);
                       onee = (OnePeople)primeJSON.DeserializeObject(income.data[i], typeof(OnePeople));
                       this.win.friends.Add(onee);
                   }

                    
                    
               }));

            }
            //income.display();
            //Console.ReadKey();
        }

        private void conn_disconnected(object sender, EventArgs a)
        {
            Console.WriteLine("\tClient disconnected");
            this.authorized = false;
            this.currentGroup = "none";
        }

        private void _sendRefreshMsg(object obj)
        {
           // if(this.currentGroup!="none")
            //{
                typedata td = new typedata();
                td.command = "updmsg";
                td.target = this.currentGroup;
                sendDataToServer(td);
            //}
        }

        private void conn_opened(object sender, EventArgs s)
        {
            //Console.WriteLine("\tClient connected");
            //TimerCallback tc = new TimerCallback(_sendEmpty);
            // System.Threading.Timer tim = new System.Threading.Timer(tc, null, 20000, 20000);
            
            /*TimerCallback tc1 = new TimerCallback(_sendRefreshMsg);
            System.Threading.Timer tim1 = new System.Threading.Timer(tc1, null, 1000, 500);*/
            //_testLogIn("gndlf", "1234");
        }
    }

}
