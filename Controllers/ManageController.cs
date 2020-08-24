using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using testowo.App_Start;
using testowo.DAL;
using testowo.Infrastructure;
using testowo.Models;
using testowo.ViewModels;

namespace testowo.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {

        public ManageController() { db = new CourseContext(); }

        //     private static Logger logger = LogManager.GetCurrentClassLogger();
        private CourseContext db;
        //    private IMailService mailService;

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            Error
        }

        public ManageController(CourseContext context)//, IMailService mailService)
        {
            this.db = context;
            // this.mailService = mailService;
        }

        public ManageController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
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

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        // GET: Manage
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            var name = User.Identity.Name;
            //      logger.Info("Admin główna | " + name);

            if (TempData["ViewData"] != null)
            {
                ViewData = (ViewDataDictionary)TempData["ViewData"];
            }

            if (User.IsInRole("Admin"))
                ViewBag.UserIsAdmin = true;
            else
                ViewBag.UserIsAdmin = false;

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }

                  var model = new ManageCredentialsViewModel
                    {
                       Message = message,
                       UserData = user.UserData
                  };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeProfile([Bind(Prefix = "UserData")]UserData userData)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                user.UserData = userData;
                var result = await UserManager.UpdateAsync(user);

                AddErrors(result);
            }

            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword([Bind(Prefix = "ChangePasswordViewModel")]ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }

            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);

            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }

            var message = ManageMessageId.ChangePasswordSuccess;
            return RedirectToAction("Index", new { Message = message });
        }
        

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("password-error", error);
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        public ActionResult OrderList()
        {
            var name = User.Identity.Name;
            //   logger.Info("Admin zamowienia | " + name);

            bool isAdmin = User.IsInRole("Admin");
            ViewBag.UserIsAdmin = isAdmin;

            IEnumerable<Order> userOrder;

            // Dla administratora zwracamy wszystkie zamowienia
            if (isAdmin)
            {
                userOrder = db.Order.Include("OrderItem").OrderByDescending(o => o.AddDate).ToArray();
            }
            else
            {
                var userId = User.Identity.GetUserId();
                userOrder = db.Order.Where(o => o.UserId == userId).Include("OrderItem").OrderByDescending(o => o.AddDate).ToArray();
            }

            return View(userOrder);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public OrderStatus ChangeOrderStatus(Order order)
        {
            Order zamowienieDoModyfikacji = db.Order.Find(order.OrderID);
            zamowienieDoModyfikacji.OrderStatus = order.OrderStatus;
            db.SaveChanges();

            if (zamowienieDoModyfikacji.OrderStatus == OrderStatus.Realized)
            {
                //     this.mailService.WyslanieZamowienieZrealizowaneEmail(zamowienieDoModyfikacji);
            }

            return order.OrderStatus;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AddCourse(int? kursId, bool? confirm)
        {
            Course kurs;
            if (kursId.HasValue)
            {
                ViewBag.EditMode = true;
                kurs = db.Course.Find(kursId);
            }
            else
            {
                ViewBag.EditMode = false;
                kurs = new Course();
            }

            var result = new EditCourseViewModel
            {
                Cat = db.Category.ToList(),
                Course = kurs
            };
            //  result.confirm = confirm;

            return View(result);
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AddCourse(EditCourseViewModel model, HttpPostedFileBase file)
        {
           
                if (model.Course.CourseId > 0)
                {
                    // modyfikacja kursu
                   db.Entry(model.Course).State = EntityState.Modified;
                   db.SaveChanges();
                   return RedirectToAction("AddCourse", new { potwierdzenie = true });
               }
                else
                {
                // Sprawdzenie, czy użytkownik wybrał plik
                if (file != null && file.ContentLength > 0)
                {
                    if (ModelState.IsValid)
                    {
                        // Generowanie pliku
                        var fileExt = Path.GetExtension(file.FileName);
                        var filename = Guid.NewGuid() + fileExt;

                        var path = Path.Combine(Server.MapPath(AppConfig.IconCourseFolder), filename);
                        file.SaveAs(path);

                           model.Course.NamePicture = filename;
                               model.Course.DateAdd = DateTime.Now;

                               db.Entry(model.Course).State = EntityState.Added;
                        db.SaveChanges();

                        return RedirectToAction("AddCourse", new { potwierdzenie = true });
                    }
                    else
                    {
                        var Category = db.Category.ToList();
                                         model.Cat = Category;
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Nie wskazano pliku");
                    var category = db.Category.ToList();
                                model.Cat = category;
                    return View(model);
                }
            }

        }

        [Authorize(Roles = "Admin")]
            public ActionResult CourseHide(int kursId)
            {
                var kurs = db.Course.Find(kursId);
                kurs.Hidden = true;
                db.SaveChanges();

                return RedirectToAction("AddCourse", new { potwierdzenie = true });
            }

            [Authorize(Roles = "Admin")]
            public ActionResult CourseShow(int kursId)
            {
                var kurs = db.Course.Find(kursId);
                kurs.Hidden = false;
                db.SaveChanges();

                return RedirectToAction("AddCourse", new { potwierdzenie = true });
            }

            [AllowAnonymous]
            public ActionResult WyslaniePotwierdzenieZamowieniaEmail(int zamowienieId, string nazwisko)
            {
                var zamowienie = db.Order.Include("OrderItem").Include("OrderItem.Course")
                                   .SingleOrDefault(o => o.OrderID == zamowienieId && o.LastName == nazwisko);

                if (zamowienie == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            OrderConfirmEmail email = new OrderConfirmEmail();
            email.To = zamowienie.Email;
            email.From = "elo@gmail.com";
            email.NrOrder = zamowienie.OrderID;
            email.Value = zamowienie.OrderValue;
            email.NrOrder = zamowienie.OrderID;
                email.Items = zamowienie.OrderItem;              
                email.Send();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            [AllowAnonymous]
            public ActionResult WyslanieZamowienieZrealizowaneEmail(int zamowienieId, string nazwisko)
            {
                var zamowienie = db.Order.Include("OrderItem").Include("OrderItem.Course")
                                      .SingleOrDefault(o => o.OrderID == zamowienieId && o.LastName == nazwisko);

                if (zamowienie == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

                OrderConfirmEmail email = new OrderConfirmEmail();
                email.To = zamowienie.Email;
                email.From = "elo@gmail.com";
               email.NrOrder = zamowienie.OrderID;
                email.Send();

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
        }
    }