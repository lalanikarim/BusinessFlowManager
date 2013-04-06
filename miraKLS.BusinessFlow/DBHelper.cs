using System;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using miraKLS.BusinessFlow.Metadata;
using MongoDB.Driver.Linq;

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
		private MongoCollection<Flow> flowsCollection = null;

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

		public MongoCollection<Flow> FlowsCollection()
		{
			if(flowsCollection == null)
			{
				flowsCollection = GetDatabase().GetCollection<Flow>("flows");
			}
			return flowsCollection;
		}
	}

	public enum ContentAction
	{
		Add,
		Remove
	}

	public enum Error
	{
		///TODO: Error enumerations
	}
}

