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
    public class Map
    {
        [Test]
        public void CreateEmpty()
        {
            var lambda = LambdaCompiler.Compile("map()");
            var result = lambda();
            Assert.That(result is Dictionary<string, object>, Is.True);
        }

        [Test]
        public void CreateWithInitialValues()
        {
            var lambda = LambdaCompiler.Compile(@"
map(
  'foo', 57,
  'bar', 77
)
");
            var result = lambda();
            var map = result as Dictionary<string, object>;
            Assert.That(map.Count, Is.EqualTo(2));
            Assert.That(map["foo"], Is.EqualTo(57));
            Assert.That(map["bar"], Is.EqualTo(77));
        }

        [Test]
        public void Count()
        {
            var lambda = LambdaCompiler.Compile(@"
count(map(
  'foo', 57,
  'bar', 77
))
");
            var result = lambda();
            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void RetrieveValues()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@my-map, map(
  'foo', 57,
  'bar', 77
))
get(my-map, 'foo')
");
            var result = lambda();
            Assert.That(result, Is.EqualTo(57));
        }

        [Test]
        public void Add()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@my-map, map(
  'foo', 57
))
add(my-map, 'bar', 77, 'howdy', 99)
my-map
");
            var result = lambda();
            var map = result as Dictionary<string, object>;
            Assert.That(map.Count, Is.EqualTo(3));
            Assert.That(map["foo"], Is.EqualTo(57));
            Assert.That(map["bar"], Is.EqualTo(77));
            Assert.That(map["howdy"], Is.EqualTo(99));
        }

        [Test]
        public void ComplexAdd_01()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@my-map, map(
  'foo', 57
))
add(my-map, 'bar', 77, 'howdy', 99)
add(my-map, 'world', list(1,2,3))
my-map
");
            var result = lambda();
            var map = result as Dictionary<string, object>;
            Assert.That(map.Count, Is.EqualTo(4));
            Assert.That(map["foo"], Is.EqualTo(57));
            Assert.That(map["bar"], Is.EqualTo(77));
            Assert.That(map["howdy"], Is.EqualTo(99));
            Assert.That(map["world"] is List<object>, Is.True);
            var list = map["world"] as List<object>;
            Assert.That(list.Count, Is.EqualTo(3));
            Assert.That(list[0], Is.EqualTo(1));
            Assert.That(list[1], Is.EqualTo(2));
            Assert.That(list[2], Is.EqualTo(3));
        }

        [Test]
        public void ComplexAdd_02()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@my-map, map(
  'world', list(1,2,3)
))
add(get(my-map, 'world'),4,5)
my-map
");
            var result = lambda();
            var map = result as Dictionary<string, object>;
            Assert.That(map.Count, Is.EqualTo(1));
            var list = map["world"] as List<object>;
            Assert.That(list, Is.Not.Null);
            Assert.That(list.Count, Is.EqualTo(5));
            Assert.That(list[0], Is.EqualTo(1));
            Assert.That(list[1], Is.EqualTo(2));
            Assert.That(list[2], Is.EqualTo(3));
            Assert.That(list[3], Is.EqualTo(4));
            Assert.That(list[4], Is.EqualTo(5));
        }

        [Test]
        public void ComplexAdd_03()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@my-map, map(
  'world', list(1,2,3)
))
add(
  get(my-map, 'world'),
  map(
    'foo', 'bar'))
my-map
");
            var result = lambda();
            var map = result as Dictionary<string, object>;
            Assert.That(map.Count, Is.EqualTo(1));
            var list = map["world"] as List<object>;
            Assert.That(list, Is.Not.Null);
            Assert.That(list.Count, Is.EqualTo(4));
            Assert.That(list[0], Is.EqualTo(1));
            Assert.That(list[1], Is.EqualTo(2));
            Assert.That(list[2], Is.EqualTo(3));
            var innerMap = list[3] as Dictionary<string, object>;
            Assert.That(innerMap, Is.Not.Null);
            Assert.That(innerMap["foo"], Is.EqualTo("bar"));
        }

        [Test]
        public void Each_01()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@my-map, map(
  'foo', 47,
  'bar', 10
))
var(@result, 0)
each(@ix, my-map, {
  set(@result, +(result, get(my-map, ix)))
})
result
");
            var result = lambda();
            Assert.That(result, Is.EqualTo(57));
        }
    }
}
