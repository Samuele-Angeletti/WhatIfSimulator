using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using WhatIfSimulator.Core.Randomness;

namespace WhatIfSimulator.Core.Logging
{
    public sealed class SimulationTransitionRecord
    {
        public SimulationTransitionRecord(int tick, int timestamp, string worldStateJson, DeterministicRandomSnapshot randomSnapshot)
        {
            Tick = tick;
            Timestamp = timestamp;
            WorldStateJson = worldStateJson ?? throw new ArgumentNullException(nameof(worldStateJson));
            RandomSnapshot = randomSnapshot;
        }

        public int Tick { get; }

        public int Timestamp { get; }

        public string WorldStateJson { get; }

        public DeterministicRandomSnapshot RandomSnapshot { get; }
    }

    public interface ISimulationStateLogger
    {
        void Record(SimulationTransitionRecord record);
    }

    public sealed class SimulationTransitionLog
    {
        private readonly List<SimulationTransitionRecord> records = new List<SimulationTransitionRecord>();

        public ReadOnlyCollection<SimulationTransitionRecord> Records => records.AsReadOnly();

        internal void Add(SimulationTransitionRecord record)
        {
            records.Add(record);
        }
    }

    public sealed class InMemorySimulationStateLogger : ISimulationStateLogger
    {
        private readonly SimulationTransitionLog transitionLog = new SimulationTransitionLog();

        public SimulationTransitionLog TransitionLog => transitionLog;

        public void Record(SimulationTransitionRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            transitionLog.Add(record);
        }
    }
}
