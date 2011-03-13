//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2011 - Laurent Etiemble
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
using System.Xml.Serialization;

namespace Monobjc.Tools.Generator.Model.Entities
{
    /// <summary>
    ///   This class serves as a base for all the documentation elements (classes, interfaces, methods, etc).
    /// </summary>
    [Serializable]
    [XmlRoot("Base")]
    public class BaseEntity : IEquatable<BaseEntity>
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "BaseEntity" /> class.
        /// </summary>
        public BaseEntity()
        {
            this.Summary = new List<String>();
            this.Generate = true;
        }

        /// <summary>
        ///   Gets or sets the summary.
        /// </summary>
        /// <value>The summary.</value>
        [XmlArray]
        [XmlArrayItem("Line")]
        public List<String> Summary { get; set; }

        /// <summary>
        ///   Gets or sets the mininum availability.
        /// </summary>
        /// <value>The mininum availability.</value>
        [XmlElement]
        public String MinAvailability { get; set; }

        /// <summary>
        ///   Gets or sets the maximum availability.
        /// </summary>
        /// <value>The maximum availability.</value>
        [XmlElement]
        public String MaxAvailability { get; set; }

        /// <summary>
        ///   Gets or sets the obsolete attribute content.
        /// </summary>
        /// <value>The obsolete attribute content.</value>
        [XmlElement(IsNullable = true)]
        public String Obsolete { get; set; }

        /// <summary>
        ///   Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [XmlElement]
        public String Name { get; set; }

        /// <summary>
        ///   Gets or sets whether this entity must be generated.
        /// </summary>
        /// <value><code>true</code> to generate; <code>false</code> otherwise.</value>
        [XmlAttribute]
        public bool Generate { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to copy this instance in descendants.
        /// </summary>
        /// <value><c>true</c> to copy this instance in descendants; otherwise, <c>false</c>.</value>
        [XmlAttribute]
        public bool CopyInDescendants { get; set; }

        /// <summary>
        ///   Gets the children.
        /// </summary>
        /// <value>The children.</value>
        [XmlIgnore]
        public virtual List<BaseEntity> Children
        {
            get { return new List<BaseEntity>(); }
        }

        /// <summary>
        ///   Adjusts the availability.
        /// </summary>
        public void AdjustAvailability()
        {
            String minAvailability = "VERSION";
            String maxAvailability = "VERSION";

            foreach (BaseEntity value in this.Children.Where(c => c.Generate))
            {
                string valueMinAvailability = value.MinAvailability ?? String.Empty;
                if (String.Compare(valueMinAvailability, minAvailability) < 0)
                {
                    minAvailability = valueMinAvailability;
                }
                string valueMaxAvailability = value.MaxAvailability ?? String.Empty;
                if (String.Compare(valueMaxAvailability, maxAvailability) > 0)
                {
                    maxAvailability = valueMaxAvailability;
                }
            }

            this.MinAvailability = minAvailability;
            this.MaxAvailability = maxAvailability;
        }

        /// <summary>
        ///   Saves this entity to the given path.
        /// </summary>
        /// <param name = "path">The path.</param>
        public void SaveTo(String path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            using (StreamWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, this);
            }
        }

        /// <summary>
        ///   Loads an entity from the given path.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "path">The path.</param>
        /// <returns></returns>
        public static T LoadFrom<T>(String path) where T : BaseEntity
        {
            return LoadFrom(path, typeof (T)) as T;
        }

        /// <summary>
        ///   Loads an entity from the given path.
        /// </summary>
        /// <param name = "path">The path.</param>
        /// <param name = "type">The type.</param>
        /// <returns></returns>
        public static BaseEntity LoadFrom(String path, Type type)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            using (StreamReader reader = new StreamReader(path))
            {
                return serializer.Deserialize(reader) as BaseEntity;
            }
        }

        /// <summary>
        ///   Creates an entity from the given content.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "content">The content.</param>
        /// <returns></returns>
        public static T CreateFrom<T>(String content) where T : BaseEntity
        {
            return CreateFrom(content, typeof (T)) as T;
        }

        /// <summary>
        ///   Creates an entity from the given content.
        /// </summary>
        /// <param name = "content">The content.</param>
        /// <param name = "type">The type.</param>
        /// <returns></returns>
        public static BaseEntity CreateFrom(String content, Type type)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            using (StringReader reader = new StringReader(content))
            {
                return serializer.Deserialize(reader) as BaseEntity;
            }
        }

        /// <summary>
        ///   Parse the given object query and performs the operation.
        /// </summary>
        /// <param name = "query">The query.</param>
        public bool SetValue(String query)
        {
            int index = this.Next(query, '=');
            String predicate = query.Substring(0, index);
            String stringValue = query.Substring(index + 1);

            Object target = this;
            while (true)
            {
                index = this.Next(predicate, '.');
                bool lastPart = (index == -1);
                String part = lastPart ? predicate : predicate.Substring(0, index);

                if (part.Contains("["))
                {
                    String propertyName = part.Substring(0, part.IndexOf("["));
                    PropertyInfo property = target.GetType().GetProperty(propertyName);
                    if (property == null)
                    {
                        Console.WriteLine("Cannot find property '{0}' in '{1}'", propertyName, query);
                        return false;
                    }

                    Object collection = property.GetValue(target, null);
                    IEnumerable result = (IEnumerable) collection;
                    IEnumerable<BaseEntity> entities = result.Cast<BaseEntity>();
                    if (entities == null)
                    {
                        Console.WriteLine("Cannot load collection '{0}' in '{1}'", propertyName, query);
                        return false;
                    }

                    int pos1 = part.IndexOf("[");
                    int pos2 = this.Next(part, ']', pos1 + 1);
                    String name = part.Substring(pos1 + 1, pos2 - pos1 - 1);
                    bool hasSelector = part.IndexOf('[', pos2) != -1;

                    // If there are multiple result, then another selector must be present
                    IEnumerable<BaseEntity> targets = entities.Where(e => e.Name.Equals(name));
                    switch (targets.Count())
                    {
                        case 0:
                            target = null;
                            break;
                        case 1:
                            if (!hasSelector)
                            {
                                target = targets.First();
                            }
                            else
                            {
                                // Check for selector 
                                pos1 = part.IndexOf("[", pos2);
                                pos2 = part.IndexOf("]", pos1);
                                String differentiator = part.Substring(pos1 + 1, pos2 - pos1 - 1);
                                String[] diffparts = differentiator.Split('=');

                                BaseEntity entity = targets.First();
                                property = entity.GetType().GetProperty(diffparts[0]);
                                if (property != null)
                                {
                                    String value = "" + property.GetValue(entity, null);
                                    if (String.Equals(value, diffparts[1], StringComparison.OrdinalIgnoreCase))
                                    {
                                        target = entity;
                                        break;
                                    }
                                }

                                Console.WriteLine("Target has selector but it does not match the only one result for '{0}' in '{1}'", propertyName, query);

                                target = null;
                            }
                            break;
                        default:
                            {
                                pos1 = part.IndexOf("[", pos2);
                                if (pos1 != -1)
                                {
                                    pos2 = part.IndexOf("]", pos1);
                                    String differentiator = part.Substring(pos1 + 1, pos2 - pos1 - 1);

                                    String[] diffparts = differentiator.Split('=');
                                    switch (diffparts.Length)
                                    {
                                        case 1:
                                            int position = Int32.Parse(diffparts[0]);
                                            if (position < targets.Count())
                                            {
                                                target = targets.ElementAt(position);
                                            }
                                            else
                                            {
                                                target = null;
                                            }
                                            break;
                                        case 2:
                                            foreach (BaseEntity entity in targets)
                                            {
                                                property = entity.GetType().GetProperty(diffparts[0]);
                                                if (property != null)
                                                {
                                                    String value = "" + property.GetValue(entity, null);
                                                    if (String.Equals(value, diffparts[1], StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        target = entity;
                                                        break;
                                                    }
                                                }
                                            }
                                            break;
                                        default:
                                            target = null;
                                            break;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Target has no selector but it does match multiple values for '{0}' in '{1}'", propertyName, query);

                                    target = null;
                                }
                            }
                            break;
                    }

                    if (target == null)
                    {
                        // If this is the last, then add it
                        if (lastPart)
                        {
                            Type collectionType = property.PropertyType;
                            Type elementType = collectionType.GetGenericArguments()[0];
                            MethodInfo adder = collectionType.GetMethod("Add");

                            BaseEntity entity = CreateFrom(stringValue, elementType);
                            adder.Invoke(collection, new object[] {entity});
                            target = entity;

                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Not the last part '{0}' in '{1}'", name, query);
                            return false;
                        }
                    }
                }
                else
                {
                    PropertyInfo property = target.GetType().GetProperty(part);
                    if (property == null)
                    {
                        Console.WriteLine("Cannot find property '{0}' in '{1}'", part, query);
                        return false;
                    }

                    // If this is the last part
                    if (lastPart)
                    {
                        String currentValue = "" + property.GetValue(target, null);
                        if (property.PropertyType.Equals(typeof (String)))
                        {
                            if (String.Equals(currentValue, stringValue))
                            {
                                return false;
                            }
                            property.SetValue(target, stringValue, null);
                            return true;
                        }
                        if (property.PropertyType.Equals(typeof (Boolean)))
                        {
                            if (String.Equals(currentValue, stringValue, StringComparison.OrdinalIgnoreCase))
                            {
                                return false;
                            }
                            property.SetValue(target, Boolean.Parse(stringValue), null);
                            return true;
                        }
                    }
                    else
                    {
                        target = property.GetValue(target, null);
                    }
                }

                if (lastPart)
                {
                    break;
                }

                predicate = predicate.Substring(index + 1);
            }

            return false;
        }

        /// <summary>
        ///   Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        ///   true if the current object is equal to the <paramref name = "other" /> parameter; otherwise, false.
        /// </returns>
        /// <param name = "other">An object to compare with this object.
        /// </param>
        public bool Equals(BaseEntity other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(other.Name, this.Name);
        }

        /// <summary>
        ///   Determines whether the specified <see cref = "T:System.Object" /> is equal to the current <see cref = "T:System.Object" />.
        /// </summary>
        /// <returns>
        ///   true if the specified <see cref = "T:System.Object" /> is equal to the current <see cref = "T:System.Object" />; otherwise, false.
        /// </returns>
        /// <param name = "obj">The <see cref = "T:System.Object" /> to compare with the current <see cref = "T:System.Object" />. 
        /// </param>
        /// <exception cref = "T:System.NullReferenceException">The <paramref name = "obj" /> parameter is null.
        /// </exception>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != typeof (BaseEntity))
            {
                return false;
            }
            return this.Equals((BaseEntity) obj);
        }

        /// <summary>
        ///   Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///   A hash code for the current <see cref = "T:System.Object" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return (this.Name != null ? this.Name.GetHashCode() : 0);
        }

        /// <summary>
        ///   Implements the operator ==.
        /// </summary>
        /// <param name = "left">The left.</param>
        /// <param name = "right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(BaseEntity left, BaseEntity right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///   Implements the operator !=.
        /// </summary>
        /// <param name = "left">The left.</param>
        /// <param name = "right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(BaseEntity left, BaseEntity right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        ///   Advance into the string until the
        /// </summary>
        /// <param name = "predicate">The predicate.</param>
        /// <param name = "termination">The termination.</param>
        /// <returns></returns>
        private int Next(String predicate, char termination, int startPos = 0)
        {
            int i = startPos;
            int nested = 0;
            while (i < predicate.Length)
            {
                char c = predicate[i];
                if (c == termination && nested == 0)
                {
                    return i;
                }
                if (c == '[')
                {
                    nested++;
                }
                else if (c == ']')
                {
                    nested--;
                }
                i++;
            }
            return -1;
        }
    }
}