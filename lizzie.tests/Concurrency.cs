/*
 * Copyright (c) 2018 Thomas Hansen - thomas@gaiasoul.com
 *
 * Licensed under the terms of the MIT license, see the enclosed LICENSE
 * file for details.
 */

using NUnit.Framework;

namespace lizzie.tests
{
    public class Concurrency
    {
        [Test]
        public void TaskReturnsValue()
        {
            var code = @"
var(@t, task({
  57
}))
await(t)
";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<LambdaCompiler.Nothing>(tokenizer, code);
            var ctx = new LambdaCompiler.Nothing();
            var binder = new Binder<LambdaCompiler.Nothing>();
            binder["var"] = Functions<LambdaCompiler.Nothing>.Var;
            binder["task"] = Functions<LambdaCompiler.Nothing>.Task;
            binder["await"] = Functions<LambdaCompiler.Nothing>.Await;
            var result = function(ctx, binder);
            Assert.That(result, Is.EqualTo(57));
        }

        [Test]
        public void ThreadSetsValue()
        {
            var code = @"
var(@result, 0)
var(@th, thread({
  set(@result, 57)
}))
join(th)
result
";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<LambdaCompiler.Nothing>(tokenizer, code);
            var ctx = new LambdaCompiler.Nothing();
            var binder = new Binder<LambdaCompiler.Nothing>();
            binder["var"] = Functions<LambdaCompiler.Nothing>.Var;
            binder["set"] = Functions<LambdaCompiler.Nothing>.Set;
            binder["thread"] = Functions<LambdaCompiler.Nothing>.Thread;
            binder["join"] = Functions<LambdaCompiler.Nothing>.Join;
            var result = function(ctx, binder);
            Assert.That(result, Is.EqualTo(57));
        }
    }
}

