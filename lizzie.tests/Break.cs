/*
 * Copyright (c) 2018 Thomas Hansen - thomas@gaiasoul.com
 *
 * Licensed under the terms of the MIT license, see the enclosed LICENSE
 * file for details.
 */

using NUnit.Framework;
using lizzie.exceptions;

namespace lizzie.tests
{
    public class Break
    {
        [Test]
        public void BreaksOutOfLoop()
        {
            var lambda = LambdaCompiler.Compile(@"var(@i,0)
while({
  lt(i,10)
},{
  if(eq(i,5), { break(i) })
  set(@i, +(i,1))
})");
            var result = lambda();
            Assert.AreEqual(5, result);
        }

        [Test]
        public void BreakOutsideLoopThrows()
        {
            var lambda = LambdaCompiler.Compile("break()");
            var success = false;
            try {
                lambda();
            } catch (LizzieRuntimeException) {
                success = true;
            }
            Assert.AreEqual(true, success);
        }
    }
}
