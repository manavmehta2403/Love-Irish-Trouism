using Prism.Commands;
using SQLDataAccessLayer.DAL;
using System.ComponentModel;
using System.Windows;
using LIT.Modules.TabControl.ViewModels;
using SQLDataAccessLayer.Models;
using System.Collections.ObjectModel;
using LIT.Common;

namespace LIT.Modules.TabControl.Commands
{
    public class FollowUpCommand : IOperations
    {

        List<Userdetails> ListUserdet = new List<Userdetails>();
        LoadDropDownListValues loadDropDownListValues = new LoadDropDownListValues();
        public DelegateCommand SaveCommand
        {
            get; set;
        }

        public DelegateCommand UpdateCommand
        {
            get;
            set;
        }

        public DelegateCommand DeleteCommand
        {
            get;
            set;
        }

        public DelegateCommand AddCommand
        {
            get;
            set;
        }
        public DelegateCommand RetrieveCommand
        {
            get;
            set;
        }

        public new event EventHandler CanExecuteChanged;
        public readonly FollowupViewModel FuviewModel;
        public FollowUpCommand(FollowupViewModel viewModel)
        {
            FuviewModel = viewModel;


            FuviewModel.objFollup = new FollowupModel();
            ListUserdet = loadDropDownListValues.LoadUserDropDownlist("User");
            var observablecollection1 = new ObservableCollection<Userdetails>(ListUserdet);
            FuviewModel.Assignedtoval = observablecollection1;
            //  FuviewModel.objFollup.Assignedtoval = observablecollection1;
            FuviewModel.PropertyChanged += ViewModel_PropertyChanged;

            AddCommand = new DelegateCommand(ExecuteAdd, CanExecuteAdd);
            DeleteCommand = new DelegateCommand(ExecuteDelete, CanExecuteDelete);
            SaveCommand = new DelegateCommand(ExecuteSave, CanExecuteSave);
            RetrieveCommand = new DelegateCommand(ExecuteRetrieve, CanExecuteRetrieve);
        }


        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
        private bool CanExecuteAdd()
        {
            return FuviewModel.IntrFollowupViewModel.CanExecuteCommand("Add");
        }

        private void ExecuteAdd()
        {
            // Implement your Add logic here
            // Add a new item to the list
            // FuviewModel.Items.Add(new FollowupModel());

            AddtaskCommandExecute();
        }

        private bool CanExecuteDelete()
        {
            return FuviewModel.IntrFollowupViewModel.CanExecuteCommand("Delete");
        }

        private void ExecuteDelete()
        {
            // Implement your Delete logic here
            // Delete the selected item from the list

            if (FuviewModel.SelectedItem != null)
            {
                DeleteFollowuptaskCommandExecute();
                // FuviewModel.Items.Remove(FuviewModel.SelectedItem);
            }
        }

        private bool CanExecuteSave()
        {
            return FuviewModel.IntrFollowupViewModel.CanExecuteCommand("Save");
        }

        private void ExecuteSave()
        {
            // Implement your Add logic here
            // Add a new item to the list
            SaveFollowUpTaskCommandExecute();
            // FuviewModel.Items.Add(new FollowupModel());
        }


        private bool CanExecuteRetrieve()
        {
            return FuviewModel.IntrFollowupViewModel.CanExecuteCommand("Retrieve");
        }

        private void ExecuteRetrieve()
        {
            // Implement your Add logic here
            // Add a new item to the list
            // FuviewModel.Items.Add(new FollowupModel());

            ReteriveCommandExecute();
        }

        public bool CanExecuteCommand(string commandName)
        {
            //throw new System.NotImplementedException();
            return true;
        }


        #region private method

        private void SaveFollowUpTaskCommandExecute()
        {
            // MessageBox.Show("You are in followup command");
            int k = 0,j=0;
            k = FuviewModel.Folluptaskfull.Count;
            foreach (FollowupModel objfu in FuviewModel.Folluptaskfull)
            {
                FuviewModel.objFollup.Taskid = objfu.Taskid;
                FuviewModel.objFollup.TaskName = objfu.TaskName;
                FuviewModel.objFollup.Notes = objfu.Notes;
                FuviewModel.objFollup.DateDue = objfu.DateDue;
                FuviewModel.objFollup.DateCreated = objfu.DateCreated;
                // FuviewModel.objFollup.Assignedto = objfu.Assignedto;
                FuviewModel.objFollup.Assignedto = ((Userdetails)objfu.AssignedtoSelectedItem).Userid.ToString();
                FuviewModel.objFollup.Datecompleted = objfu.Datecompleted;
                FuviewModel.objFollup.Bookingid = objfu.Bookingid;
                FuviewModel.objFollup.Itineraryid = objfu.Itineraryid;
                if (string.IsNullOrEmpty(objfu.CreatedBy) || (objfu.CreatedBy == "00000000-0000-0000-0000-000000000000"))
                {
                    if (((LIT.Modules.TabControl.Commands.FollowUpCommand)((System.Delegate)SaveFollowUpTaskCommandExecute).Target).FuviewModel.Loginuserid != null)
                        FuviewModel.objFollup.CreatedBy = ((LIT.Modules.TabControl.Commands.FollowUpCommand)((System.Delegate)SaveFollowUpTaskCommandExecute).Target).FuviewModel.Loginuserid;
                }
                else
                {
                    FuviewModel.objFollup.CreatedBy = objfu.CreatedBy;
                }

                if (string.IsNullOrEmpty(objfu.ModifiedBy) || (objfu.ModifiedBy == "00000000-0000-0000-0000-000000000000"))
                {
                    if (((LIT.Modules.TabControl.Commands.FollowUpCommand)((System.Delegate)SaveFollowUpTaskCommandExecute).Target).FuviewModel.Loginuserid != null)
                        FuviewModel.objFollup.ModifiedBy = ((LIT.Modules.TabControl.Commands.FollowUpCommand)((System.Delegate)SaveFollowUpTaskCommandExecute).Target).FuviewModel.Loginuserid;
                }
                else
                {
                    FuviewModel.objFollup.ModifiedBy = objfu.ModifiedBy;
                }
                if (string.IsNullOrEmpty(objfu.DeletedBy) || (objfu.DeletedBy == "00000000-0000-0000-0000-000000000000"))
                {
                    if (((LIT.Modules.TabControl.Commands.FollowUpCommand)((System.Delegate)SaveFollowUpTaskCommandExecute).Target).FuviewModel.Loginuserid != null)
                        FuviewModel.objFollup.DeletedBy = ((LIT.Modules.TabControl.Commands.FollowUpCommand)((System.Delegate)SaveFollowUpTaskCommandExecute).Target).FuviewModel.Loginuserid;
                }
                else
                {
                    FuviewModel.objFollup.DeletedBy = objfu.DeletedBy;
                }

                string res = FuviewModel.objitindal.SaveUpdateFollowupTasks("I", FuviewModel.objFollup);
                if(res=="1")
                j = j + 1;
            }

            if(k==j)
            {
                MessageBox.Show("Follow up notes saved successfully");
            }

        }

        private void UpdateFollowuptaskCommandExecute()
        {

        }
        private void DeleteFollowuptaskCommandExecute()
        {

            MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to delete this item?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                FollowupModel fnvmobj = FuviewModel.SelectedItem;
                DeleteFollowup(fnvmobj, FuviewModel.Loginuserid);

            }

        }

        private void DeleteFollowup(FollowupModel objfu, string Loginuserid)
        {
            FuviewModel.objFollup.Taskid = objfu.Taskid;
            FuviewModel.objFollup.DeletedBy = objfu.DeletedBy;

            string res = FuviewModel.objitindal.DeleteFollowupTasks(FuviewModel.objFollup);



            if (!string.IsNullOrEmpty(res))
            {
                if (res.ToString().ToLower() == "1")
                {
                    MessageBox.Show("Follow up Deleted successfully");
                    FuviewModel.Folluptask.Remove(FuviewModel.Folluptask.Where(m => m.Taskid == FuviewModel.objFollup.Taskid).FirstOrDefault());
                    FuviewModel.Folluptaskfull.Remove(FuviewModel.Folluptaskfull.Where(m => m.Taskid == FuviewModel.objFollup.Taskid).FirstOrDefault());

                }
                else if (res.ToString().ToLower() == "-1")
                {
                    if (FuviewModel.Folluptask.Where(m => m.Taskid == FuviewModel.objFollup.Taskid).FirstOrDefault() != null)
                    {
                        MessageBox.Show("Follow up Deleted successfully");
                        FuviewModel.Folluptask.Remove(FuviewModel.Folluptask.Where(m => m.Taskid == FuviewModel.objFollup.Taskid).FirstOrDefault());
                        FuviewModel.Folluptaskfull.Remove(FuviewModel.Folluptaskfull.Where(m => m.Taskid == FuviewModel.objFollup.Taskid).FirstOrDefault());

                    }
                    if (FuviewModel.objFollup.Itineraryid != null && FuviewModel.objFollup.Bookingid != null)
                        ReteriveFollowup(FuviewModel.objFollup.Itineraryid, FuviewModel.objFollup.Bookingid, Loginuserid);
                }
            }
        }

        private void ReteriveCommandExecute()
        {
            FollowupViewModel fnvmobj = ((LIT.Modules.TabControl.Commands.FollowUpCommand)((System.Delegate)ReteriveCommandExecute).Target).FuviewModel;
            if (fnvmobj != null)
            {
                ReteriveFollowup(fnvmobj.Itineraryid, fnvmobj.Bookingid, fnvmobj.Loginuserid);
            }
        }

        private void ReteriveFollowup(string Itineraryid, long Bookingid, string Loginuserid)
        {
            List<FollowupModel> listfw = new List<FollowupModel>();
            // Folluptask = new ObservableCollection<FollowupViewModel>();
            listfw = FuviewModel.objitindal.FollowupRetrive(Guid.Parse(Itineraryid), Bookingid);
            //  var observablecollectionfoltask = new CustomObservable.CustomObservableCollection<FollowupModel>();
            if (listfw.Count > 0)
            {
                foreach (FollowupModel obj in listfw)
                {
                    FollowupModel objfuvm = new FollowupModel();
                    objfuvm.Taskid = obj.Taskid;
                    objfuvm.TaskName = obj.TaskName;
                    objfuvm.Itineraryid = obj.Itineraryid;
                    objfuvm.Bookingid = obj.Bookingid;
                    objfuvm.Notes = obj.Notes;
                    objfuvm.DateDue = obj.DateDue;
                    if (!string.IsNullOrEmpty(obj.Assignedto))
                    {
                        objfuvm.AssignedtoSelectedItem = ListUserdet.Where(x => x.Userid == Guid.Parse(obj.Assignedto)).FirstOrDefault();
                    }
                    else { objfuvm.AssignedtoSelectedItem = null; }
                    objfuvm.Datecompleted = obj.Datecompleted;
                    objfuvm.DateCreated = obj.DateCreated;
                    if (string.IsNullOrEmpty(obj.CreatedBy) || (obj.CreatedBy == "00000000-0000-0000-0000-000000000000"))
                    {
                        objfuvm.CreatedBy = Loginuserid;
                    }
                    else
                    {
                        objfuvm.CreatedBy = obj.CreatedBy;
                    }
                    if (string.IsNullOrEmpty(obj.ModifiedBy) || (obj.ModifiedBy == "00000000-0000-0000-0000-000000000000"))
                    {
                        objfuvm.ModifiedBy = Loginuserid;
                    }
                    else
                    {
                        objfuvm.ModifiedBy = obj.ModifiedBy;
                    }
                    if (string.IsNullOrEmpty(obj.DeletedBy) || (obj.DeletedBy == "00000000-0000-0000-0000-000000000000"))
                    {
                        objfuvm.DeletedBy = Loginuserid;
                    }
                    else
                    {
                        objfuvm.DeletedBy = obj.DeletedBy;
                    }


                    //objfuvm.ModifiedBy = obj.ModifiedBy;
                    //objfuvm.DeletedBy = obj.DeletedBy;
                    objfuvm.IsDeleted = obj.IsDeleted;
                    // FuviewModel.Folluptask.Add(objfuvm);
                    if (FuviewModel.Folluptaskfull.Where(x => x.Taskid == obj.Taskid).Count() == 0)
                    {
                        FuviewModel.Folluptask.Add(objfuvm);
                        FuviewModel.Folluptaskfull.Add(objfuvm);

                        //FuviewModel.Folluptask=



                        var observablecollectionft = new ObservableCollection<FollowupModel>(FuviewModel.Folluptaskfull.Where(x => x.Bookingid == obj.Bookingid).ToList());

                        // observablecollectionfoltask = (CustomObservable.CustomObservableCollection<FollowupModel>)observablecollectionft;
                        // FuviewModel.Folluptask = observablecollectionfoltask;
                        FuviewModel.Folluptask = observablecollectionft;
                    }
                    else
                    {
                        var observablecollectionft = new ObservableCollection<FollowupModel>(FuviewModel.Folluptaskfull.Where(x => x.Bookingid == obj.Bookingid).ToList());

                        // observablecollectionfoltask = (CustomObservable.CustomObservableCollection<FollowupModel>)observablecollectionft;
                        // FuviewModel.Folluptask = observablecollectionfoltask;
                        //FuviewModel.Folluptask.Where(x => x.Bookingid == obj.Bookingid).ToList();
                        FuviewModel.Folluptask = observablecollectionft;
                    }
                }



            }
            else
            {

                //// Remove the filtered bookings from the collection
                //var filteredBookings = bookings.Where(booking => booking.BookingId == targetBookingId).ToList();
                //foreach (var booking in filteredBookings)
                //{
                //    bookings.Remove(booking);
                //}

                //// Add back the other bookings
                //var otherBookings = bookings.Where(booking => booking.BookingId != targetBookingId).ToList();
                //foreach (var booking in otherBookings)
                //{
                //    bookings.Add(booking);
                // //}


                // var filteredBookings = FuviewModel.Folluptask.Where(x => x.Bookingid == Bookingid).ToList();

                // // Clear the current collection and add the filtered records
                //// FuviewModel.Folluptask.Clear();
                // foreach (var booking in filteredBookings)
                // {
                //     FuviewModel.Folluptask.Remove(booking);
                // }

                // var otherBookings = FuviewModel.Folluptask.Where(x => x.Bookingid == Bookingid).ToList();

                // // Clear the current collection and add the filtered records
                // // FuviewModel.Folluptask.Clear();
                // foreach (var booking in otherBookings)
                // {
                //     FuviewModel.Folluptask.Add(booking);
                // }

                if (FuviewModel.Folluptask.Where(x => x.Bookingid == Bookingid).ToList().Count > 0)
                {
                    var observablecollectionft = new ObservableCollection<FollowupModel>(FuviewModel.Folluptaskfull.Where(x => x.Bookingid == Bookingid).ToList());
                    FuviewModel.Folluptask = observablecollectionft;

                    // var observablecollectionft = new ObservableCollection<FollowupModel>(FuviewModel.Folluptaskfull.Where(x => x.Bookingid == obj.Bookingid).ToList());

                    // observablecollectionfoltask = (CustomObservable.CustomObservableCollection<FollowupModel>)observablecollectionft;
                    // FuviewModel.Folluptask = observablecollectionfoltask;
                    //FuviewModel.Folluptask.Where(x => x.Bookingid == Bookingid).ToList();
                }
                else
                {
                    var observablecollectionft = new ObservableCollection<FollowupModel>(FuviewModel.Folluptaskfull.Where(x => x.Bookingid == Bookingid).ToList());
                    FuviewModel.Folluptask = observablecollectionft;
                    // var observablecollectionft = new ObservableCollection<FollowupModel>(FuviewModel.Folluptaskfull.Where(x => x.Bookingid == obj.Bookingid).ToList());

                    // observablecollectionfoltask = (CustomObservable.CustomObservableCollection<FollowupModel>)observablecollectionft;
                    // FuviewModel.Folluptask = observablecollectionfoltask;
                    // FuviewModel.Folluptask.Clear();
                }
                //////Folluptask = new ObservableCollection<FollowupViewModel>();
            }
        }

        private void AddtaskCommandExecute()
        {
            //MessageBox.Show("Add Task Command Exec");
            AddItem();
        }
        private void AddItem()
        {
            // MessageBox.Show("Add item fun");
            FollowupModel flup;
            flup = new FollowupModel();

            flup.Taskid = FuviewModel.Taskid;
            flup.TaskName = FuviewModel.TaskName;
            flup.Notes = FuviewModel.Notes;
            flup.DateDue = FuviewModel.DateDue;
            flup.AssignedtoSelectedItem = FuviewModel.AssignedtoSelectedItem;
            flup.Datecompleted = FuviewModel.Datecompleted;
            flup.DateCreated = FuviewModel.DateCreated;

            flup.Bookingid = FuviewModel.Bookingid;
            flup.Itineraryid = FuviewModel.Itineraryid;

            flup.CreatedBy = FuviewModel.CreatedBy;
            flup.ModifiedBy = FuviewModel.ModifiedBy;
            flup.DeletedBy = FuviewModel.DeletedBy;

            FuviewModel.Folluptask.Add(flup);
            FuviewModel.Folluptaskfull.Add(flup);
            //this.Folluptask = Folluptask;
            //Followup fuu=new Followup();
            //fuu.dgFollowupTask.DataContext = Folluptask;


            //this.Taskid = Guid.NewGuid().ToString();
            //this.TaskName = "";// "New Service" + " (" + (SupplierSM.Count + 1) + ")";
            //this.Notes = "";
            //this.DateDue = DateTime.Now;
            //this.Assignedto = null;
            //this.Datecompleted = DateTime.Now;
            //this.Bookingid = null;
            //this.Itineraryid = null;            
            //this.CreatedBy = string.Empty;
            //this.ModifiedBy = string.Empty;
            //this.DeletedBy = string.Empty;

            // Folluptask.Add(this);


        }


        #endregion

    }




    //public class FollowUpCommand : DelegateCommandBase
    //{
    //    private static ItineraryDAL _objitindal;
    //    private static FollowupModel _objFollup;
    //    private static FollowupViewModel _FollowupviewModel;
    //    private readonly Errorlog _erobj;
    //    public FollowUpCommand(FollowupViewModel Follupcmd)
    //    {
    //        _FollowupviewModel = Follupcmd ?? throw new ArgumentNullException(nameof(Follupcmd));
    //        _objitindal = new ItineraryDAL();
    //        _erobj = new Errorlog();
    //        _objFollup = new FollowupModel();
    //        _FollowupviewModel.PropertyChanged += ViewModel_PropertyChanged;
    //    }

    //    public new event EventHandler CanExecuteChanged;

    //    protected override bool CanExecute(object parameter)
    //    {
    //        return true;
    //       // return !string.IsNullOrEmpty(_FollowupviewModel.UserName) && !string.IsNullOrEmpty(_FollowupviewModel.Password);
    //    }

    //    protected override void Execute(object parameter)
    //    {
    //        try
    //        {

    //            //string retobj = _objuserdal.UserloginVerify(_FollowupviewModel., _viewModel.Password);
    //            //if (retobj.ToUpper() == "EXISTS")
    //            //{
    //            //    // Navigate to MainWindow
    //            //    LIT.Old_LIT.MainWindow mainWindow = new(_viewModel.UserName);
    //            //    mainWindow.Show();

    //            //    // Close the Login window
    //            //    Application.Current.MainWindow?.Close();
    //            //}
    //            //else if (retobj.ToUpper() == "NOT EXISTS")
    //            //{
    //            //    // Handle the case where login credentials are incorrect.
    //            //    MessageBox.Show("Please provide valid Username & Password");
    //            //}
    //        }
    //        catch (Exception ex)
    //        {
    //            _erobj.WriteErrorLoginfo("Login", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
    //        }
    //    }


    //    /* public DelegateCommand SaveFollowuptaskCommand { get; set; } = new DelegateCommand(() =>
    //     {
    //         _objFollup = new FollowupModel();
    //         _objFollup.Taskid = _FollowupviewModel.Taskid;
    //         _objFollup.TaskName = _FollowupviewModel.TaskName;
    //         _objFollup.Notes = _FollowupviewModel.Notes;
    //         _objFollup.DateDue = _FollowupviewModel.DateDue;
    //         _objFollup.Assignedto = _FollowupviewModel.Assignedto;
    //         _objFollup.Datecompleted = _FollowupviewModel.Datecompleted;
    //         _objFollup.Bookingid = _FollowupviewModel.Bookingid;
    //         _objFollup.Itineraryid = _FollowupviewModel.Itineraryid;
    //         _objFollup.CreatedBy = _FollowupviewModel.CreatedBy;
    //         _objFollup.ModifiedBy = _FollowupviewModel.ModifiedBy;
    //         _objFollup.DeletedBy = _FollowupviewModel.DeletedBy;

    //         string res=_objitindal.SaveUpdateFollowupTasks("I", _objFollup);
    //         // Logic for Save operation
    //     });
    //    */


    //    //private ObservableCollection<FollowupViewModel> _Folluptask;
    //    //public ObservableCollection<FollowupViewModel> Folluptask
    //    //{
    //    //    get { return _Folluptask ?? (_Folluptask = new ObservableCollection<FollowupViewModel>()); }
    //    //    set
    //    //    {
    //    //        _Folluptask = value;
    //    //    }
    //    //}
    //    public DelegateCommand AddtaskCommand { get; set; } = new DelegateCommand(() =>
    //    {
    //        //AddItem();
    //        // Logic for Update operation
    //    });
    //    public DelegateCommand UpdateFollowuptaskCommand { get; } = new DelegateCommand(() =>
    //    {
    //        // Logic for Update operation
    //    });

    //    public DelegateCommand DeleteFollowuptaskCommand { get; } = new DelegateCommand(() =>
    //    {
    //        // Logic for Delete operation
    //    });




    //    private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    //    {
    //        CanExecuteChanged?.Invoke(this, new EventArgs());
    //    }
    //}





}
