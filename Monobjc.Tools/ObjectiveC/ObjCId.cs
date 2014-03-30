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

namespace Monobjc.Tools.ObjectiveC
{
    /// <summary>
    ///   Simple wrapper for an Objective-C instsance.
    /// </summary>
    public class ObjCId
    {
        private readonly IntPtr pointer;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ObjCId" /> class.
        /// </summary>
        /// <param name = "pointer">The pointer.</param>
        public ObjCId(IntPtr pointer)
        {
            this.pointer = pointer;
        }

        /// <summary>
        ///   Gets the pointer.
        /// </summary>
        /// <value>The pointer.</value>
        public IntPtr Pointer
        {
            get { return this.pointer; }
        }

        /// <summary>
        ///   Allocs this instance.
        /// </summary>
        /// <returns></returns>
        public ObjCId Alloc()
        {
            return new ObjCId(NativeMethods.objc_msgSend_IntPtr(this.pointer, NativeMethods.register_selector("alloc")));
        }

        /// <summary>
        ///   Inits this instance.
        /// </summary>
        /// <returns></returns>
        public ObjCId Init()
        {
            return new ObjCId(NativeMethods.objc_msgSend_IntPtr(this.pointer, NativeMethods.register_selector("init")));
        }

        /// <summary>
        ///   Releases this instance.
        /// </summary>
        public void Release()
        {
            NativeMethods.objc_msgSend_Void(this.pointer, NativeMethods.register_selector("release"));
        }

        /// <summary>
        ///   Sends the message.
        /// </summary>
        /// <param name = "selector">The selector.</param>
        public IntPtr SendMessage(String selector)
        {
            return NativeMethods.objc_msgSend_IntPtr(this.pointer, NativeMethods.register_selector(selector));
        }

        /// <summary>
        ///   Sends the message.
        /// </summary>
        /// <param name = "selector">The selector.</param>
        /// <param name = "parameter1">The first parameter.</param>
        /// <returns></returns>
        public IntPtr SendMessage(String selector, IntPtr parameter1)
        {
            return NativeMethods.objc_msgSend_IntPtr_IntPtr(this.pointer, NativeMethods.register_selector(selector), parameter1);
        }

        /// <summary>
        ///   Sends the message.
        /// </summary>
        /// <param name = "selector">The selector.</param>
        /// <param name = "parameter1">The first parameter.</param>
        /// <returns></returns>
        public IntPtr SendMessage(String selector, String parameter1)
        {
            return NativeMethods.objc_msgSend_IntPtr_String(this.pointer, NativeMethods.register_selector(selector), parameter1);
        }

        /// <summary>
        ///   Get the select of the specified name.
        /// </summary>
        /// <param name = "name">The name.</param>
        public static IntPtr Selector(String name)
        {
            return NativeMethods.register_selector(name);
        }
    }
}