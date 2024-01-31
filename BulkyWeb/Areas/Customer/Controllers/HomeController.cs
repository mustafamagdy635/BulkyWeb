using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
 

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork UnitOfWork;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            UnitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if(claim != null)
            {
                HttpContext.Session.SetInt32(SD.SessionCart,
                               UnitOfWork.ShoppingCart.GetAll(u => u.ApplicationUSerID == claim.Value).Count());
            }
                IEnumerable<Product> ProductList = UnitOfWork.Product.GetAll(includeProperties: "category");

            return View(ProductList);
        }
        public IActionResult Details(int productId)
        {
            ShoppingCart Shopping = new ShoppingCart
            {
                Product = UnitOfWork.Product.Get(u => u.Id == productId, includeProperties: "category"),
                Count = 1,
                ProductId = productId

            };


            return View("Details", Shopping);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shop)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;

            var USerId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shop.ApplicationUSerID = USerId;
            ShoppingCart cartFromDb = UnitOfWork.ShoppingCart.Get(u => u.ApplicationUSerID == USerId && u.ProductId == shop.ProductId);
            if(cartFromDb !=null)
            {
                cartFromDb.Count += shop.Count;

                UnitOfWork.ShoppingCart.Update(cartFromDb);

                UnitOfWork.Save();

            }
            else
            {

                UnitOfWork.ShoppingCart.Add(shop);
                UnitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart,
                UnitOfWork.ShoppingCart.GetAll(u => u.ApplicationUSerID == USerId).Count());
            }
            TempData["success"] = "Cart updated successfully";
    


            return RedirectToAction(nameof(Index));
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
