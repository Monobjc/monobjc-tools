#region using

using System;
using System.IO;
using System.Text;

#endregion using

namespace Mvp.Xml.Common
{
	/// <summary>
	/// Allows streams without a root element (i.e. multiple document 
	/// fragments) to be passed to an <see cref="System.Xml.XmlReader"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class is obsolete. Use <see cref="XmlFragmentReader"/> instead.
	/// </para>
	/// A faked root element is added at the stream 
	/// level to enclose the fragments, which can be customized 
	/// using the overloaded constructors.
	/// <para>Author: Daniel Cazzulino, <a href="http://clariusconsulting.net/kzu">blog</a></para>
	/// See: http://weblogs.asp.net/cazzu/archive/2004/04/23/119263.aspx.
	/// </remarks>
	[Obsolete("Use XmlFragmentReader instead.", false)]
	public class XmlFragmentStream : Stream
	{
		#region Fields

		// Holds the inner stream with the XML fragments.
		Stream _stream;

		bool _first = true;
		bool _done = false;
		bool _eof = false;

		// TODO: there's a potential encoding issue here.
		byte[] _rootstart = Encoding.UTF8.GetBytes("<root>");
		byte[] _rootend = Encoding.UTF8.GetBytes("</root>");
		int _endidx = -1;

		#endregion Fields

		#region Ctors

		/// <summary>
		/// Initializes the class with the underlying stream to use, and 
		/// uses the default &lt;root&gt; container element. 
		/// </summary>
		/// <param name="innerStream">The stream to read from.</param>
		public XmlFragmentStream(Stream innerStream)
		{
			if (innerStream == null)
				throw new ArgumentNullException("innerStream");
			_stream = innerStream;
		}

		/// <summary>
		/// Initializes the class with the underlying stream to use, with 
		/// a custom root element. 
		/// </summary>
		/// <param name="innerStream">The stream to read from.</param>
		/// <param name="rootName">Custom root element name to use.</param>
		public XmlFragmentStream(Stream innerStream, string rootName)
			: this(innerStream)
		{
			_rootstart = Encoding.UTF8.GetBytes("<" + rootName + ">");
			_rootend = Encoding.UTF8.GetBytes("</" + rootName + ">");
		}

		/// <summary>
		/// Initializes the class with the underlying stream to use, with 
		/// a custom root element. 
		/// </summary>
		/// <param name="innerStream">The stream to read from.</param>
		/// <param name="rootName">Custom root element name to use.</param>
		/// <param name="ns">The namespace of the root element.</param>
		public XmlFragmentStream(Stream innerStream, string rootName, string ns)
			: this(innerStream)
		{
			_rootstart = Encoding.UTF8.GetBytes("<" + rootName + " xmlns=\"" + ns + "\">");
			_rootend = Encoding.UTF8.GetBytes("</" + rootName + ">");
		}

		#endregion Ctors

		#region Stream abstract implementation

		/// <summary>See <see cref="Stream.Close"/>.</summary>
		public override void Close()
		{
			_stream.Close();
			base.Close();
		}

		/// <summary>See <see cref="Stream.Flush"/>.</summary>
		public override void Flush()
		{
			_stream.Flush();
		}

		/// <summary>See <see cref="Stream.Seek"/>.</summary>
		public override long Seek(long offset, SeekOrigin origin)
		{
			return _stream.Seek(offset, origin);
		}

		/// <summary>See <see cref="Stream.SetLength"/>.</summary>
		public override void SetLength(long value)
		{
			_stream.SetLength(value);
		}

		/// <summary>See <see cref="Stream.Write"/>.</summary>
		public override void Write(byte[] buffer, int offset, int count)
		{
			_stream.Write(buffer, offset, count);
		}

		/// <summary>See <see cref="Stream.CanRead"/>.</summary>
		public override bool CanRead { get { return _stream.CanRead; } }

		/// <summary>See <see cref="Stream.CanSeek"/>.</summary>
		public override bool CanSeek { get { return _stream.CanSeek; } }

		/// <summary>See <see cref="Stream.CanWrite"/>.</summary>
		public override bool CanWrite { get { return _stream.CanWrite; } }

		/// <summary>See <see cref="Stream.Length"/>.</summary>
		public override long Length { get { return _stream.Length; } }

		/// <summary>See <see cref="Stream.Position"/>.</summary>
		public override long Position
		{
			get { return _stream.Position; }
			set { _stream.Position = value; }
		}

		#endregion Stream abstract implementation

		#region Read method

		/// <summary>See <see cref="Stream.Read"/>.</summary>
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (_done)
			{
				if (!_eof)
				{
					_eof = true;
					return 0;
				}
				else
				{
					throw new System.IO.EndOfStreamException(Monobjc.Tools.Sdp.Properties.Resources.XmlFragmentStream_EOF);
				}
			}

			// If this is the first one, return the wrapper root element.
			if (_first)
			{
				_rootstart.CopyTo(buffer, 0);

				_stream.Read(buffer, _rootstart.Length, count - _rootstart.Length);

				_first = false;
				return count;
			}

			// We have a pending closing wrapper root element.
			if (_endidx != -1)
			{
				for (int i = _endidx; i < _rootend.Length; i++)
				{
					buffer[i] = _rootend[i];
				}
				return _rootend.Length - _endidx;
			}

			int ret = _stream.Read(buffer, offset, count);

			// Did we reached the end?
			if (ret < count)
			{
				_rootend.CopyTo(buffer, ret);
				if (count - ret > _rootend.Length)
				{
					_done = true;
					return ret + _rootend.Length;
				}
				else
				{
					_endidx = count - ret;
					return count;
				}
			}

			return ret;
		}

		#endregion Read method
	}
}
