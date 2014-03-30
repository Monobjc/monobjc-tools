//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2014 - Laurent Etiemble
//
// Monobjc is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// any later version.
//
// Monobjc is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with Monobjc.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Monobjc.Tools.Generator.Model
{
	public partial class BaseEntity
	{
//		private static Regex partExpression = new Regex (@"^(\w+)(\[(.+)\])?(\{(.+)\})?(\((\d+)\))?$");

		/// <summary>
		///   Saves this entity to the given path.
		/// </summary>
		/// <param name = "path">The path.</param>
		public void SaveTo (String path)
		{
			Directory.CreateDirectory (Path.GetDirectoryName (path));
			XmlSerializer serializer = new XmlSerializer (this.GetType ());
			using (StreamWriter writer = new StreamWriter(path)) {
				serializer.Serialize (writer, this);
			}
		}

		/// <summary>
		///   Loads an entity from the given path.
		/// </summary>
		/// <typeparam name = "T"></typeparam>
		/// <param name = "path">The path.</param>
		/// <returns></returns>
		public static T LoadFrom<T> (String path) where T : BaseEntity
		{
			return LoadFrom (path, typeof(T)) as T;
		}

		/// <summary>
		///   Loads an entity from the given path.
		/// </summary>
		/// <param name = "path">The path.</param>
		/// <param name = "type">The type.</param>
		/// <returns></returns>
		public static BaseEntity LoadFrom (String path, Type type)
		{
			if (!File.Exists (path)) {
				return null;
			}
			XmlSerializer serializer = new XmlSerializer (type);
			using (StreamReader reader = new StreamReader(path)) {
				return serializer.Deserialize (reader) as BaseEntity;
			}
		}

		/// <summary>
		///   Creates an entity from the given content.
		/// </summary>
		/// <typeparam name = "T"></typeparam>
		/// <param name = "content">The content.</param>
		/// <returns></returns>
		public static T CreateFrom<T> (String content) where T : BaseEntity
		{
			T entity = CreateFrom (content, typeof(T)) as T;
			return entity;
		}

		/// <summary>
		///   Creates an entity from the given content.
		/// </summary>
		/// <param name = "content">The content.</param>
		/// <param name = "type">The type.</param>
		/// <returns></returns>
		public static BaseEntity CreateFrom (String content, Type type)
		{
			XmlSerializer serializer = new XmlSerializer (type);
			using (StringReader reader = new StringReader(content)) {
				return serializer.Deserialize (reader) as BaseEntity;
			}
		}
	}
}
