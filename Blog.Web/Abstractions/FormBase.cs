using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Blog.Web.Abstractions
{
    public abstract class FormBase
    {
        /// <summary>
        /// Message sender's email address
        /// </summary>
        [Required]
        public string Email { get; set; }

        public string ErrorMessage { get; set; }

        [JsonIgnore]
        public bool HasErrorMessage => !string.IsNullOrWhiteSpace(ErrorMessage);

        public bool HadSuccessfulSend { get; set; }
    }
}
