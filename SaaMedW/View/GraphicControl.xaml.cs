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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SaaMedW.View
{
    /// <summary>
    /// Логика взаимодействия для GraphicControl.xaml
    /// </summary>
    public partial class GraphicControl : UserControl
    {
        public GraphicControl()
        {
            InitializeComponent();
        }

        private void ListBox_LostFocus(object sender, RoutedEventArgs e)
        {
            //var listBox = sender as ListBox;
            //listBox.find
            //listBox.SelectedIndex = -1;

        }
    }
}