#region using

using System;
using System.Globalization;

#endregion

namespace Mvp.Xml.XInclude
{
	/// <summary>
	/// Generic XInclude exception.	
	/// </summary>
	public abstract class XIncludeException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="XIncludeException"/> 
		/// class with a specified error message.
		/// </summary>
		/// <param name="message">Error message</param>
		public XIncludeException(string message) : base(message) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="XIncludeException"/> 
		/// class with a specified error message and a reference to the 
		/// inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">Error message</param>
		/// <param name="innerException">Inner exceptiion</param>
		public XIncludeException(string message, Exception innerException)
			: base(message, innerException) { }
	}

	/// <summary>
	/// <c>ResourceException</c> represents resource error as per XInclude specification.
	/// </summary>
	/// <remarks>
	/// Resource error is internal error and should lead to the fallback processing.
	/// <c>ResourceException</c> therefore should never be thrown outside 
	/// the XInclude Processor.
	/// </remarks>
	internal class ResourceException : XIncludeException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ResourceException"/> 
		/// class with a specified error message.
		/// </summary>
		/// <param name="message">Error message</param>
		public ResourceException(string message) : base(message) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="ResourceException"/> 
		/// class with a specified error message and a reference to the 
		/// inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">Error message</param>
		/// <param name="innerException">Inner exceptiion</param>
		public ResourceException(string message, Exception innerException)
			: base(message, innerException) { }
	}

	/// <summary>
	/// <c>FatalException</c> represents fatal error as per XInclude spcification.
	/// </summary>
	public abstract class FatalException : XIncludeException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FatalException"/> 
		/// class with a specified error message.
		/// </summary>
		/// <param name="message">Error message</param>
		public FatalException(string message) : base(message) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="FatalException"/> 
		/// class with a specified error message and a reference to the 
		/// inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">Error message</param>
		/// <param name="innerException">Inner exceptiion</param>
		public FatalException(string message, Exception innerException)
			: base(message, innerException) { }
	}

	/// <summary>
	/// Non XML character in a document to include exception.
	/// </summary>
	public class NonXmlCharacterException : FatalException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NonXmlCharacterException"/> 
		/// class with a specified invalid character.
		/// </summary>
		/// <param name="c">Invalid character</param>
		public NonXmlCharacterException(char c)
			:
			base(String.Format(CultureInfo.CurrentCulture, Monobjc.Tools.Sdp.Properties.Resources.NonXmlCharacter, ((int)c).ToString("X2"))) { }
	}

	/// <summary>
	/// Circular inclusion exception.
	/// </summary>
	public class CircularInclusionException : FatalException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CircularInclusionException"/> 
		/// class with a specified Uri that causes inclusion loop.
		/// </summary>
		/// <param name="uri">Uri that causes inclusion loop</param>
		public CircularInclusionException(Uri uri)
			:
			base(String.Format(CultureInfo.CurrentCulture, Monobjc.Tools.Sdp.Properties.Resources.CircularInclusion, uri.AbsoluteUri)) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="CircularInclusionException"/> 
		/// class with a specified Uri that causes inclusion loop and error location within
		/// XML document.
		/// </summary>
		/// <param name="uri">Uri that causes inclusion loop</param>
		/// <param name="line">Line number</param>
		/// <param name="locationUri">Location Uri</param>
		/// <param name="position">Column number</param>
		public CircularInclusionException(Uri uri, string locationUri, int line, int position)
			:
			base(String.Format(CultureInfo.CurrentCulture, Monobjc.Tools.Sdp.Properties.Resources.CircularInclusionLong, uri.AbsoluteUri, locationUri, line, position)) { }
	}

	/// <summary>
	/// Resource error not backed up by xi:fallback exception.
	/// </summary>	
	public class FatalResourceException : FatalException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FatalResourceException"/> 
		/// class with the specified inner exception that is the cause of this exception.
		/// </summary>        
		/// <param name="re">Inner exceptiion</param>
		public FatalResourceException(Exception re)
			:
			base(String.Format(CultureInfo.CurrentCulture, Monobjc.Tools.Sdp.Properties.Resources.FatalResourceException, re.Message), re) { }
	}

	/// <summary>
	/// XInclude syntax error exception.
	/// </summary>
	public class XIncludeSyntaxError : FatalException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="XIncludeSyntaxError"/> 
		/// class with a specified error message.
		/// </summary>
		/// <param name="message">Error message</param>
		public XIncludeSyntaxError(string message) : base(message) { }
	}

	/// <summary>
	/// Include location identifies an attribute or namespace node.
	/// </summary>
	public class AttributeOrNamespaceInIncludeLocationError : FatalException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AttributeOrNamespaceInIncludeLocationError"/> 
		/// class with a specified error message.
		/// </summary>
		/// <param name="message">Error message</param>
		public AttributeOrNamespaceInIncludeLocationError(string message) : base(message) { }
	}

	/// <summary>
	/// Not wellformed inclusion result (e.g. top-level xi:include
	/// includes multiple elements).
	/// </summary>
	public class MalformedXInclusionResultError : FatalException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MalformedXInclusionResultError"/> 
		/// class with a specified error message.
		/// </summary>
		/// <param name="message">Error message</param>
		public MalformedXInclusionResultError(string message) : base(message) { }
	}

	/// <summary>
	/// Value of the "accept" attribute contains an invalid for 
	/// HTTP header character (outside #x20 through #x7E range).
	/// </summary>
	public class InvalidAcceptHTTPHeaderValueError : FatalException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidAcceptHTTPHeaderValueError"/> 
		/// class with a specified invalid character.
		/// </summary>
		/// <param name="c">Invalid character</param>
		public InvalidAcceptHTTPHeaderValueError(char c)
			:
			base(String.Format(CultureInfo.CurrentCulture, Monobjc.Tools.Sdp.Properties.Resources.InvalidCharForAccept, ((int)c).ToString("X2"))) { }
	}
}
