using LIT.Core.Mvvm;
using LIT.Modules.TabControl.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Prism.Commands;
using SQLDataAccessLayer.DAL;
using SQLDataAccessLayer.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Timers;
using System.Windows;

namespace LIT.Modules.TabControl.Commands
{
    public class ItineraryCommentsTabCommands : CrudOperations<ItineraryCommentsTabCommands>
    {
        private readonly ItineraryCommentsTabViewModel _viewModel;
        private readonly Commentsdal _commentsdal;

        public DelegateCommand AddCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand RetrieveCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        private readonly System.Timers.Timer _saveTimer;

        public ItineraryCommentsTabCommands(ItineraryCommentsTabViewModel viewModel)
            : base("Add", "Delete", "Retrieve", "Save") 
        {
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            _commentsdal = new Commentsdal();

            _viewModel.PropertyChanged += ViewModel_PropertyChanged;

            AddCommand = new DelegateCommand(ExecuteAdd);
            DeleteCommand = new DelegateCommand(ExecuteDelete);
            RetrieveCommand = new DelegateCommand(ExecuteRetrieve);
            SaveCommand = new DelegateCommand(ExecuteSave);
            //_saveTimer = new System.Timers.Timer(30000);
            //_saveTimer.Elapsed += SaveTimerElapsed;
            //_saveTimer.Start();
        }

        public new event EventHandler CanExecuteChanged;

        //private void SaveTimerElapsed(object sender, ElapsedEventArgs e)
        //{
        //    Execute the SaveCommand asynchronously
        //    Task.Run(() => SaveCommand.Execute());
        //}

        protected override void ExecuteAdd()
        {
            var newComment = new Commentsmodel
            {
                Commentsid = Guid.NewGuid().ToString(),
                SupplierName = string.Empty,
                SupplierRefNo = string.Empty,
                Comments = string.Empty,
                CreatedBy = _viewModel.LoginId,
                ModifiedBy = _viewModel.LoginId,
                IsDeleted = false,
                DeletedBy = _viewModel.LoginId,
                CommentedOn = DateTime.Now,
                Itineraryid = _viewModel.ItineraryId,
                CommentedBy = _viewModel.LoginId,
                Commentedname = _viewModel.UserName
            };

            _viewModel.CommentsCollection.Add(newComment);
        }

        protected override void ExecuteDelete()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this item?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (_viewModel.Commentsmodel != null)
                {
                    // Find the selected item in the CommentsCollection and remove it
                    Commentsmodel selectedComment = _viewModel.Commentsmodel;
                    _viewModel.CommentsCollection.Remove(selectedComment);

                    // Delete the item from your data store (e.g., database) using your DAL
                    _commentsdal.DeleteCommentsDetails(selectedComment.Commentsid, selectedComment.DeletedBy);

                    // Optionally reset the _viewModel.Commentsmodel property
                    _viewModel.Commentsmodel = null;
                }
            }
        }

        protected override void ExecuteSave()
        {
            //var newComment = new Commentsmodel
            //{
            //    Commentsid = Guid.NewGuid().ToString(),
            //    SupplierName = "SumitKumar",
            //    SupplierRefNo = string.Empty,
            //    Comments = "sumit kumar",
            //    CreatedBy = _viewModel.LoginId,
            //    ModifiedBy = _viewModel.LoginId,
            //    IsDeleted = false,
            //    DeletedBy = _viewModel.LoginId,
            //    CommentedOn = DateTime.Now,
            //    Itineraryid = _viewModel.ItineraryId,
            //    CommentedBy = _viewModel.LoginId,
            //    Commentedname = _viewModel.UserName
            //};
            //_viewModel.CommentsCollection.Add(newComment);
            //_commentsdal.SaveUpdateCommentsDetails("I", newComment);

            foreach (Commentsmodel comment in _viewModel.CommentsCollection) 
            {
                comment.CommentedBy = _viewModel.LoginId;
                _commentsdal.SaveUpdateCommentsDetails("I",comment);
            }
        } 
        
        protected override void ExecuteRetrieve()
        {
            // Retrieve data from the database using your DAL
            //Guid itineraryId = Guid.Parse("6120f3c4-1b10-4270-b92d-073564c3851f");
            //var comments = _commentsdal.RetriveCommentsDetails(itineraryId);

            //// Clear the existing collection and add the retrieved data
            //_viewModel.CommentsCollection.AddRange(comments);

            Guid itineraryId = Guid.Parse(_viewModel.ItineraryId);
            if (itineraryId != Guid.Empty)
            {
                // Retrieve data from the database using your DAL with the provided itineraryId
                var comments = _commentsdal.RetriveCommentsDetails(itineraryId);

                //foreach (Commentsmodel comment in comments)
                //{
                //    comment.comm = _viewModel.UserName;
                //    comments.Add(comment);
                //}

                // Use the Dispatcher to update the UI collection on the UI thread
                _viewModel.CommentsCollection.AddRange(comments);
            }
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
