namespace HEMANTH.HOME_EXPENCE.Models.AdminMaster.Family
{
    public class FamilyMemberDetails
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserPhoneNumber { get; set; }
        public string UserEmailAdress { get; set; }
        public int ActiveStatus { get; set; }
        public int FamilyId { get; set; }
        public string FamilyKey { get; set; }

        public int Status { get; set; }
        public string StatusMessage { get; set; }

        public List<FamilyMemberDetails> lstUsers = new List<FamilyMemberDetails>();
    }
}
