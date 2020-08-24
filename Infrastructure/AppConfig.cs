using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace testowo.Infrastructure
{
    public class AppConfig
    {
        public static string IconCatFolder { get; } = ConfigurationManager.AppSettings["IconsCatFolder"];
        public static string IconCourseFolder { get; } = ConfigurationManager.AppSettings["IconsCourseFolder"];
    }
}