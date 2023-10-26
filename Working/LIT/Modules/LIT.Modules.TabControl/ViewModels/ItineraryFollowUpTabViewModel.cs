using System.Collections.ObjectModel;
using LIT.Modules.TabControl.Commands;
using Prism.Mvvm;
using SQLDataAccessLayer.DAL;
using SQLDataAccessLayer.Models;
using LIT.Common;

namespace LIT.Modules.TabControl.ViewModels
{
    public class ItineraryFollowUpTabViewModel : BindableBase
    {

        private readonly ItineraryFollowUpTabCommand _followCommand;

        public ItineraryFollowUpTabCommand followCommand { get; set; }
        public IOperations IntrFollowupViewModel { get; private set; }

        //   public ObservableCollection<FollowupModel> Items = new();

        private FollowupModel _selectedItem;
        public FollowupModel SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        public ItineraryFollowUpTabViewModel()
        {
            objitindal = new ItineraryDAL();
            followCommand = new ItineraryFollowUpTabCommand(this);
            IntrFollowupViewModel = new ItineraryFollowUpTabCommand(this);
        }


        #region private

        public FollowupModel objFollup;
        public ItineraryDAL objitindal;

        private string _Taskid;
        private string _TaskName;
        private string _Notes;
        private DateTime? _DateCreated;
        private DateTime? _DateDue;
        private string _Assignedto;
        private DateTime? _Datecompleted;
        private long _Bookingid;
        private string _Itineraryid;
        private string _CreatedBy;
        private string _ModifiedBy;
        private bool _IsDeleted;
        private string _DeletedBy;
        private string _Loginuserid;

        #endregion


        #region public proptery


        public string Taskid
        {
            get { return _Taskid; }
            set
            {
                SetProperty(ref _Taskid, value);
            }
        }

        public string TaskName
        {
            get { return _TaskName; }
            set
            {
                SetProperty(ref _TaskName, value);
                //  FollowUpCommand.RaiseCanExecuteChanged();
            }
        }
        public string Notes
        {
            get { return _Notes; }
            set
            {
                SetProperty(ref _Notes, value);
                // FollowUpCommand.RaiseCanExecuteChanged();
            }
        }
        public DateTime? DateDue
        {
            get { return _DateDue; }
            set
            {
                SetProperty(ref _DateDue, value);
                // FollowUpCommand.RaiseCanExecuteChanged();
            }
        }

        public DateTime? DateCreated
        {
            get { return _DateCreated; }
            set
            {
                SetProperty(ref _DateCreated, value);
                // FollowUpCommand.RaiseCanExecuteChanged();
            }
        }

        //public string Assignedto
        //{
        //    get { return _Assignedto; }
        //    set
        //    {
        //        SetProperty(ref _Assignedto, value);
        //        FollowUpCommand.RaiseCanExecuteChanged();
        //    }
        //}

        private object _AssignedtoselectedItem;
        public object AssignedtoSelectedItem
        {
            get { return _AssignedtoselectedItem; }
            set
            {
                SetProperty(ref _AssignedtoselectedItem, value);
                // FollowUpCommand.RaiseCanExecuteChanged();
            }
        }


        public DateTime? Datecompleted
        {
            get { return _Datecompleted; }
            set
            {
                SetProperty(ref _Datecompleted, value);
                // FollowUpCommand.RaiseCanExecuteChanged();
            }
        }

        public long Bookingid
        {
            get { return _Bookingid; }
            set
            {
                SetProperty(ref _Bookingid, value);
                // _loginCommand.RaiseCanExecuteChanged();
            }
        }

        public string Itineraryid
        {
            get { return _Itineraryid; }
            set
            {
                SetProperty(ref _Itineraryid, value);
                // _loginCommand.RaiseCanExecuteChanged();
            }
        }

        public string CreatedBy
        {
            get { return _CreatedBy; }
            set
            {
                SetProperty(ref _CreatedBy, value);
                // _loginCommand.RaiseCanExecuteChanged();
            }
        }

        public string ModifiedBy
        {
            get { return _ModifiedBy; }
            set
            {
                SetProperty(ref _ModifiedBy, value);
                // _loginCommand.RaiseCanExecuteChanged();
            }
        }

        public string DeletedBy
        {
            get { return _DeletedBy; }
            set
            {
                SetProperty(ref _DeletedBy, value);
                // _loginCommand.RaiseCanExecuteChanged();
            }
        }

        public string Loginuserid
        {
            get { return _Loginuserid; }
            set
            {
                SetProperty(ref _Loginuserid, value);
                // _loginCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsDeleted
        {
            get { return _IsDeleted; }
            set
            {
                SetProperty(ref _IsDeleted, value);
                // _loginCommand.RaiseCanExecuteChanged();
            }
        }


        //private CustomObservableCollection<FollowupModel> _Folluptask;
        //public CustomObservableCollection<FollowupModel> Folluptask
        //{
        //    get { return _Folluptask ?? (_Folluptask = new CustomObservableCollection<FollowupModel>()); }
        //    set
        //    {
        //        // _Folluptask = value;
        //        SetProperty(ref _Folluptask, value);
        //        // ReteriveCommand.RaiseCanExecuteChanged();
        //    }
        //    //get;set;
        //}
        //private CustomObservableCollection<FollowupModel> _Folluptaskfull;
        //public CustomObservableCollection<FollowupModel> Folluptaskfull
        //{
        //    get { return _Folluptaskfull ?? (_Folluptaskfull = new CustomObservableCollection<FollowupModel>()); }
        //    set
        //    {
        //        // _Folluptask = value;
        //        SetProperty(ref _Folluptaskfull, value);
        //        // ReteriveCommand.RaiseCanExecuteChanged();
        //    }
        //    //get;set;
        //}

        private ObservableCollection<Userdetails> _Assignedtoval;
        public ObservableCollection<Userdetails> Assignedtoval
        {
            get { return _Assignedtoval ?? (_Assignedtoval = new ObservableCollection<Userdetails>()); }
            set
            {
                _Assignedtoval = value;
            }
        }


        private ObservableCollection<FollowupModel> _Folluptask;
        public ObservableCollection<FollowupModel> Folluptask
        {
            get { return _Folluptask ?? (_Folluptask = new ObservableCollection<FollowupModel>()); }
            set
            {
                // _Folluptask = value;
                SetProperty(ref _Folluptask, value);
                // ReteriveCommand.RaiseCanExecuteChanged();
            }
            //get;set;
        }
        private ObservableCollection<FollowupModel> _Folluptaskfull;
        public ObservableCollection<FollowupModel> Folluptaskfull
        {
            get { return _Folluptaskfull ?? (_Folluptaskfull = new ObservableCollection<FollowupModel>()); }
            set
            {
                // _Folluptask = value;
                SetProperty(ref _Folluptaskfull, value);
                // ReteriveCommand.RaiseCanExecuteChanged();
            }
            //get;set;
        }
        #endregion



        //#region private

        //private static FollowupModel _objFollup;
        //private static ItineraryDAL _objitindal;

        //private string _Taskid;
        //private string _TaskName;
        //private string _Notes;
        //private DateTime? _DateDue;
        //private string _Assignedto;
        //private DateTime? _Datecompleted;
        //private long _Bookingid;
        //private string _Itineraryid;
        //private string _CreatedBy;
        //private string _ModifiedBy;
        //private bool _IsDeleted;
        //private string _DeletedBy;
        //private string _Loginuserid;

        //#endregion

        //#region ctor
        //LoadDropDownListValues loadDropDownListValues = new LoadDropDownListValues();
        //List<Userdetails> ListUserdet = new List<Userdetails>();
        //public FollowupViewModel()
        //// IRegionManager regionManager) :
        ////base(regionManager)
        //{
        //    FollowUpCommand = new FollowUpCommand(this);
        //    _objFollup = new FollowupModel();
        //    _objitindal = new ItineraryDAL();

        //    //Folluptask = new ObservableCollection<FollowupViewModel>();

        //    this.SaveFollowUpTaskCommand = new DelegateCommand(SaveFollowUpTaskCommandExecute);
        //    this.UpdateFollowuptaskCommand = new DelegateCommand(UpdateFollowuptaskCommandExecute);
        //    this.DeleteFollowuptaskCommand = new DelegateCommand(DeleteFollowuptaskCommandExecute);
        //    this.AddtaskCommand = new DelegateCommand(AddtaskCommandExecute);

        //    this.ReteriveCommand = new DelegateCommand(ReteriveCommandExecute);
        //    // Folluptask = ReteriveFollowup();
        //    ListUserdet = loadDropDownListValues.LoadUserDropDownlist("User");
        //    var observablecollection1 = new ObservableCollection<Userdetails>(ListUserdet);
        //    Assignedtoval = observablecollection1;
        //}

        //#endregion

        //#region public proptery

        //public FollowUpCommand FollowUpCommand
        //{
        //    get;
        //    set;
        //}

        //public DelegateCommand SaveFollowUpTaskCommand
        //{
        //    get;
        //    set;
        //}

        //public DelegateCommand UpdateFollowuptaskCommand
        //{
        //    get;
        //    set;
        //}

        //public DelegateCommand DeleteFollowuptaskCommand
        //{
        //    get;
        //    set;
        //}

        //public DelegateCommand AddtaskCommand
        //{
        //    get;
        //    set;
        //}
        //public DelegateCommand ReteriveCommand
        //{
        //    get;
        //    set;
        //}

        //public string Taskid
        //{
        //    get { return _Taskid; }
        //    set
        //    {
        //        SetProperty(ref _Taskid, value);
        //        // _loginCommand.RaiseCanExecuteChanged();
        //    }
        //}

        //public string TaskName
        //{
        //    get { return _TaskName; }
        //    set
        //    {
        //        SetProperty(ref _TaskName, value);
        //        FollowUpCommand.RaiseCanExecuteChanged();
        //    }
        //}
        //public string Notes
        //{
        //    get { return _Notes; }
        //    set
        //    {
        //        SetProperty(ref _Notes, value);
        //        FollowUpCommand.RaiseCanExecuteChanged();
        //    }
        //}
        //public DateTime? DateDue
        //{
        //    get { return _DateDue; }
        //    set
        //    {
        //        SetProperty(ref _DateDue, value);
        //        FollowUpCommand.RaiseCanExecuteChanged();
        //    }
        //}

        ////public string Assignedto
        ////{
        ////    get { return _Assignedto; }
        ////    set
        ////    {
        ////        SetProperty(ref _Assignedto, value);
        ////        FollowUpCommand.RaiseCanExecuteChanged();
        ////    }
        ////}

        //private object _AssignedtoselectedItem;
        //public object AssignedtoSelectedItem
        //{
        //    get { return _AssignedtoselectedItem; }
        //    set
        //    {
        //        SetProperty(ref _AssignedtoselectedItem, value);
        //        FollowUpCommand.RaiseCanExecuteChanged();
        //    }
        //}


        //public DateTime? Datecompleted
        //{
        //    get { return _Datecompleted; }
        //    set
        //    {
        //        SetProperty(ref _Datecompleted, value);
        //        FollowUpCommand.RaiseCanExecuteChanged();
        //    }
        //}

        //public long Bookingid
        //{
        //    get { return _Bookingid; }
        //    set
        //    {
        //        SetProperty(ref _Bookingid, value);
        //        // _loginCommand.RaiseCanExecuteChanged();
        //    }
        //}

        //public string Itineraryid
        //{
        //    get { return _Itineraryid; }
        //    set
        //    {
        //        SetProperty(ref _Itineraryid, value);
        //        // _loginCommand.RaiseCanExecuteChanged();
        //    }
        //}

        //public string CreatedBy
        //{
        //    get { return _CreatedBy; }
        //    set
        //    {
        //        SetProperty(ref _CreatedBy, value);
        //        // _loginCommand.RaiseCanExecuteChanged();
        //    }
        //}

        //public string ModifiedBy
        //{
        //    get { return _ModifiedBy; }
        //    set
        //    {
        //        SetProperty(ref _ModifiedBy, value);
        //        // _loginCommand.RaiseCanExecuteChanged();
        //    }
        //}

        //public string DeletedBy
        //{
        //    get { return _DeletedBy; }
        //    set
        //    {
        //        SetProperty(ref _DeletedBy, value);
        //        // _loginCommand.RaiseCanExecuteChanged();
        //    }
        //}

        //public string Loginuserid
        //{
        //    get { return _Loginuserid; }
        //    set
        //    {
        //        SetProperty(ref _Loginuserid, value);
        //        // _loginCommand.RaiseCanExecuteChanged();
        //    }
        //}

        //public bool IsDeleted
        //{
        //    get { return _IsDeleted; }
        //    set
        //    {
        //        SetProperty(ref _IsDeleted, value);
        //        // _loginCommand.RaiseCanExecuteChanged();
        //    }
        //}

        //private ObservableCollection<Userdetails> _Assignedtoval;
        //public ObservableCollection<Userdetails> Assignedtoval
        //{
        //    get { return _Assignedtoval ?? (_Assignedtoval = new ObservableCollection<Userdetails>()); }
        //    set
        //    {
        //        _Assignedtoval = value;
        //    }
        //}

        //private ObservableCollection<FollowupViewModel> _Folluptask;
        //public ObservableCollection<FollowupViewModel> Folluptask
        //{
        //    get { return _Folluptask ?? (_Folluptask = new ObservableCollection<FollowupViewModel>()); }
        //    set
        //    {
        //        // _Folluptask = value;
        //        SetProperty(ref _Folluptask, value);
        //        // ReteriveCommand.RaiseCanExecuteChanged();
        //    }
        //    //get;set;
        //}
        //#endregion

        //#region private method

        //private void SaveFollowUpTaskCommandExecute()
        //{
        //    MessageBox.Show("You are in followup command");
        //    foreach (FollowupViewModel objfu in Folluptask)
        //    {
        //        _objFollup.Taskid = objfu.Taskid;
        //        _objFollup.TaskName = objfu.TaskName;
        //        _objFollup.Notes = objfu.Notes;
        //        _objFollup.DateDue = objfu.DateDue;
        //        // _objFollup.Assignedto = objfu.Assignedto;
        //        _objFollup.Assignedto = ((LIT.Models.Userdetails)objfu.AssignedtoSelectedItem).Userid.ToString();
        //        _objFollup.Datecompleted = objfu.Datecompleted;
        //        _objFollup.Bookingid = objfu.Bookingid;
        //        _objFollup.Itineraryid = objfu.Itineraryid;
        //        if (string.IsNullOrEmpty(objfu.CreatedBy) || (objfu.CreatedBy == "00000000-0000-0000-0000-000000000000"))
        //        {
        //            if (((LIT.Modules.TabControl.ViewModels.FollowupViewModel)((System.Delegate)SaveFollowUpTaskCommandExecute).Target).Loginuserid != null)
        //                _objFollup.CreatedBy = ((LIT.Modules.TabControl.ViewModels.FollowupViewModel)((System.Delegate)SaveFollowUpTaskCommandExecute).Target).Loginuserid;
        //        }
        //        else
        //        {
        //            _objFollup.CreatedBy = objfu.CreatedBy;
        //        }

        //        if (string.IsNullOrEmpty(objfu.ModifiedBy) || (objfu.ModifiedBy == "00000000-0000-0000-0000-000000000000"))
        //        {
        //            if (((LIT.Modules.TabControl.ViewModels.FollowupViewModel)((System.Delegate)SaveFollowUpTaskCommandExecute).Target).Loginuserid != null)
        //                _objFollup.ModifiedBy = ((LIT.Modules.TabControl.ViewModels.FollowupViewModel)((System.Delegate)SaveFollowUpTaskCommandExecute).Target).Loginuserid;
        //        }
        //        else
        //        {
        //            _objFollup.ModifiedBy = objfu.ModifiedBy;
        //        }
        //        if (string.IsNullOrEmpty(objfu.DeletedBy) || (objfu.DeletedBy == "00000000-0000-0000-0000-000000000000"))
        //        {
        //            if (((LIT.Modules.TabControl.ViewModels.FollowupViewModel)((System.Delegate)SaveFollowUpTaskCommandExecute).Target).Loginuserid != null)
        //                _objFollup.DeletedBy = ((LIT.Modules.TabControl.ViewModels.FollowupViewModel)((System.Delegate)SaveFollowUpTaskCommandExecute).Target).Loginuserid;
        //        }
        //        else
        //        {
        //            _objFollup.DeletedBy = objfu.DeletedBy;
        //        }

        //        string res = _objitindal.SaveUpdateFollowupTasks("I", _objFollup);
        //    }

        //}

        //private void UpdateFollowuptaskCommandExecute()
        //{

        //}
        //private void DeleteFollowuptaskCommandExecute()
        //{

        //    MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to delete this item?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
        //    if (messageBoxResult == MessageBoxResult.Yes)
        //    {
        //        FollowupViewModel fnvmobj = ((LIT.Modules.TabControl.ViewModels.FollowupViewModel)((System.Delegate)DeleteFollowuptaskCommandExecute).Target);
        //        DeleteFollowup(fnvmobj, Loginuserid);

        //    }

        //}

        //private void DeleteFollowup(FollowupViewModel objfu, string Loginuserid)
        //{
        //    _objFollup.Taskid = objfu.Taskid;
        //    _objFollup.DeletedBy = objfu.DeletedBy;

        //    string res = _objitindal.DeleteFollowupTasks(_objFollup);



        //    if (!string.IsNullOrEmpty(res))
        //    {
        //        if (res.ToString().ToLower() == "1")
        //        {
        //            MessageBox.Show("Follow up Deleted successfully");
        //            Folluptask.Remove(Folluptask.Where(m => m.Taskid == _objFollup.Taskid).FirstOrDefault());
        //        }
        //        else if (res.ToString().ToLower() == "-1")
        //        {
        //            if (Folluptask.Where(m => m.Taskid == _objFollup.Taskid).FirstOrDefault() != null)
        //            {
        //                MessageBox.Show("Follow up Deleted successfully");
        //                Folluptask.Remove(Folluptask.Where(m => m.Taskid == _objFollup.Taskid).FirstOrDefault());

        //            }
        //            if (_objFollup.Itineraryid != null && _objFollup.Bookingid != null)
        //                ReteriveFollowup(_objFollup.Itineraryid, _objFollup.Bookingid, Loginuserid);
        //        }
        //    }
        //}

        //private void ReteriveCommandExecute()
        //{
        //    FollowupViewModel fnvmobj = ((LIT.Modules.TabControl.ViewModels.FollowupViewModel)((System.Delegate)DeleteFollowuptaskCommandExecute).Target);
        //    if (fnvmobj != null)
        //    {
        //        ReteriveFollowup(fnvmobj.Itineraryid, fnvmobj.Bookingid, fnvmobj.Loginuserid);
        //    }
        //}

        //private void ReteriveFollowup(string Itineraryid, long Bookingid, string Loginuserid)
        //{
        //    List<FollowupModel> listfw = new List<FollowupModel>();
        //    // Folluptask = new ObservableCollection<FollowupViewModel>();
        //    listfw = _objitindal.FollowupRetrive(Guid.Parse(Itineraryid), Bookingid);
        //    if (listfw.Count > 0)
        //    {
        //        foreach (FollowupModel obj in listfw)
        //        {
        //            FollowupViewModel objfuvm = new FollowupViewModel();
        //            objfuvm.Taskid = obj.Taskid;
        //            objfuvm.TaskName = obj.TaskName;
        //            objfuvm.Itineraryid = obj.Itineraryid;
        //            objfuvm.Bookingid = obj.Bookingid;
        //            objfuvm.Notes = obj.Notes;
        //            objfuvm.DateDue = obj.DateDue;
        //            if (!string.IsNullOrEmpty(obj.Assignedto))
        //            {
        //                objfuvm.AssignedtoSelectedItem = ListUserdet.Where(x => x.Userid == Guid.Parse(obj.Assignedto)).FirstOrDefault();
        //            }
        //            else { objfuvm.AssignedtoSelectedItem = null; }
        //            objfuvm.Datecompleted = obj.Datecompleted;
        //            if (string.IsNullOrEmpty(obj.CreatedBy) || (obj.CreatedBy == "00000000-0000-0000-0000-000000000000"))
        //            {
        //                objfuvm.CreatedBy = Loginuserid;
        //            }
        //            else
        //            {
        //                objfuvm.CreatedBy = obj.CreatedBy;
        //            }
        //            if (string.IsNullOrEmpty(obj.ModifiedBy) || (obj.ModifiedBy == "00000000-0000-0000-0000-000000000000"))
        //            {
        //                objfuvm.ModifiedBy = Loginuserid;
        //            }
        //            else
        //            {
        //                objfuvm.ModifiedBy = obj.ModifiedBy;
        //            }
        //            if (string.IsNullOrEmpty(obj.DeletedBy) || (obj.DeletedBy == "00000000-0000-0000-0000-000000000000"))
        //            {
        //                objfuvm.DeletedBy = Loginuserid;
        //            }
        //            else
        //            {
        //                objfuvm.DeletedBy = obj.DeletedBy;
        //            }


        //            //objfuvm.ModifiedBy = obj.ModifiedBy;
        //            //objfuvm.DeletedBy = obj.DeletedBy;
        //            objfuvm.IsDeleted = obj.IsDeleted;
        //            Folluptask.Add(objfuvm);
        //        }


        //    }
        //    else
        //    {
        //        //this.Folluptask.Clear();
        //        //////Folluptask = new ObservableCollection<FollowupViewModel>();
        //    }
        //}

        //private void AddtaskCommandExecute()
        //{
        //    //MessageBox.Show("Add Task Command Exec");
        //    AddItem();
        //}







        //private void AddItem()
        //{
        //    // MessageBox.Show("Add item fun");
        //    FollowupViewModel flup;
        //    flup = new FollowupViewModel();

        //    flup.Taskid = this.Taskid;
        //    flup.TaskName = this.TaskName;
        //    flup.Notes = this.Notes;
        //    flup.DateDue = this.DateDue;
        //    flup.AssignedtoSelectedItem = this.AssignedtoSelectedItem;
        //    flup.Datecompleted = this.Datecompleted;
        //    flup.Bookingid = this.Bookingid;
        //    flup.Itineraryid = this.Itineraryid;

        //    flup.CreatedBy = this.CreatedBy;
        //    flup.ModifiedBy = this.ModifiedBy;
        //    flup.DeletedBy = this.DeletedBy;

        //    this.Folluptask.Add(flup);
        //    //this.Folluptask = Folluptask;
        //    //ItineraryFollowUpTab fuu=new ItineraryFollowUpTab();
        //    //fuu.dgFollowupTask.DataContext = Folluptask;


        //    //this.Taskid = Guid.NewGuid().ToString();
        //    //this.TaskName = "";// "New Service" + " (" + (SupplierSM.Count + 1) + ")";
        //    //this.Notes = "";
        //    //this.DateDue = DateTime.Now;
        //    //this.Assignedto = null;
        //    //this.Datecompleted = DateTime.Now;
        //    //this.Bookingid = null;
        //    //this.Itineraryid = null;            
        //    //this.CreatedBy = string.Empty;
        //    //this.ModifiedBy = string.Empty;
        //    //this.DeletedBy = string.Empty;

        //    // Folluptask.Add(this);


        //}


        //#endregion
    }

}
