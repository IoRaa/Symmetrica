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
    /// Interaction logic for Demonstration3Ar.xaml
    /// </summary>
    public partial class Demonstration3Ar : Page
    {
        public Demonstration3Ar()
        {
            InitializeComponent();
        }
        private void Button_Click_Svt_Demonstration3(object sender, RoutedEventArgs e)
        {
            MainWindow.MainFrame.NavigationService.Navigate(new P4CourAxeAr());
            ViexBox_Demonstration3Ar.Visibility = Visibility.Hidden;
            if (!MainWindow.modeEns) MainWindow.eleve.incProgress();

        }

        private void Button_Click_Prcdt_Demonstration3(object sender, RoutedEventArgs e)
        {
            MainWindow.MainFrame.NavigationService.Navigate(new Demonstration2Ar());
            ViexBox_Demonstration3Ar.Visibility = Visibility.Hidden;
            if (!MainWindow.modeEns) MainWindow.eleve.decProgress();

        }

        private void etape3_MediaEnded(object sender, RoutedEventArgs e)
        {
            etape3.Position = TimeSpan.Zero;
            etape3.LoadedBehavior = MediaState.Play;

        }
    }
}
