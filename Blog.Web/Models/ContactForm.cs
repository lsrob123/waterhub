using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Blog.Web.Abstractions;

namespace Blog.Web.Models
{
    public class ContactForm : FormBase
    {
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }

        public static implicit operator ContactForm(string json)
        {
            return JsonSerializer.Deserialize<ContactForm>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public static implicit operator string(ContactForm form)
        {
            return JsonSerializer.Serialize(form);
        }
    }
}
