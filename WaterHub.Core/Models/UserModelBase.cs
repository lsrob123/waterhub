namespace WaterHub.Core.Models
{
    public abstract class UserModelBase
    {
        public virtual string Username { get; set; }
        public virtual string PlainTextPassword { get; set; }
    }
}
