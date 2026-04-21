using System;

using WhatIfSimulator.Core.Logging;
using WhatIfSimulator.Core.Models;
using WhatIfSimulator.Core.Randomness;
using WhatIfSimulator.Core.Serialization;

namespace WhatIfSimulator.Core.Simulation
{
    public sealed class SimulationEngine
    {
        private readonly WorldStateJsonStore worldStateJsonStore = new WorldStateJsonStore();

        public SimulationStepResult AdvanceOneTick(
            WorldState worldState,
            SimulationTickSettings tickSettings,
            ISimulationSystem[] systems,
            DeterministicRandom random,
            ISimulationStateLogger logger)
        {
            if (worldState == null)
            {
                throw new ArgumentNullException(nameof(worldState));
            }

            if (tickSettings == null)
            {
                throw new ArgumentNullException(nameof(tickSettings));
            }

            if (random == null)
            {
                throw new ArgumentNullException(nameof(random));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            var nextWorldState = worldStateJsonStore.Clone(worldState);
            var orderedSystems = systems ?? Array.Empty<ISimulationSystem>();

            nextWorldState.tick += 1;
            nextWorldState.currentTimestamp += tickSettings.TimestampStep;

            var context = new SimulationTickContext(
                nextWorldState.tick,
                nextWorldState.currentTimestamp,
                tickSettings.TimestampStep,
                random);

            for (var index = 0; index < orderedSystems.Length; index++)
            {
                orderedSystems[index]?.Apply(nextWorldState, context);
            }

            var worldStateJson = worldStateJsonStore.ToJson(nextWorldState, false);
            logger.Record(new SimulationTransitionRecord(
                nextWorldState.tick,
                nextWorldState.currentTimestamp,
                worldStateJson,
                random.GetSnapshot()));

            var tickResult = new SimulationTickResult(
                nextWorldState.tick,
                nextWorldState.currentTimestamp,
                random.CallsConsumed);

            return new SimulationStepResult(nextWorldState, tickResult);
        }

        public SimulationRunResult RunTicks(
            WorldState initialState,
            int tickCount,
            SimulationTickSettings tickSettings,
            ISimulationSystem[] systems)
        {
            if (initialState == null)
            {
                throw new ArgumentNullException(nameof(initialState));
            }

            if (tickCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(tickCount), "tickCount must be greater than or equal to zero.");
            }

            var currentState = worldStateJsonStore.Clone(initialState);
            var random = new DeterministicRandom(currentState.seed);
            var logger = new InMemorySimulationStateLogger();

            for (var index = 0; index < tickCount; index++)
            {
                var stepResult = AdvanceOneTick(currentState, tickSettings, systems, random, logger);
                currentState = stepResult.WorldState;
            }

            return new SimulationRunResult(currentState, logger.TransitionLog, random.GetSnapshot());
        }
    }
}
