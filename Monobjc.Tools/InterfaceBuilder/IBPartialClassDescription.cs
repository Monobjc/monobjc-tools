//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2012 - Laurent Etiemble
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
using System.Collections.Generic;

namespace Monobjc.Tools.InterfaceBuilder
{
    /// <summary>
    ///   This subclass of <see cref = "IBArray" /> is used to carry the class description: the class name, the super class name, the outlets and the actions.
    ///   <para>A class can be described by zero or more <see cref = "IBPartialClassDescription" /> (from the IB file, from a header file, etc).</para>
    /// </summary>
    public class IBPartialClassDescription : IBArray
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "IBPartialClassDescription" /> class.
        /// </summary>
        /// <param name = "attributes">The attributes.</param>
        public IBPartialClassDescription(IDictionary<String, String> attributes)
            : base(attributes)
        {
            this.Actions = new List<IBActionDescriptor>();
            this.Outlets = new List<IBOutletDescriptor>();
        }

        /// <summary>
        ///   Gets or sets the name of the class.
        /// </summary>
        /// <value>The name of the class.</value>
        public String ClassName { get; private set; }

        /// <summary>
        ///   Gets or sets the name of the super class.
        /// </summary>
        /// <value>The name of the super class.</value>
        public String SuperClassName { get; private set; }

        /// <summary>
        ///   Gets or sets the actions.
        /// </summary>
        /// <value>The actions.</value>
        public IList<IBActionDescriptor> Actions { get; private set; }

        /// <summary>
        ///   Gets or sets the outlets.
        /// </summary>
        /// <value>The outlets.</value>
        public IList<IBOutletDescriptor> Outlets { get; private set; }

        /// <summary>
        ///   Accepts the specified visitor.
        /// </summary>
        /// <param name = "visitor">The visitor.</param>
        public override void Accept(IIBVisitor visitor)
        {
            visitor.Visit(this);
            foreach (IIBItem item in this)
            {
                item.Accept(visitor);
            }
        }

        /// <summary>
        ///   Finishes the population of this instance.
        /// </summary>
        public override void Finish(IIBReferenceResolver resolver)
        {
            base.Finish(resolver);

            IBString className = this.Find<IBString>("classname", StringComparison.OrdinalIgnoreCase);
            this.ClassName = className != null ? className.Value : String.Empty;
            IBString superclassName = this.Find<IBString>("superclassname", StringComparison.OrdinalIgnoreCase);
            this.SuperClassName = superclassName != null ? superclassName.Value : String.Empty;

            this.CollectActions(resolver);
            this.CollectOutlets(resolver);
        }

        private void CollectActions(IIBReferenceResolver resolver)
        {
            IBDictionary dict = this.Find<IBDictionary>("actions");
            if (dict == null)
            {
                return;
            }

            foreach (String key in dict.Keys)
            {
                IBString itemKey = dict[key] as IBString;
                if (itemKey == null)
                {
                    continue;
                }

                IIBItem itemValue = dict[key];
                IBReference reference = itemValue as IBReference;
                if ((resolver != null) && (reference != null))
                {
                    itemValue = resolver.ResolveReference(reference);
                }
                this.Actions.Add(new IBActionDescriptor(key, itemValue.ToString()));
            }
        }

        private void CollectOutlets(IIBReferenceResolver resolver)
        {
            IBDictionary dict = this.Find<IBDictionary>("outlets");
            if (dict == null)
            {
                return;
            }

            foreach (String key in dict.Keys)
            {
                IBString itemKey = dict[key] as IBString;
                if (itemKey == null)
                {
                    continue;
                }

                IIBItem itemValue = dict[key];
                IBReference reference = itemValue as IBReference;
                if ((resolver != null) && (reference != null))
                {
                    itemValue = resolver.ResolveReference(reference);
                }
                this.Outlets.Add(new IBOutletDescriptor(key, itemValue.ToString()));
            }
        }
    }
}