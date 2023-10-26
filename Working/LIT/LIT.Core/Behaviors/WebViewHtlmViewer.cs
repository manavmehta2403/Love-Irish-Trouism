using Microsoft.Web.WebView2.Wpf;
using System.Windows;

namespace LIT.Core.Behaviors
{
    public class WebViewHtlmViewer
    {
        public static readonly DependencyProperty SourceStringProperty = DependencyProperty.RegisterAttached("SourceString", typeof(string), typeof(WebViewHtlmViewer), new PropertyMetadata("", OnSourceStringChanged));

        public static string GetSourceString(DependencyObject obj)
        {
            return obj.GetValue(SourceStringProperty).ToString();
        }

        public static void SetSourceString(DependencyObject obj, string value)
        {
            obj.SetValue(SourceStringProperty, value);
        }

        private static async void OnSourceStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WebView2 wv = d as WebView2;
            await wv.EnsureCoreWebView2Async();
            if (wv != null)
            {
                wv.NavigateToString(e.NewValue.ToString());
            }
        }
    }
}
