using HEMANTH.HOME_EXPENCE.Models.Login;

namespace HEMANTH.HOME_EXPENCE.RepoInterfaces.Login
{
    public interface ILoginRepo
    {
        public Task<LoginRequest> CheckLogin(LoginRequest request);
        public Task<LoginRequest> RegisterNewUser(LoginRequest request);
        public Task<bool> VerifyEmailExist(string email);
        public Task<bool> ResetPassword(LoginRequest details);

    }
}
