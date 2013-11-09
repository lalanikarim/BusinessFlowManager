using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using miraKLS.BusinessFlow;
using miraKLS.BusinessFlow.Metadata;

namespace BusinessFlowManager.Controllers
{
    public class GroupController : Controller
    {
		MongoCollection<Group> groupsCollection = DBHelper.GetInstance().GroupsCollection();

        public ActionResult Index(string Id = null)
        {
			IList<Group> model = null;
			ViewData["SelectedGroup"] = null;

			if(groupsCollection != null)
			{
				model = groupsCollection.FindAll().ToList();

				if(!string.IsNullOrEmpty(Id))
				{
					ViewData["SelectedGroup"] = GetGroup(Id);
				}
			}

            return View (model);
        }

		public ActionResult Get(string Id)
		{
			return View (GetGroup(Id));
		}

		private Group GetGroup(string Id)
		{
			return DBHelper.GetInstance().GetGroup(Id);
		}

		public ActionResult Save(Group group)
		{
			if(groupsCollection != null)
			{
				groupsCollection.Save(group);
			}
			return RedirectToAction("Index");
		}

		public ActionResult Delete(string Id)
		{
			if(!string.IsNullOrEmpty(Id) && groupsCollection != null)
			{
				var query = Query<Group>.EQ(g => g.Name,Id);
				groupsCollection.Remove(query);
			}
			return RedirectToAction("Index");
		}
    }
}