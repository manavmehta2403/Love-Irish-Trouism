using System.Windows;
using System.Windows.Controls;

namespace LIT.Core.Controls.TimePickerControl
{
    [TemplatePart(Name = "PART_Watermark", Type = typeof(ContentControl))]
    public sealed class TimePickerTextBox : TextBox
    {
        public TimePickerTextBox()
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            // Add custom template-related logic here
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            // Add focus-related logic here
        }
    }
}
