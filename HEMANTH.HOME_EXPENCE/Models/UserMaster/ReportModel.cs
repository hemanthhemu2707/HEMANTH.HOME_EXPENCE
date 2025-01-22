namespace HEMANTH.HOME_EXPENCE.Models.UserMaster
{
    public class ReportModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<Transaction> Transactions { get; set; }
        public string SearchQuery { get; set; }
    }

    public class Transaction
    {
        
        public string UserName { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
                

        public string FamilyName { get; set; }

        public DateTime Date { get; set; }

        public List<UserSplittedDetails> SplitDetails { get; set; }

    }

    public class UserSplittedDetails
    {
        public decimal MainAmount { get; set; }

        public decimal Amount { get; set; }

        public string UserName { get; set; }
         
        public int UserId { get; set; }
    }



}
