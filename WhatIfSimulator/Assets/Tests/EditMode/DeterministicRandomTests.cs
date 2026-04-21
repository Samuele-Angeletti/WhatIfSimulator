using NUnit.Framework;

using WhatIfSimulator.Core.Randomness;

namespace WhatIfSimulator.Tests.EditMode
{
    public class DeterministicRandomTests
    {
        [Test]
        public void DeterministicRandom_SameSeed_ProducesSameSequence()
        {
            var first = new DeterministicRandom(42);
            var second = new DeterministicRandom(42);

            for (var index = 0; index < 8; index++)
            {
                Assert.That(first.NextUInt(), Is.EqualTo(second.NextUInt()));
            }
        }

        [Test]
        public void DeterministicRandom_DifferentSeeds_ProduceDifferentSequence()
        {
            var first = new DeterministicRandom(42);
            var second = new DeterministicRandom(84);

            var allEqual = true;
            for (var index = 0; index < 5; index++)
            {
                allEqual &= first.NextUInt() == second.NextUInt();
            }

            Assert.That(allEqual, Is.False);
        }

        [Test]
        public void DeterministicRandom_SnapshotReflectsCallCountAndState()
        {
            var first = new DeterministicRandom(9001);
            first.NextUInt();
            first.NextFloat01();
            first.NextInt(0, 10);

            var second = new DeterministicRandom(9001);
            second.NextUInt();
            second.NextFloat01();
            second.NextInt(0, 10);

            Assert.That(first.GetSnapshot(), Is.EqualTo(second.GetSnapshot()));
            Assert.That(first.CallsConsumed, Is.EqualTo(3));
        }

        [Test]
        public void DeterministicRandom_NextFloat01_StaysWithinNormalizedRange()
        {
            var random = new DeterministicRandom(1234);

            for (var index = 0; index < 16; index++)
            {
                var value = random.NextFloat01();
                Assert.That(value, Is.GreaterThanOrEqualTo(0f));
                Assert.That(value, Is.LessThan(1f));
            }
        }
    }
}
