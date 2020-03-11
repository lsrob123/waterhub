namespace WaterHub.Core.Models
{
    public class EmailContact
    {
        public string Name { get; set; }
        public string Address { get; set; }

        public EmailContact()
        {

        }

        public EmailContact(string name, string address = null)
        {
            Name = name;
            Address = address;
        }
    }
}
