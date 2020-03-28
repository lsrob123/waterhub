using System.ComponentModel.DataAnnotations;

namespace Blog.Web.Models
{
    public class VasayoForm
    {
        [Required]
        public string Email { get; set; }
        public string MoreInfo { get; set; }
    }
}
