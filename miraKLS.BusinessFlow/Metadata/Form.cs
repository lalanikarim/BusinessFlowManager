using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using MongoDB.Driver;

namespace miraKLS.BusinessFlow.Metadata
{
	public class Form : LinkedObject<MongoDBRef> //of Fields
	{
		public Form ()
		{
		}
		public LinkedList<Field> GetContents()
		{
			LinkedList<Field> tempList = new LinkedList<Field>();
			foreach (var item in Contents) {
				tempList.AddLast(DBHelper.GetInstance().GetDatabase().FetchDBRefAs<Field>(item));
			}
			return tempList;
		}
	}
}

