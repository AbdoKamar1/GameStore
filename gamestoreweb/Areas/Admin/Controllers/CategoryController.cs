using gamestore.dataacess.Data;
using gamestore.dataacess.Repository.IRepository;
using gamestore.models;
using gamestore.utilty;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace gamestoreweb.Controllers;
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        public CategoryController(IUnitOfWork unitofwork)
        {
         _unitofwork = unitofwork;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objCategory = _unitofwork.Category.GetAll();
            return View(objCategory);
        }
        //Get
        public IActionResult Create()
        {
            return View();
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if(obj.Name==obj.DisplayOreder.ToString())
            {
                ModelState.AddModelError("name", "The Name Cannot Match The DisplayOrder");
            }
            if (ModelState.IsValid)
            {
                _unitofwork.Category.Add(obj);
                _unitofwork.Save();
                TempData["sucess"] = "Category Created Sucssefully";
                return RedirectToAction("Index");
            } 
            return View(obj);
        }
        //Get
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var categortyFromDb = _db.Categories.Find(id);
            var categortyFromDbFirst = _unitofwork.Category.GetFirstOrDefult(u => u.Id == id);

            if (categortyFromDbFirst == null)
            {
                return NotFound();
            }
            return View(categortyFromDbFirst);
        }
        
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOreder.ToString())
            {
                ModelState.AddModelError("name", "The Name Cannot Match The DisplayOrder");
            }
            if (ModelState.IsValid)
            {
                _unitofwork.Category.Update(obj);
                _unitofwork.Save();
                TempData["sucess"] = "Category Edited Sucssefully";

                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //Get
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var categortyFromDb = _db.Categories.Find(id);
            var categortyFromDbFirst = _unitofwork.Category.GetFirstOrDefult(u => u.Id == id);
            if (categortyFromDbFirst == null)
            {
                return NotFound();
            }
            return View(categortyFromDbFirst);
        }

        //Post
        [HttpPost,ActionName("DeletePost")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            //var obj = _db.Categories.Find(id);
            var obj = _unitofwork.Category.GetFirstOrDefult(u => u.Name == "id");


            if (obj == null)
            {
                return NotFound();
            }
            _unitofwork.Category.Remove(obj);
            _unitofwork.Save();
            TempData["sucess"] = "Category Deleted Sucssefully";

            return RedirectToAction("Index");
            
        }


    }



