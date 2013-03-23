using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace miraKLS.BusinessFlow
{
	public class MyClass
	{
		public MyClass ()
		{
			LinkedList<int> test = new LinkedList<int>();
			LinkedListNode<int> node = test.AddFirst(1);
			node = test.AddAfter(node,5);
			node = test.AddAfter(node,3);
			node = test.AddBefore(node,2);
			LinkedListNode<int> third = node = test.AddAfter(node,6);
			node = test.AddAfter(node,2);

			test.AddAfter(third,4);
			node = test.First;
			while(node != null)
			{
				Console.WriteLine(node.Value);
				node = node.Next;
			}
		}
	}
}

