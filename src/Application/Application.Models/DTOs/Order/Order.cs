using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Application.Models.DTOs.Customer;

namespace Application.Models.DTOs.Order
{
    public record Order
    {

        [JsonPropertyName("orderId")]
        public int OrderId { get; set; }

        [JsonPropertyName("customerId")]
        public string? CustomerId { get; set; }

        [JsonPropertyName("employeeId")]
        public int? EmployeeId { get; set; }

        [JsonPropertyName("orderDate")]
        public DateTime? OrderDate { get; set; }

        [JsonPropertyName("requiredDate")]
        public DateTime? RequiredDate { get; set; }

        [JsonPropertyName("shippedDate")]
        public DateTime? ShippedDate { get; set; }

        [JsonPropertyName("shipVia")]
        public int? ShipVia { get; set; }

        [JsonPropertyName("freight")]
        public decimal? Freight { get; set; }

        [JsonPropertyName("shipName")]
        public string? ShipName { get; set; }

        [JsonPropertyName("shipAddress")]
        public string? ShipAddress { get; set; }

        [JsonPropertyName("shipCity")]
        public string? ShipCity { get; set; }

        [JsonPropertyName("shipRegion")]
        public string? ShipRegion { get; set; }

        [JsonPropertyName("shipPostalCode")]
        public string? ShipPostalCode { get; set; }

        [JsonPropertyName("shipCountry")]
        public string? ShipCountry { get; set; }

        [JsonPropertyName("customer")]
        public Application.Models.DTOs.Customer.Customer? Customer { get; set; }

        [JsonPropertyName("orderDetails")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        [JsonPropertyName("shipViaNavigation")]
        public virtual Shipper? ShipViaNavigation { get; set; }
    }
}
