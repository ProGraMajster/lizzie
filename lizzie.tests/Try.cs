/*
 * Copyright (c) 2018 Thomas Hansen - thomas@gaiasoul.com
 *
 * Licensed under the terms of the MIT license, see the enclosed LICENSE
 * file for details.
 */

using System.Collections.Generic;
using NUnit.Framework;

namespace lizzie.tests
{
    public class Try
    {
        [Test]
        public void TryWithoutExceptionExecutesFinally()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@catchFlag)
var(@finallyFlag)
var(@result, try({
  57
}, function({
  set(@catchFlag, 1)
  67
}, @ex), {
  set(@finallyFlag, 1)
}))
list(result, finallyFlag, catchFlag)
");
            var list = lambda() as List<object>;
            Assert.AreEqual(57, list[0]);
            Assert.AreEqual(1, list[1]);
            Assert.IsNull(list[2]);
        }

        [Test]
        public void TryCatchReceivesExceptionAndFinallyRuns()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@finallyFlag)
var(@catchMsg)
var(@result, try({
  +(1)
}, function({
  set(@catchMsg, string(ex))
  67
}, @ex), {
  set(@finallyFlag, 1)
}))
list(result, finallyFlag, catchMsg)
");
            var list = lambda() as List<object>;
            Assert.AreEqual(67, list[0]);
            Assert.AreEqual(1, list[1]);
            StringAssert.Contains("The 'add' keyword", list[2] as string);
        }
    }
}

