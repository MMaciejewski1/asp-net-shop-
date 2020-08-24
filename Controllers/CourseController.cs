using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using testowo.DAL;

namespace testowo.Controllers
{
    public class CourseController : Controller
    {


        private CourseContext db = new CourseContext();
        // GET: Course
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(string nameCat, string searchQuery = null)
        {
            var cat = db.Category.Include("Courses").Where(k => k.CategoryName.ToUpper() == nameCat.ToUpper()).Single();


            // var course = cat.Courses.ToList();
            var course = cat.Courses.Where(a=>(searchQuery==null||a.TitleCourse.ToLower().Contains(searchQuery.ToLower())|| a.AutorCourse.ToLower().Contains(searchQuery.ToLower()))&&!a.Hidden);
            if (Request.IsAjaxRequest()) {
                return PartialView("_CourseList", course);

            }
            return View(course);
        }
        public ActionResult Details(string id)
        {
            var c = db.Course.Find(Int32.Parse(id));
            return View(c);
        }


        [ChildActionOnly]
        [OutputCache(Duration = 60000)]
        public ActionResult CatMeni(string id)
        {    
            var cat = db.Category.ToList();
            //HttpContext.Cache.Add(cat);
            return PartialView("_CatMeni",cat);
        }


        public ActionResult CourseHint(string term){
            var course = db.Course.Where(a=>!a.Hidden&&a.TitleCourse.ToLower().Contains(term.ToLower())).Take(5).Select(a=> new { label = a.TitleCourse});

            return Json(course,JsonRequestBehavior.AllowGet);
        }

    }
}
