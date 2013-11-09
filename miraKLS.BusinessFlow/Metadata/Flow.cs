using System;
using System.Collections;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace miraKLS.BusinessFlow.Metadata
{
	public class Flow : LinkedObject<FlowContentRef>
	{
		public Flow ()
		{
		}

		public IList<FlowState> States{get; set;}
		public IList<FlowContent> GetContents()
		{
			IList<FlowContent> result = new List<FlowContent>();
			if(Contents != null)
			{
				DBHelper dbh = DBHelper.GetInstance();
				foreach(FlowContentRef fcr in Contents)
				{
					Group grpInfo = dbh.FetchFromRef<Group>(fcr.Group);
					Task tskInfo = dbh.FetchFromRef<Task>(fcr.Task);
					result.Add(new FlowContent{GroupInfo = grpInfo, TaskInfo =  tskInfo});
				}
			}
			return result;
		}
	}

	public class FlowState
	{
		[BsonId]
		public string State{get;set;}
		public MongoDBRef Task{get;set;} // Task
		public IDictionary<string, MongoDBRef> NextStates{get;set;} // Route, FlowState
		public IDictionary<string, FlowState> GetNextStates()
		{
			IDictionary<string, FlowState> result = new Dictionary<string, FlowState>();
			if(NextStates != null)
			{
				DBHelper dbh = DBHelper.GetInstance();
				foreach(KeyValuePair<string,MongoDBRef> state in NextStates)
				{
					result.Add(state.Key,dbh.FetchFromRef<FlowState>(state.Value));
				}
			}
			return result;
		}
		public Task GetTask()
		{
			return DBHelper.GetInstance().FetchFromRef<Task>(Task);
		}
	}

	public class FlowContentRef : IEquatable<FlowContentRef>
	{
		public MongoDBRef Group{get;set;} // Group
		public MongoDBRef Task{get;set;} // Task

		#region IEquatable implementation

		public bool Equals (FlowContentRef other)
		{
			return this.Group.Id.AsString == other.Group.Id.AsString 
				&& this.Task.Id.AsString == other.Task.Id.AsString;
		}

		#endregion
	}

	public class FlowContent
	{
		public Group GroupInfo{get;set;}
		public Task TaskInfo{get;set;}
	}
}

