namespace WaterHub.Core.Models
{
    public class EmailContact
    {
        public string Name { get; set; }
        public string Address { get; set; }

        public EmailContact()
        {

        }

        public EmailContact(string address, string name = null)
        {
            Name = name;
            Address = address;
        }

        public static implicit operator EmailContact(string address)
        {
            return new EmailContact(address);
        }  
    }
}
