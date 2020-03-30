using System.Text.Json;
using Blog.Web.Abstractions;

namespace Blog.Web.Models
{
    public class VasayoForm: FormBase
    {
        public string MoreInfo { get; set; }
  
        public static implicit operator VasayoForm(string json)
        {
            return JsonSerializer.Deserialize<VasayoForm>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public static implicit operator string(VasayoForm form)
        {
            return JsonSerializer.Serialize(form);
        }

    }
}
