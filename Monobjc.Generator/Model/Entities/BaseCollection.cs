using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Monobjc.Tools.Generator.Model
{
	public class BaseCollection<T> : List<T> where T : BaseEntity, new()
	{
		public BaseCollection ()
		{
		}

		[IndexerNameAttribute("Value")]
		public T this[String name]
		{
			get {
				T result = this.FirstOrDefault(e => e.Name == name);
				if (result == null) {
					// Create a dummy object
					result = new T();
				}
				return result;
			}
			set {
				int index = this.FindIndex(e => e.Name == name);
				if (index != -1) {
					this[index] = value;
				} else {
					this.Add(value);
				}
			}
		}

		[IndexerNameAttribute("Value")]
		public T this[String name, int rank]
		{
			get {
				IEnumerable<T> result = this.Where(e => e.Name == name);
				switch(result.Count()) {
				case 0:
					// Return a dummy object
					return new T();
				case 1:
					return result.First();
				default:
					throw new ArgumentException("Multiple match found for " + name + " and " + rank);
				}
			}
		}

		public void Set(String name, String value)
		{
			int index = this.FindIndex(e => e.Name == name);
			T entity = BaseEntity.CreateFrom<T>(value);
			//Console.WriteLine("index=" + index);
			//Console.WriteLine("entity=" + entity);
			if (index != -1) {
				this[index] = entity;
			} else {
				this.Add(entity);
			}
		}

		public override int GetHashCode ()
		{
			unchecked {
				int hash = base.GetHashCode();
				return this.Aggregate(hash, (current, item) => current * 23 + item.GetHashCode()); 
			}
		}
	}
}
