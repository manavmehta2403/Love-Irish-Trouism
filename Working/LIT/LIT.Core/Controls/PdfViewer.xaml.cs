using System;
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

namespace LIT.Core.Controls
{
    /// <summary>
    /// Interaction logic for PdfViewer.xaml
    /// </summary>
    public partial class PdfViewer : Window
    {
        public PdfViewer()
        {
            InitializeComponent();
        }
        public PdfViewer(string htlmString)
        {
            InitializeComponent();
            InitializeWebView(htlmString);

            // WebView2 webView = this.webView;
        }
        private async void InitializeWebView(string htlmString)
        {
            await webView.EnsureCoreWebView2Async(null);

            webView.CoreWebView2.Navigate(htlmString);
        }
    }
}
