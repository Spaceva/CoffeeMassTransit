using System.ComponentModel.DataAnnotations;

namespace CoffeeMassTransit.Web
{
    public class PaymentViewModel
    {
        [Required]
        public string OrderId { get; set; }
        [Required]
        public string CardNumber { get; set; }
        [Required]
        public string CC { get; set; }
    }
}
