using HEMANTH.HOME_EXPENCE.Models.Login;
using HEMANTH.HOME_EXPENCE.RepoInterfaces.Login;
using HEMANTH.HOME_EXPENCE.ServiceInterface.Login;

namespace HEMANTH.HOME_EXPENCE.Services
{
    public class LoginService : ILoingService
    {
        private readonly ILoginRepo _login;

        public LoginService(ILoginRepo login)
        {
            _login = login;
        }

        public async Task<LoginRequest> CheckLogin(LoginRequest request)
        {
            LoginRequest loginRequest = new LoginRequest();
            loginRequest= await _login.CheckLogin(request);
            return  loginRequest;
        }

        public async Task<LoginRequest> RegisterUser(LoginRequest request)
        {
            LoginRequest loginRequest = new LoginRequest();
            loginRequest = await _login.RegisterNewUser(request);
            return loginRequest;
        }


        public async Task<bool> VerifyEmailExist(string email)
        {
            var response = await _login.VerifyEmailExist(email);
            return response;
        }

        public async Task<bool> ResetPassword(LoginRequest Details)
        {
            var response = await _login.ResetPassword(Details);
            return response;
        }

    }
}
