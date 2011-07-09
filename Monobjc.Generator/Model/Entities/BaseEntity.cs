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
using System.Text.RegularExpressions;

namespace Monobjc.Tools.Generator.Model.Entities
{
    /// <summary>
    ///   This class serves as a base for all the documentation elements (classes, interfaces, methods, etc).
    /// </summary>
    [Serializable]
    [XmlRoot("Base")]
    public class BaseEntity : IEquatable<BaseEntity>
    {
        private static Regex PART_EXPRESSION = new Regex(@"^(\w+)(\[(.+)\])?(\{(.+)\})?(\((\d+)\))?$");

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
            if (!File.Exists(path))
            {
                return null;
            }
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
            int equalSign = this.Next(query, '=');
            String predicate = query.Substring(0, equalSign);
            String stringValue = query.Substring(equalSign + 1);
            
            // Split the predicate
            IList<String> parts = new List<String>();
            int dot = 0;
            while(true)
            {
                dot = this.Next(predicate, '.');
                if (dot == -1)
                {
                    parts.Add(predicate);
                    break;
                }
                
                String part = predicate.Substring(0, dot);
                parts.Add(part);
                
                predicate = predicate.Substring(dot + 1);
            }
            
            Object target = this;
            for (int i = 0; i < parts.Count; i++)
            {
                String part = parts[i];
                bool lastPart = (i == parts.Count - 1);

                Match m = PART_EXPRESSION.Match(part);
                if (!m.Success)
                {
                    Console.WriteLine("Error while parsing '{0}'", predicate);
                    return false;
                }

                String propertyName = m.Groups[1].Value;
                String nameSelector = m.Groups[3].Value;
                String otherSelector = m.Groups[5].Value;
                String indexSelector = m.Groups[7].Value;

                if (String.IsNullOrEmpty(propertyName))
                {
                    Console.WriteLine("Error while parsing '{0}': No property found", predicate);
                    return false;
                }

                PropertyInfo propertyInfo = target.GetType().GetProperty(propertyName);
                if (propertyInfo == null)
                {
                    Console.WriteLine("Cannot found property {0} on {1}", propertyName, target);
                    return false;
                }

                Type propertyType = propertyInfo.PropertyType;
                Object propertyValue = propertyInfo.GetValue(target, null);
                IEnumerable collection = propertyValue as IList;

                bool hasNameSelector = !String.IsNullOrEmpty(nameSelector);
                bool hasOtherSelector = !String.IsNullOrEmpty(otherSelector);
                bool hasIndexSelector = !String.IsNullOrEmpty(indexSelector);

                // Check if a name selector is present
                if (collection != null)
                {
                    // This is a collection
                    IEnumerable<BaseEntity> enumerable = collection.Cast<BaseEntity>();
                    if (enumerable == null)
                    {
                        Console.WriteLine("Cannot load collection '{0}' on '{1}'", propertyName, target);
                        return false;
                    }
                    Type collectionType = enumerable.GetType();
                    Type elementType = collectionType.GetGenericArguments()[0];

                    // Apply name selector
                    if (hasNameSelector)
                    {
                        enumerable = enumerable.Where(e => String.Equals(e.Name, nameSelector));
                    }

                    // Apply other selector
                    if (hasOtherSelector && collectionType != null)
                    {
                        String[] selectorParts = otherSelector.Split('=');
                        String property = selectorParts[0];
                        String value = selectorParts[1];
                        PropertyInfo otherPropertyInfo = elementType.GetProperty(property);

                        enumerable = enumerable.Where(e => String.Equals("" + otherPropertyInfo.GetValue(e, null), value));
                    }

                    // Apply index selector
                    if (hasIndexSelector)
                    {
                        int index = Int32.Parse(indexSelector);
                        if (index < enumerable.Count())
                        {
                            enumerable = new BaseEntity[] { enumerable.ElementAt(index) };
                        }
                    }

                        int count = enumerable.Count();
                        switch (count)
                        {
                            case 0:
                                if (lastPart)
                                {
                                    MethodInfo addMethod = collectionType.GetMethod("Add");
                                    BaseEntity newEntity = CreateFrom(stringValue, elementType);
                                    addMethod.Invoke(collection, new object[] { newEntity });
                                    return true;
                                }
                                else
                                {
                                    Console.WriteLine("No result for property '{0}' on '{1}'", propertyName, target);
                                    return false;
                                }
                            case 1:
                                target = enumerable.First();
                                break;
                            default:
                                Console.WriteLine("Too much result for property '{0}' on '{1}'", propertyName, target);
                                return false;
                        }
                }
                else
                {
                    // This is a property
                    if (lastPart)
                    {
                        String currentValue = "" + propertyValue;
                        if (propertyType.Equals(typeof(String)))
                        {
                            if (String.Equals(currentValue, stringValue))
                            {
                                return false;
                            }
                            propertyInfo.SetValue(target, stringValue, null);
                            return true;
                        }
                        else if (propertyType.Equals(typeof(Boolean)))
                        {
                            if (String.Equals(currentValue, stringValue, StringComparison.OrdinalIgnoreCase))
                            {
                                return false;
                            }
                            propertyInfo.SetValue(target, Boolean.Parse(stringValue), null);
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Unhandled type for property '{0}' on '{1}'", propertyName, target);
                            return false;
                        }
                    }
                    else
                    {
                        target = propertyValue;
                    }
                }
            }

            return false;
        }

        /// <summary>
        ///   Returns a <see cref = "System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///   A <see cref = "System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
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
                if (c == '[' || c == '{' || c == '(')
                {
                    nested++;
                }
                else if (c == ']' || c == '}' || c == ')')
                {
                    nested--;
                }
                i++;
            }
            return -1;
        }
    }
}