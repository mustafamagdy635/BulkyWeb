using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class CategoriesController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult GetAllCategories()
        {

            List<Category> categories = unitOfWork.Category.GetAll().ToList();
            return View(categories);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View("Add");
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "CatNAme and Display Order cannt same ");
            }
            if (ModelState.IsValid)
            {
                unitOfWork.Category.Add(obj);
                unitOfWork.Save();
                TempData["success"] = "Category created SuccessFully";
                return RedirectToAction("GetAllCategories");

            }
            else
            {
                return View("Add");
            }

        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            if (Id == null || Id == 0) 
            {
                return NotFound();
            }
            Category cat = unitOfWork.Category.Get(u => u.Id == Id);
            if (cat == null)
            {
                return NotFound();
            } 

            return View(cat);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {

            if (ModelState.IsValid)
            {
                unitOfWork.Category.Update(obj);
                unitOfWork.Save();
                TempData["success"] = "Category Updated SuccessFully";
                return RedirectToAction("GetAllCategories");

            }
            else
            {
                return View("Edit");
            }

        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            Category cat = unitOfWork.Category.Get(u => u.Id == Id);
            if (cat == null)
            {
                return NotFound();
            }

            return View(cat);
        }
        [HttpPost]
        public IActionResult DeletePost(int id)
        {
            Category? cat = unitOfWork.Category.Get(u => u.Id == id);

            if (cat == null)
            {
                return NotFound();
            }

            unitOfWork.Category.Remove(cat);
            unitOfWork.Save();
            TempData["success"] = "Category Removed SuccessFully";
            return RedirectToAction("GetAllCategories");


        }


    }
}
