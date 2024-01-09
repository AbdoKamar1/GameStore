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


public class CompanyController : Controller
{
    private readonly IUnitOfWork _unitofwork;
    public CompanyController(IUnitOfWork unitofwork)
    {
        _unitofwork = unitofwork;
    }
    public IActionResult Index()
    {
        return View();
    }
    //Get
    public IActionResult Upsert(int? id)
    {
        Company company = new();


        
        if (id == null || id == 0)
        {
            return View(company);
        }
        else
        {
            company = _unitofwork.Company.GetFirstOrDefult(u=>u.Id==id);
            return View(company);

        }
    }

    //Post
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(Company obj, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            {

                

                if (obj.Id == 0)
                {

                    _unitofwork.Company.Add(obj);
                    TempData["sucess"] = "Company Created Sucssefully";

                }
                else
                {
                    _unitofwork.Company.Update (obj);
                    TempData["sucess"] = "Company Updated Sucssefully";


                }

                _unitofwork.Save();

                return RedirectToAction("Index");
            }


        }
        return View(obj);
 
    }


    //Get

    //Post


    #region APICALLS
    [HttpGet]
    public IActionResult GetAll()
    {
        var companylist = _unitofwork.Company.GetAll();
        return Json(new {Data= companylist });
    }

    [HttpDelete]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int? id)
    {
        var obj = _unitofwork.Company.GetFirstOrDefult(u => u.Id == id);
        if (obj == null)
        {
            return Json(new { sucess = false, massege = "Error While Deleting" });
        }


        _unitofwork.Company.Remove(obj);
        _unitofwork.Save();
        return Json(new { sucess = true, massege = "Deleted Successfully" });

    }

}

#endregion




