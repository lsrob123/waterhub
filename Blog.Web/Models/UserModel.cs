using WaterHub.Core.Models;

namespace Blog.Web.Models
{
    public class UserModel: UserModelBase
    {
        public override string MobilePhone { get; set; } = Admin;
    }
}
