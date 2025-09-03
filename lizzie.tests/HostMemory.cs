using System;
using lizzie;
using lizzie.Std;
using lizzie.Runtime;
using lizzie.exceptions;
using Xunit;

namespace lizzie.tests
{
    public class HostMemory
    {
        [Fact]
        public void VariablePersistsAcrossRuns()
        {
            var memory = new DefaultVariableStore();
            var nothing = new LambdaCompiler.Nothing();

            // First run: create variable in host memory
            var binder1 = new Binder<LambdaCompiler.Nothing>(memory: memory);
            binder1["host-var"] = Host<LambdaCompiler.Nothing>.Var;
            var func1 = Compiler.Compile<LambdaCompiler.Nothing>(new Tokenizer(new LizzieTokenizer()), "host-var(@foo, 123)");
            func1(nothing, binder1);

            // Second run: verify persistence
            var binder2 = new Binder<LambdaCompiler.Nothing>(memory: memory);
            var func2 = Compiler.Compile<LambdaCompiler.Nothing>(new Tokenizer(new LizzieTokenizer()), "foo");
            var value = func2(nothing, binder2);
            Assert.Equal(123, Convert.ToInt32(value));

            // Third run: modify variable
            var binder3 = new Binder<LambdaCompiler.Nothing>(memory: memory);
            binder3["host-set"] = Host<LambdaCompiler.Nothing>.Set;
            var func3 = Compiler.Compile<LambdaCompiler.Nothing>(new Tokenizer(new LizzieTokenizer()), "host-set(@foo, 456)");
            func3(nothing, binder3);

            // Fourth run: verify modification
            var binder4 = new Binder<LambdaCompiler.Nothing>(memory: memory);
            var func4 = Compiler.Compile<LambdaCompiler.Nothing>(new Tokenizer(new LizzieTokenizer()), "foo");
            var updated = func4(nothing, binder4);
            Assert.Equal(456, Convert.ToInt32(updated));

            // Fifth run: delete variable
            var binder5 = new Binder<LambdaCompiler.Nothing>(memory: memory);
            binder5["host-del"] = Host<LambdaCompiler.Nothing>.Del;
            var func5 = Compiler.Compile<LambdaCompiler.Nothing>(new Tokenizer(new LizzieTokenizer()), "host-del(@foo)");
            func5(nothing, binder5);

            // Sixth run: ensure variable removed
            var binder6 = new Binder<LambdaCompiler.Nothing>(memory: memory);
            var func6 = Compiler.Compile<LambdaCompiler.Nothing>(new Tokenizer(new LizzieTokenizer()), "foo");
            Assert.Throws<LizzieRuntimeException>(() => func6(nothing, binder6));
        }
    }
}
