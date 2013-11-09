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
    public class FlowController : Controller
    {
		MongoCollection<Flow> flowsCollection = DBHelper.GetInstance().FlowsCollection();
		MongoCollection<FlowState> flowStatesCollection = DBHelper.GetInstance().FlowStatesCollection();

		public ActionResult Index(string Id)
        {
			IList<Flow> model = null;
			ViewData["SelectedFlow"] = null;
			if(flowsCollection != null)
			{
				model = flowsCollection.FindAll().ToList();
				if(!string.IsNullOrEmpty(Id))
				{
					ViewData["SelectedFlow"] = GetFlow(Id);
				}
			}
			return View (model);
        }

		public ActionResult Get(string Id)
		{
			return View (GetFlow(Id));
		}

		private Flow GetFlow(string Id)
		{
			return DBHelper.GetInstance().GetFlow(Id);
		}

		private	FlowState GetFlowState(string Id)
		{
			return DBHelper.GetInstance().GetFlowState(Id);
		}

		public ActionResult Delete(string Id)
		{
			if(!string.IsNullOrEmpty(Id) && flowsCollection != null)
			{
				var query = Query<Flow>.EQ(f => f.Name,Id);
				flowsCollection.Remove(query);
			}
			return RedirectToAction("Index");
		}

		public ActionResult Save(Flow flowMD)
		{
			if(flowsCollection != null && flowMD != null)
			{
				Flow flow = GetFlow(flowMD.Name);
				if(flow != null)
				{
					flow.Description = flowMD.Description;
				}
				else
				{
					flow = flowMD;
					flow.Contents = new LinkedList<FlowContentRef>();
				}
				flowsCollection.Save(flow);
			}
			return RedirectToAction("Index");
		}
		
		public ActionResult ModifyContent(string flowId, string groupId, string taskId, ContentAction contentAction)
		{
			if(flowsCollection != null && !string.IsNullOrEmpty(flowId) && !string.IsNullOrEmpty(groupId) && !string.IsNullOrEmpty(taskId))
			{
				Flow flow = GetFlow(flowId);
				if(flow != null)
				{
					Group group = DBHelper.GetInstance().GetGroup(groupId);
					Task task = DBHelper.GetInstance().GetTask(taskId);
					if(group != null && task != null)
					{
						MongoDBRef groupdbref = new MongoDBRef(DBHelper.GetInstance().GroupsCollection().Name,group.Name);
						MongoDBRef taskdbref = new MongoDBRef(DBHelper.GetInstance().TasksCollection().Name,task.Name);
						FlowContentRef dbref = new FlowContentRef{Group = groupdbref, Task = taskdbref};
						if(new ContentAction[]{ContentAction.Remove,ContentAction.Up,ContentAction.Down}.Contains(contentAction))
						{
							bool found = false;
							foreach(FlowContentRef fcref in flow.Contents)
							{
								if(fcref.Equals(dbref))
								{
									dbref = fcref;
									found = true;
									break;
								}
							}

							if(found)
							{
								if(contentAction == ContentAction.Remove)
								{
									flow.Contents.Remove(dbref);
								}
								else if(contentAction == ContentAction.Up)
								{
									LinkedListNode<FlowContentRef> prev = flow.Contents.Find(dbref).Previous;
									if(prev != null)
									{
										flow.Contents.Remove(dbref);
										flow.Contents.AddBefore(prev,dbref);
									}
								}
								else if(contentAction == ContentAction.Down)
								{
									LinkedListNode<FlowContentRef> next = flow.Contents.Find(dbref).Next;
									if(next != null)
									{
										flow.Contents.Remove(dbref);
										flow.Contents.AddAfter(next,dbref);
									}
								}
							}
						}
						else
						{
							if(!flow.Contents.Contains(dbref))
							{
								flow.Contents.AddLast(dbref);
							}
						}

						flowsCollection.Save(flow);
					}
				}
			}
			return RedirectToAction("Index",new{Id = flowId});
		}

		public ActionResult SaveState(string flowId, string stateId, string taskId)
		{
			SaveFlowState (flowId, stateId, taskId);
			return RedirectToAction("Index");
		}

		public ActionResult SaveNextState(string flowId, string stateId, string route, string nextStateId, string nextTaskId)
		{
			if(flowsCollection != null && flowStatesCollection != null
			   && !string.IsNullOrEmpty(flowId) && !string.IsNullOrEmpty(stateId)
			   && !string.IsNullOrEmpty(route) && !string.IsNullOrEmpty(nextStateId)
			   && !string.IsNullOrEmpty(nextTaskId))
			{
				Flow flow = GetFlow(flowId);
				FlowState state = GetFlowState(stateId);
				FlowState nextState = SaveFlowState(flowId,nextStateId,nextTaskId);
				if(state.NextStates == null)
				{
					state.NextStates = new Dictionary<string,MongoDBRef>();
				}
				if(nextState != null)
				{
					MongoDBRef stateRef = new MongoDBRef(flowStatesCollection.Name,nextState.State);
					if(!state.NextStates.Keys.Contains(route))
					{
						state.NextStates.Add(route, stateRef);
					}
					else
					{
						state.NextStates[route] = stateRef;
					}
					flowStatesCollection.Save(state);
				}
			}
			return RedirectToAction("Index");
		}

		private FlowState SaveFlowState (string flowId, string stateId, string taskId)
		{
			FlowState state = null;
			if (flowsCollection != null && flowStatesCollection != null && !string.IsNullOrEmpty (flowId) && !string.IsNullOrEmpty (taskId)) 
			{
				Flow flow = GetFlow (flowId);
				Task task = DBHelper.GetInstance ().GetTask (taskId);
				if (flow.States != null && (
					from fs in flow.States
					select fs.State).Contains (stateId)) 
				{
					state = GetFlowState (stateId);
					if (task != null && state.Task.Id.AsString != taskId) 
					{
						state.Task = new MongoDBRef (DBHelper.GetInstance ().TasksCollection ().Name, taskId);
						flowStatesCollection.Save (state);
					}
				}
				else 
				{
					state = new FlowState {
						State = stateId,
						Task = new MongoDBRef (DBHelper.GetInstance ().TasksCollection ().Name, taskId),
						NextStates = null
					};
					flowStatesCollection.Save (state);
					if (flow.States == null) {
						flow.States = new List<FlowState> ();
					}
					flow.States.Add (state);
					flowsCollection.Save (flow);
				}
			}
			return state;
		}
    }
}