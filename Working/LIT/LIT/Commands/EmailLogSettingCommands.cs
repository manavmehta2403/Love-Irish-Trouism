using LIT.Core.Controls;
using LIT.Core.Mvvm;
using LIT.ViewModels;
using LITModels.LITModels.Models;
using Prism.Commands;
using Prism.Regions;
using SQLDataAccessLayer.DAL;
using SQLDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LIT.Commands
{
    public class EmailLogSettingCommands : CrudOperations<EmailLogSettingCommands>
    {
        private readonly EmailLogSettingViewModel _viewModel;
        private readonly CustomerEmailSettingsDal _emailDal;

        public DelegateCommand RetrieveCommand { get; set; }
        public DelegateCommand SearchCommand { get; set; }

        public EmailLogSettingCommands(EmailLogSettingViewModel viewModel)
            : base("Retrieve")
        {
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            _emailDal = new CustomerEmailSettingsDal();

            RetrieveCommand = new DelegateCommand(ExecuteRetrieve);
            SearchCommand = new DelegateCommand(SearchRetrieve);
        }

        public new event EventHandler CanExecuteChanged;

        // ... (existing methods)

        private List<EmailLogsSettingCollection> originalData = new List<EmailLogsSettingCollection>();
        private Dictionary<string, List<EmailLogsSettingCollection>> filteredData = new Dictionary<string, List<EmailLogsSettingCollection>>();

        protected override void ExecuteRetrieve()
        {
            _viewModel.EmailLogsSettingCollection.Clear();
            if (filteredData.ContainsKey("All"))
            {
                foreach (EmailLogsSettingCollection logs in filteredData["All"])
                {
                    _viewModel.EmailLogsSettingCollection.Add(logs);
                }
            }

            else if (_viewModel.EmailLogsSettingCollection.Count == 0) 
            {
                Retrieval();
            }
        }

        private  void Retrieval()
        {
            _viewModel.EmailLogsSettingCollection.Clear();
            if (_viewModel.Itineraryid != null)
            {
                foreach (EmailLogsSettingCollection logs in _emailDal.GetCustomerEmailSettings(_viewModel.Itineraryid))
                {
                    _viewModel.EmailLogsSettingCollection.Add(logs);
                }
            }
        }
        protected void SearchRetrieve()
        {
            string selectedType = _viewModel.PdfType.Content.ToString();

            //if (originalData.Count == 0)
            //{
            // Store the original data if it hasn't been stored yet
            originalData.Clear();
            originalData = _emailDal.GetCustomerEmailSettings(_viewModel.Itineraryid).ToList();
                filteredData["All"] = originalData;
            //}

            if (selectedType == "All")
            {
                ExecuteRetrieve();
            }
            else
            {
                if (!filteredData.ContainsKey(selectedType))
                {
                    // Filter and store data for the selected type if it hasn't been stored yet
                    List<EmailLogsSettingCollection> temp = originalData
                        .Where(x => x.Type == selectedType)
                        .ToList();
                    filteredData[selectedType] = temp;
                }

                _viewModel.EmailLogsSettingCollection.Clear();

                foreach (EmailLogsSettingCollection logs in filteredData[selectedType])
                {
                    _viewModel.EmailLogsSettingCollection.Add(logs);
                }
            }
        }


        // ... (existing methods)
    }
}
