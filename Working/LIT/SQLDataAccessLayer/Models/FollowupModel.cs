using System;



namespace SQLDataAccessLayer.Models
{
    public class FollowupModel
    {

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
        private object _AssignedtoSelectedItem;
        private string _Loginuserid;


        public string Loginuserid
        {
            get { return _Loginuserid; }
            set
            {
                _Loginuserid = value;
            }
        }
        public string Taskid
        {
            get { return _Taskid; }
            set
            {
                _Taskid = value;
            }
        }

        public string TaskName
        {
            get { return _TaskName; }
            set
            {
                _TaskName = value;
            }
        }
        public string Notes
        {
            get { return _Notes; }
            set
            {
                _Notes = value;
            }
        }
        public DateTime? DateDue
        {
            get { return _DateDue; }
            set
            {
                _DateDue = value;
            }
        }
        public DateTime? DateCreated
        {
            get { return _DateCreated; }
            set
            {
                _DateCreated = value;
            }
        }
        public string Assignedto
        {
            get { return _Assignedto; }
            set
            {
                _Assignedto = value;
            }
        }

        public DateTime? Datecompleted
        {
            get { return _Datecompleted; }
            set
            {
                _Datecompleted = value;
            }
        }

        public long Bookingid
        {
            get { return _Bookingid; }
            set
            {
                _Bookingid = value;
            }
        }

        public string Itineraryid
        {
            get { return _Itineraryid; }
            set
            {
                _Itineraryid = value;
            }
        }

        public string CreatedBy
        {
            get { return _CreatedBy; }
            set
            {
                _CreatedBy = value;
            }
        }

        public string ModifiedBy
        {
            get { return _ModifiedBy; }
            set
            {
                _ModifiedBy = value;
            }
        }

        public string DeletedBy
        {
            get { return _DeletedBy; }
            set
            {
                _DeletedBy = value;
            }
        }

        public bool IsDeleted
        {
            get { return _IsDeleted; }
            set
            {
                _IsDeleted = value;

            }
        }

        public object AssignedtoSelectedItem
        {
            get { return _AssignedtoSelectedItem; }
            set
            {
                _AssignedtoSelectedItem = value;
            }
        }
    }
}
