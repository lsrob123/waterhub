using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Blog.Web.Models
{
    public class ContactForm
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
        public string ErrorMessage { get; set; }

        [JsonIgnore]
        public bool HasErrorMessage => !string.IsNullOrWhiteSpace(ErrorMessage);

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
