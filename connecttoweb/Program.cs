﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using WebSocket4Net;
using System.IO;
using System.Windows.Forms;

namespace connecttoweb
{
    //some data classes
    [DataContract]
    class typemsg
    {
        
        [DataMember]
        public string msg;
        [DataMember]
        public string from;
        public void display()
        {
            Console.WriteLine($"\t\t{from}: {msg}");
        }
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
                } else
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
    class BridgeClient
    {
        WebSocket conn = new WebSocket("ws://gndlfserverbd.herokuapp.com");
        private string login = "unknown";
        private string currentGroup = "none";
        private bool authorized = false;

        public BridgeClient()
        {
            conn.Opened += new EventHandler(conn_opened);
            conn.MessageReceived += new EventHandler<MessageReceivedEventArgs>(conn_message);
            conn.Open();

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

        private void _logIn(typedata td)
        {
            td.command = "authuser";
            td.data = new string[2];
            td.data[0] = inputBoxx("login:    ");
            td.data[1] = inputBoxx("password: ");
            sendDataToServer(td);
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

        public typedata inputMessage()
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
            if(startcommand == "leave current chat")
            {
                _leaveCurrent(td);
            }
            if(startcommand == "exit")
            {
                _exit();
            }
            return td;
        }

        private void sendDataToServer(object td)
        {
            conn.Send(primeJSON.SerializeObject(td));
        }

        private void conn_message(object sender, WebSocket4Net.MessageReceivedEventArgs msg)
        {
            typedata income = new typedata();
            income = (typedata)primeJSON.DeserializeObject(msg.Message, income.GetType());
            doMessage(income);
        }

        private void doMessage(typedata income)
        {
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
                    }
                    else if (income.msg == "enduser")
                    {
                        this.authorized = false;
                        this.currentGroup = "none";
                    }

                }
            }
            else
            if (income.command == "upddata")
            {
                for (int i = 0; i < income.data.Length; i++)
                {
                    typegetdata getdata = new typegetdata();
                    getdata = (typegetdata)primeJSON.DeserializeObject(income.data[i], getdata.GetType());
                    getdata.display();
                }
            }
            else
            if (income.command == "updmsg")
            {
                for (int i = 0; i < income.data.Length; i++)
                {
                    typemsg tmsg = new typemsg();
                    tmsg = (typemsg)primeJSON.DeserializeObject(income.data[i], tmsg.GetType());
                    tmsg.display();
                }
            }
            else
            if (income.command == "notification")
            {
                if (income.data[0] == "newmsg" && income.data[1]!=login)
                // Console.WriteLine($"\t\t{income.msg}");
                {
                    MessageBox.Show(income.msg, "notification", MessageBoxButtons.OK);
                }
                else
                    if(income.data[0]!="newmsg")
                        income.display();
                //income.display();
            }
            else
                income.display();
            //income.display();
            //Console.ReadKey();
        }

        private void conn_opened(object sender, EventArgs s)
        {
            Console.WriteLine("\tClient connected");
        }
    }





    class Program
    {


        static void Main(string[] args)
        {
           
            BridgeClient bc = new BridgeClient();
            Console.WriteLine("\t\t\tENTER");
            Console.ReadKey();
            while (true)
            {
                bc.inputMessage();
                Console.WriteLine("\t\t\tENTER");
                Console.ReadKey();
            }

        }





    }
}


/* Инструкция как пользоваться этой фигней
 * Как только зашел, ждешь пару сек чтобы прога законнектилась
 * Потом доступны две команды (sign up - регистрация и log in - вход)
 * Когда войдешь станут доступны остальные функции. Вкратце о них:
 * 
 * make chat - создаешь новый чат(группу). вроде устойчиво к неправильным вводам
 * join chat - подключение пользователя за которого залогинился к другому чату(НО НЕ ВХОД В НЕГО!)
 * leave chat - удаление текущего пользователя из чата( который ты введешь). ЭТА ФУНКЦИЯ НЕБЕЗОПАСНА
 *              т.е. если ты удалишь его из текущего чата то будет плохо. не стоит так пока делать
 * log out - выход пользователя. после опять доступны только две команды - регистрация и вход( ну и exit офк)
 * send msg - отправка сообщения в текущий чат (currentGroup)
 * update msg - отображение сообщений в текущем чате (currentGroup)
 * show chats - список всех чатов пользователя
 * show users - список всех пользователей в currentGroup
 * change current chat - установка текущего чата ( currentGroup ). ПОСЛЕ этой комманды можно работать с чатами!!!
 * leave current chat - делаем currentGroup = none (т.е. неопределенным). ТЕПЕРЬ с чатом нельзя работать пока не войдем
 *                      с помощью change current chat
 * exit - выход (можно использовать только после log out, ИНАЧЕ СЛОМАЕТСЯ НАХУЙ СЕРВЕР)
 * 
 * И еще. Не стоит запускать одновременно два клиента на одном ноуте, почему то тогда ЛОМАЕТСЯ СЕРВЕР НАХУЙ (((
 * Не забывай писать log out и exit перед выходом из проги. НЕ ЗАКРЫВАЙ ЧЕРЕЗ КРЕСТИК
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * */