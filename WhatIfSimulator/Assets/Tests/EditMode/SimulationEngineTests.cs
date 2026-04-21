using System;
using System.IO;
using System.Text;

using NUnit.Framework;
using UnityEngine;

using WhatIfSimulator.Core.Logging;
using WhatIfSimulator.Core.Models;
using WhatIfSimulator.Core.Randomness;
using WhatIfSimulator.Core.Serialization;
using WhatIfSimulator.Core.Simulation;

namespace WhatIfSimulator.Tests.EditMode
{
    public class SimulationEngineTests
    {
        [Test]
        public void SimulationEngine_AdvanceOneTick_IncrementsTickAndTimestamp()
        {
            var engine = new SimulationEngine();
            var initialState = LoadCanonicalWorldState();
            var random = new DeterministicRandom(initialState.seed);
            var logger = new InMemorySimulationStateLogger();

            var result = engine.AdvanceOneTick(
                initialState,
                new SimulationTickSettings(25),
                new ISimulationSystem[0],
                random,
                logger);

            Assert.That(result.WorldState.tick, Is.EqualTo(initialState.tick + 1));
            Assert.That(result.WorldState.currentTimestamp, Is.EqualTo(initialState.currentTimestamp + 25));
            Assert.That(logger.TransitionLog.Records.Count, Is.EqualTo(1));
            Assert.That(initialState.tick, Is.EqualTo(12));
        }

        [Test]
        public void SimulationEngine_RunTicks_LogsEveryStateTransition()
        {
            var engine = new SimulationEngine();
            var result = engine.RunTicks(
                LoadCanonicalWorldState(),
                3,
                new SimulationTickSettings(10),
                new ISimulationSystem[] { new PopulationGrowthSystem() });

            Assert.That(result.TransitionLog.Records.Count, Is.EqualTo(3));
            Assert.That(result.TransitionLog.Records[0].Tick, Is.EqualTo(13));
            Assert.That(result.TransitionLog.Records[2].Tick, Is.EqualTo(15));
            Assert.That(result.FinalState.regions[0].population, Is.EqualTo(1500300));
        }

        [Test]
        public void SimulationEngine_RunTwiceWithSameSeedAndInput_ProducesIdenticalFinalStateAndLog()
        {
            var engine = new SimulationEngine();
            var first = engine.RunTicks(
                LoadCanonicalWorldState(),
                4,
                new SimulationTickSettings(5),
                new ISimulationSystem[] { new RandomMetricSystem() });
            var second = engine.RunTicks(
                LoadCanonicalWorldState(),
                4,
                new SimulationTickSettings(5),
                new ISimulationSystem[] { new RandomMetricSystem() });

            var store = new WorldStateJsonStore();

            Assert.That(store.ToJson(first.FinalState, false), Is.EqualTo(store.ToJson(second.FinalState, false)));
            Assert.That(SerializeLog(first.TransitionLog), Is.EqualTo(SerializeLog(second.TransitionLog)));
            Assert.That(first.FinalRandomSnapshot, Is.EqualTo(second.FinalRandomSnapshot));
        }

        [Test]
        public void SimulationEngine_RunWithDifferentSeed_ProducesDifferentLogWhenSystemUsesRandomness()
        {
            var engine = new SimulationEngine();
            var firstState = LoadCanonicalWorldState();
            var secondState = LoadCanonicalWorldState();
            secondState.seed = firstState.seed + 1;

            var first = engine.RunTicks(
                firstState,
                3,
                new SimulationTickSettings(7),
                new ISimulationSystem[] { new RandomMetricSystem() });
            var second = engine.RunTicks(
                secondState,
                3,
                new SimulationTickSettings(7),
                new ISimulationSystem[] { new RandomMetricSystem() });

            Assert.That(SerializeLog(first.TransitionLog), Is.Not.EqualTo(SerializeLog(second.TransitionLog)));
        }

        [Test]
        public void SimulationEngine_InvalidTickSettings_ThrowsArgumentOutOfRange()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new SimulationTickSettings(0));
        }

        private static WorldState LoadCanonicalWorldState()
        {
            var store = new WorldStateJsonStore();
            var result = store.TryLoadFromFile(ProjectPath("Assets/Data/WorldStates/world-state.example.json"));
            Assert.That(result.Success, Is.True, result.Message);
            return result.WorldState;
        }

        private static string SerializeLog(SimulationTransitionLog transitionLog)
        {
            var builder = new StringBuilder();
            foreach (var record in transitionLog.Records)
            {
                builder.Append(record.Tick);
                builder.Append('|');
                builder.Append(record.Timestamp);
                builder.Append('|');
                builder.Append(record.WorldStateJson);
                builder.Append('|');
                builder.Append(record.RandomSnapshot.State);
                builder.Append('|');
                builder.Append(record.RandomSnapshot.CallsConsumed);
                builder.AppendLine();
            }

            return builder.ToString();
        }

        private static string ProjectPath(string relativePath)
        {
            return Path.Combine(ProjectRootPath(), relativePath.Replace('/', Path.DirectorySeparatorChar));
        }

        private static string ProjectRootPath()
        {
            return Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        }

        private sealed class PopulationGrowthSystem : ISimulationSystem
        {
            public void Apply(WorldState worldState, SimulationTickContext context)
            {
                worldState.regions[0].population += 100;
            }
        }

        private sealed class RandomMetricSystem : ISimulationSystem
        {
            public void Apply(WorldState worldState, SimulationTickContext context)
            {
                var randomPulse = context.Random.NextFloat01();
                var metricEntries = worldState.globalMetrics;
                Array.Resize(ref metricEntries, metricEntries.Length + 1);
                metricEntries[metricEntries.Length - 1] = new KeyValueEntry
                {
                    key = "randomPulse_" + context.TickIndex,
                    value = randomPulse
                };
                worldState.globalMetrics = metricEntries;
            }
        }
    }
}
