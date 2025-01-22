using HEMANTH.HOME_EXPENCE.Models.Login;
using HEMANTH.HOME_EXPENCE.ServiceInterface.Login;
using HEMANTH.HOME_EXPENCE.Services;
using HEMANTH.HOME_EXPENCE.Services.AdminMaster;
using HEMANTH.HOME_EXPENSE.ServiceInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using static System.Net.WebRequestMethods;
namespace HEMANTH.HOME_EXPENCE.Controllers.Login
{
    public class LoginController : Controller
    {
        private readonly EmailService _emailService;

        private readonly ILoingService _loginService;

        public LoginController(ILoingService loginService, EmailService emailService)
        {
            _loginService = loginService;
            _emailService = emailService;

        }

        [HttpGet]
        public IActionResult Login()
        {
            HttpContext.Session.Remove("SessionExpiredMessage");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest _login)
        {
            var res = await _loginService.CheckLogin(_login);
            HttpContext.Session.SetString("UserId", res.UserId.ToString());

            // res.EncryptedUserId = EncryptionHelper.Encrypt(res.UserId.ToString());
            res.EncryptedUserId = EncryptionHelper.UrlEncrypt(res.UserId.ToString());
            return Json(res);
        }





        [HttpGet]
        public IActionResult RegisterUser(string? name, string? email, string? phoneNumber,string? acessk)
        {
            LoginRequest _login = new LoginRequest();
            if (name != null)
            {
                _login.UserEmail = EncryptionHelper.UrlDecrypt( email);
                _login.UserPhoneNumber = EncryptionHelper.UrlDecrypt(phoneNumber);
                _login.UserName = EncryptionHelper.UrlDecrypt(name);
                _login.AccessKey = EncryptionHelper.UrlDecrypt(acessk);
                _login.EmilThroghLogin = 1;
            }
            return View(_login);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] LoginRequest _login)
        {

            var res = await _loginService.RegisterUser(_login);

            return Json(res);
        }



        public IActionResult ForgotPassword()
        {

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GenerateOTP(string email, bool isJoiningFamily)
        {
            var otp = new Random().Next(1000, 9999).ToString();
            var response = await _loginService.VerifyEmailExist(email);
            if (response && !isJoiningFamily)
            {

                return Json(new { success = false, Message = "Email Already Used Please Use Diffrent !" });

            }
            else
            {
    
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

                    bool statusM = await _emailService.SendEmailAsync(email, "Your OTP", emailBody);
                    if (statusM)
                    {
                        return Json(new { success = true, otp = otp });
                    }
       


                return Json(new { success = false, otp = otp });
            }
        }


        public async Task<IActionResult> ForgotGenerateOTP(string email)
        {
            var otp = new Random().Next(1000, 9999).ToString();
            var response = await _loginService.VerifyEmailExist(email);

                if (response)
                {
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

                    bool statusM = await _emailService.SendEmailAsync(email, "Your OTP", emailBody);
                    if (statusM)
                    {
                        return Json(new { success = true, otp = otp });
                    }
                }
                else
                {
                    return Json(new { success = false, otp = "Emial Not Found !" });

                }


                return Json(new { success = false, otp = otp });
            }





        



        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] LoginRequest Details)
            {
            var res = await _loginService.ResetPassword(Details);
            return Json(new { success = res });
        }




    }
}
