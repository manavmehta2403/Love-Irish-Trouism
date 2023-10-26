using LIT.Commands;
using LIT.Core.Controls;
using LITModels.LITModels.Models;
using Prism.Commands;
using Prism.Mvvm;
using SQLDataAccessLayer.DAL;
using SQLDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LIT.ViewModels
{
    public class EmailLogSettingViewModel : BindableBase
    {
        #region private field

        private readonly EmailLogSettingCommands _emailLogCommands;
        private ObservableCollection<EmailLogsSettingCollection> _EmailLogsSetting;
        private EmailLogsSettingCollection _selecteEmailSetting;
        private ComboBoxItem _pdfType;
        private string _itineraryid;


        #endregion

        #region ctor
        public EmailLogSettingViewModel()
        {            
            _emailLogCommands = new EmailLogSettingCommands(this);
            EmailLogsSettingCollection = new CustomObservableCollection<EmailLogsSettingCollection>();
            
        }
        #endregion

        
        #region public field
        public EmailLogSettingCommands EmailLogsSettingCommand => _emailLogCommands;

        public ObservableCollection<EmailLogsSettingCollection> EmailLogsSettingCollection
        {
            get { return _EmailLogsSetting; }
            set
            {
                SetProperty(ref _EmailLogsSetting, value);
                _emailLogCommands.RaiseCanExecuteChanged();
            }
        }

        public EmailLogsSettingCollection SelecteEmailSetting
        {
            get { return _selecteEmailSetting; }
            set
            {
                SetProperty(ref _selecteEmailSetting, value);
                _emailLogCommands.RaiseCanExecuteChanged();
            }
        }
        
        public ComboBoxItem PdfType
        {
            get { return _pdfType; }
            set
            {
                SetProperty(ref _pdfType, value);
                _emailLogCommands.RaiseCanExecuteChanged();
            }
        }
        public string Itineraryid
        {
            get { return _itineraryid; }
            set
            {
                SetProperty(ref _itineraryid, value);
                _emailLogCommands.RaiseCanExecuteChanged();
            }
        }
        #endregion

    }
}
