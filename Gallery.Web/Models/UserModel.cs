using WaterHub.Core.Models;

namespace Gallery.Web.Models
{
    public class UserModel : UserModelBase
    {
        public override string MobilePhone { get; set; } = Admin;
        public override bool IsAdmin { get; set; } = true;
    }
}