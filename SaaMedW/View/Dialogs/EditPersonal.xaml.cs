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

namespace SaaMedW.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для EditPersonal.xaml
    /// </summary>
    public partial class EditPersonal : Window
    {
        public EditPersonal()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidationDialog.IsValid(this))
            {
                return;
            }
            DialogResult = true;
            Close();
        }
    }
}
