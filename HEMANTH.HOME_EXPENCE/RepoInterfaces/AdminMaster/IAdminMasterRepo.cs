using HEMANTH.HOME_EXPENCE.Models.AdminMaster;
using HEMANTH.HOME_EXPENCE.Models.AdminMaster.ApprovalInbox;
using HEMANTH.HOME_EXPENCE.Models.AdminMaster.Category;
using HEMANTH.HOME_EXPENCE.Models.AdminMaster.Family;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HEMANTH.HOME_EXPENCE.RepoInterfaces.AdminMaster
{
    public interface IAdminMasterRepo
    {
        public Task<DashBoard> GetDashboardCountAdminDash(int UserId);

        public Task<List<FamilyRequest>> GetFamilyDetails(int UserId);
        public Task<List<FamilyMemberDetails>> GetFamilyMembersDetails(int UserId);

        public Task<FamilyRequest> SaveUpdateFamilyDetails(FamilyRequest familyRequest);
        public Task<FamilyMemberDetails> SaveUpdateFamilyMemberDetails(FamilyMemberDetails familyRequest);

        public Task<List<ExpenceCategory>> GetExpenseCategory();
        public Task<List<UserRequest>> GetApprovalInbox();
        public Task<List<SelectListItem>> GetFamilyDetailsList();

        public Task<ExpenceCategory> DeleteExpenseCategory(int Category);
        public Task<bool> DeleteUserById(int UserId, int? status);
        public Task<bool> CheckIsEmailExist(string email);

        public Task<Approvallboc> ApproveUser(string userId, string email, string familyId);

        public Task<ExpenceCategory> SaveUpdateCategoryDetails(int categoryId, string categoryName, string categoryDescription);
    }
}
