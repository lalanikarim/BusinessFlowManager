using System;
using System.Collections.Generic;
using MongoDB.Bson;
using miraKLS.BusinessFlow;
using miraKLS.BusinessFlow.Metadata;
using NUnit.Framework;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Linq;

namespace BusinessFlowManager.Tests
{
	[TestFixture()]
	public class BusinessFlowTests
	{
		[Test()]
		public void FieldTest ()
		{
			Field f = new Field("F1","First Field");

			Assert.AreEqual(f.Name,"F1");
			Assert.AreEqual(f.Description,"First Field");
		}

		[Test()]
		public void DBHelperTest()
		{
			Assert.IsNotNull(DBHelper.GetInstance(),"DBHelper Instance Is Not Null");
			Assert.IsNotNull(DBHelper.GetInstance().GetDatabase(),"Database Is Not Null");
			Console.WriteLine("{0} collections found", DBHelper.GetInstance().GetDatabase().GetCollectionNames().Count());
		}
	}
}

