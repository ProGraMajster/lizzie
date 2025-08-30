/*
 * Copyright (c) 2018 Thomas Hansen - thomas@gaiasoul.com
 *
 * Licensed under the terms of the MIT license, see the enclosed LICENSE
 * file for details.
 */

using NUnit.Framework;
using lizzie.tests.context_types;
using lizzie.exceptions;

namespace lizzie.tests
{
    public class Functions
    {
        [Test]
        public void ReturnsNumberConstant()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@foo, function({
  57
}))
foo()");
            var result = lambda();
            Assert.That(result, Is.EqualTo(57));
        }

        [Test]
        public void ReturnsStringConstant()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@foo, function({
  ""Hello World""
}))
foo()");
            var result = lambda();
            Assert.That(result, Is.EqualTo("Hello World"));
        }

        [Test]
        public void SingleParameter()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@foo, function({
  +(""Hello "", input)
}, @input))
foo(""Thomas"")");
            var result = lambda();
            Assert.That(result, Is.EqualTo("Hello Thomas"));
        }

        [Test]
        public void MultipleParameters()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@foo, function({
  +(""Hello "", name, "" it seems you are "", old, "" years old"")
}, @name, @old))
foo(""Thomas"", 44)");
            var result = lambda();
            Assert.That(result, Is.EqualTo("Hello Thomas it seems you are 44 years old"));
        }

        [Test]
        public void EvaluateFunctionFromWithinFunction()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@bar, function({
  77
}))
var(@foo, function({
  bar()
}))
foo()");
            var result = lambda();
            Assert.That(result, Is.EqualTo(77));
        }

        [Test]
        public void ChangeStackFromWithinFunction()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@bar, function({
  var(@arg, 50)
}))
var(@foo, function({
  var(@arg, 27)
  +(arg, bar())
}))
foo()");
            var result = lambda();
            Assert.That(result, Is.EqualTo(77));
        }

        [Test]
        public void VariableDeclaredWithinFunctionDoesNotExistThrows()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@foo, function({
  var(@bar, 50)
}))
foo()
bar");
            var success = false;
            try {
                lambda();
            } catch(LizzieRuntimeException) {
                success = true;
            }
            Assert.That(success, Is.True);
        }

        [Test]
        public void NestedFunctions()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@func1, function({
  var(@func2, function({
    +(bar2, '_yup')
  }, @bar2))
  func2(bar1)
}, @bar1))

func1('success')
");
            var result = lambda();
            Assert.That(result, Is.EqualTo("success_yup"));
        }

        [Test]
        public void NestedFunctionsThrows()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@func1, function({
  var(@func2, function({
    bar2
  }, @bar2))
  func2(bar1)
}, @bar1))

func2('success')
");
            var success = false;
            try {
                lambda();
            } catch(LizzieRuntimeException) {
                success = true;
            }
            Assert.That(success, Is.True);
        }

        [Test]
        public void TwoFunctions()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@func1, function({
  +('success1_',bar1)
}, @bar1))
var(@func2, function({
  +('_success2_',bar2)
}, @bar2))
+(func1('foo'), func2('bar'))
");
            var result = lambda();
            Assert.That(result, Is.EqualTo("success1_foo_success2_bar"));
        }

        [Test]
        public void NoCode()
        {
            var lambda = LambdaCompiler.Compile("");
            var result = lambda();
            Assert.That(result, Is.Null);
        }

        [Test]
        public void EmptyFunction()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@func, function({
}))
func()
");
            var result = lambda();
            Assert.That(result, Is.Null);
        }
    }
}
