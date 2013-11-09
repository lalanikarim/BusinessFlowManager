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
    public class TaskController : Controller
    {
		MongoCollection<Task> tasksCollection = DBHelper.GetInstance().TasksCollection();

        public ActionResult Index(string Id)
        {
			IList<Task> model = null;
			if(tasksCollection != null)
			{
				model = tasksCollection.FindAll().ToList();
				if(!string.IsNullOrEmpty(Id))
				{
					ViewData["SelectedTask"] = GetTask(Id);
				}
			}
			return View (model);
        }

		public ActionResult Get(string Id)
		{
			return View (GetTask(Id));
		}

		private Task GetTask(string Id)
		{
			return DBHelper.GetInstance().GetTask(Id);
		}

		public ActionResult Delete(string Id)
		{
			if(!string.IsNullOrEmpty(Id) && tasksCollection != null)
			{
				var query = Query<Task>.EQ(t => t.Name,Id);
				tasksCollection.Remove(query);
			}
			return RedirectToAction("Index");
		}

		public ActionResult Save(Task taskMD)
		{
			if(tasksCollection != null && taskMD != null)
			{
				Task task = GetTask(taskMD.Name);
				if(task != null)
				{
					task.Description = taskMD.Description;
				}
				else
				{
					task = taskMD;
					task.Contents = new LinkedList<MongoDBRef>();
				}
				tasksCollection.Save(task);
			}
			return RedirectToAction("Index");
		}
		
		public ActionResult ModifyContent(string taskId, string formId,ContentAction contentAction)
		{
			if(tasksCollection != null && !string.IsNullOrEmpty(taskId) && !string.IsNullOrEmpty(formId))
			{
				Task task = GetTask(taskId);
				if(task != null)
				{
					if(DBHelper.GetInstance().GetForm(formId) != null)
					{
						MongoDBRef dbref = new MongoDBRef(DBHelper.GetInstance().FormsCollection().Name,formId);

						if(new ContentAction[]{ContentAction.Remove,ContentAction.Up,ContentAction.Down}.Contains(contentAction))
						{
							if(task.Contents.Contains(dbref))
							{
								if(contentAction == ContentAction.Remove)
								{
									task.Contents.Remove(dbref);
								}
								else if(contentAction == ContentAction.Up)
								{
									LinkedListNode<MongoDBRef> prev = task.Contents.Find(dbref).Previous;
									if(prev != null)
									{
										task.Contents.Remove(dbref);
										task.Contents.AddBefore(prev,dbref);
									}
								}
								else if(contentAction == ContentAction.Down)
								{
									LinkedListNode<MongoDBRef> next = task.Contents.Find(dbref).Next;
									if(next != null)
									{
										task.Contents.Remove(dbref);
										task.Contents.AddAfter(next,dbref);
									}
								}
							}
						}
						else
						{
							if(!task.Contents.Contains(dbref))
							{
								task.Contents.AddLast(dbref);
							}
						}

						tasksCollection.Save(task);
					}
				}
			}
			return RedirectToAction("Index",new{Id = taskId});
		}
    }
}
