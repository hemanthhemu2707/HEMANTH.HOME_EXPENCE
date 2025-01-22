using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIITS.LMS.Repositories.GeneralTables.Login
{
    public class LoginDBTypes
    {
        [Key]
        [Column("us_id")]
        public int UserId { get; set; }

        [Column("us_name")]
        public string UserName { get; set; }

        [Column("us_pswd")]
        public string UserPassword { get; set; }

        [Column("us_phone_number")]
        public string UserPhone { get; set; }

        [Column("us_email_add")]
        public string UserEmailAdress { get; set; }

        [Column("us_is_admin")]
        public int AdminStatus { get; set; }

        [Column("us_approve_status")]
        public int UserApproveStatus { get; set; }

        [Column("us_family_id")]
        public int UserFamilyId { get; set; }

        [Column("us_isactive")]
        public int UserActiveStatus { get; set; }

    }
}
