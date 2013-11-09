using System;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using miraKLS.BusinessFlow.Metadata;
using MongoDB.Driver.Linq;
using System.Collections.Generic;

namespace miraKLS.BusinessFlow
{
	public class DBHelper
	{
		private DBHelper ()
		{
		}

		private static DBHelper instance = null;

		private MongoServer server = null;
		private MongoClient client = null;
		private MongoDatabase database = null;

		private MongoCollection<Field> fieldsCollection = null;
		private MongoCollection<Form> formsCollection = null;
		private MongoCollection<Task> tasksCollection = null;
		private MongoCollection<Flow> flowsCollection = null;
		private MongoCollection<FlowState> flowStatesCollection = null;
		private MongoCollection<Group> groupsCollection = null;
		private MongoCollection<BsonDocument> tempCollection = null;

		private Error dbError;

		public static DBHelper GetInstance()
		{
			if(instance == null)
			{
				instance = new DBHelper();				
			}
			return instance;
		}

		public MongoDatabase GetDatabase()
		{
			if(database == null)
			{
				if(client == null)
				{
					client = new MongoClient("mongodb://localhost/");
					server = client.GetServer();
				}
				database = server.GetDatabase("businessflow");
			}
			return database;
		}

		public MongoCollection<Field> FieldsCollection()
		{
			if(fieldsCollection == null)
			{
				fieldsCollection = GetDatabase().GetCollection<Field>("fields");
			}
			return fieldsCollection;
		}

		public Field GetField(string Id)
		{
			Field model = null;
			if(!string.IsNullOrEmpty(Id))
			{
				model = (from f in FieldsCollection().AsQueryable() where f.Name == Id select f).FirstOrDefault();
			}

			return model;
		}

		public MongoCollection<Form> FormsCollection()
		{
			if(formsCollection == null)
			{
				formsCollection = GetDatabase().GetCollection<Form>("forms");
			}
			return formsCollection;
		}

		public Form GetForm(string Id)
		{
			Form model = null;
			if(!string.IsNullOrEmpty(Id))
			{
				model = (from f in FormsCollection().AsQueryable() where f.Name == Id select f).FirstOrDefault();
			}
			
			return model;
		}

		public MongoCollection<Task> TasksCollection()
		{
			if(tasksCollection == null)
			{
				tasksCollection = GetDatabase().GetCollection<Task>("tasks");
			}
			return tasksCollection;
		}
		
		public Task GetTask(string Id)
		{
			Task model = null;
			if(!string.IsNullOrEmpty(Id))
			{
				model = (from t in TasksCollection().AsQueryable() where t.Name == Id select t).FirstOrDefault();
			}
			
			return model;
		}

		public MongoCollection<Flow> FlowsCollection()
		{
			if(flowsCollection == null)
			{
				flowsCollection = GetDatabase().GetCollection<Flow>("flows");
			}
			return flowsCollection;
		}

		public Flow GetFlow(string Id)
		{
			Flow model = null;
			if(!string.IsNullOrEmpty(Id))
			{
				model = (from f in FlowsCollection().AsQueryable() where f.Name == Id select f).FirstOrDefault();
			}
			
			return model;
		}

		public MongoCollection<FlowState> FlowStatesCollection()
		{
			if(flowStatesCollection == null)
			{
				flowStatesCollection = GetDatabase().GetCollection<FlowState>("flowstates");
			}
			return flowStatesCollection;
		}

		public FlowState GetFlowState(string Id)
		{
			FlowState model = null;
			if(!string.IsNullOrEmpty(Id))
			{
				model = (from fs in FlowStatesCollection().AsQueryable() where fs.State == Id select fs).FirstOrDefault();
			}
			return model;
		}

		public MongoCollection<Group> GroupsCollection()
		{
			if(groupsCollection == null)
			{
				groupsCollection = GetDatabase().GetCollection<Group>("groups");
			}
			return groupsCollection;
		}

		public Group GetGroup(string Id)
		{
			Group model = null;
			if(!string.IsNullOrEmpty(Id))
			{
				model = (from g in GroupsCollection().AsQueryable() where g.Name == Id select g).FirstOrDefault();
			}
			
			return model;
		}

		public MongoCollection<BsonDocument> TempCollection()
		{
			if(tempCollection == null)
			{
				tempCollection = GetDatabase().GetCollection<BsonDocument>("temps");
			}
			return tempCollection;
		}


		public T FetchFromRef<T>(MongoDBRef dbref)
		{
			return database.FetchDBRefAs<T>(dbref);
		}
	}

	public enum ContentAction
	{
		Add,
		Remove,
		Up,
		Down
	}

	public enum Error
	{
		///TODO: Error enumerations
	}
}

