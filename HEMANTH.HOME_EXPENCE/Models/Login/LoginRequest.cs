namespace HEMANTH.HOME_EXPENCE.Models.Login
{
    public class LoginRequest
    {
        public string UserName { get; set; }

        public int UserId { get; set; }
        public string Password { get; set; }
        public string UserEmail {  get; set; }
        public string UserPhoneNumber { get; set; }
        public string EncryptedUserId { get; set; }

        public int isAdmin { get; set; }
        public int ApprovalStatus { get; set; }
        public int RoleType { get; set; }
        public string AccessKey { get; set; }
        public int EmilThroghLogin { get; set; }

        public int Status { get; set; }
        public string StatusMessage { get; set; }

    }
}
