/*
 * Copyright (c) 2018 Thomas Hansen - thomas@gaiasoul.com
 *
 * Licensed under the terms of the MIT license, see the enclosed LICENSE
 * file for details.
 */

using NUnit.Framework;

namespace lizzie.tests
{
    public class While
    {
        [Test]
        public void ConditionInitiallyFalse()
        {
            var lambda = LambdaCompiler.Compile(@"while({
  null
}, {
  57
})");
            var result = lambda();
            Assert.IsNull(result);
        }

        [Test]
        public void IteratesUntilConditionIsNull()
        {
            var lambda = LambdaCompiler.Compile(@"var(@i,0)
while({
  lt(i, 3)
}, {
  set(@i, add(i, 1))
})");
            var result = lambda();
            Assert.AreEqual(3, result);
        }
    }
}

