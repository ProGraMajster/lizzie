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
    /// Internal exception used to return from a function.
    /// </summary>
    public class ReturnException : Exception
    {
        /// <summary>
        /// The value associated with the return statement.
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReturnException"/> class.
        /// </summary>
        /// <param name="value">Value to return from the function.</param>
        public ReturnException(object value)
        {
            Value = value;
        }
    }
}
