using System;

using WhatIfSimulator.Core.Models;
using WhatIfSimulator.Core.Randomness;

namespace WhatIfSimulator.Core.Simulation
{
    public sealed class SimulationTickSettings
    {
        public SimulationTickSettings(int timestampStep)
        {
            if (timestampStep <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(timestampStep), "timestampStep must be greater than zero.");
            }

            TimestampStep = timestampStep;
        }

        public int TimestampStep { get; }
    }

    public sealed class SimulationTickContext
    {
        public SimulationTickContext(int tickIndex, int currentTimestamp, int timestampStep, DeterministicRandom random)
        {
            TickIndex = tickIndex;
            CurrentTimestamp = currentTimestamp;
            TimestampStep = timestampStep;
            Random = random ?? throw new ArgumentNullException(nameof(random));
        }

        public int TickIndex { get; }

        public int CurrentTimestamp { get; }

        public int TimestampStep { get; }

        public DeterministicRandom Random { get; }
    }

    public sealed class SimulationTickResult
    {
        public SimulationTickResult(int tickIndex, int timestamp, int randomCallsConsumed)
        {
            TickIndex = tickIndex;
            Timestamp = timestamp;
            RandomCallsConsumed = randomCallsConsumed;
        }

        public int TickIndex { get; }

        public int Timestamp { get; }

        public int RandomCallsConsumed { get; }
    }

    public sealed class SimulationStepResult
    {
        public SimulationStepResult(WorldState worldState, SimulationTickResult tickResult)
        {
            WorldState = worldState ?? throw new ArgumentNullException(nameof(worldState));
            TickResult = tickResult ?? throw new ArgumentNullException(nameof(tickResult));
        }

        public WorldState WorldState { get; }

        public SimulationTickResult TickResult { get; }
    }

    public sealed class SimulationRunResult
    {
        public SimulationRunResult(WorldState finalState, Logging.SimulationTransitionLog transitionLog, DeterministicRandomSnapshot finalRandomSnapshot)
        {
            FinalState = finalState ?? throw new ArgumentNullException(nameof(finalState));
            TransitionLog = transitionLog ?? throw new ArgumentNullException(nameof(transitionLog));
            FinalRandomSnapshot = finalRandomSnapshot;
        }

        public WorldState FinalState { get; }

        public Logging.SimulationTransitionLog TransitionLog { get; }

        public DeterministicRandomSnapshot FinalRandomSnapshot { get; }
    }
}
