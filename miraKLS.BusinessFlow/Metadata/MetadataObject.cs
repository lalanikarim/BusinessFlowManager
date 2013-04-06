using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace miraKLS.BusinessFlow.Metadata
{
	public abstract class MetadataObject
	{
		public MetadataObject ()
		{
		}

		public MetadataObject(string name, string description)
		{
			Name = name;
			Description = description;
		}

		[BsonId]
		public string Name{get;set;}
		public string Description{get;set;}
	}
}

