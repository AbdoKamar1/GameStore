using DocumentFormat.OpenXml.Office2010.Excel;
using gamestore.dataacess.Repository.IRepository;
using gamestore.models;
using gamestore.models.ViewModel;
using gamestore.utilty;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using System.Security.Claims;

namespace gamestoreweb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private IEnumerable<ShoppingCart> ListCart;

        public ShoppingCartVM ShoppingCartVM { get; set; }
        public int OrderTotal { get; set; }
        public CartController(IUnitOfWork unitOfWork,IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }
        public IActionResult Index()
        {
            var clamisIdentity = (ClaimsIdentity)User.Identity;
            var clamis = clamisIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == clamis.Value,
                includeProoperties: "Product"),
                OrderHeader =new()

            };
            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price5, cart.Product.Price10);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            return View(ShoppingCartVM);
        }
        public IActionResult Summary()
        {
            var clamisIdentity = (ClaimsIdentity)User.Identity;
            var clamis = clamisIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == clamis.Value,
                includeProoperties: "Product"),
                OrderHeader =new()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefult(u => u.Id == clamis.Value);

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.FirstName;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;
            





            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price5, cart.Product.Price10);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            return View(ShoppingCartVM);
        }
        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPost(ShoppingCartVM ShoppingCartVM)
        {
            var clamisIdentity = (ClaimsIdentity)User.Identity;
            var clamis = clamisIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM.ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == clamis.Value,
                includeProoperties: "Product");

            ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = clamis.Value;
    

            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price5, cart.Product.Price10);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefult(u => u.Id == clamis.Value);

            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                ShoppingCartVM.OrderHeader.PaymrntStatus = SD.PaymentStatusPending;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            }
            else
            {
                ShoppingCartVM.OrderHeader.PaymrntStatus = SD.PaymentStatusDelayedPayment;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.PaymentStatusApproved;

            }

            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();
            foreach (var cart in ShoppingCartVM.ListCart)
            {
                OrderDetails orderDetails = new()
                {
                    ProductId = cart.ProductId,
                    OrderId = ShoppingCartVM.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count,

                };
                _unitOfWork.OrderDetails.Add(orderDetails);
                _unitOfWork.Save();
            }
            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                // stripe setting
                var domain = "https://localhost:44369/";
                var options = new SessionCreateOptions
                {
                    LineItems = new List<SessionLineItemOptions>(),


                    Mode = "payment",
                    SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                    CancelUrl = domain + $"customer/cart/index",
                };
                foreach (var item in ShoppingCartVM.ListCart)
                {
                    var sessionitem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Title,
                            },
                        },
                        Quantity = item.Count,
                    };
                    options.LineItems.Add(sessionitem);


                }
                var service = new SessionService();
                Session session = service.Create(options);
                _unitOfWork.OrderHeader.UpdateStripePaymentID(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }
            else
            {
                return RedirectToAction("OrderConfirmation", "Cart", new { Id = ShoppingCartVM.OrderHeader.Id });
            }
                //_unitOfWork.ShoppingCart.RemoveRage(ShoppingCartVM.ListCart);
                //_unitOfWork.Save();
                //return RedirectToAction("Index","Home");
            }
            public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefult(u=>u.Id == id,includeProoperties:"ApplicationUser");
            if(orderHeader.PaymrntStatus != SD.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
                _emailSender.SendEmailAsync(orderHeader.ApplicationUser.Email, "new order- gamestore", "<p> new order created</p>");

            }

            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetAll(u=>u.ApplicationUserId==
            orderHeader.ApplicationUserId).ToList();
            HttpContext.Session.Clear();
            _unitOfWork.ShoppingCart.RemoveRage(shoppingCarts);
            _unitOfWork.Save();
            return View(id);


        }

        public IActionResult Remove(int cartId)
        {
        var cart = _unitOfWork.ShoppingCart.GetFirstOrDefult(u=>u.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(cart);
            _unitOfWork.Save();
            var count = _unitOfWork.ShoppingCart.GetAll(u=>u.ApplicationUserId==cart.ApplicationUserId).ToList().Count - 1;
            //HttpContext.Session.SetInt32(SD.SessionCart,count);
            return RedirectToAction("Index");
        }


        private double GetPriceBasedOnQuantity(double quantity,double price, double price5, double price10) {
            if (quantity <= 5)
            {
                return price;
                    }
            else {
                if (quantity <= 10) { return price5; }
            }
            { return price10; } 
        }
    }
}
