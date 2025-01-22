using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEMANTH.HOME_EXPENCE.Repositories.DBConfig.AdminMaster.Family
{
    public class FamilyTableDBTypes
    {
        [Key]
        [Column("fam_id")]
        public int FamilyID { get; set; }

        [Column("fam_name")]
        public string FamilyName { get; set; }

        [Column("fam_description")]
        public string FamilyDesc { get; set; }

        [Column("fam_entry_date")]
        public DateTime FamilyEntryDate { get; set; }

        [Column("fam_home_name")]
        public string HomeName { get; set; }

        [Column("fam_home_location")]
        public string FamilyLocation { get; set; }

        [Column("fam_home_photo")]
        public string FamilyPhoto { get; set; }

        [Column("fam_electric_bill_number")]
        public int FamilyEleBillNumber { get; set; }

        [Column("fam_location_map")]
        public string FamilyMap { get; set; }

        [Column("fam_home_owner_name")]
        public string FamilyOwnerName { get; set; }

        [Column("fam_home_owner_mobile")]
        public string FamilyMobileNumber { get; set; }

        [Column("fam_door_no")]
        public int FamilyDoorNumber { get; set; }

        [Column("fam_floor_no")]
        public int FamilyFloorNo { get; set; }

        [Column("fam_ref_key")]
        public string FamilyRefrenceKey { get; set; }


    }
}
