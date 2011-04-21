#region using

using System;

#endregion

namespace Mvp.Xml.XPointer
{
	/// <summary>
	/// Generic XPointer exception.
	/// </summary>
	public abstract class XPointerException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ApplicationException"/> 
		/// class with a specified error message.
		/// </summary>
		/// <param name="message">Error message</param>
		public XPointerException(string message) : base(message) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="ApplicationException"/> 
		/// class with a specified error message and a reference to the 
		/// inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">Error message</param>
		/// <param name="innerException">Inner exception</param>
		public XPointerException(string message, Exception innerException)
			: base(message, innerException) { }
	}

	/// <summary>
	/// XPointer Framework syntax error.
	/// </summary>
	public class XPointerSyntaxException : XPointerException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="XPointerSyntaxException"/> 
		/// class with a specified error message.
		/// </summary>
		/// <param name="message">Error message</param>
		public XPointerSyntaxException(string message) : base(message) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="XPointerSyntaxException"/> 
		/// class with a specified error message and a reference to the 
		/// inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">Error message</param>
		/// <param name="innerException">Inner exception</param>
		public XPointerSyntaxException(string message, Exception innerException)
			: base(message, innerException) { }
	}

	/// <summary>
	/// XPointer doesn't identify any subresources - it's an error as per 
	/// XPointer Framework.
	/// </summary>
	public class NoSubresourcesIdentifiedException : XPointerException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NoSubresourcesIdentifiedException"/> 
		/// class with a specified error message.
		/// </summary>
		/// <param name="message">Error message</param>
		public NoSubresourcesIdentifiedException(string message) : base(message) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="NoSubresourcesIdentifiedException"/> 
		/// class with a specified error message and a reference to the 
		/// inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">Error message</param>
		/// <param name="innerException">Inner exception</param>
		public NoSubresourcesIdentifiedException(string message, Exception innerException)
			: base(message, innerException) { }
	}
}
