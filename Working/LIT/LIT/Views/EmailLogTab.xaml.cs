using LIT.Core.Controls;
using LIT.Core.pdfTemplates;
using LIT.ViewModels;
using LITModels.LITModels.Models;
using Microsoft.Web.WebView2.Core;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LIT.Views
{
    /// <summary>
    /// Interaction logic for EmailLogTabView.xaml
    /// </summary>
    public partial class EmailLogTab : UserControl
    {        
        private LIT.Old_LIT.MainWindow _parentWindow;

        public EmailLogTab()
        {
            InitializeComponent();
        }

        public EmailLogTab(LIT.Old_LIT.MainWindow ParentWindow)
        {
            InitializeComponent();
            _parentWindow = ParentWindow;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            frompicker.SelectedDate = null;
            topicker.SelectedDate = null;
            cmbrecipent.SelectedIndex = 0;
        }
    }
}
