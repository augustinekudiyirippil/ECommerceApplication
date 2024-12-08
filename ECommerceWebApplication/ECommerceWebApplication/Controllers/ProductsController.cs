using ECommerceWebApplication.Services;
using Microsoft.AspNetCore.Mvc;
using ECommerceWebApplication.Models;
using System.Linq;

namespace ECommerceWebApplication.Controllers
{
    public class ProductsController : Controller
    {
        public readonly ApplicationDbContext context;
        public readonly IWebHostEnvironment environment;


        public ProductsController(ApplicationDbContext context , IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment= environment;



        }

       

        public IActionResult Index()
        {
            
            var products = context.Products.OrderByDescending(p=> p.Id).ToList();
            return View(products);

        }


        public IActionResult Create()
        {

           // var products = context.Products.OrderBy(p => p.Category).ToList();
            return View();

        }

        [HttpPost]
        public IActionResult Create(ProductDto productDto)
        {
            if(productDto.ImageFile==null)
            {
                ModelState.AddModelError("ImageFIle","Product image missing");

            }

            if(!ModelState.IsValid)
            {
                return View(productDto);

            }


            //Save product image
            string newFileName= DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(productDto.ImageFile!.FileName);
            string imageURLPath= environment.WebRootPath +"/products/"+ newFileName;

            using (var stream= System.IO.File.Create(imageURLPath))
            {
                productDto.ImageFile.CopyTo(stream);
            }


            // Save the sata to Product

            Product product = new Product()
            {

                Name = productDto.Name,
                Brand = productDto.Brand,
                Category = productDto.Category,
                Price = productDto.Price,
                Description = productDto.Description,
                ImageFileName = newFileName,
                CreatedAt = DateTime.Now

            };

            context.Products.Add(product);
            context.SaveChanges();


                return RedirectToAction("Index", "Products");

        }



        public IActionResult Edit(int id)
        {

            var product = context.Products.Find(id);

            if(product== null)
            {
                return RedirectToAction("Index", "Products");
            }

            var productDto = new ProductDto()
            {

                Name = product.Name,
                Brand = product.Brand,
                Category = product.Category,
                Price = product.Price,
                Description = product.Description 
               
             
            };

            ViewData["ProductID"] = product.Id;
            ViewData["ImageFileName"] = product.ImageFileName;
            ViewData["CreatedAt"] = product.CreatedAt;



            return View(productDto);

        }


        [HttpPost]
        public IActionResult Edit(int id, ProductDto productDto)
        {
            var product = context.Products.Find(id);



            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }


            if (!ModelState.IsValid)
            {
                ViewData["ProductID"] = product.Id;
                ViewData["ImageFileName"] = product.ImageFileName;
                ViewData["CreatedAt"] = product.CreatedAt;

                return View(productDto);

            }

            //Update image
            string newFileName = product.ImageFileName;

          if(productDto.ImageFile != null)
            { 
            
                    newFileName += Path.GetExtension(productDto.ImageFile!.FileName);
                    string imageURLPath = environment.WebRootPath + "/products/" + newFileName;

                    using (var stream = System.IO.File.Create(imageURLPath))
                    {
                        productDto.ImageFile.CopyTo(stream);
                    }

                    //delete the old file
                    string oldImageFilePath = environment.WebRootPath + "/products/" + product.ImageFileName;
                    System.IO.File.Delete(oldImageFilePath);
            }

            product.Name = productDto.Name;
            product.Brand = productDto.Brand;
            product.Category= productDto.Category;
            product.Description = productDto.Description;
            product.Price= productDto.Price;
            product.ImageFileName = newFileName;

            context.SaveChanges();


            return RedirectToAction("Index", "Products");
        }




    }
}
