using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace LIT.Common
{
    public class CustomObservable
    {

        public class CustomObservableCollection<T> : ObservableCollection<T>
        {
            private int selectedIndex;
            private T selectedValue;


            public event EventHandler<SelectedSelectionListItemChangedEvent> SelectedSelectionListItemChanged;
            public int SelectedIndex
            {
                get { return selectedIndex; }
                set
                {
                    if (selectedIndex != value && value >= 0 && value < Count)
                    {
                        selectedIndex = value;
                        SelectedValue = this[value]; // Update selected value based on the index
                        OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedIndex)));
                        OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedValue)));
                        this.RaiseSelectedSelectionListItemChanged();
                    }
                }
            }

            public T SelectedValue
            {
                get { return selectedValue; }
                set
                {
                    int index = IndexOf(value);
                    if (!EqualityComparer<T>.Default.Equals(selectedValue, value) && index >= 0)
                    {
                        selectedValue = value;
                        selectedIndex = index; // Update selected index based on the value
                        OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedValue)));
                        OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedIndex)));
                        this.RaiseSelectedSelectionListItemChanged();
                    }
                }
            }

            private void RaiseSelectedSelectionListItemChanged()
            {
                var handler = this.SelectedSelectionListItemChanged;
                if (handler != null)
                {
                    var e = new SelectedSelectionListItemChangedEvent(this.SelectedIndex, this.SelectedValue);
                    handler(this, e);
                }
            }
        }

        public class SelectedSelectionListItemChangedEvent : EventArgs
        {
            public SelectedSelectionListItemChangedEvent(int selectedIndex, object SelectedValue)
            {
                this.SelectedIndex = selectedIndex;
                this.SelectedItem = SelectedValue;
            }

            public int SelectedIndex
            {
                get;
                private set;
            }

            public object SelectedItem
            {
                get;
                private set;
            }
        }
    }
}
