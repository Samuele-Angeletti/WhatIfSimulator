using System;

namespace WhatIfSimulator.Core.Randomness
{
    public readonly struct DeterministicRandomSnapshot : IEquatable<DeterministicRandomSnapshot>
    {
        public DeterministicRandomSnapshot(int seed, uint state, int callsConsumed)
        {
            Seed = seed;
            State = state;
            CallsConsumed = callsConsumed;
        }

        public int Seed { get; }

        public uint State { get; }

        public int CallsConsumed { get; }

        public bool Equals(DeterministicRandomSnapshot other)
        {
            return Seed == other.Seed
                && State == other.State
                && CallsConsumed == other.CallsConsumed;
        }

        public override bool Equals(object obj)
        {
            return obj is DeterministicRandomSnapshot other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Seed;
                hashCode = (hashCode * 397) ^ (int)State;
                hashCode = (hashCode * 397) ^ CallsConsumed;
                return hashCode;
            }
        }
    }

    public sealed class DeterministicRandom
    {
        private const uint ZeroSeedFallback = 0x6D2B79F5u;

        private readonly int seed;
        private uint state;
        private int callsConsumed;

        public DeterministicRandom(int seed)
        {
            this.seed = seed;
            state = NormalizeSeed(seed);
        }

        public int Seed => seed;

        public int CallsConsumed => callsConsumed;

        public uint NextUInt()
        {
            state ^= state << 13;
            state ^= state >> 17;
            state ^= state << 5;
            callsConsumed++;
            return state;
        }

        public float NextFloat01()
        {
            return (NextUInt() & 0x00FFFFFFu) / 16777216f;
        }

        public int NextInt(int minInclusive, int maxExclusive)
        {
            if (maxExclusive <= minInclusive)
            {
                throw new ArgumentOutOfRangeException(nameof(maxExclusive), "maxExclusive must be greater than minInclusive.");
            }

            var range = (uint)(maxExclusive - minInclusive);
            return (int)(NextUInt() % range) + minInclusive;
        }

        public DeterministicRandomSnapshot GetSnapshot()
        {
            return new DeterministicRandomSnapshot(seed, state, callsConsumed);
        }

        private static uint NormalizeSeed(int seed)
        {
            var normalized = unchecked((uint)seed);
            return normalized == 0u ? ZeroSeedFallback : normalized;
        }
    }
}
