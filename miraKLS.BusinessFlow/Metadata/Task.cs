using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using MongoDB.Driver;

namespace miraKLS.BusinessFlow.Metadata
{
	public class Task : LinkedObject<MongoDBRef> //of Forms
	{
		public Task ()
		{
		}
		public LinkedList<Form> GetContents()
		{
			LinkedList<Form> tempList = new LinkedList<Form>();
			foreach (var item in Contents) {
				tempList.AddLast(DBHelper.GetInstance().GetDatabase().FetchDBRefAs<Form>(item));
			}
			return tempList;
		}
	}
}

