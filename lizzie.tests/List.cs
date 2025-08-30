/*
 * Copyright (c) 2018 Thomas Hansen - thomas@gaiasoul.com
 *
 * Licensed under the terms of the MIT license, see the enclosed LICENSE
 * file for details.
 */

using System.Collections.Generic;
using NUnit.Framework;
using lizzie.tests.context_types;

namespace lizzie.tests
{
    public class List
    {
        [Test]
        public void CreateList()
        {
            var lambda = LambdaCompiler.Compile("list(57, 67, 77)");
            var result = lambda();
            Assert.That(result is List<object>, Is.True);
            var list = result as List<object>;
            Assert.That(list.Count, Is.EqualTo(3));
            Assert.That(list[0], Is.EqualTo(57));
            Assert.That(list[1], Is.EqualTo(67));
            Assert.That(list[2], Is.EqualTo(77));
        }

        [Test]
        public void CountListContent()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@foo, list(57, 67, 77))
count(foo)");
            var result = lambda();
            Assert.That(result, Is.EqualTo(3));
        }

        [Test]
        public void GetListValue()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@foo, list(57, 67, 77))
get(foo, 2)");
            var result = lambda();
            Assert.That(result, Is.EqualTo(77));
        }

        [Test]
        public void AddToList()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@foo, list(57))
add(foo, 67, 77)
foo");
            var result = lambda();
            Assert.That(result is List<object>, Is.True);
            var list = result as List<object>;
            Assert.That(list.Count, Is.EqualTo(3));
            Assert.That(list[0], Is.EqualTo(57));
            Assert.That(list[1], Is.EqualTo(67));
            Assert.That(list[2], Is.EqualTo(77));
        }

        [Test]
        public void SliceList_01()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@foo, list(57, 67, 77, 87))
slice(foo, 1, 3)");
            var result = lambda();
            Assert.That(result is List<object>, Is.True);
            var list = result as List<object>;
            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list[0], Is.EqualTo(67));
            Assert.That(list[1], Is.EqualTo(77));
        }

        [Test]
        public void SliceList_02()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@foo, list(57, 67, 77, 87))
slice(foo, 1)");
            var result = lambda();
            Assert.That(result is List<object>, Is.True);
            var list = result as List<object>;
            Assert.That(list.Count, Is.EqualTo(3));
            Assert.That(list[0], Is.EqualTo(67));
            Assert.That(list[1], Is.EqualTo(77));
            Assert.That(list[2], Is.EqualTo(87));
        }

        [Test]
        public void SliceList_03()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@foo, list(57, 67, 77, 87))
slice(foo)");
            var result = lambda();
            Assert.That(result is List<object>, Is.True);
            var list = result as List<object>;
            Assert.That(list.Count, Is.EqualTo(4));
            Assert.That(list[0], Is.EqualTo(57));
            Assert.That(list[1], Is.EqualTo(67));
            Assert.That(list[2], Is.EqualTo(77));
            Assert.That(list[3], Is.EqualTo(87));
        }

        [Test]
        public void SliceList_04()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@foo, list(57, 67, 77, 87))
slice(foo, 1, 1)");
            var result = lambda();
            Assert.That(result is List<object>, Is.True);
            var list = result as List<object>;
            Assert.That(list.Count, Is.EqualTo(0));
        }

        [Test]
        public void Each_01()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@foo, list(57, 67, 77, 87))
var(@bar, list())
each(@ix, foo, {
  if(any(@eq(57, ix), @eq(77, ix)), {
    add(bar, ix)
  })
})
bar");
            var result = lambda();
            Assert.That(result is List<object>, Is.True);
            var list = result as List<object>;
            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list[0], Is.EqualTo(57));
            Assert.That(list[1], Is.EqualTo(77));
        }

        [Test]
        public void Each_02()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@foo, list(57, 67, 77, 87))
var(@bar, list())
each(@ix, foo, {
  if(any(@eq(57, ix), @eq(77, ix)), {
    add(bar, string(ix))
  })
})
bar");
            var result = lambda();
            Assert.That(result is List<object>, Is.True);
            var list = result as List<object>;
            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list[0], Is.EqualTo("57"));
            Assert.That(list[1], Is.EqualTo("77"));
        }

        [Test]
        public void Each_03()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@foo, list(""57"", ""67"", ""77"", ""88.88"", ""97""))
var(@bar, list())
each(@ix, foo, {
  if(any(@eq(""57"", ix), @eq(""77"", ix), @eq(""88.88"", ix)), {
    add(bar, number(ix))
  })
})
bar");
            var result = lambda();
            Assert.That(result is List<object>, Is.True);
            var list = result as List<object>;
            Assert.That(list.Count, Is.EqualTo(3));
            Assert.That(list[0], Is.EqualTo(57));
            Assert.That(list[1], Is.EqualTo(77));
            Assert.That(list[2], Is.EqualTo(88.88));
        }

        [Test]
        public void Each_04()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@foo, list(57, 67, 77))
var(@bar, each(@ix, foo, {
  +(ix, 10)
}))
bar");
            var result = lambda();
            Assert.That(result is List<object>, Is.True);
            var list = result as List<object>;
            Assert.That(list.Count, Is.EqualTo(3));
            Assert.That(list[0], Is.EqualTo(67));
            Assert.That(list[1], Is.EqualTo(77));
            Assert.That(list[2], Is.EqualTo(87));
        }

        [Test]
        public void ApplyArgumentsToAdd()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@foo, +(apply(list(57,10,10))))
");
            var result = lambda();
            Assert.That(result, Is.EqualTo(77));
        }
    }
}
