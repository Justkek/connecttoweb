﻿using System;
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
    /// Логика взаимодействия для addUserWindow.xaml
    /// </summary>
    public partial class addUserWindow : Window
    {
        public BridgeClient eng;
        public addUserWindow(BridgeClient e)
        {
            eng = e;
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            eng._addPersonToCurrentChat(tbPerson.Text);
            Close();
        }

        private void tbPerson_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                eng._addPersonToCurrentChat(tbPerson.Text);
                Close();
            }
        }
    }
}
