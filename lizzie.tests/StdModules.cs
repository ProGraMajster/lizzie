using System;
using System.Threading.Tasks;
using NUnit.Framework;
using lizzie.Runtime;
using lizzie.Std;

namespace lizzie.tests
{
    public class StdModules
    {
        private class TestLimiter : IResourceLimiter
        {
            public Capability? Requested { get; private set; }
            public void Enter() { }
            public void Exit() { }
            public void Tick() { }
            public void Demand(Capability capability) => Requested = capability;
        }

        [Test]
        public void TimeNowRequiresCapability()
        {
            var limiter = new TestLimiter();
            var result = Time.now(limiter);
            Assert.That(limiter.Requested, Is.EqualTo(Capability.Time));
            Assert.That(result, Is.GreaterThan(0));
        }

        [Test]
        public async Task AsyncNextTickRunsCallback()
        {
            var limiter = new TestLimiter();
            bool called = false;
            await Async.nextTick(() => called = true, limiter);
            Assert.That(called, Is.True);
            Assert.That(limiter.Requested, Is.EqualTo(Capability.Async));
        }

        [Test]
        public void RandNextIntProducesValue()
        {
            var limiter = new TestLimiter();
            Rand.seed(42, limiter);
            var value = Rand.nextInt(0, 10, limiter);
            Assert.That(limiter.Requested, Is.EqualTo(Capability.Random));
            Assert.That(value, Is.InRange(0, 9));
        }
    }
}
