using HEMANTH.HOME_EXPENCE.Models.UserMaster;
using HEMANTH.HOME_EXPENCE.Models.UserMaster.AddExpence;
using HEMANTH.HOME_EXPENCE.Models.UserMaster.DownloadLastBill;
using HEMANTH.HOME_EXPENCE.Models.UserMaster.FamilyInfo;
using HEMANTH.HOME_EXPENCE.Models.UserMaster.UserDashBoard;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HEMANTH.HOME_EXPENCE.ServiceInterface.UserMaster
{
    public interface IUserMasterService
    {
        public Task<UserDashBoard> GetDashboardDetails(int UserId);

        public Task<AddExpence> GetExpenceDetails(int UserId);
        public Task<SelectListItem> AddExpenceAsync(AddExpence modelAdd);

        public Task<DownloadLastBill> GetMontlyBillDetails(DateTime fromDate, DateTime toDate,int UserID);

        public Task<FamilyDetails> GetFamilyInformation(int UserId);

        public Task<IEnumerable< Transaction>> GetExpenceReportDetails(int UserId,DateTime fromDate,DateTime toDate);


        public Task<SelectListItem> DeleteExpense(int userId,int ExpMasterId);

        public Task<AddExpence> GetExpenceDetailsEdit(int userId, int ExpMasterId);
    }
}
