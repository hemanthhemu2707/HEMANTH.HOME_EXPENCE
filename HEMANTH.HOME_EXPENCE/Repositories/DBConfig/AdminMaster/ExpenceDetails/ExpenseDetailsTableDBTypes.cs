using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIITS.LMS.Repositories.GeneralTables.ExpenseDetailsTableDBTypes
{
    public class ExpenseDetailsTableDBTypes
    {
        [Key]
        [Column("expd_id")]
        public int ExpenceDetailsID { get; set; }

        [Column("expd_expe_id")]
        public int ExpenceDetailsExpID { get; set; }

        [Column("e_us_id")]
        public int ExpenceUserID { get; set; }

        [Column("expd_exp_price")]
        public decimal ExpencePersPrice { get; set; }

        [Column("expd_fam_id")]
        public int ExpenceFamilyId { get; set; }


        [Column("expd_entry_date")]
        public DateTime ExpenceDetailsEntryDate { get; set; }

        [Column("expd_to_user")]
        public int ParentUserId { get; set; }
    }
}
