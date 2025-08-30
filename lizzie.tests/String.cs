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
    public class String
    {
        [Test]
        public void SubstringOnlyOffset()
        {
            var lambda = LambdaCompiler.Compile(@"substr(""foobarxyz"", 3)");
            var result = lambda();
            Assert.That(result, Is.EqualTo("barxyz"));
        }

        [Test]
        public void SubstringWithCount()
        {
            var lambda = LambdaCompiler.Compile(@"substr(""foobarxyz"", 3, 3)");
            var result = lambda();
            Assert.That(result, Is.EqualTo("bar"));
        }

        [Test]
        public void LengthOfString()
        {
            var lambda = LambdaCompiler.Compile(@"length(""foo"")");
            var result = lambda();
            Assert.That(result, Is.EqualTo(3));
        }

        [Test]
        public void Replace()
        {
            var lambda = LambdaCompiler.Compile(@"replace(""foo"", ""o"", ""xx"")");
            var result = lambda();
            Assert.That(result, Is.EqualTo("fxxxx"));
        }

        [Test]
        public void SingleQuoteStrings()
        {
            var lambda = LambdaCompiler.Compile(@"replace('foo', 'o', 'xx')");
            var result = lambda();
            Assert.That(result, Is.EqualTo("fxxxx"));
        }

        [Test]
        public void EscapedSingleQuotedString()
        {
            var lambda = LambdaCompiler.Compile(@"'foo\'bar'");
            var result = lambda();
            Assert.That(result, Is.EqualTo("foo'bar"));
        }

        [Test]
        public void ConvertFromNumber()
        {
            var lambda = LambdaCompiler.Compile(@"string(57)");
            var result = lambda();
            Assert.That(result, Is.EqualTo("57"));
        }

        [Test]
        public void ConvertToNumber()
        {
            var lambda = LambdaCompiler.Compile(@"number('57')");
            var result = lambda();
            Assert.That(result, Is.EqualTo(57));
        }

        [Test]
        public void ConvertFromStringToString()
        {
            var lambda = LambdaCompiler.Compile(@"string('57')");
            var result = lambda();
            Assert.That(result, Is.EqualTo("57"));
        }

        [Test]
        public void EscapedDoubleQuotedString()
        {
            var lambda = LambdaCompiler.Compile(@"""foo\""bar""");
            var result = lambda();
            Assert.That(result, Is.EqualTo("foo\"bar"));
        }

        [Test]
        public void JSONString_01()
        {
            var lambda = LambdaCompiler.Compile(@"
string(map(
  'foo', 57,
  'bar', 67
))
");
            var result = lambda();
            Assert.That(result, Is.EqualTo(@"{""foo"":57,""bar"":67}"));
        }

        [Test]
        public void JSONString_02()
        {
            var lambda = LambdaCompiler.Compile(@"
string(map(
  'foo', 'howdy',
  'bar', 'world'
))
");
            var result = lambda();
            Assert.That(result, Is.EqualTo(@"{""foo"":""howdy"",""bar"":""world""}"));
        }

        [Test]
        public void JSONString_03()
        {
            var lambda = LambdaCompiler.Compile(@"
string(map(
  'foo', 'howdy',
  'bar', 'wor""ld'
))
");
            var result = lambda();
            Assert.That(result, Is.EqualTo(@"{""foo"":""howdy"",""bar"":""wor\""ld""}"));
        }

        [Test]
        public void JSONString_04()
        {
            var lambda = LambdaCompiler.Compile(@"
string(list(
  'foo',
  'bar'
))
");
            var result = lambda();
            Assert.That(result, Is.EqualTo(@"[""foo"",""bar""]"));
        }

        [Test]
        public void JSONString_05()
        {
            var lambda = LambdaCompiler.Compile(@"
string(list(
  'foo',
  map(
    'bar1',57,
    'bar2',77,
    'bar3',list(1,2,map('hello','world'))
  )
))
");
            var result = lambda();
            Assert.That(result, Is.EqualTo(@"[""foo"",{""bar1"":57,""bar2"":77,""bar3"":[1,2,{""hello"":""world""}]}]"));
        }

        [Test]
        public void JSONStringToObject_01()
        {
            var lambda = LambdaCompiler.Compile(@"
json(""{'foo':57}"")
");
            var result = lambda();
            var map = result as Dictionary<string, object>;
            Assert.That(map, Is.Not.Null);
            Assert.That(map["foo"], Is.EqualTo(57));
        }

        [Test]
        public void JSONStringToObject_02()
        {
            var lambda = LambdaCompiler.Compile(@"
json(""[0,1,2]"")
");
            var result = lambda();
            var list = result as List<object>;
            Assert.That(list, Is.Not.Null);
            Assert.That(list.Count, Is.EqualTo(3));
            Assert.That(list[0], Is.EqualTo(0));
            Assert.That(list[1], Is.EqualTo(1));
            Assert.That(list[2], Is.EqualTo(2));
        }

        [Test]
        public void JSONStringToObject_03()
        {
            var lambda = LambdaCompiler.Compile(@"
json(""[0,1,{'foo':57,'bar':77,'hello':'world'}]"")
");
            var result = lambda();
            var list = result as List<object>;
            Assert.That(list, Is.Not.Null);
            Assert.That(list.Count, Is.EqualTo(3));
            Assert.That(list[0], Is.EqualTo(0));
            Assert.That(list[1], Is.EqualTo(1));
            var map = list[2] as Dictionary<string, object>;
            Assert.That(map, Is.Not.Null);
            Assert.That(map.Count, Is.EqualTo(3));
            Assert.That(map["foo"], Is.EqualTo(57));
            Assert.That(map["bar"], Is.EqualTo(77));
            Assert.That(map["hello"], Is.EqualTo("world"));
        }

        [Test]
        public void NonTerminatedStringThrows()
        {
            var success = false;
            try {
                var lambda = LambdaCompiler.Compile(@"'foo");
            } catch {
                success = true;
            }
            Assert.That(success, Is.True);
        }

        [Test]
        public void NewLineInStringThrows_01()
        {
            var success = false;
            try {
                var lambda = LambdaCompiler.Compile("'foo\n'");
            } catch {
                success = true;
            }
            Assert.That(success, Is.True);
        }

        [Test]
        public void NewLineInStringThrows_02()
        {
            var success = false;
            try {
                var lambda = LambdaCompiler.Compile("'foo\r'");
            } catch {
                success = true;
            }
            Assert.That(success, Is.True);
        }
    }
}
