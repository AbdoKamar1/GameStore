using DocumentFormat.OpenXml.Office2010.Excel;
using gamestore.dataacess.Repository;
using gamestore.dataacess.Repository.IRepository;
using gamestore.models;
using gamestore.utilty;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;

namespace gamestoreweb.Areas.Customer.Controllers;
[Area("Customer")]

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitofwork;


        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
        _unitofwork = unitOfWork;
        }

    public IActionResult Index()
    {
        IEnumerable<Product> productList = _unitofwork.Product.GetAll();
        return View(productList);
    }
    public IActionResult Details(int Productid)
    {
        ShoppingCart CartObj = new()
        {
            Count = 1,
            ProductId = Productid,
            Product = _unitofwork.Product.GetFirstOrDefult(u => u.Id == Productid)
        };
        return View(CartObj);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public IActionResult Details(ShoppingCart shoppingCart)
    {
        var clamisIdentity = (ClaimsIdentity)User.Identity;
        var clamis = clamisIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = clamis.Value;
        ShoppingCart cartFromDb = _unitofwork.ShoppingCart.GetFirstOrDefult(
            u => u.ApplicationUserId == clamis.Value && u.ProductId == shoppingCart.ProductId);
        if (cartFromDb == null) {

            _unitofwork.ShoppingCart.Add(shoppingCart);
            _unitofwork.Save();

            HttpContext.Session.SetInt32(SD.SessionCart,
                _unitofwork.ShoppingCart.GetAll(u=>u.ApplicationUserId==clamis.Value).ToList().Count);
        }
        else
        {
            _unitofwork.ShoppingCart.IncrementCount(cartFromDb, shoppingCart.Count);
            _unitofwork.Save();

        }






        return RedirectToAction(nameof(Index));
    }

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
