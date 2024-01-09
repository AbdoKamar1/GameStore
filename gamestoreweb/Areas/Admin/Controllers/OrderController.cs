using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using gamestore.dataacess.Repository.IRepository;
using gamestore.models;
using gamestore.models.ViewModel;
using gamestore.utilty;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace gamestoreweb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM OrderVM { get; set; }
            public OrderController(IUnitOfWork unitOfWork)

        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int orderId)
        {
            OrderVM = new OrderVM()
            {
                orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefult(u => u.Id == orderId, includeProoperties: "ApplicationUser"),
                orderDetails = _unitOfWork.OrderDetails.GetAll(u => u.OrderId == orderId, includeProoperties: "Product"),
        };
            return View(OrderVM);
        }
        #region APICALLS


        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> orderHeaders;
            if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
                orderHeaders = _unitOfWork.OrderHeader.GetAll(includeProoperties: "ApplicationUser");

            } 
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                orderHeaders = _unitOfWork.OrderHeader.GetAll(u=> u .ApplicationUserId==claim.Value,includeProoperties: "ApplicationUser");

            }

            switch (status)
            {
                case "pending":
                    orderHeaders = orderHeaders.Where(u => u.PaymrntStatus == SD.PaymentStatusDelayedPayment);
                    break;
                case "inprocess":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusShipped);
                    break;
                case "approved":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusApproved);
                    break;
                default:
                    break;
    
            }

            return Json(new { Data = orderHeaders });
        }
        #endregion


    }
}
