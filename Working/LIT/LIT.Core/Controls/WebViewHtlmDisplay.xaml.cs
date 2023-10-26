using LIT.Core.Behaviors;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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

namespace LIT.Core.Controls
{
    /// <summary>
    /// Interaction logic for WebViewHtlmDisplay.xaml
    /// </summary>
    public partial class WebViewHtlmDisplay : Window
    {
        public WebViewHtlmDisplay()
        {
            InitializeComponent();
        }


        public WebViewHtlmDisplay(string htlmString)
        {
            InitializeComponent();
            InitializeWebView(htlmString);

            // WebView2 webView = this.webView;
        }
        private async void InitializeWebView(string htlmString)
        {
            await webView.EnsureCoreWebView2Async(null);

            webView.CoreWebView2.NavigateToString(htlmString);
        }
    }
}
