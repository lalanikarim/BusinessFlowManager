using System;
using MongoDB.Bson;

namespace miraKLS.BusinessFlow.Metadata
{
	public class Field : MetadataObject
	{
		public Field ()
		{
		}

		public Field(string name, string description):base(name,description)
		{
		}

		public BsonType FieldType{get;set;}

	}
}

