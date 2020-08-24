using MvcSiteMapProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using testowo.DAL;
using testowo.Models;

namespace testowo.Infrastructure
{
    public class CategoryDynamicProvider : DynamicNodeProviderBase
    {

        private CourseContext db = new CourseContext();

        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            var returnValue = new List<DynamicNode>();
            foreach (Category c in db.Category)
            {
                DynamicNode nodeD = new DynamicNode();
                nodeD.Title = c.CategoryName;
                nodeD.Key = "Category_" + c.CategoryId;
                nodeD.RouteValues.Add("nameCat", c.CategoryName);
                returnValue.Add(nodeD);
            }

            return returnValue;
        }
    }
}