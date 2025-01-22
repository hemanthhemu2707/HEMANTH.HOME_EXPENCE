using HEMANTH.HOME_EXPENCE.Models.Login;
using HEMANTH.HOME_EXPENCE.RepoInterfaces.Login;
using HEMANTH.HOME_EXPENCE.ServiceInterface;
using HEMANTH.HOME_EXPENSE.ServiceInterface;
using IIITS.EFCore.Repositories;
using IIITS.LMS.Repositories.GeneralTables.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HEMANTH.HOME_EXPENCE.Repositories.Login
{
    public class LoginRepo : ILoginRepo
    {
        private readonly EmailService _emailService;

        private readonly IConfiguration _configuration;

        private readonly LMSMasterServiceDBContext _dbContext;

        public LoginRepo(LMSMasterServiceDBContext dbContext, IConfiguration configuration, EmailService emailService)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _emailService = emailService;
        }



        public async Task<LoginRequest> CheckLogin(LoginRequest request)
        {
            LoginRequest loginRequest = request as LoginRequest;
            try
            {
                if (loginRequest.RoleType == 1)
                {

                    try
                    {
                        var dbTyp1e = await _dbContext.LoginDetails
                                                           .Where(x => (x.UserName == request.UserName || x.UserEmailAdress==request.UserName) && x.UserPassword == request.Password && x.AdminStatus == 1)
                                                           .ToListAsync();
                    }
                    catch (Exception ex)
                    {

                    }

                    var dbType = await _dbContext.LoginDetails
                                        .Where(x => (x.UserName == request.UserName || x.UserEmailAdress == request.UserName) && x.UserPassword == request.Password && x.AdminStatus == 1)
                                        .ToListAsync();
                    if (dbType.Count > 0)
                    {
                        loginRequest.UserId = dbType.FirstOrDefault().UserId;
                        loginRequest.Status = 1;
                        loginRequest.StatusMessage = "Login successful!";
                        loginRequest.RoleType = 1;

                    }
                    else
                    {
                        loginRequest.Status = 0;
                        loginRequest.StatusMessage = "Invalid Admin Crediatials !";
                    }
                }
                else
                {



                    var dbType = await _dbContext.LoginDetails
                        .Where(x => (x.UserName == request.UserName || x.UserEmailAdress == request.UserName) && x.UserPassword == request.Password)
                        .ToListAsync();
                    // Ensure the result is awaited properly

                    // Check if result is not null and contains a valid boolean status
                    if (dbType.Count > 0)
                    {
                        if (dbType.FirstOrDefault().UserActiveStatus == 0 && dbType.FirstOrDefault().AdminStatus==0)
                        {
                            loginRequest.UserId = dbType.FirstOrDefault().UserId;
                            loginRequest.Status = 0;
                            loginRequest.StatusMessage = "Your Currently Inactive Contact Your Family Admin !";
                        }
                        else if (dbType.FirstOrDefault().AdminStatus == 1 && dbType.FirstOrDefault().UserFamilyId==0)
                        {
                            loginRequest.UserId = dbType.FirstOrDefault().UserId;
                            loginRequest.Status = 0;
                            loginRequest.StatusMessage = "Please Login as Admin Create Family then try to Login!";
                        }
                        else
                                {
                            loginRequest.Status = 1;
                            loginRequest.UserId = dbType.FirstOrDefault().UserId;
                            loginRequest.StatusMessage = "Login successful!";
                        }

                    }
                    else
                    {
                        loginRequest.Status = 0;
                        loginRequest.StatusMessage = "Invalid credentials!";
                    }
                }
            }
            catch(Exception ex)
            {
                loginRequest.Status = 0;
                loginRequest.StatusMessage = "Something Went Wrong ! Try again !";
            }
                
            // Return the login request after processing
            return loginRequest;
            

        }



        public async Task<LoginRequest> RegisterNewUser(LoginRequest request)
        {
            LoginRequest loginRequest = request as LoginRequest;
            if (loginRequest == null)
            {
                loginRequest = new LoginRequest();
            }
            

            var dbType = await _dbContext.LoginDetails
       .Where(x => x.UserEmailAdress == request.UserEmail)
       .ToListAsync();
            var UsrrNameValidation = await _dbContext.LoginDetails
      .Where(x => x.UserName == request.UserName && request.AccessKey==null)
      .ToListAsync();

            var AlreadyRegister = await _dbContext.LoginDetails
      .Where(x=> x.UserEmailAdress == request.UserEmail && request.EmilThroghLogin == 1 && x.UserPassword!="NA" && x.UserActiveStatus==1)

      .ToListAsync();

            if (AlreadyRegister != null && AlreadyRegister.Count > 0)
            {
                loginRequest.Status = 0;
                loginRequest.StatusMessage = "Your Aready Registerd Please Login !";
            }
            else if (UsrrNameValidation != null && UsrrNameValidation.Count > 0)
            {
                loginRequest.Status = 0;
                loginRequest.StatusMessage = "User Name already  Used Please Use differnt Username";
            }
            else if (dbType != null && dbType.Count > 0 && request.EmilThroghLogin == 0 && request.AccessKey == null)
            {
                loginRequest.Status = 0;
                loginRequest.StatusMessage = "Email Already Registerd ! Please Login ";
            }

            else if (request.EmilThroghLogin == 0 && (request.AccessKey != null && request.AccessKey != ""))
            {
                var FamilyId = await _dbContext.FamilyTbl.FirstOrDefaultAsync(x => x.FamilyRefrenceKey == request.AccessKey);
                if (FamilyId != null)
                {


                    var newLeaveRecord = new LoginDBTypes

                    {
                        UserName = request.UserName,
                        UserPhone = request.UserPhoneNumber,
                        UserEmailAdress = request.UserEmail,
                        UserPassword = request.Password,
                        AdminStatus = request.isAdmin,
                        UserActiveStatus = 1,
                        UserApproveStatus=1,
                        UserFamilyId = FamilyId.FamilyID

                    };

                    await _dbContext.AddAsync(newLeaveRecord);
                    await _dbContext.SaveChangesAsync();

                    loginRequest.Status = 1;
                    loginRequest.StatusMessage = "Registation  Sucess ! You Joned to Family please Login";
                }
                else
                {
                    loginRequest.Status = 0;
                    loginRequest.StatusMessage = "Check Acess Key Once ! on Email";
                }

            }




            else if(request.EmilThroghLogin>0)
            {
                var newLeaveRecord = new LoginDBTypes
                 {
                    UserName=request.UserName,
                    UserId= dbType.FirstOrDefault().UserId,
                    UserPhone = request.UserPhoneNumber,
                    UserPassword = request.Password,
                    UserActiveStatus=1
                };

                var existingRecord = await _dbContext.LoginDetails
                                      .FirstOrDefaultAsync(l => l.UserId == newLeaveRecord.UserId);

                if (existingRecord != null)
                {
                    existingRecord.UserName = request.UserName;
                    existingRecord.UserPhone = newLeaveRecord.UserPhone;
                    existingRecord.UserPassword = newLeaveRecord.UserPassword;
                    existingRecord.UserActiveStatus = newLeaveRecord.UserActiveStatus;

                    await _dbContext.SaveChangesAsync();
                    loginRequest.Status = 1;
                    loginRequest.StatusMessage = "Registation  Sucess ! You Joned to Family please Login";
                }

            }

            else
            {
                var newLeaveRecord = new LoginDBTypes

                {
                    UserName = request.UserName,
                    UserPhone = request.UserPhoneNumber,
                    UserEmailAdress = request.UserEmail,
                    UserPassword = request.Password,
                    AdminStatus= request.isAdmin,
                    UserActiveStatus=1

                };

                var existingUserByEmail = await _dbContext.LoginDetails
        .Where(x => x.UserEmailAdress == request.UserEmail)
        .FirstOrDefaultAsync();

                var existingUserByUserNameUpperCase = await _dbContext.LoginDetails
        .Where(x => x.UserName.ToUpper() == request.UserName.ToUpper())
        .FirstOrDefaultAsync();


                if (existingUserByEmail != null)
                {
                    loginRequest.Status = 0;
                    loginRequest.StatusMessage = "Email Already Registerd ! Please Login ";

                }
                else if (existingUserByUserNameUpperCase != null)

                {
                    loginRequest.Status = 0;
                    loginRequest.StatusMessage = "User Name Already Registerd ! Please Use Diffrent ";

                }
                else
                {
                    try
                    {
                        await _dbContext.LoginDetails.AddAsync(newLeaveRecord);
                        await _dbContext.SaveChangesAsync();
                        loginRequest.Status = 1;
                        loginRequest.StatusMessage = "Registration Sucessfully  ? Request Sent to ADMIN wait for Approve Check Email !";

                    }
                    catch(Exception ex)
                    {


                    }

                    string userEmailBody = $"<p>Dear {request.UserName},</p><p>Your registration was successful. Please Login.</p>";
                    await _emailService.SendEmailAsync(request.UserEmail, "Registration Successful", userEmailBody);

                    var AdminEmail = await _dbContext.LoginDetails.FirstOrDefaultAsync();
                    string adminEmailBody = $"<p>A new user, {request.UserName}, has registered please Login.</p>";
                    await _emailService.SendEmailAsync(AdminEmail.UserEmailAdress, "New User Registration", adminEmailBody);
                }




            }

            return loginRequest;
        }


        public async Task<bool> VerifyEmailExist(string email)
        {
            var details = await _dbContext.LoginDetails.Where(x => x.UserEmailAdress == email).ToListAsync();
            if(details.Count>0)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> ResetPassword(LoginRequest _details)
        {
            var details = await _dbContext.LoginDetails
                                           .Where(x => x.UserEmailAdress == _details.UserEmail)
                                           .ToListAsync();
            if (details.Count > 0)
            {
                foreach (var detail in details)
                {
                    detail.UserPassword = _details.Password; 
                }
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }


    }
}
