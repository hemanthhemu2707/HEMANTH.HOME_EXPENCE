using Microsoft.AspNetCore.Mvc.Rendering;

namespace HEMANTH.HOME_EXPENCE.Models.AdminMaster.ApprovalInbox
{
    public class Approvallboc
    {
        public List<UserRequest> lstUserRequests = new List<UserRequest>();
        public List<SelectListItem> Families=new List<SelectListItem>();

        public string StatusMessage { get; set; }
        public int Status { get; set; }
    }
}
