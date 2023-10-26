using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Xml.Linq;


namespace SQLDataAccessLayer.Models
{
    public class Commentsmodel
    {
        public string Commentsid { get; set; }
        public long BookingId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierRefNo { get; set; }
        public string Comments { get; set; }
        public string Itineraryid { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime CommentedOn { get; set; }
        public string CommentedBy { get; set; }
        public string Commentedname { get; set; }
    }
}
