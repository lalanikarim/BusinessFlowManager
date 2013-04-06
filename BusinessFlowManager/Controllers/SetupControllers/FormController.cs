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
    public class FormController : Controller
    {
        public ActionResult Index(string Id)
        {
			IList<Form> model = null;
			MongoCollection<Form> formsCollection = DBHelper.GetInstance().FormsCollection();
			if(formsCollection != null)
			{
				model = formsCollection.FindAll().ToList();
				if(!string.IsNullOrEmpty(Id))
				{
					ViewData["SelectedForm"] = GetForm(Id);
				}
			}
			else
			{
				model = new List<Form>();
			}
			return View (model);
        }

		public ActionResult Get(string Id)
		{
			return View (GetForm(Id));
		}

		private Form GetForm(string Id)
		{
			return DBHelper.GetInstance().GetForm(Id);
		}

		public ActionResult Delete(string Id)
		{
			MongoCollection<Form> formsCollection = DBHelper.GetInstance().FormsCollection();
			if(!string.IsNullOrEmpty(Id) && formsCollection != null)
			{
				var query = Query<Field>.EQ(f => f.Name,Id);
				formsCollection.Remove(query);
			}
			return RedirectToAction("Index");
		}

		public ActionResult Save(Form formMD)
		{
			MongoCollection<Form> formsCollection = DBHelper.GetInstance().FormsCollection();
			if(formsCollection != null && formMD != null)
			{
				Form form = GetForm(formMD.Name);
				if(form != null)
				{
					form.Description = formMD.Description;
				}
				else
				{
					form = formMD;
					form.Contents = new LinkedList<MongoDBRef>();
				}
				formsCollection.Save(form);
			}
			return RedirectToAction("Index");
		}
		
		public ActionResult ModifyContent(string formId, string fieldId,ContentAction contentAction)
		{
			MongoCollection<Form> formsCollection = DBHelper.GetInstance().FormsCollection();
			if(formsCollection != null && !string.IsNullOrEmpty(formId) && !string.IsNullOrEmpty(fieldId))
			{
				Form form = GetForm(formId);
				if(form != null)
				{
					if(DBHelper.GetInstance().GetField(fieldId) != null)
					{
						MongoDBRef dbref = new MongoDBRef(DBHelper.GetInstance().FieldsCollection().Name,fieldId);

						if(contentAction == ContentAction.Add)
						{
							if(!form.Contents.Contains(dbref))
							{
								form.Contents.AddLast(dbref);
							}
						}
						else
						{
							if(form.Contents.Contains(dbref))
							{
								form.Contents.Remove(dbref);
							}
						}
						formsCollection.Save(form);

					}
				}
			}
			return RedirectToAction("Index",new{Id = formId});
		}


    }


}
