#region using

using System;

#endregion using

namespace Mvp.Xml.Common.XPath
{
	/// <summary>
	/// Represents a variable to use in dynamic XPath expression 
	/// queries.
	/// </summary>
	/// <remarks>Author: Daniel Cazzulino, <a href="http://clariusconsulting.net/kzu">blog</a></remarks>
	public struct XPathVariable
	{
		#region Ctor

		/// <summary>
		/// Initializes the new variable.
		/// </summary>
		/// <param name="name">The name to assign to the variable.</param>
		/// <param name="value">The variable value.</param>
		public XPathVariable(string name, object value)
		{
			_name = name;
			_value = value;
		}

		#endregion Ctor

		#region Public Properties

		/// <summary>
		/// Gets the variable name.
		/// </summary>
		public string Name
		{
			get { return _name; }
		} string _name;

		/// <summary>
		/// Gets the variable value.
		/// </summary>
		public object Value
		{
			get { return _value; }
		} object _value;

		#endregion Public Properties

		#region Equality

		/// <summary>
		/// Checks equality of two variables. They are equal 
		/// if both their <see cref="Name"/> and their <see cref="Value"/> 
		/// are equal.
		/// </summary>
		public override bool Equals(object obj)
		{
			return Name == ((XPathVariable)obj).Name &&
				Value == ((XPathVariable)obj).Value;
		}

		/// <summary>
		/// See <see cref="Object.GetHashCode"/>.
		/// </summary>
		public override int GetHashCode()
		{
			return (Name + "." + Value.GetHashCode()).GetHashCode();
		}


		/// <summary>
		/// Checks equality of two variables. They are equal 
		/// if both their <see cref="Name"/> and their <see cref="Value"/> 
		/// are equal.
		/// </summary>
		public static bool operator ==(XPathVariable a, XPathVariable b)
		{
			return a.Equals(b);
		}

		/// <summary>
		/// Checks equality of two variables. They are not equal 
		/// if both their <see cref="Name"/> and their <see cref="Value"/> 
		/// are different.
		/// </summary>
		public static bool operator !=(XPathVariable a, XPathVariable b)
		{
			return !a.Equals(b);
		}

		#endregion Equality
	}
}
