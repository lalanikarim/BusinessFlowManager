using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
		MongoCollection<Form> formsCollection = DBHelper.GetInstance().FormsCollection();
		MongoCollection<BsonDocument> tempCollection = DBHelper.GetInstance().TempCollection();

        public ActionResult Index(string Id)
        {
			IList<Form> model = null;
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
			if(!string.IsNullOrEmpty(Id) && formsCollection != null)
			{
				var query = Query<Field>.EQ(f => f.Name,Id);
				formsCollection.Remove(query);
			}
			return RedirectToAction("Index");
		}

		public ActionResult Save(Form formMD)
		{
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
			if(formsCollection != null && !string.IsNullOrEmpty(formId) && !string.IsNullOrEmpty(fieldId))
			{
				Form form = GetForm(formId);
				if(form != null)
				{
					if(DBHelper.GetInstance().GetField(fieldId) != null)
					{
						MongoDBRef dbref = new MongoDBRef(DBHelper.GetInstance().FieldsCollection().Name,fieldId);

						if(new ContentAction[]{ContentAction.Remove,ContentAction.Up,ContentAction.Down}.Contains(contentAction))
						{
							if(form.Contents.Contains(dbref))
							{
								if(contentAction == ContentAction.Remove)
								{
									form.Contents.Remove(dbref);
								}
								else if(contentAction == ContentAction.Up)
								{
									LinkedListNode<MongoDBRef> prev = form.Contents.Find(dbref).Previous;
									if(prev != null)
									{
										form.Contents.Remove(dbref);
										form.Contents.AddBefore(prev,dbref);
									}
								}
								else if(contentAction == ContentAction.Down)
								{
									LinkedListNode<MongoDBRef> next = form.Contents.Find(dbref).Next;
									if(next != null)
									{
										form.Contents.Remove(dbref);
										form.Contents.AddAfter(next,dbref);
									}
								}
							}
						}
						else
						{
							if(!form.Contents.Contains(dbref))
							{
								form.Contents.AddLast(dbref);
							}
						}

						formsCollection.Save(form);
					}
				}
			}
			return RedirectToAction("Index",new{Id = formId});
		}

		public ActionResult Preview(string id)
		{
			Form model = (from f in formsCollection.AsQueryable()
			              where f.Name == id
			              select f).FirstOrDefault();
			return View (model);
		}

		public ActionResult Process(FormCollection formValues)
		{
			BsonDocument doc = new BsonDocument();
			foreach(var key in formValues.Keys)
			{
				doc.Add(key.ToString(),formValues[key.ToString()]);
			}
			tempCollection.Save(doc);
			return RedirectToAction("Index");
		}
    }
}