/*
 * Copyright (c) 2018 Thomas Hansen - thomas@gaiasoul.com
 *
 * Licensed under the terms of the MIT license, see the enclosed LICENSE
 * file for details.
 */

using NUnit.Framework;

namespace lizzie.tests
{
    public class For
    {
        [Test]
        public void ConditionInitiallyFalse()
        {
            var lambda = LambdaCompiler.Compile(@"for({
  var(@i, 0)
}, {
  null
}, {
  set(@i, +(i, 1))
}, {
  i
})");
            var result = lambda();
            Assert.IsNull(result);
        }

        [Test]
        public void IteratesUntilConditionIsNull()
        {
            var lambda = LambdaCompiler.Compile(@"for({
  var(@i, 0)
}, {
  lt(i, 3)
}, {
  set(@i, +(i, 1))
}, {
  i
})");
            var result = lambda();
            Assert.AreEqual(2, result);
        }
    }
}
