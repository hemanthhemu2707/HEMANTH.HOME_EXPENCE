namespace HEMANTH.HOME_EXPENCE.Models.UserMaster.DownloadLastBill
{
    public class DownloadLastBill
    {
        public string FamilyName { get; set; }
        public string BillMonth { get; set; }
        public List<UserExpense> Users { get; set; }
        public List<ExprackMap> lstExprackMap { get; set; }



    }

    public class UserExpense
    {
        public int SerialNumber { get; set; }
        public string UserName { get; set; }
        public decimal AmountSpent { get; set; }
        public int UserID { get; set; }

        public decimal AmountSplitted { get; set; }

        public DateTime ExpenceDate { get; set; }
    }


    public class ExprackMap
    {
        public string Msg1 { get; set; }
        public string Msg2 { get; set; }
        public string ExpPaymentDetails { get; set; }
    }
}
