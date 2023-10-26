using CommunityToolkit.Mvvm.Messaging;
using LIT.Commands;
using LIT.Core.Controls;
using LITModels.LITModels.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LIT.ViewModels
{
    public class EmailLogTabViewModel : BindableBase
    {
        #region private

        private readonly EmailLogTabCommands _emailLogTabCommands;

        private CustomObservableCollection<SupplierEmailSetting> _supplierEmailLogs;
        private SupplierEmailSetting _selectedLogs;
        private List<string> _recipent;
        private string _selectedRecipent;

        #endregion

        #region public proptery

        public CustomObservableCollection<SupplierEmailSetting> SupplierEmailLogs
        {
            get { return _supplierEmailLogs; }
            set
            {
                SetProperty(ref _supplierEmailLogs, value);
                _emailLogTabCommands.RaiseCanExecuteChanged();
            }
        }

        public SupplierEmailSetting SelectedLog
        {
            get {                
                    return _selectedLogs; }
            set
            {
                SetProperty(ref _selectedLogs, value);
                _emailLogTabCommands.RaiseCanExecuteChanged();
            }

        }
        
        public List<string> Recipent
        {
            get {                
                    return _recipent; }
            set
            {
                SetProperty(ref _recipent, value);
                _emailLogTabCommands.RaiseCanExecuteChanged();
            }

        }
        
        public string SelectedRecipent
        {
            get {                
                    return _selectedRecipent; }
            set
            {
                SetProperty(ref _selectedRecipent, value);
                _emailLogTabCommands.RaiseCanExecuteChanged();
            }

        }

        public string EmailStatus
        {
            get;
            set;
        }

        public DateTime? To
        {
            get;
            set;
        }

        public DateTime? _From;
        public DateTime? From
        {
            get { return _From; }
            set
            {
                SetProperty(ref _From, value);
                if (_From != null)
                {
                    if (_From.Value == DateTime.MinValue)
                    {
                        _From = DateTime.Now;
                    }
                }
            }
        }

        public EmailLogTabCommands EmailLogTabCommands => _emailLogTabCommands;


        #endregion

        #region ctor

        public EmailLogTabViewModel()
        {
            _emailLogTabCommands = new EmailLogTabCommands(this);
            SupplierEmailLogs = new CustomObservableCollection<SupplierEmailSetting>();
            UpdateLogs();
        }


        #endregion

        #region private
        private void UpdateLogs()
        {
            SupplierEmailLogs.Clear();
            EmailLogTabCommands.RetrieveCommand.Execute();
            _recipent = new List<string>
            {
                "All"
            };
            foreach (var item in SupplierEmailLogs)
            {
                if(!_recipent.Contains(item.SupplierName) && !string.IsNullOrEmpty(item.SupplierName))
                {
                    _recipent.Add(item.SupplierName);
                }
            }
        }
        #endregion
    }
}
