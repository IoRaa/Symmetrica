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
using Projet2Cp;
namespace ModeCours
{
    /// <summary>
    /// Interaction logic for Demonstration4.xaml
    /// </summary>
    public partial class Demonstration4 : Page
    {
        public Demonstration4()
        {
            InitializeComponent();
        }
        private void Button_Click_Svt_Demonstration4(object sender, RoutedEventArgs e)
        {
            ViexBox_Demonstration4.Visibility = Visibility.Hidden;
            MainWindow.MainFrame.NavigationService.Navigate(new Demonstration5());
        }

        private void Button_Click_Prcdt_Demonstration4(object sender, RoutedEventArgs e)
        {
            ViexBox_Demonstration4.Visibility = Visibility.Hidden;
            MainWindow.MainFrame.NavigationService.Navigate(new P3CoursCen());
        }
        private void etape1_MediaEnded(object sender, RoutedEventArgs e)
        {
            etape1Cen.Position = TimeSpan.Zero;
            etape1Cen.LoadedBehavior = MediaState.Play;
        }
    }
}
