/*
 * Copyright (c) 2018 Thomas Hansen - thomas@gaiasoul.com
 *
 * Licensed under the terms of the MIT license, see the enclosed LICENSE
 * file for details.
 */

using NUnit.Framework;

namespace lizzie.tests
{
    public class Foreach
    {
        [Test]
        public void EmptyListReturnsNull()
        {
            var lambda = LambdaCompiler.Compile(@"foreach(@item, list(), {
  item
})");
            var result = lambda();
            Assert.IsNull(result);
        }

        [Test]
        public void IteratesOverList()
        {
            var lambda = LambdaCompiler.Compile(@"foreach(@item, list(1,2,3), {
  item
})");
            var result = lambda();
            Assert.AreEqual(3, result);
        }
    }
}
