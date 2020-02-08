using WaterHub.Core.Models;

namespace Blog.Web.Models
{
    public class UserModel: UserModelBase
    {
        public const string Admin = "admin";

        public override string Username { get; set; } = Admin;
    }
}
