using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using testowo.App_Start;
using testowo.DAL;
using testowo.Infrastructure;
using testowo.Models;
using testowo.ViewModels;
using Postal;
using Hangfire;
using System.Net;

namespace testowo.Controllers
{
    public class CartController : Controller
    {
        private CartManager cartMan;
        private ISessionManager sessionMan { get; set; }
        private CourseContext db;



        // GET: Cart
        public ActionResult Index()
        {
            var cartItems = cartMan.GetCart();
            var totalPrice = cartMan.GetValueCart();
            CartViewModel vm = new CartViewModel()
            {
                OrderItem = cartItems,
                Totalprice = totalPrice

            };
            return View(vm);
        }

        public CartController()
        {

            this.sessionMan = new SessionManager();
            this.db = new CourseContext();
            this.cartMan = new CartManager(sessionMan, db);
        }

        public ActionResult AddToCart(string id)
        {
            cartMan.AddToCart(Int32.Parse(id));


            return RedirectToAction("Index");
        }

        public int GetCountOrderItem()
        {if (cartMan==null)
                this.cartMan = new CartManager(sessionMan, db);
            return cartMan.GetCountPositionCart();
        }



        public ActionResult DeleteFromCart(int kursId)
        {
            int iloscPozycji = cartMan.DeleteFromCart(kursId);
            int iloscPozycjiKoszyka = cartMan.GetCountPositionCart();
            decimal wartoscKoszyka = cartMan.GetValueCart();

            var wynik = new DeleteFromCartViewModel
            {
                IdPositionToDelete = kursId,
                CountPositionToDelete = iloscPozycji,
                TotalCartPrice = wartoscKoszyka,
                CartCountPosition = iloscPozycjiKoszyka,
            };
            return Json(wynik);
        }



        public async Task<ActionResult> Pay()
        {
            var name = User.Identity.Name;
          //  logger.Info("Strona koszyk | zaplac | " + name);

            if (Request.IsAuthenticated)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

                var order = new Order
                {
                    Name = user.UserData.Name,
                    LastName = user.UserData.LastName,
                    Adress = user.UserData.Adress,
                    Town = user.UserData.Town,
                    ZipCode = user.UserData.ZipCode,
                    Email = user.UserData.Email,
                    Phone = user.UserData.Phone
                };
                return View(order);
            }
            else
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Pay", "Koszyk") });
        }


        [HttpPost]
        public async Task<ActionResult> Pay(Order details)
        {
            if (ModelState.IsValid)
            {
                
                var userId = User.Identity.GetUserId();

               
                var newOrder = cartMan.CreateOrder(details, userId);
                
                var user = await UserManager.FindByIdAsync(userId);
                TryUpdateModel(user.UserData);




                   string url = Url.Action("OrderMailConfirm", "Cart", new { OrderItem =newOrder.OrderID,lastName=newOrder.LastName},Request.Url.Scheme);
                  BackgroundJob.Enqueue(() => Call(url));

            //    var zam = db.Order.Include("OrderItem").Include("OrderItem.Course").SingleOrDefault(o=>o.OrderID==newOrder.OrderID);

             //   OrderConfirmEmail mail = new OrderConfirmEmail();
             //   mail.To = newOrder.Email;
             //   mail.From = "elo@ele.pozdro";
             //   mail.Value = newOrder.OrderValue;
             //   mail.Items = zam.OrderItem;
            //    mail.NrOrder = newOrder.OrderID;  
                
            //    mail.Send();
                cartMan.EmptyCart();
                return RedirectToAction("OrderConfirm");
            }
            else
                return View(details);
        }

        public void Call(string url) {
            var req = HttpWebRequest.Create(url);
            req.GetResponseAsync();

        }

        public ActionResult OrderMailConfirm(int orderId, string lastName) {
            var order = db.Order.Include("OrderItem").Include("OrderItem.Course").SingleOrDefault(o => o.OrderID == orderId);



            OrderConfirmEmail mail = new OrderConfirmEmail();
            mail.To = order.Email;
            mail.From = "elo@ele.pozdro";
            mail.Value = order.OrderValue;
            mail.Items = order.OrderItem;
            mail.NrOrder = order.OrderID;
            mail.Send();

            return new HttpStatusCodeResult(HttpStatusCode.OK);

        }



        public ActionResult OrderConfirm()
        {
            var name = User.Identity.Name;
         //   logger.Info("Strona koszyk | potwierdzenie | " + name);
            return View();
        }








        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
    }
}