using HEMANTH.HOME_EXPENCE.Models.AdminMaster;
using HEMANTH.HOME_EXPENCE.Models.AdminMaster.ApprovalInbox;
using HEMANTH.HOME_EXPENCE.Models.AdminMaster.Category;
using HEMANTH.HOME_EXPENCE.Models.AdminMaster.Family;
using HEMANTH.HOME_EXPENCE.Models.Login;
using HEMANTH.HOME_EXPENCE.Models.UserMaster.AddExpence;
using HEMANTH.HOME_EXPENCE.RepoInterfaces.AdminMaster;
using HEMANTH.HOME_EXPENCE.Repositories.DBConfig.AdminMaster.Category;
using HEMANTH.HOME_EXPENCE.Repositories.DBConfig.AdminMaster.Family;
using HEMANTH.HOME_EXPENCE.Repositories.DBConfig.AdminMaster.Family;
using HEMANTH.HOME_EXPENSE.ServiceInterface;
using IIITS.EFCore.Repositories;
using IIITS.LMS.Repositories.GeneralTables.Login;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Asn1.Ocsp;

namespace HEMANTH.HOME_EXPENCE.Repositories.AdminMaster
{
    public class AdminMasterRepo : IAdminMasterRepo
    {
        private readonly LMSMasterServiceDBContext _dbContext;
        private readonly EmailService _emailService;

        private readonly IConfiguration _configuration;


        public AdminMasterRepo(LMSMasterServiceDBContext dbContext, IConfiguration configuration, EmailService emailService)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<DashBoard> GetDashboardCountAdminDash(int UserId)
        {
            DashBoard dashBoard=new DashBoard();
            dashBoard.ApprovalCount = 0;

            var isFamExist = await _dbContext.LoginDetails.Where(x => x.UserFamilyId == 0 && x.AdminStatus == 1).ToListAsync();

            var result = from login in _dbContext.LoginDetails
                         join family in _dbContext.FamilyTbl
                             on login.UserFamilyId equals family.FamilyID
                         where login.UserId == UserId
                         select new
                         {
                             Login = login,
                             Family = family
                         };

            if (isFamExist.Count > 0)
            {
                dashBoard.FamilyName = "Not Found !";

            }
            else
            {
                dashBoard.FamilyName = result.FirstOrDefault().Family.FamilyName.ToString();

            }

            var familyCount = await result.CountAsync();
            var userName = await _dbContext.LoginDetails
                                    .Where(x => x.UserId == UserId)
                                    .Select(x => x.UserName)
                                    .FirstOrDefaultAsync();


            var DataesultDash = await _dbContext.CategoryTable.ToListAsync();
            //var CountFamily = await _dbContext.FamilyTbl.ToListAsync();
            var ApprovaCount = await _dbContext.LoginDetails.Where(x=>x.UserApproveStatus==0).ToListAsync();
            dashBoard.ApprovalCount = ApprovaCount.Count();
            dashBoard.FamilyCount = familyCount;
            dashBoard.CategoryCount = DataesultDash.Count();
            dashBoard.UserName= userName;
            dashBoard.UserId = UserId;
            return dashBoard;
        }



        public async Task<List<FamilyRequest>> GetFamilyDetails(int UserId)
        {
            List<FamilyRequest> famList = new List<FamilyRequest>();
            var UserIdDe = await _dbContext.LoginDetails.FirstOrDefaultAsync(x => x.UserId == UserId && x.UserFamilyId > 0);
            if (UserIdDe != null)
            {
                var result = await _dbContext.FamilyTbl.Where(x => x.FamilyID == UserIdDe.UserFamilyId).ToListAsync();
                foreach (var family in result)
                {
                    var familyRequest = new FamilyRequest
                    {
                        FamilyId = family.FamilyID,    // Assuming these properties exist in both models
                        FamilyName = family.FamilyName,
                        DoorNumber = family.FamilyDoorNumber,
                        FamilyDoorNumber = family.FamilyDoorNumber,
                        FamilyDesc = family.FamilyDesc,
                        FamilyEleBillNumber = family.FamilyEleBillNumber,
                        FamilyEntryDate = family.FamilyEntryDate,
                        FamilyFloorNo = family.FamilyFloorNo,
                        FamilyLocation = family.FamilyLocation,
                        FamilyMobileNumber = family.FamilyMobileNumber,
                        FamilyOwnerName = family.FamilyOwnerName,
                        FamilyRefrenceKey = family.FamilyRefrenceKey,
                        HomeName = family.HomeName


                    };
                    famList.Add(familyRequest);
                }
            }
            return famList;


        }


        public async Task<List<FamilyMemberDetails>> GetFamilyMembersDetails(int UserId)
        {
            List<FamilyMemberDetails> famList = new List<FamilyMemberDetails>();
            var userDetails = await _dbContext.LoginDetails.FirstOrDefaultAsync(x => x.UserId == UserId );
            if (userDetails != null)
            {
                try
                {
                    var FamilyDetails = await _dbContext.FamilyTbl
                                    .ToListAsync();

                    if (FamilyDetails == null)
                    {
                    }


                    string FamilyKey = FamilyDetails.FirstOrDefault().FamilyRefrenceKey ?? "DefaultKey"; // Use null-coalescing operator
                    int FamilyId = FamilyDetails.FirstOrDefault().FamilyID; // Use null-coalescing operator



                    var familyMembers = await _dbContext.LoginDetails
                        .Where(x => x.UserFamilyId == userDetails.UserFamilyId).Select(x => new
                        {
                            x.UserId,
                            x.UserEmailAdress,
                            x.UserName,
                            x.UserApproveStatus,
                            Password = x.UserPassword ?? string.Empty,
                            UserPhone = x.UserPhone ?? string.Empty,
                            ActiveStay=x.UserActiveStatus
                        })
                        .ToListAsync();


                    foreach (var family in familyMembers)
                    {
                        var familyRequest = new FamilyMemberDetails
                        {
                            UserId = family.UserId,
                            UserEmailAdress = family.UserEmailAdress,
                            UserPhoneNumber = family.UserPhone,
                            UserName = family.UserName,
                            FamilyKey = FamilyKey,
                            FamilyId= FamilyId,
                            ActiveStatus=family.ActiveStay
                        };

                        famList.Add(familyRequest);
                    }
                }

                catch (Exception ex)
                {

                }


            }
            return famList;

        }


        public async Task<List<ExpenceCategory>> GetExpenseCategory()
        {
            List<ExpenceCategory> famList = new List<ExpenceCategory>();

            var result = await _dbContext.CategoryTable.ToListAsync();
            foreach (var category in result)
            {
                var expenceCategory = new ExpenceCategory
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CatagoryName,
                    Description = category.CatergoryDescription


                };
                famList.Add(expenceCategory);
            }
            return famList;
        }


        public async Task<List<UserRequest>> GetApprovalInbox()
        {
            List<UserRequest> approvalList = new List<UserRequest>(); 
            var result = await _dbContext.LoginDetails.Where(z=>z.UserApproveStatus==0).ToListAsync();
            foreach (var user in result)
            {
                var approvalListUser = new UserRequest
                {
                    UserName = user.UserName,
                    UserEmail = user.UserEmailAdress,
                    PhoneNumber = user.UserPhone,
                    UserId=user.UserId
                };

                approvalList.Add(approvalListUser);
            }

            return approvalList; 
        }


        public async Task<List<SelectListItem>> GetFamilyDetailsList()
        {
            List<SelectListItem> famList = new List<SelectListItem>(); 
            var result = await _dbContext.FamilyTbl.ToListAsync();
            foreach (var user in result)
            {
                var famListItem = new SelectListItem
                {
                    Text = user.FamilyName, 
                    Value = user.FamilyID.ToString()
                };
                famList.Add(famListItem);
            }

            return famList; 
        }


        public async Task<ExpenceCategory> DeleteExpenseCategory(int CatId)
        {
            ExpenceCategory expence = new ExpenceCategory();
            try
            {
                var category = await _dbContext.CategoryTable.FindAsync(CatId);
                if (category != null)
                {
                    _dbContext.CategoryTable.Remove(category);
                    await _dbContext.SaveChangesAsync();
                    expence.Status = 1;
                    expence.StatusMessage = "Deleted !";
                    return expence;
                }
                else
                {
                    expence.Status = 0;
                    expence.StatusMessage = "Something went wrong !";
                    return expence;
                }
            }
            catch(Exception ex)
            {
                expence.Status = 0;
                expence.StatusMessage = "Something went wrong !";
                return expence;
            }
        }


        public async Task<Approvallboc> ApproveUser(string userId, string email, string familyId)
        {
            Approvallboc obj = new Approvallboc();

            try
            {
                var user = await _dbContext.LoginDetails.FirstOrDefaultAsync(u => u.UserId.ToString() == userId && u.UserApproveStatus > 0);

                if (user != null)
                {
                    user.UserApproveStatus = 1; 
                    user.UserFamilyId = Convert.ToInt32(familyId); 

                    _dbContext.LoginDetails .Update(user); 
                    await _dbContext.SaveChangesAsync();

                    obj.Status = 1; 
                    obj.StatusMessage = "User approved successfully.";
                }
                else
                {
                    obj.Status = 0; 
                    obj.StatusMessage = "User not found.";
                }
            }
            catch (Exception ex)
            {
                obj.Status = 0; 
                obj.StatusMessage = "Failed to approve user. Please try again.";
            }

            return obj;
        }






        public async Task<FamilyRequest> SaveUpdateFamilyDetails(FamilyRequest familyRequest)
        {
            familyRequest.FamilyMap = familyRequest.FamilyMap == null ? "NA" : familyRequest.FamilyMap;

            familyRequest.FamilyRefrenceKey = Path.GetRandomFileName().Replace(".", "").Substring(0, 5).ToUpper();
            var newFamilyRecord = new FamilyTableDBTypes
            {
                FamilyPhoto = "NA",
                FamilyName = familyRequest.FamilyName,
                FamilyDesc = familyRequest.FamilyDesc,
                FamilyDoorNumber = familyRequest.FamilyDoorNumber,
                FamilyEleBillNumber = familyRequest.FamilyEleBillNumber,
                FamilyEntryDate = DateTime.SpecifyKind(familyRequest.FamilyEntryDate, DateTimeKind.Utc),
                FamilyFloorNo = familyRequest.FamilyFloorNo,
                FamilyLocation = familyRequest.FamilyLocation,
                FamilyMap = familyRequest.FamilyMap,
                FamilyMobileNumber = familyRequest.FamilyMobileNumber,
                FamilyOwnerName = familyRequest.FamilyOwnerName,
                HomeName = familyRequest.HomeName,
                FamilyRefrenceKey = familyRequest.FamilyRefrenceKey,
                
                
            };

            try
            {
                var existingFamily = await _dbContext.FamilyTbl
                    .FirstOrDefaultAsync(f => f.FamilyID == familyRequest.FamilyId);

                if (existingFamily != null)
                {
                    existingFamily.FamilyName = familyRequest.FamilyName;
                    existingFamily.FamilyDesc = familyRequest.FamilyDesc;
                    existingFamily.FamilyDoorNumber = familyRequest.FamilyDoorNumber;
                    existingFamily.FamilyEleBillNumber = familyRequest.FamilyEleBillNumber;
                    existingFamily.FamilyEntryDate = DateTime.SpecifyKind(familyRequest.FamilyEntryDate, DateTimeKind.Utc);
                    existingFamily.FamilyFloorNo = familyRequest.FamilyFloorNo;
                    existingFamily.FamilyLocation = familyRequest.FamilyLocation;
                    existingFamily.FamilyMap = familyRequest.FamilyMap;
                    existingFamily.FamilyMobileNumber = familyRequest.FamilyMobileNumber;
                    existingFamily.FamilyOwnerName = familyRequest.FamilyOwnerName;
                    existingFamily.HomeName = familyRequest.HomeName;
                    existingFamily.FamilyRefrenceKey = familyRequest.FamilyRefrenceKey;

                    _dbContext.FamilyTbl.Update(existingFamily);
                }
                else
                {
                    _dbContext.FamilyTbl.Add(newFamilyRecord);
                }

                await _dbContext.SaveChangesAsync();

                int newFamilyId = newFamilyRecord.FamilyID;
                var userDetails = await _dbContext.LoginDetails.FirstOrDefaultAsync(ld => ld.UserId == familyRequest.UserId && ld.UserFamilyId==0);
                if (userDetails != null)
                {
                    userDetails.UserFamilyId = newFamilyId;
                    _dbContext.LoginDetails.Update(userDetails);
                    await _dbContext.SaveChangesAsync();
                }
                //var userDetails = await _dbContext.LoginDetails
                //    .FirstOrDefaultAsync(ld => ld.UserId == familyRequest.UserId);
                //if (userDetails != null)
                //{
                //    userDetails.UserFamilyId = newFamilyId;
                //    _dbContext.LoginDetails.Update(userDetails);
                //    await _dbContext.SaveChangesAsync();
                //}

                familyRequest.Status = 1;
                familyRequest.StatusMessage = "Family Details Saved Successfully";
            }
            catch (Exception ex)
            {
                familyRequest.Status = 0;
                familyRequest.StatusMessage = "Failed to save family details. Please try again.";
            }

            return familyRequest;
        }




        public async Task<FamilyMemberDetails> SaveUpdateFamilyMemberDetails(FamilyMemberDetails familyRequest)
        {
            
            try
            {
                var familyId = await _dbContext.LoginDetails.Where(x => x.UserId == familyRequest.UserId).Select(x => x.UserFamilyId).FirstOrDefaultAsync();
                familyRequest.FamilyId = familyId;
            }
            catch (Exception ex)
            {

            }

            var newFamilyRecord = new LoginDBTypes
            {
                UserName = familyRequest.UserName,
                UserFamilyId= familyRequest.FamilyId,
                UserEmailAdress=familyRequest.UserEmailAdress,
                
                UserApproveStatus=1,
                UserPassword="NA",
                UserPhone="NA"
            };
            var familyDetilas = await _dbContext.FamilyTbl.FirstOrDefaultAsync(x => x.FamilyID == familyRequest.FamilyId);
            var FamilyKey = familyDetilas.FamilyRefrenceKey;
            var FamilyName = familyDetilas.FamilyName;

            try
            {
                _dbContext.LoginDetails.Add(newFamilyRecord);
                _dbContext.SaveChanges();


                var RedirectUrlInfo = _configuration.GetSection("RedirectUrlInfo");
                var BaseUrl = RedirectUrlInfo["URl"];

                // Constructing the registration URL with the necessary data.
                var registerUrl = $"{BaseUrl}?name={Uri.EscapeDataString( EncryptionHelper.UrlEncrypt(  familyRequest.UserName))}&" +
                                  $"email={Uri.EscapeDataString(EncryptionHelper.UrlEncrypt(familyRequest.UserEmailAdress))}&" +
                                  $"phoneNumber={Uri.EscapeDataString(EncryptionHelper.UrlEncrypt(familyRequest.UserPhoneNumber))}&"+
                $"acessk={Uri.EscapeDataString(EncryptionHelper.UrlEncrypt(FamilyKey))}";

                string userEmailBody = $"<p>Dear {familyRequest.UserName},</p>" +
                                       $"<p>Your Family Member Added You for <b>''ನಮ್ಮ ಮನೆ ಕರ್ಚು''</b> Please join by using the link below:</p>" +
                                      $"<b>Your Family Name is.{FamilyName}</b>"+
                                        $"<p><a href='{registerUrl}'>Click here to register</a></p>" +
                                       $"<b>Your Family Access Key for Join is.{FamilyKey}</b>";    

                await _emailService.SendEmailAsync(familyRequest.UserEmailAdress, "Registration Successful", userEmailBody);

                familyRequest.StatusMessage = "User Details Saved Successfully and Sent to the User Mail!";

            }
            catch (Exception ex)
            {
                familyRequest.Status = 0;
                familyRequest.StatusMessage = "Failed to save User details. Please try again.";
            }

            return familyRequest;
        }





        public async Task<ExpenceCategory> SaveUpdateCategoryDetails(int categoryId, string categoryName, string categoryDescription)
        {
            ExpenceCategory expenceCategory = new ExpenceCategory();

        
            try
            {
                if (categoryId != 0)
                {
                    var newCategor = new CategoryTableDBTypes
                    {
                        CategoryId = categoryId,
                        CatagoryName = categoryName,
                        CatergoryDescription = categoryDescription
                    };
                    _dbContext.CategoryTable.Update(newCategor);
                    await _dbContext.SaveChangesAsync();


                }
                else
                {
                    var newCategory = new CategoryTableDBTypes
                    {
                        CatagoryName = categoryName,
                        CatergoryDescription = categoryDescription
                    };

                    _dbContext.CategoryTable.Add(newCategory);

                    await _dbContext.SaveChangesAsync();
                }
                expenceCategory.Status = 1;
                expenceCategory.StatusMessage = "Category Details Saved Successfully";
            }
            catch (Exception ex)
            {
                expenceCategory.Status = 0;
                expenceCategory.StatusMessage = "Failed to save Category details. Please try again.";
            }

            return expenceCategory;
        }


        public async Task<bool> DeleteUserById(int UserID, int? status)
        {
            var user = await _dbContext.LoginDetails.FirstOrDefaultAsync(x => x.UserId == UserID);
            if (user != null)
            {
                user.UserActiveStatus = status==1?1:0;
                _dbContext.LoginDetails.Update(user);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> CheckIsEmailExist(string email)
        {
            var isExist = await _dbContext.LoginDetails.FirstOrDefaultAsync(x => x.UserEmailAdress == email);
            if (isExist != null)
            {
                return true;
            }   
            return false;
        }


    }
}
