using System.ComponentModel.DataAnnotations;

namespace WebinarMassTransit.Web
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
