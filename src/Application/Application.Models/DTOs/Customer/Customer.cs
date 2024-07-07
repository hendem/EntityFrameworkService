using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Models.DTOs.Customer
{
    public record Customer
    {

        [JsonPropertyName("customerId")]
        [StringLength(5, MinimumLength = 1, ErrorMessage = "CustomerId must be between 1 and 5 characters.")]        
        public string CustomerId { get; set; } = null!;

        [JsonPropertyName("companyName")]
        [StringLength(40)]
        public string CompanyName { get; set; } = null!;

        [JsonPropertyName("contactName")]
        [StringLength(30)]
        public string? ContactName { get; set; }

        [JsonPropertyName("contactTitle")]
        [StringLength(30)]
        public string? ContactTitle { get; set; }

        [JsonPropertyName("address")]
        [StringLength(60)]
        public string? Address { get; set; }

        [JsonPropertyName("city")]
        [StringLength(15)]
        public string? City { get; set; }

        [JsonPropertyName("region")]
        [StringLength(15)]
        public string? Region { get; set; }

        [JsonPropertyName("postalCode")]
        [StringLength(10)]
        public string? PostalCode { get; set; }

        [JsonPropertyName("country")]
        [StringLength(15)]
        public string? Country { get; set; }

        [JsonPropertyName("phone")]
        [StringLength(24)]
        public string? Phone { get; set; }

        [JsonPropertyName("fax")]
        [StringLength(24)]
        public string? Fax { get; set; }
        
    }
}
