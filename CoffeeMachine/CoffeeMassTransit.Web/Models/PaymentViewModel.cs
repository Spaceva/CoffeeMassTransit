using System.ComponentModel.DataAnnotations;

namespace CoffeeMassTransit.Web;

public class PaymentViewModel
{
    [Required]
    public string OrderId { get; set; } = default!;
    [Required]
    public string CardNumber { get; set; } = default!;
    [Required]
    public string CC { get; set; } = default!;
}
