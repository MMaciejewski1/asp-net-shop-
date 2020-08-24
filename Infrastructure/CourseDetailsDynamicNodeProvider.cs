using MvcSiteMapProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using testowo.DAL;
using testowo.Models;
namespace testowo.Infrastructure
{
    public class CourseDetailsDynamicNodeProvider : DynamicNodeProviderBase
    {

        private CourseContext db = new CourseContext();
 
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            var returnValue = new List<DynamicNode>();
            foreach (Course c in db.Course)
            {
                DynamicNode nodeD = new DynamicNode();
                nodeD.Title = c.TitleCourse;
                nodeD.Key = "Course_" + c.CourseId;
                nodeD.ParentKey = "Category_" + c.CategoryId;
                nodeD.RouteValues.Add("id", c.CourseId);
                returnValue.Add(nodeD);
            }

            return returnValue;
        }
    }
}