using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace testowo.Infrastructure
{
    public static class UrlHelpers
    {
        public static string CatIconPath(this UrlHelper helper, string nameIconCat) {
            var iconCatFolder = AppConfig.IconCatFolder;
            var path = Path.Combine(iconCatFolder,nameIconCat);
            return helper.Content(path);
        }

        public static string CourseIconPath(this UrlHelper helper, string nameCourseCat)
        {
            var iconCourseFolder = AppConfig.IconCourseFolder;
            var path = Path.Combine(iconCourseFolder, nameCourseCat);
            return helper.Content(path);
        }
    }
}