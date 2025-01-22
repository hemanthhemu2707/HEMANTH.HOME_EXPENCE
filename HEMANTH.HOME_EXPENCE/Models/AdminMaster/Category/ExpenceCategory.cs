namespace HEMANTH.HOME_EXPENCE.Models.AdminMaster.Category
{
    public class ExpenceCategory
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }

        public int Status { get; set; }

        public string StatusMessage { get; set; }

        public List<ExpenceCategory> ExpenseCategories = new List<ExpenceCategory>();
    }
}
