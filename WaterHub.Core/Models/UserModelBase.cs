using System.Text.Json.Serialization;
using WaterHub.Core.Abstractions;

namespace WaterHub.Core.Models
{
    public class UserModelBase : EntityBase, IUserModelBase
    {
        public const string Admin = "admin";

        public virtual string Username { get; set; }
        public virtual string Email { get; set; }
        public virtual string HashedPassword { get; set; }
        public virtual bool IsAdmin { get; set; }
        public virtual string MobilePhone { get; set; }

        [JsonIgnore]
        public virtual string PlainTextPassword { get; set; }
    }
}