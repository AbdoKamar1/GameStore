using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.Excel;
using gamestore.dataacess.Data;
using gamestore.dataacess.Repository.IRepository;
using gamestore.models;
using gamestore.models.ViewModel;
using gamestore.utilty;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace gamestoreweb.Controllers;
[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]


public class ProductController : Controller
{
    private readonly IUnitOfWork _unitofwork;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public ProductController(IUnitOfWork unitofwork, IWebHostEnvironment hostEnvironment)
    {
        _unitofwork = unitofwork;
        _webHostEnvironment = hostEnvironment;
    }
    public IActionResult Index()
    {
        return View();
    }
    //Get
    public IActionResult Upsert(int? id)
    {
        ProductVM productVM = new()
        {
            product = new(),
            CategoryList = _unitofwork.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),
            CoverTypeList = _unitofwork.CoverType.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),


        };
        if (id == null || id == 0)
        {
            //ViewBag.CategoryList = CategoryList;
            //ViewData["CoverTypeList"] = CoverTypeList;
            //Create Product
            return View(productVM);
        }
        else
        {
            productVM.product = _unitofwork.Product.GetFirstOrDefult(u=>u.Id==id);
            return View(productVM);
           //Update Product

        }
    }

    //Post
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(ProductVM obj, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (file != null)
            {
                string FileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"images\Products");
                var Extension = Path.GetExtension(file.FileName);
                if(obj.product.ImageUrl != null)
                {
                    var OldImagePath = Path.Combine(wwwRootPath, obj.product.ImageUrl.TrimStart('\\'));
                    if(System.IO.File.Exists(OldImagePath))
                    {
                        System.IO.File.Delete(OldImagePath);
                    }
                    
                }

                using (var fileStreams = new FileStream(Path.Combine(uploads, FileName + Extension), FileMode.Create))
                {
                    {

                        file.CopyTo(fileStreams);
                    }
                    obj.product.ImageUrl = @"\images\Products\" + FileName + Extension;

                }
                if (obj.product.Id == 0)
                {

                    _unitofwork.Product.Add(obj.product);
                }
                else
                {
                    _unitofwork.Product.Update (obj.product);

                }

                _unitofwork.Save();
                TempData["sucess"] = "Product Created Sucssefully";

                return RedirectToAction("Index");
            }

            return View(obj);

        }
        return View(obj);
 
    }


    //Get

    //Post


    #region APICALLS
    [HttpGet]
    public IActionResult GetAll()
    {
        var productlist = _unitofwork.Product.GetAll();
        return Json(new {Data= productlist });
    }

    [HttpDelete]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int? id, Product product)
    {
        var obj = _unitofwork.Product.GetFirstOrDefult(u => u.Id == id);
        if (obj == null)
        {
            return Json(new { sucess = false, massege = "Error While Deleting" });
        }
        var OldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
        if (System.IO.File.Exists(OldImagePath))
        {
            System.IO.File.Delete(OldImagePath);
        }


        _unitofwork.Product.Remove(obj);
        _unitofwork.Save();
        return Json(new { sucess = true, massege = "Deleted Successfully" });

    }

}

#endregion




