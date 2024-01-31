using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
   [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;
        public ProductController(IUnitOfWork db,IWebHostEnvironment webHostEnvironments)
        {
            this.UnitOfWork = db;
            this.webHostEnvironment = webHostEnvironments;
        }

        public IActionResult GetAllProduct()
        {
            List<Product> Product=UnitOfWork.Product.GetAll(includeProperties:"category").ToList();
          
            return View("GetAllProduct", Product);
        }

        [HttpGet]
        public IActionResult Upser(int ? id)
        {

            ProductVM productVM = new()
            {
                CategoryList = UnitOfWork.Category.GetAll().Select(
              u => new SelectListItem
              {
                  Text = u.Name,
                  Value = u.Id.ToString()
              }),
                Product = new Product()
            };
            if(id==null || id == 0)
            {
                return View("Upser", productVM);

            }
            else
            {

                productVM.Product = UnitOfWork.Product.Get(u => u.Id == id);
                return View("Upser", productVM);

            }
        }

        [HttpPost]
        public IActionResult Upser(ProductVM productVM,IFormFile? file)
        {
           
            if (ModelState.IsValid)
            {
                string WWWRootPath = webHostEnvironment.WebRootPath;
                if(file !=null)
                {   
                    string fileName=Guid.NewGuid().ToString()+Path.GetExtension(file.FileName);
                    string ProductPath = Path.Combine(WWWRootPath, @"Images\Product");
                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        var oldImagePath =
                            Path.Combine(WWWRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                                System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var filestream =new FileStream(Path.Combine(ProductPath, fileName),FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    productVM.Product.ImageUrl = @"\Images\Product\" + fileName;
                }
                if (productVM.Product.Id == null || productVM.Product.Id == 0)
                {
                    UnitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    UnitOfWork.Product.Update(productVM.Product);
                }
                UnitOfWork.Save();
                TempData["success"] = "Product created SuccessFully";
                return RedirectToAction("GetAllProduct");

            }
            else
            {
                productVM.CategoryList = UnitOfWork.Category.GetAll().Select(
              u => new SelectListItem
              {
                  Text = u.Name,
                  Value = u.Id.ToString()
              });
            return View("Upser", productVM);
            }

        }


        

        #region API CALLS


        [HttpGet]
        public IActionResult GetALL(int id)
        {
            List<Product> Product = UnitOfWork.Product.GetAll(includeProperties: "category").ToList();

            return Json(new { data = Product });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = UnitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            string productPath = @"images\products\product-" + id;
            string finalPath = Path.Combine(webHostEnvironment.WebRootPath, productPath);

            if (Directory.Exists(finalPath))
            {
                string[] filePaths = Directory.GetFiles(finalPath);
                foreach (string filePath in filePaths)
                {
                    System.IO.File.Delete(filePath);
                }

                Directory.Delete(finalPath);
            }


            UnitOfWork.Product.Remove(productToBeDeleted);
            UnitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }


        #endregion

    }
}
