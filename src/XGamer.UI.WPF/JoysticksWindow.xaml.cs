﻿using System.IO;
using System.Windows;

namespace XGamer.UI.WPF
{
    public partial class JoysticksWindow : Window
    {
        public JoysticksWindow()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            if (chkMessage.IsChecked == true)
            {
                try
                {
                    File.Create("startupmessage");
                }
                catch
                { 
                    //do nothing.
                }
            }

            Close();
        }
    }
}