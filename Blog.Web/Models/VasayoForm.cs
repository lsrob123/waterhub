﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Blog.Web.Models
{
    public class VasayoForm
    {
        [Required]
        public string Email { get; set; }
        public string MoreInfo { get; set; }
        public string ErrorMessage { get; set; }

        [JsonIgnore]
        public bool HasErrorMessage => !string.IsNullOrWhiteSpace(ErrorMessage);

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
