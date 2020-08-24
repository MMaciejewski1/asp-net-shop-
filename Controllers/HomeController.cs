using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using testowo.Models;
using testowo.DAL;
using testowo.ViewModels;
using testowo.Infrastructure;
using NLog;

namespace testowo.Controllers
{
    public class HomeController : Controller
    {

        private CourseContext db = new CourseContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();



        public ActionResult Index()
        {
            logger.Info("Strona glowna mordo");

            List<Course> news;
            List<Course> bestSeller;
            List<Category> listCat;
            ICacheProvider Cache = new DefaultCacheProvider();
            if (Cache.IsSet(Consts.NewsCacheKey))
            {
                news = Cache.Get(Consts.NewsCacheKey) as List<Course>;
              
            }

            else
            {
                news = db.Course.Where(a => !a.Hidden).OrderByDescending(a => a.DateAdd).Take(3).ToList();
                Cache.Set(Consts.NewsCacheKey,news,60);
                
            }
            if (Cache.IsSet(Consts.CatCacheKey))
            {

                listCat = Cache.Get(Consts.CatCacheKey) as List<Category>;
            }
            else
            {
                listCat = db.Category.ToList();
                Cache.Set(Consts.CatCacheKey, listCat, 60);
            }



            if (Cache.IsSet(Consts.bestSellerCacheKey))
            {

                bestSeller = Cache.Get(Consts.bestSellerCacheKey) as List<Course>;
            }
            else
            {
                bestSeller = db.Course.Where(a => !a.Hidden || a.Bestseller).OrderBy(a => Guid.NewGuid()).Take(3).ToList();
                Cache.Set(Consts.bestSellerCacheKey, bestSeller, 60);
            }
            
            
            //guid.new guid generuje unikalny identyfikator za kazdym razem, wiec mozna otrzymac losowe bestsellery
           
            var vm = new HomeViewModel()
            {
                Bestsellers = bestSeller,
                Cat = listCat,
                News = news
            };
            return View(vm);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult StaticPages(string name)
        {
            return View(name);
        }



    }
}