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
    public class FieldController : Controller
    {
		MongoCollection<Field> fieldsCollection = DBHelper.GetInstance().FieldsCollection();

        public ActionResult Index(string Id = null)
        {
			IList<Field> model = null;
			ViewData["SelectedField"] = default(Field);

			if(fieldsCollection != null)
			{
				model = fieldsCollection.FindAll().ToList();

				if(!string.IsNullOrEmpty(Id))
				{
					ViewData["SelectedField"] = GetField(Id);
				}
			}
			else
			{
				model = new List<Field>();
			}

            return View (model);
        }

		public ActionResult Get(string Id)
		{
			return View (GetField(Id));
		}

		private Field GetField(string Id)
		{
			return DBHelper.GetInstance().GetField(Id);
		}

		public ActionResult Save(Field field)
		{
			if(fieldsCollection != null)
			{
				fieldsCollection.Save(field);
			}
			return RedirectToAction("Index");
		}

		public ActionResult Delete(string Id)
		{
			if(!string.IsNullOrEmpty(Id) && fieldsCollection != null)
			{
				var query = Query<Field>.EQ(f => f.Name,Id);
				fieldsCollection.Remove(query);
			}
			return RedirectToAction("Index");
		}
    }
}