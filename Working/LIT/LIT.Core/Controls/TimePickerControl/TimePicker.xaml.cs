using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Globalization;

namespace LIT.Core.Controls.TimePickerControl
{
    public partial class TimePicker : UserControl
    {
        public TimePicker()
        {
            InitializeComponent();
            InitializeComboBoxes();
        }

        public static readonly DependencyProperty SelectedTimeProperty =
            DependencyProperty.Register("SelectedTime", typeof(TimeSpan), typeof(TimePicker), new PropertyMetadata(TimeSpan.Zero, OnSelectedTimeChanged));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TimePicker), new PropertyMetadata(string.Empty));

        public event EventHandler<TimeSpan> SelectedTimeChanged;

        public TimeSpan SelectedTime
        {
            get { return (TimeSpan)GetValue(SelectedTimeProperty); }
            set { SetValue(SelectedTimeProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private void InitializeComboBoxes()
        {
            // Populate ComboBox items for hours, minutes, and seconds
            for (int hour = 0; hour < 24; hour++)
            {
                HoursComboBox.Items.Add(hour);
            }

            for (int minute = 0; minute < 60; minute++)
            {
                MinutesComboBox.Items.Add(minute);
                SecondsComboBox.Items.Add(minute); // Add seconds
            }
        }

        private void OpenTimePicker(object sender, RoutedEventArgs e)
        {
            TimePickerPopup.IsOpen = true;
        }

        private void CloseTimePicker(object sender, RoutedEventArgs e)
        {
            TimePickerPopup.IsOpen = false;
            UpdateTextBoxValue();
        }

        private void UpdateTextBoxValue()
        {
            TimeTextBox.Text = $"{HoursComboBox.SelectedItem:D2}:{MinutesComboBox.SelectedItem:D2}:{SecondsComboBox.SelectedItem:D2}";
            SelectedTime = new TimeSpan(GetHour(), GetMinute(), GetSecond());

            // Raise the SelectedTimeChanged event
            SelectedTimeChanged?.Invoke(this, SelectedTime);
        }

        private int GetHour()
        {
            return (int)HoursComboBox.SelectedItem;
        }

        private int GetMinute()
        {
            return (int)MinutesComboBox.SelectedItem;
        }

        private int GetSecond()
        {
            return (int)SecondsComboBox.SelectedItem;
        }

        private void TimeTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Validate and format the time input with leading zeros
            if (!string.IsNullOrEmpty(e.Text) && !char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
                return;
            }

            var textBox = (TextBox)sender;
            string newText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
            e.Handled = !IsValidTimeFormat(newText);
        }

        private bool IsValidTimeFormat(string input)
        {
            // Validate time format (hh:mm:ss)
            return TimeSpan.TryParseExact(input, "hh\\:mm\\:ss", CultureInfo.InvariantCulture, out _);
        }

        private static void OnSelectedTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var timePicker = (TimePicker)d;
            timePicker.UpdateTextBoxValue();
        }
    }
}
