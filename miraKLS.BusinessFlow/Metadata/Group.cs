using System;
using MongoDB.Bson;

namespace miraKLS.BusinessFlow.Metadata
{
	public class Group : MetadataObject
	{
		public Group ()
		{
		}

		public Group(string name, string description):base(name,description)
		{
		}
	}
}

