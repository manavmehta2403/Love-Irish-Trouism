using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataAccessLayer.Models
{
    internal class UserManager
    {

        public string UserName { get; set; }
        public string Password { get; set; }


    }
    public class Userdetails
    {
        public string UserName { get; set; }
        public string Fullname { get; set; }
        public string EmailAddress { get; set; }
        public bool Disableaccount { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string EmailSignature { get; set; }
        public string Userroles { get; set; }
        public Guid UserRoldID { get; set; }


        public Guid Userid { get; set; }
        public string Createdby { get; set; }
        public string Updatedby { get; set; }
        public string Deletedby { get; set; }

    }
}
