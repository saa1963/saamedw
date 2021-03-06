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

namespace SaaMedW
{
    /// <summary>
    /// Логика взаимодействия для frmLogin.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void tbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            tbPassword0.Text = tbPassword.Password;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidationDialog.IsValid(this))
            {
                return;
            }
            DialogResult = true;
        }
    }
}
