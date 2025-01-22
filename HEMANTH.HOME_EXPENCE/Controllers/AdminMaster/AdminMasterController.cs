using HEMANTH.HOME_EXPENCE.Models.AdminMaster;
using HEMANTH.HOME_EXPENCE.Models.AdminMaster.ApprovalInbox;
using HEMANTH.HOME_EXPENCE.Models.AdminMaster.Category;
using HEMANTH.HOME_EXPENCE.Models.AdminMaster.Family;
using HEMANTH.HOME_EXPENCE.Models.Login;
using HEMANTH.HOME_EXPENCE.ServiceInterface.AdminMaster;
using HEMANTH.HOME_EXPENSE.ServiceInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HEMANTH.HOME_EXPENCE.Controllers.AdminMaster
{
    public class AdminMasterController : Controller
    {
        private readonly IAdminMasterService adminMasterService;
        private readonly EmailService _emailService;

        public AdminMasterController(IAdminMasterService adminMasterService, EmailService emailService)
        {
               this.adminMasterService = adminMasterService;
                        _emailService = emailService;

        }


        [HttpGet]
        public async Task<IActionResult> AdminDashBoard(string UserId)
        {
            if (int.TryParse(UserId, out int numericUserId))
            {
                UserId = numericUserId.ToString();
            }
            else if (UserId != null)
            {
                UserId = EncryptionHelper.UrlDecrypt(UserId);
            }

            else
            {
                UserId = HttpContext.Session.GetString("UserId");
            }
            var response = await adminMasterService.GetDashboardCountAdminDash(Convert.ToInt32(UserId));
            HttpContext.Session.SetString("UserName", response.UserName);
            HttpContext.Session.SetString("UserId", response.UserId.ToString());
            HttpContext.Session.SetString("FamilyCount", response.FamilyCount.ToString());
            HttpContext.Session.SetString("FamilyName", response.FamilyName.ToString());


            return View(response);
        }


        [HttpGet]
        public async Task<IActionResult> FamilyDetails()
        {
            string UserId=HttpContext.Session.GetString("UserId");

            var familyRequests = await adminMasterService.GetFamilyDetails(Convert.ToInt32(UserId));

           
            return View(familyRequests);
        }


        [HttpGet]
        public async Task<IActionResult> AddFamilyMember()
        {
            string UserId = HttpContext.Session.GetString("UserId");

            var familyRequests = await adminMasterService.GetFamilyMembersDetails(Convert.ToInt32(UserId));
            FamilyMemberDetails family = new FamilyMemberDetails();
            family.lstUsers = familyRequests.ToList(); 
            return View(family); 
        }


        [HttpPost]
        public async Task<IActionResult> AddFamilyMember(FamilyMemberDetails familyMemberDetails)
        {
            string UserId = HttpContext.Session.GetString("UserId");
            familyMemberDetails.UserId = Convert.ToInt32( UserId);
           var FamD= await adminMasterService.SaveUpdateFamilyMemberDetails(familyMemberDetails);

            return Json(new { success = true });
        }




        public async Task<ActionResult> ApprovalInbox()
        {
            Approvallboc model = new Approvallboc();

            var result = await adminMasterService.GetApprovalInbox();
            model.lstUserRequests = result;
            var familes = await adminMasterService.GetFamilyDetailsList();
            model.Families = familes;
            return View(model);
        }

        [HttpPost]
        public async Task< ActionResult> ApproveUser(int userId, string email, int familyId)
        {
            var result = await adminMasterService.ApproveUser(userId.ToString(), email, familyId.ToString());
            return Json(new { success = true, message = "User approved successfully." });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteExpenseCategory(int categoryId)
        {
            try
            {
                var result = adminMasterService.DeleteExpenseCategory(categoryId);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Failed to delete category. Please try again." });
            }
        }




        [HttpGet]
        public async Task<IActionResult> ExpenseCategory()
        {
            
            ExpenceCategory model = new ExpenceCategory();

            var result = await adminMasterService.GetExpenseCategory();
            model.ExpenseCategories = result;

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> EditExpenseCategory(int categoryId, string categoryName, string categoryDescription)
        {
            var rsult = await adminMasterService.SaveUpdateCategoryDetails(categoryId, categoryName, categoryDescription);
               
            return Json(new { success = true, message = "Category updated successfully." });
         }


        [HttpGet]
        public async Task<ActionResult> AddFamily()
        {
            FamilyRequest familyRequest = new FamilyRequest();
            string UserId = HttpContext.Session.GetString("UserId");

            var familyRequests = await adminMasterService.GetFamilyDetails(Convert.ToInt32(UserId));
            if (familyRequests.Count > 0)
            {
                familyRequest.FamilyMap = familyRequests.FirstOrDefault().FamilyMap;
                familyRequest.FamilyFloorNo = familyRequests.FirstOrDefault().FamilyFloorNo;
                familyRequest.FamilyEntryDate = familyRequests.FirstOrDefault().FamilyEntryDate;
                familyRequest.FamilyDesc = familyRequests.FirstOrDefault().FamilyDesc;
                familyRequest.FamilyDoorNumber = familyRequests.FirstOrDefault().FamilyDoorNumber;
                familyRequest.FamilyEleBillNumber = familyRequests.FirstOrDefault().FamilyEleBillNumber;
                familyRequest.FamilyName = familyRequests.FirstOrDefault().FamilyName;
                familyRequest.HomeName = familyRequests.FirstOrDefault().HomeName;
                familyRequest.FamilyLocation = familyRequests.FirstOrDefault().FamilyLocation;
                familyRequest.FamilyOwnerName = familyRequests.FirstOrDefault().FamilyOwnerName;
                familyRequest.FamilyId = familyRequests.FirstOrDefault().FamilyId;
                familyRequest.HomeName = familyRequests.FirstOrDefault().HomeName;
            }
           return View(familyRequest);
        }

        [HttpPost]
        public IActionResult AddFamily([FromBody] FamilyRequest formData)
        {
            formData.UserId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            var response = adminMasterService.SaveUpdateFamilyDetails(formData);
            return Json(response.Result);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }


        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id, string? action)
        {
            try
            {
                if (action != "DeleteUser")
                {
                    bool isActivated = await adminMasterService.DeleteUserById(id,1);
                    return Json(new { success = true });
                }
                bool isDeleted = await adminMasterService.DeleteUserById(id, 0); 

                if (isDeleted)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to delete user." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> SendEmailVerification(string email,string? type="NULL")
        {
            var otp = new Random().Next(1000, 9999).ToString();

            bool isEmailExist = await adminMasterService.CheckIsEmailExist(email);
            if(isEmailExist)
            {
                return Json(new { success = false, Message = "Email Already Used Please Use Diffrent !"  });

            }
            var emailBody = @"
    <!DOCTYPE html>
    <html>
    <head>
        <style>
            body {
                background-color: green;
                color: white;
                font-family: Arial, sans-serif;
                text-align: center;
                padding: 20px;
            }
            .otp {
                color: red;
                font-size: 24px;
                font-weight: bold;
            }
        </style>
    </head>
    <body>
        <h1>Verification Code</h1>
        <p>Your One-Time Password (OTP) is:</p>
        <div class='otp'>" + otp + @"</div>
        <p>Please use this code to complete your verification process.</p>
    </body>
    </html>";
            try
            {
               bool status= await _emailService.SendEmailAsync(email, "Your OTP", emailBody);
                if (status)
                {
                    return Json(new { success = true, otp = otp });
                }
                return Json(new { success = false });


            }
            catch (Exception ex)
            {
                return Json(new { success = false });

            }
        }


    }

}
