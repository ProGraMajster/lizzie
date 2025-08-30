/*
 * Copyright (c) 2018 Thomas Hansen - thomas@gaiasoul.com
 *
 * Licensed under the terms of the MIT license, see the enclosed LICENSE
 * file for details.
 */

using System;

namespace lizzie.exceptions
{
    /// <summary>
    /// Internal exception used to break out of loops.
    /// </summary>
    public class BreakException : Exception
    {
        /// <summary>
        /// The value optionally associated with the break.
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BreakException"/> class.
        /// </summary>
        /// <param name="value">Optional value to return from the loop.</param>
        public BreakException(object value)
        {
            Value = value;
        }
    }
}
