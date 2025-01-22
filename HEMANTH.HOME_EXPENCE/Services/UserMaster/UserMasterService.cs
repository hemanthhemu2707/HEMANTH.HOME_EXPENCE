using HEMANTH.HOME_EXPENCE.Models.UserMaster;
using HEMANTH.HOME_EXPENCE.Models.UserMaster.AddExpence;
using HEMANTH.HOME_EXPENCE.Models.UserMaster.DownloadLastBill;
using HEMANTH.HOME_EXPENCE.Models.UserMaster.FamilyInfo;
using HEMANTH.HOME_EXPENCE.Models.UserMaster.UserDashBoard;
using HEMANTH.HOME_EXPENCE.RepoInterfaces.UserMaster;
using HEMANTH.HOME_EXPENCE.ServiceInterface.UserMaster;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HEMANTH.HOME_EXPENCE.Services.UserMaster
{
    public class UserMasterService : IUserMasterService
    {
        private readonly IUserMasterRepo _userMasterRepo; 

        public UserMasterService(IUserMasterRepo userMasterRepo) 
        {
            _userMasterRepo = userMasterRepo ?? throw new ArgumentNullException(nameof(userMasterRepo));
        }

        public async Task<UserDashBoard> GetDashboardDetails(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid User ID", nameof(userId));
            }

            var response = await _userMasterRepo.GetUserDetails(userId);
            return response;
        }

        public async Task<AddExpence> GetExpenceDetails( int userId)
            {
            AddExpence objAdd = new AddExpence();
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid User ID", nameof(userId));
            }

            var CategoryDropDown = await _userMasterRepo.GetCategoryDetails();
            var UserDetails = await _userMasterRepo.GetUserDetailsForAddExence(userId);
            var ExpenceDetails = await _userMasterRepo.GetExpenceDetails(userId);
            objAdd.Categories = CategoryDropDown.ToList();

            objAdd.Users = UserDetails.ToList();


            objAdd.Expenses = ExpenceDetails.ToList();


            return objAdd;
        }


        public async Task<SelectListItem> AddExpenceAsync(AddExpence model)
        {
            if (model.UserId <= 0)
            {
                throw new ArgumentException("Invalid User ID", nameof(model.UserId));
            }

            var response = await _userMasterRepo.AddExpenceAsync(model);

            return response;
        }



        public async Task<DownloadLastBill> GetMontlyBillDetails(DateTime fromDate, DateTime toDate, int Userid)
        {
            var ExpenceDetails = await _userMasterRepo.GetExpenceDetailsBill(fromDate, toDate, Userid);
            int serial = 1;
            foreach (var res in ExpenceDetails.Users)
            {
                res.SerialNumber = serial++;
            }
            return ExpenceDetails;
        }



        public async Task<FamilyDetails> GetFamilyInformation(int Userid)
        {
            FamilyDetails objFamilyDetails = new FamilyDetails();
            var familyDetails = await _userMasterRepo.GetFamilyInformation(Userid);
            if(familyDetails != null)
            {
                objFamilyDetails.FamilyName = familyDetails.FamilyName.ToString();
                objFamilyDetails.DoorNumber = familyDetails.FamilyDoorNumber.ToString();
                objFamilyDetails.FloorNumber=familyDetails.FamilyFloorNo;
                objFamilyDetails.OwnerName=familyDetails.FamilyOwnerName.ToString();
                objFamilyDetails.ElectricBillNumber=familyDetails.FamilyEleBillNumber.ToString();   
                objFamilyDetails.ContactNumber=familyDetails.FamilyMobileNumber.ToString();
                objFamilyDetails.FamilyId = familyDetails.FamilyID;
                objFamilyDetails.LocationAdress = familyDetails.FamilyLocation;
                var FamilyMembers= await _userMasterRepo.GetUserDetailsUnderFamily(objFamilyDetails.FamilyId);
                foreach (var member in FamilyMembers)
                {
                    objFamilyDetails.Members.Add(new Models.Login.LoginRequest
                    {
                        UserId=member.UserId,
                        UserName=member.UserName,
                        UserEmail= member.UserEmailAdress,
                        UserPhoneNumber=member.UserPhone,
                    });
                }

            }
            else
            {
                return new FamilyDetails();
            }
           
            return objFamilyDetails;
        }



        public async Task<IEnumerable<Transaction>> GetExpenceReportDetails(int userId, DateTime fromDate, DateTime toDate)
        {
            ReportModel objReportModel = new ReportModel();
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid User ID", nameof(userId));
            }
         var ExpenceDetails = await _userMasterRepo.GetExpenceReportDetails(userId,  fromDate,  toDate);
            return ExpenceDetails;
        }


        public async Task<SelectListItem> DeleteExpense(int userId, int ExpMasterId)
        {
            ReportModel objReportModel = new ReportModel();
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid User ID", nameof(userId));
            }
            var ExpenceDetails = await _userMasterRepo.DeleteExpense(userId, ExpMasterId);
            return ExpenceDetails;
        }

        public  async Task<AddExpence> GetExpenceDetailsEdit(int userId, int ExpMasterId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid User ID", nameof(userId));
            }
            var CategoryDropDown = await _userMasterRepo.GetCategoryDetails();
            var UserDetails = await _userMasterRepo.GetUserDetailsForAddExenceEdit(userId, ExpMasterId);
            var ExpenceDetails = await _userMasterRepo.GetExpenceDetails(userId);
            var ExpenceMainDetails = await _userMasterRepo.GetMainExpenceDetails(userId, ExpMasterId);
            ExpenceMainDetails.Expenses= ExpenceDetails.ToList();
            ExpenceMainDetails.Users = UserDetails.ToList();
            ExpenceMainDetails.Categories = CategoryDropDown.ToList();
            return ExpenceMainDetails;
        }

    }
}
