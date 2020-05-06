using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace WebinarMassTransit.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly PaymentService paymentService;
        private readonly CoffeeService coffeeService;

        public HomeController(PaymentService paymentService, CoffeeService coffeeService)
        {
            this.paymentService = paymentService;
            this.coffeeService = coffeeService;
        }

        public IActionResult Index()
        {
            var payments = this.paymentService.GetAll();
            var coffee = this.coffeeService.GetAll().ToDictionary(c => c.Id);
            ViewBag.Coffees = coffee;
            ViewBag.Payments = payments;
            ViewBag.Orders = coffee.Keys.ToArray().Union(payments.Keys.ToArray()).ToArray();
            return View();
        }
    }
}
