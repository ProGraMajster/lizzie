/*
 * Copyright (c) 2018 Thomas Hansen - thomas@gaiasoul.com
 *
 * Licensed under the terms of the MIT license, see the enclosed LICENSE
 * file for details.
 */

using NUnit.Framework;

namespace lizzie.tests
{
    public class Return
    {
        [Test]
        public void ReturnsValueAndSkipsRest()
        {
            var lambda = LambdaCompiler.Compile(@"var(@foo, function({
  return(57)
  99
}))
foo()");
            var result = lambda();
            Assert.That(result, Is.EqualTo(57));
        }

        [Test]
        public void ReturnFromRootStopsEvaluation()
        {
            var lambda = LambdaCompiler.Compile(@"return(57)
99");
            var result = lambda();
            Assert.That(result, Is.EqualTo(57));
        }
    }
}
