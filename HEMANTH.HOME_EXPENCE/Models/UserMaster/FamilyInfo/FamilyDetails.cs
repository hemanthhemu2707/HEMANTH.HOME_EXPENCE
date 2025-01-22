using HEMANTH.HOME_EXPENCE.Models.Login;

namespace HEMANTH.HOME_EXPENCE.Models.UserMaster.FamilyInfo
{
    public class FamilyDetails
    {
        public int FamilyId { get; set; }
        public string LocationAdress { get; set; }

        public string FamilyName { get; set; }
        public int FloorNumber { get; set; }
        public string DoorNumber { get; set; }
        public string ElectricBillNumber { get; set; }
        public string OwnerName { get; set; }
        public string ContactNumber { get; set; }
        public List<LoginRequest> Members { get; set; } = new List<LoginRequest>();
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
