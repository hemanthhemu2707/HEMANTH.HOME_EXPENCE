namespace HEMANTH.HOME_EXPENCE.Models.AdminMaster.Family
{
    public class FamilyRequest
    {
        public int FamilyId { get; set; }
        public string FamilyName { get; set; }
        public string HomeName { get; set; }
        public int DoorNumber { get; set; }
        public DateTime JoinedOn { get; set; }

        public int FamilyFloorNo { get; set; }

        public int FamilyDoorNumber { get; set; }

        public string FamilyMobileNumber { get; set; }

        public string FamilyOwnerName { get; set; }

        public string FamilyMap { get; set; }

        public int FamilyEleBillNumber { get; set; }

        public string FamilyPhoto { get; set; }

        public string FamilyLocation { get; set; }

        public DateTime FamilyEntryDate { get; set; }

        public string FamilyDesc { get; set; }

        public string StatusMessage { get; set; }
        public int Status { get; set; }
        public string FamilyRefrenceKey { get; set; }

        public int UserId { get; set; }
    }
}
