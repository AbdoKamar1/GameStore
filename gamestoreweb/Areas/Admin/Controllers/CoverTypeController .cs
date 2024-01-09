using gamestore.dataacess.Data;
using gamestore.dataacess.Repository.IRepository;
using gamestore.models;
using gamestore.utilty;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace gamestoreweb.Controllers;
        [Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]


public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        public CoverTypeController(IUnitOfWork unitofwork)
        {
         _unitofwork = unitofwork;
        }
        public IActionResult Index()
        {
            IEnumerable<CoverType> objCoverType = _unitofwork.CoverType.GetAll();
            return View(objCoverType);
        }
        //Get
        public IActionResult Create()
        {
            return View();
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
            if (ModelState.IsValid)
            {
                _unitofwork.CoverType.Add(obj);
                _unitofwork.Save();
                TempData["sucess"] = "CoverTypey Created Sucssefully";
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
            var CoverTypeFromDbFirst = _unitofwork.CoverType.GetFirstOrDefult(u => u.Id == id);

            if (CoverTypeFromDbFirst == null)
            {
                return NotFound();
            }
            return View(CoverTypeFromDbFirst);
        }
        
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {
            //if (obj.Name == obj.DisplayOreder.ToString())
            //{
            //    ModelState.AddModelError("name", "The Name Cannot Match The DisplayOrder");
            //}
            if (ModelState.IsValid)
            {
                _unitofwork.CoverType.Update(obj);
                _unitofwork.Save();
                TempData["sucess"] = "CoverType Edited Sucssefully";

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
            var CoverTypeFromDbFirst = _unitofwork.CoverType.GetFirstOrDefult(u => u.Id == id);
            if (CoverTypeFromDbFirst == null)
            {
                return NotFound();
            }
            return View(CoverTypeFromDbFirst);
        }

        //Post
        [HttpPost,ActionName("DeletePost")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            //var obj = _db.Categories.Find(id);
            var obj = _unitofwork.CoverType.GetFirstOrDefult(u => u.Name == "id");


            if (obj == null)
            {
                return NotFound();
            }
            _unitofwork.CoverType.Remove(obj);
            _unitofwork.Save();
            TempData["sucess"] = "CoverType Deleted Sucssefully";

            return RedirectToAction("Index");
            
        }


    }


