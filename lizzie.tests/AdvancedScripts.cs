/*
 * Copyright (c) 2018 Thomas Hansen - thomas@gaiasoul.com
 *
 * Licensed under the terms of the MIT license, see the enclosed LICENSE
 * file for details.
 */

using NUnit.Framework;

namespace lizzie.tests
{
    public class AdvancedScripts
    {
        [Test]
        public void RecursiveFactorial()
        {
            var code = @"
var(@fact, function({
  if(lte(n, 1), {
    1
  }, {
    *(n, fact(-(n,1)))
  })
}, @n))
fact(5)";
            var lambda = LambdaCompiler.Compile(code);
            var result = lambda();
            Assert.That(result, Is.EqualTo(120));
        }

        [Test]
        public void SumUsingForeach()
        {
            var code = @"
var(@sum, 0)
foreach(@item, list(1,2,3,4,5), {
  set(@sum, +(sum, item))
})
sum";
            var lambda = LambdaCompiler.Compile(code);
            var result = lambda();
            Assert.That(result, Is.EqualTo(15));
        }

        [Test]
        public void AsyncFactorialTask()
        {
            var code = @"
var(@fact, function({
  if(lte(n, 1), {
    1
  }, {
    *(n, fact(-(n,1)))
  })
}, @n))
var(@t, task({
  fact(5)
}))
await(t)";
            var lambda = LambdaCompiler.Compile(code);
            var result = lambda();
            Assert.That(result, Is.EqualTo(120));
        }
    }
}
