using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIITS.LMS.Repositories.GeneralTables.ExpenseMasterTableDBTypes
{
    public class ExpenseMasterTableDBTypes
    {
        [Key]
        [Column("expense_id")]
        public int ExpenceID { get; set; }

        [Column("exp_name")]
        public string ExpenceName { get; set; }

        [Column("e_price")]
        public decimal ExpensePrice { get; set; }

        [Column("e_catergory_id")]
        public int ExpenceCategory { get; set; }

        [Column("e_date")]
        public DateTime ExpenceDate { get; set; }

        [Column("e_us_id")]
        public int ExpenceUserId { get; set; }

        [Column("e_fam_id")]
        public int ExpenceFamId { get; set; }

        [Column("e_desc")]
        public string ExpenceDescription { get; set; }

        [Column("e_entry_date")]
        public DateTime ExpenceEntryDate { get; set; }



    }
}
