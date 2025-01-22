using HEMANTH.HOME_EXPENCE.Models.UserMaster;
using HEMANTH.HOME_EXPENCE.Models.UserMaster.AddExpence;
using HEMANTH.HOME_EXPENCE.Models.UserMaster.DownloadLastBill;
using HEMANTH.HOME_EXPENCE.Models.UserMaster.FamilyInfo;
using HEMANTH.HOME_EXPENCE.Models.UserMaster.UserDashBoard;
using HEMANTH.HOME_EXPENCE.Repositories.DBConfig.AdminMaster.Family;
using IIITS.LMS.Repositories.GeneralTables.Login;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HEMANTH.HOME_EXPENCE.RepoInterfaces.UserMaster
{
    public interface IUserMasterRepo
    {
        public Task<UserDashBoard> GetUserDetails(int UserId);

        public Task<IEnumerable<SelectListItem>> GetCategoryDetails();

        public Task<IEnumerable<User>> GetUserDetailsForAddExence(int userId);

        public Task<IEnumerable<Expense>> GetExpenceDetails(int UserId);

        public Task<DownloadLastBill> GetExpenceDetailsBill(DateTime fromDate, DateTime toDate, int Userid);

        public Task<FamilyTableDBTypes> GetFamilyInformation(int UserId);
        public Task<IEnumerable<LoginDBTypes>> GetUserDetailsUnderFamily(int familyId);

        public Task<IEnumerable<Models.UserMaster.Transaction>> GetExpenceReportDetails(int UserId, DateTime fromDate, DateTime toDate);

        public Task<SelectListItem> AddExpenceAsync(AddExpence modle);

        public Task<SelectListItem> DeleteExpense(int userId,int  expMasterID);

        public Task<IEnumerable<User>> GetUserDetailsForAddExenceEdit(int userId, int expMasterID);

        public Task<AddExpence> GetMainExpenceDetails(int userId, int expMasterID);

    }
}
