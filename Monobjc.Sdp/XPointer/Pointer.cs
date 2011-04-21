#region using

using System;
using System.Xml;
using System.Xml.XPath;

#endregion

namespace Mvp.Xml.XPointer
{
	/// <summary>
	/// Abstract XPointer pointer.
	/// </summary>
	public abstract class Pointer
	{

		#region public methods

		/// <summary>
		/// Parses XPointer pointer and compiles it into
		/// an instance of <see cref="Pointer"/> class.
		/// </summary>
		/// <param name="xpointer">XPointer pointer</param>
		/// <returns>Parsed and compiled XPointer</returns>
		public static Pointer Compile(string xpointer)
		{
			return XPointerParser.ParseXPointer(xpointer);
		}

		/// <summary>
		/// Evaluates <see cref="XPointer"/> pointer and returns 
		/// iterator over pointed nodes.
		/// </summary>
		/// <param name="nav">Navigator to evaluate the 
		/// <see cref="XPointer"/> on.</param>
		/// <returns><see cref="XPathNodeIterator"/> over pointed nodes. Note - this iterator is moved to the first node already.</returns>	    					
		public abstract XPathNodeIterator Evaluate(XPathNavigator nav);

		#endregion
	}
}
