using DocumentFormat.OpenXml.Math;
using LIT.Core.Controls;
using LIT.Core.Mvvm;
using LIT.ViewModels;
using LITModels.LITModels.Models;
using Prism.Commands;
using SQLDataAccessLayer.DAL;
using SQLDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LIT.Commands
{
    public class EmailLogTabCommands : CrudOperations<EmailLogTabCommands>
    {
        private readonly EmailLogTabViewModel _viewModel;
        private readonly EmailDal _emailDal;

        public DelegateCommand AddCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand RetrieveCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand SearchCommand { get; set; }
        public DelegateCommand OpenHtmlCommand { get; set; }
        public DelegateCommand OpenPdfCommand { get; set; }

        public EmailLogTabCommands(EmailLogTabViewModel viewModel)
            : base("Retrieve")
        {
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            _emailDal = new EmailDal();

            RetrieveCommand = new DelegateCommand(ExecuteRetrieve);
            SearchCommand = new DelegateCommand(ExecuteSearch);
            OpenHtmlCommand = new DelegateCommand(ExecuteOpenHtlm);
            OpenPdfCommand = new DelegateCommand(ExecuteOpenPdf);
        }

        public new event EventHandler CanExecuteChanged;


        protected void ExecuteSearch()
        {
            ExecuteRetrieve();

            // List<SupplierEmailSetting> searchList = _viewModel.SupplierEmailLogs.Where(to => to.CreatedOn == _viewModel.From || to.CreatedOn == _viewModel.To).ToList();
            var baseQuery = _viewModel.SupplierEmailLogs.AsQueryable();

            if (_viewModel.From != null && _viewModel.To == null)
            {
                var fliterList = baseQuery.Where(date => date.CreatedOn != null).AsQueryable();
                var date = $"{_viewModel.From.Value.Day}/{_viewModel.From.Value.Month}/{_viewModel.From.Value.Year}";
                baseQuery = fliterList.Where(to => $"{to.CreatedOn.Value.Day}/{to.CreatedOn.Value.Month}/{to.CreatedOn.Value.Year}" == date);
            }

            if (_viewModel.To != null && _viewModel.From == null)
            {
                var fliterList = baseQuery.Where(date => date.CreatedOn != null).AsQueryable();
                var date = $"{_viewModel.To.Value.Day}/{_viewModel.To.Value.Month}/{_viewModel.To.Value.Year}";
                baseQuery = fliterList.Where(to => $"{to.CreatedOn.Value.Day}/{to.CreatedOn.Value.Month}/{to.CreatedOn.Value.Year}" == date);
            }

            if (_viewModel.To != null && _viewModel.From != null)
            {
                var startDate = DateTime.Parse($"{_viewModel.From.Value.Day}/{_viewModel.From.Value.Month}/{_viewModel.From.Value.Year}");
                var endDate = DateTime.Parse($"{_viewModel.To.Value.Day}/{_viewModel.To.Value.Month}/{_viewModel.To.Value.Year}");

                var fliterList = baseQuery.Where(date => date.CreatedOn != null).AsQueryable();
                baseQuery = fliterList.Where(to => DateTime.Parse($"{to.CreatedOn.Value.Day}/{to.CreatedOn.Value.Month}/{to.CreatedOn.Value.Year}") >= startDate &&
                                                   DateTime.Parse($"{to.CreatedOn.Value.Day}/{to.CreatedOn.Value.Month}/{to.CreatedOn.Value.Year}") <= endDate);
            }

            //if (_viewModel.SelectedRecipent == "All")
            //{
            //    ExecuteRetrieve();
            //    return;
            //}

            if (!string.IsNullOrEmpty(_viewModel.SelectedRecipent))
            {
                if (_viewModel.SelectedRecipent != "All")
                {
                    baseQuery = baseQuery.Where(to => to.SupplierName == _viewModel.SelectedRecipent);
                }
            }

            List<SupplierEmailSetting> searchList = baseQuery.ToList();
            _viewModel.SupplierEmailLogs.Clear();
            foreach (SupplierEmailSetting search in searchList)
            {
                _viewModel.SupplierEmailLogs.Add(search);
            }
        }

        protected override void ExecuteRetrieve()
        {
            _viewModel.SupplierEmailLogs.Clear();
            foreach (SupplierEmailSetting logs in _emailDal.GetSavedEmailSettings())
            {                
                if (logs.CreatedOn == DateTime.MinValue)
                    {
                        logs.CreatedOn = null;                        
                    }
                else
                {
                    // logs.CreatedOn= DateTime.ParseExact(logs.CreatedOn.Value.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                   // logs.CreatedOn.Value.ToShortDateString();
                }

                
                _viewModel.SupplierEmailLogs.Add(logs);
            }
        }

        protected void ExecuteOpenPdf()
        {
            // Get the DataContext of the clicked element (the data item)
            string pdfFilePath = _viewModel.SelectedLog.Attachment;
            //string pdfFilePath = @"D:\Sumit\TFS\LITPrism\LIT\LIT\bin\Debug\net6.0-windows\Edwin.pdf";
            if (string.IsNullOrEmpty(pdfFilePath))
            {
                MessageBox.Show("PDF file not found.");
            }
            else
            {
                var webViewWindow = new PdfViewer(pdfFilePath);
                webViewWindow.ShowDialog();
            }
        }

        protected void ExecuteOpenHtlm()
        {
            var htmlContent = _viewModel.SelectedLog.EmailBodyContentPreview;

            var webViewWindow = new WebViewHtlmDisplay(htmlContent);
            webViewWindow.ShowDialog();
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }


}
