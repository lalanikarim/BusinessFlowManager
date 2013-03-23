using System;

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

		public string Name{get;set;}
		public string Description{get;set;}
	}
}

