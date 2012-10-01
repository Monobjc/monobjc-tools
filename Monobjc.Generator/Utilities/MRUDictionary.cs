using System;
using System.Collections;
using System.Collections.Generic;

namespace Monobjc.Tools.Generator.Utilities
{
	public class MRUDictionary<TKey, TValue>
	{
		private List<TKey> innerList;
		private Dictionary<TKey, TValue> innerDictionary;
		private int maxSize = -1;

		public MRUDictionary (int size)
		{
			this.innerList = new List<TKey> ();
			this.innerDictionary = new Dictionary<TKey, TValue>();
			this.maxSize = size;
		}

		public void Add (TKey key, TValue value)
		{
			// If the item is already in the set, remove it
			int i = this.innerList.IndexOf (key);
			if (i > -1) {
				this.innerList.RemoveAt (i);
				this.innerDictionary.Remove(key);
			}
				
			// Add the item to the front of the list.
			this.innerList.Insert (0, key);
			this.innerDictionary.Add(key, value);
				
			this.Trim();
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return this.innerDictionary.TryGetValue(key, out value);
		}

		public int Count {
			get { return this.innerList.Count; }
		}

		private void Trim ()
		{
			while (this.innerList.Count > this.maxSize) {
				int index = this.innerList.Count -1 ;
				TKey key = this.innerList[index];
				this.innerList.RemoveAt (index);
				this.innerDictionary.Remove(key);
			}
		}
	}
}
