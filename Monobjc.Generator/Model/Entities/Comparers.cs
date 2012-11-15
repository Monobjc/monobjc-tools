using System;
using System.Collections.Generic;

namespace Monobjc.Tools.Generator.Model
{
	public class MethodComparer : IEqualityComparer<MethodEntity>
	{
		public bool Equals (MethodEntity x, MethodEntity y)
		{
			if (x.Static != y.Static) {
				return false;
			}
			if (!x.Selector.Equals (y.Selector)) {
				return false;
			}
			return true;
		}
		
		public int GetHashCode (MethodEntity obj)
		{
			unchecked {
				int hash = 17;
				hash = hash * 23 + obj.Static.GetHashCode ();
				hash = hash * 23 + obj.Selector.GetHashCode ();
				return hash;
			}
		}			
	}

	public class PropertyComparer : IEqualityComparer<PropertyEntity>
	{
		public bool Equals (PropertyEntity x, PropertyEntity y)
		{
			if (x.Static != y.Static) {
				return false;
			}
			if (!x.Getter.Selector.Equals (y.Getter.Selector)) {
				return false;
			}
			return true;
		}
		
		public int GetHashCode (PropertyEntity obj)
		{
			unchecked {
				int hash = 17;
				hash = hash * 23 + obj.Static.GetHashCode ();
				hash = hash * 23 + obj.Getter.Selector.GetHashCode ();
				return hash;
			}
		}			
	}

	public class FunctionComparer : IEqualityComparer<FunctionEntity>
	{
		public bool Equals (FunctionEntity x, FunctionEntity y)
		{
			if (!x.Signature.Equals (y.Signature)) {
				return false;
			}
			return true;
		}
		
		public int GetHashCode (FunctionEntity obj)
		{
			unchecked {
				int hash = 17;
				hash = hash * 23 + obj.Signature.GetHashCode ();
				return hash;
			}
		}			
	}
}
