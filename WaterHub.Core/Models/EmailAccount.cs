namespace WaterHub.Core.Models
{
    public class EmailAccount
    {
        public string Name { get; set; }
        public string Address { get; set; }

        public EmailAccount()
        {
        }

        public EmailAccount(string address, string name = null)
        {
            Name = name;
            Address = address;
        }

        public static implicit operator EmailAccount(string address)
        {
            return new EmailAccount(address);
        }

        public bool HasAccountName => !string.IsNullOrWhiteSpace(Name);
    }
}
