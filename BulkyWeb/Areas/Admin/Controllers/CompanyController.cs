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
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork UnitOfWork;
        public CompanyController(IUnitOfWork db)
        {
            this.UnitOfWork = db;
        }

        public IActionResult GetAllCompany()
        {
            List<Company> Company=UnitOfWork.Company.GetAll().ToList();
          
            return View("GetAllCompany", Company);
        }

        [HttpGet]
        public IActionResult Upser(int ? id)
        {

            if(id==null || id == 0)
            {
                return View("Upser",new Company());

            }
            else
            {

                Company company = UnitOfWork.Company.Get(u => u.Id == id);
                return View("Upser", company);

            }
        }

        [HttpPost]
        public IActionResult Upser( Company obj)
        {
           
            if (ModelState.IsValid)
            {
               
                if (obj.Id == null || obj.Id == 0)
                {
                    UnitOfWork.Company.Add(obj);
                }
                else
                {
                    UnitOfWork.Company.update(obj);
                }
                UnitOfWork.Save();
                TempData["success"] = "Company created SuccessFully";
                return RedirectToAction("GetAllCompany");

            }
            else
            {
             
            return View("Upser", obj);
            }

        }


        

        #region API CALLS


        [HttpGet]
        public IActionResult GetALL(int id)
        {
            List<Company> Company = UnitOfWork.Company.GetAll().ToList();

            return Json(new { data = Company });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = UnitOfWork.Company.Get(u => u.Id == id);
            if (CompanyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

           


            UnitOfWork.Company.Remove(CompanyToBeDeleted);
            UnitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }


        #endregion

    }
}
