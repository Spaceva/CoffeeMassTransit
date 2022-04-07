using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using CoffeeMassTransit.Contracts;

namespace CoffeeMassTransit.Web;

public class OrderController : Controller
{
    private readonly OrderService orderService;
    private readonly ILogger<OrderController> logger;

    public OrderController(OrderService orderService, ILogger<OrderController> logger)
    {
        this.orderService = orderService;
        this.logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new OrderViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(OrderViewModel model)
    {
        if (ModelState.IsValid)
        {
            var toppings = new List<Topping>();
            if (model.WithCaramel)
            {
                toppings.Add(Topping.Caramel);
            }

            if (model.WithChocolate)
            {
                toppings.Add(Topping.Chocolate);
            }

            if (model.WithWhippedCream)
            {
                toppings.Add(Topping.WhippedCream);
            }

            if (model.WithWhiskey)
            {
                toppings.Add(Topping.Whiskey);
            }

            ViewBag.OrderSent = orderService.Submit(new Coffee { Type = model.CoffeeType, Toppings = toppings }, model.Name);
        }
        return View(model);
    }

}
