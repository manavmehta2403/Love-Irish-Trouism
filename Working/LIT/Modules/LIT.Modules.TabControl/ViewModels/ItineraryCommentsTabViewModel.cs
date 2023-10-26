using LIT.Core.Mvvm;
using LIT.Modules.TabControl.Commands;
using Prism.Mvvm;
using SQLDataAccessLayer.Models;
using System.Collections.ObjectModel;

namespace LIT.Modules.TabControl.ViewModels
{
    public class ItineraryCommentsTabViewModel : BindableBase
    {
        #region field
        private readonly ItineraryCommentsTabCommands _commentsTabCommands;

        private ObservableCollection<Commentsmodel> _commentsCollection;

        private Commentsmodel _commentsmodel;

        private string _itineraryId;

        private string _loginId;

        private string _userName;
        #endregion

        #region ctor

        public ItineraryCommentsTabViewModel()
        {
            _commentsTabCommands = new ItineraryCommentsTabCommands(this);
            _commentsCollection = new ObservableCollection<Commentsmodel>();
        }
        #endregion

        public ItineraryCommentsTabCommands CommentsTabCommands => _commentsTabCommands;
        public ObservableCollection<Commentsmodel> CommentsCollection
        {
            get { return _commentsCollection; }
            set
            {
                SetProperty(ref _commentsCollection, value);
                _commentsTabCommands.RaiseCanExecuteChanged();
            }
        }

        public Commentsmodel Commentsmodel
        {
            get { return _commentsmodel; }
            set
            {
                SetProperty(ref _commentsmodel, value);
                _commentsTabCommands.RaiseCanExecuteChanged();
            }
        }

        public string LoginId
        {
            get { return _loginId; }
            set
            {
                SetProperty(ref _loginId, value);
                _commentsTabCommands.RaiseCanExecuteChanged();
            }

        }

        public string ItineraryId
        {
            get { return _itineraryId; }
            set
            {
                SetProperty(ref _itineraryId, value);
                _commentsTabCommands.RaiseCanExecuteChanged();
                _commentsTabCommands.RetrieveCommand.Execute();
            }
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                SetProperty(ref _userName, value);
                _commentsTabCommands.RaiseCanExecuteChanged();
                //_commentsTabCommands.RetrieveCommand.Execute();
            }
        }
    }
}
