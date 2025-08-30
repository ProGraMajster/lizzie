/*
 * Copyright (c) 2018 Thomas Hansen - thomas@gaiasoul.com
 *
 * Licensed under the terms of the MIT license, see the enclosed LICENSE
 * file for details.
 */

using System.Collections.Generic;
using NUnit.Framework;
using lizzie.exceptions;

namespace lizzie.tests
{
    public class TokenizerTests
    {
        [Test]
        public void FunctionInvocationInteger()
        {
            var code = "a(1)";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var list = new List<string>(tokenizer.Tokenize(code));
            Assert.That(list.Count, Is.EqualTo(4));
        }

        [Test]
        public void FunctionInvocationString()
        {
            var code = @"a(""foo"")";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var list = new List<string>(tokenizer.Tokenize(code));
            Assert.That(list.Count, Is.EqualTo(6));
        }

        [Test]
        public void FunctionInvocationMixed()
        {
            var code = @"a(""foo"", 5, ""bar"")";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var list = new List<string>(tokenizer.Tokenize(code));
            Assert.That(list.Count, Is.EqualTo(12));
        }

        [Test]
        public void FunctionInvocationMixedNested()
        {
            var code = @"a(""foo"", bar(5), ""bar"")";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var list = new List<string>(tokenizer.Tokenize(code));
            Assert.That(list.Count, Is.EqualTo(15));
        }

        [Test]
        public void WeirdSpacing()
        {
            var code = @"  a (   ""\""fo\""o""   ,bar(  5 )     ,""bar""   )   ";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var list = new List<string>(tokenizer.Tokenize(code));
            Assert.That(list.Count, Is.EqualTo(15));
        }

        [Test]
        public void SingleLineComment()
        {
            var code = @"a(""foo"", bar(5)) // , ""bar"")";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var list = new List<string>(tokenizer.Tokenize(code));
            Assert.That(list.Count, Is.EqualTo(11));
        }

        [Test]
        public void MultiLineComment()
        {
            var code = @"
a(""foo"" /* comment */,
     bar(5)/* FOO!! *** */ ) // , ""bar"")
   /* 
 * hello
 */
jo()";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var list = new List<string>(tokenizer.Tokenize(code));
            Assert.That(list.Count, Is.EqualTo(14));
        }

        [Test]
        public void MultiLineCommentThrows()
        {
            var code = @"/*/";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var success = false;
            try {
                var list = new List<string>(tokenizer.Tokenize(code));
            } catch (LizzieTokenizerException) {
                success = true;
            }
            Assert.That(success, Is.True);
        }

        [Test]
        public void EmptyMultiLineComment()
        {
            var code = @"/**/";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var list = new List<string>(tokenizer.Tokenize(code));
            Assert.That(list.Count, Is.EqualTo(0));
        }
    }
}
