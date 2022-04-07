using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CoffeeMassTransit.Web.Controllers;

public class PaymentController : Controller
{
    private readonly PaymentService paymentService;

    public PaymentController(PaymentService paymentService)
    {
        this.paymentService = paymentService;
    }

    [HttpGet]
    public IActionResult Index(string id)
    {
        ViewBag.ActivePayment = paymentService.GetActive(Guid.Parse(id));
        return View(new PaymentViewModel { OrderId = id});
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(PaymentViewModel model)
    {
        ViewBag.ActivePayment = paymentService.GetActive(Guid.Parse(model.OrderId));
        try
        {
            await paymentService.Pay(Guid.Parse(model.OrderId), model.CardNumber, model.CC);
            ViewBag.IsPaid = true;
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            ViewBag.IsPaid = false;
        }
        return View(model);
    }
}
