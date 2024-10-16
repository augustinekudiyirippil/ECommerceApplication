using ECommerceWebApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWebApplication.Controllers
{
    public class ProductsController : Controller
    {
        public readonly ApplicationDbContext Context;


        public ProductsController(ApplicationDbContext context)
        {
            Context = context;
        }

       

        public IActionResult Index()
        {
            return View();

        }
    }
}
