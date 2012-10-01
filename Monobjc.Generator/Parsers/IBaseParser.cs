using System;
using System.Collections.Specialized;
using System.IO;
using Monobjc.Tools.Generator.Model;

namespace Monobjc.Tools.Generator.Parsers
{
	public interface IBaseParser
    {
		/// <summary>
		///   Gets the settings.
		/// </summary>
		NameValueCollection Settings { get; }

		/// <summary>
		/// Gets or sets the logger.
		/// </summary>
		TextWriter Logger { get; }

		/// <summary>
        ///   Parses the specified entity.
        /// </summary>
		void Parse(BaseEntity entity, String file);

        /// <summary>
        ///   Parses the specified entity.
        /// </summary>
		void Parse(BaseEntity entity, TextReader reader);
    }
}
