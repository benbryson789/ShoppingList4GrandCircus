using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingList4.Context;
using ShoppingList4.Models;

namespace ShoppingList4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ShopCart2Context dbContext;

        public List<Products> Products { get; set; }

        public HomeController(ILogger<HomeController> logger, ShopCart2Context dbcontext)
        {
            _logger = logger;
            this.dbContext = dbcontext;
        }

        public IActionResult Index()
        {
            // get all products from dbContext and cast to list
            Products = dbContext.Products.ToList();
            return View(Products);
        }

        public IActionResult Cart()
        {
            var cartId = HttpContext.Session.GetInt32("CartId") ?? 0;
            var items = dbContext.CartItems.Include(c => c.Product).Where(c => c.CartId == cartId).ToList();

            return View(items);
        }

        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            //Check the Product ID is valid
            if(id == 0)
            {
                //Temp Data allows you to store values across requests
                TempData["Message"] = "Invalid Cart ID";
                return RedirectToAction("index");
            }

            var cartId = HttpContext.Session.GetInt32("CartId");

            //Retrieve the Shopping Cart
            Carts cart = dbContext.Carts.Find(cartId ?? 0);

            //If the Cart is null
            if(cart == null)
            {
                //Create the Shopping Cart
                cart = new Carts();
                dbContext.Carts.Add(cart);

                //Save the Cart to the Database
                dbContext.SaveChanges();

                //Save the new Cart ID to the Session
                HttpContext.Session.SetInt32("CartId", cart.CartId);
            }


            //Get the CartItem for this Product from the shopping Cart
            var cartItem = dbContext.CartItems
                                    .Where(c => c.ProductId == id && c.CartId  == cart.CartId)
                                    .FirstOrDefault();
            if(cartItem == null)
            {
                //Then Create Cart Item
                cartItem = new CartItems
                {
                    CartId = cart.CartId,
                    ProductId = id,
                    Quantity = 1
                };
                
                dbContext.CartItems.Add(cartItem);
                TempData["Message"] = "Product was Added to the Cart";
            }
            else
            {
                //Increment the Cart Item Quantity
                cartItem.Quantity = cartItem.Quantity + 1;
                dbContext.Update(cartItem);

                TempData["Message"] = "Product quantity was updated";
            }
            dbContext.SaveChanges();

            //Save the CartItem Count in session so we can retrieve it in the Layout Page
            HttpContext.Session.SetInt32("CartCount", cart.CartItems.Count);

            return RedirectToAction("Index");
        }
        //C:\repos\ShoppingList4\ShoppingList4\wwwroot\
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
