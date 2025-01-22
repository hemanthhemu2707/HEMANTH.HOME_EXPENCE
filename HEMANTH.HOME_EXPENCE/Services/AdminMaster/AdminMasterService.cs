using HEMANTH.HOME_EXPENCE.Models.AdminMaster;
using HEMANTH.HOME_EXPENCE.Models.AdminMaster.ApprovalInbox;
using HEMANTH.HOME_EXPENCE.Models.AdminMaster.Category;
using HEMANTH.HOME_EXPENCE.Models.AdminMaster.Family;
using HEMANTH.HOME_EXPENCE.RepoInterfaces.AdminMaster;
using HEMANTH.HOME_EXPENCE.ServiceInterface.AdminMaster;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HEMANTH.HOME_EXPENCE.Services.AdminMaster
{
    public class AdminMasterService : IAdminMasterService
    {

        private readonly IAdminMasterRepo adminMasterRepo;

        public AdminMasterService(IAdminMasterRepo adminMasterRepo)
        {
            this.adminMasterRepo = adminMasterRepo;
        }

        public async Task<DashBoard> GetDashboardCountAdminDash(int UserId)
        {
            var result = await adminMasterRepo.GetDashboardCountAdminDash(UserId);
            if(result.FamilyCount==0)
            {
                result.MessageMarquee = "Welcome " + result.UserName + " Pease Create the Family and Add the Members !";
            }
                return result;
        }


        public async Task<List<FamilyRequest>> GetFamilyDetails(int UserId)
        {
            var result = await adminMasterRepo.GetFamilyDetails(Convert.ToInt32(UserId));
            return result;
        }

        public async Task<List<FamilyMemberDetails>> GetFamilyMembersDetails(int UserId)
        {
            var result = await adminMasterRepo.GetFamilyMembersDetails(Convert.ToInt32(UserId));
            return result;
        }

        public async Task<List<ExpenceCategory>> GetExpenseCategory()
        {
            var result = await adminMasterRepo.GetExpenseCategory();
            return result;
        }


        public async Task<List<UserRequest>> GetApprovalInbox()
        {
            var result = await adminMasterRepo.GetApprovalInbox();
            return result;
        }

        public async Task<List<SelectListItem>> GetFamilyDetailsList()
        {
            var result = await adminMasterRepo.GetFamilyDetailsList();
            return result;
        }

        public async Task<ExpenceCategory> DeleteExpenseCategory(int Category)
        {
            var result = await adminMasterRepo.DeleteExpenseCategory(Category);
            return result;
        }


        public async Task<bool> DeleteUserById(int UserId,int? status)
        {
            bool result = await adminMasterRepo.DeleteUserById(UserId, status);
            return result;
        }


        public async Task<bool> CheckIsEmailExist(string email)
        {
            bool result = await adminMasterRepo.CheckIsEmailExist(email);
            return result;
        }


        public async Task<Approvallboc> ApproveUser(string userId, string email, string familyId)
        {
            var result = await adminMasterRepo.ApproveUser(userId,email,familyId);
            return result;
        }


        public async Task<FamilyRequest> SaveUpdateFamilyDetails(FamilyRequest familyRequest)
        {
            var result = await adminMasterRepo.SaveUpdateFamilyDetails(familyRequest);
            return result;
        }

        public async Task<FamilyMemberDetails> SaveUpdateFamilyMemberDetails(FamilyMemberDetails familyMemberDetails)
        {
            var result = await adminMasterRepo.SaveUpdateFamilyMemberDetails(familyMemberDetails);
            return result;
        }

        public async Task<ExpenceCategory> SaveUpdateCategoryDetails(int categoryId, string categoryName, string categoryDescription)
        {
            var result = await adminMasterRepo.SaveUpdateCategoryDetails(categoryId,categoryName,categoryDescription);
            return result;
        }
    }
}
