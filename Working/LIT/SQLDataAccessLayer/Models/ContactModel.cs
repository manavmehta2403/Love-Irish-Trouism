using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataAccessLayer.Models
{
    public class ContactModel
    {
        public Guid ContactId { get; set; }
        public Guid ContactTypeID { get; set; }
        private string _Contactautoid;
        public string Contactautoid { get { return _Contactautoid; } set {  _Contactautoid= value; if (_Contactautoid == "0") { _Contactautoid = ""; } } }
        public string ContactType { get; set; }
        public Guid ContactTitle { get; set; }
        public string UserName { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactGender { get; set; }
        public string PhoneWork { get; set; }
        public string PhoneHome { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Address { get; set; }
        public Guid City { get; set; }
        public Guid Region { get; set; }
        public Guid State { get; set; }
        public Guid Country { get; set; }
        public string Postcode { get; set; }
        public string CompanyName { get; set; }
        public string CompanyPosition { get; set; }
        public string CompanyDescription { get; set; }
        public string EmailOne { get; set; }
        public string EmailTwo { get; set; }
        public string Website { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
        public string ConfrimPassword { get; set; }
        public Guid UserRoleId { get; set; }
    }

    public class contacttype
    {
        public string ContactTypeid { get; set; }
        public string ContactTypename { get; set; }
    }

}
