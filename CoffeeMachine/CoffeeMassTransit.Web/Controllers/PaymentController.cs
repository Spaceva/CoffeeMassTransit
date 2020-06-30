using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CoffeeMassTransit.Web.Controllers
{
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
            ViewBag.ActivePayment = this.paymentService.GetActive(Guid.Parse(id));
            return View(new PaymentViewModel { OrderId = id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(PaymentViewModel model)
        {
            ViewBag.ActivePayment = this.paymentService.GetActive(Guid.Parse(model.OrderId));
            try
            {
                await this.paymentService.Pay(Guid.Parse(model.OrderId), model.CardNumber, model.CC);
                this.ViewBag.IsPaid = true;
            }
            catch (Exception ex)
            {
                this.ViewBag.Error = ex.Message;
                this.ViewBag.IsPaid = false;
            }
            return View(model);
        }
    }
}
