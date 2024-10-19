using ECommerceWebApplication.Services;
using Microsoft.AspNetCore.Mvc;
using ECommerceWebApplication.Models;
using System.Linq;

namespace ECommerceWebApplication.Controllers
{
    public class ProductsController : Controller
    {
        public readonly ApplicationDbContext context;


        public ProductsController(ApplicationDbContext context)
        {
            this.context = context;
           
        }

       

        public IActionResult Index()
        {
            
            var products = context.Products.OrderBy(p=> p.Category).ToList();
            return View(products);

        }


        public IActionResult Create()
        {

            var products = context.Products.OrderBy(p => p.Category).ToList();
            return View(products);

        }


    }
}
