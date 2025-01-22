namespace HEMANTH.HOME_EXPENCE.Models.UserMaster.UserDashBoard
{
    public class UserDashBoard
    {
        public int FamilMembersCount {  get; set; }
        public decimal PersonalExpence {  get; set; }
        public decimal FamilyExpence { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string EncryptedUserId {  get; set; }
        public string FamilyName { get; set; }
        public int AdminStatus { get; set; }

        public List<MonthlyExpense> MonthlyExpenses { get; set; }


    }
}
