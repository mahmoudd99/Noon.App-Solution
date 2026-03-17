using Noon.Core.Entities.BasketModule;
using System.ComponentModel.DataAnnotations;

namespace Noon.App.Dtos
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; } //Guid
        public List<BasketItemDto> Items { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DelivertMethodId { get; set; }
        public decimal ShippingCost { get; set; }

    }
}
