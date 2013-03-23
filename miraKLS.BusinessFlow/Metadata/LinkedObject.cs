using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace miraKLS.BusinessFlow.Metadata
{
	public class LinkedObject<T> : MetadataObject
	{
		public LinkedObject ()
		{
		}

		public LinkedList<T> Contents{get;set;}
	}
}

