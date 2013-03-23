using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace miraKLS.BusinessFlow
{
	public class DBHelper
	{
		private DBHelper ()
		{
		}

		private static DBHelper instance = null;

		private MongoServer server = null;
		private MongoDatabase database = null;
		private MongoClient client = null;

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
	}
}

