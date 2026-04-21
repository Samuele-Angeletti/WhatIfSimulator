using System;

namespace WhatIfSimulator.Core.Models
{
    [Serializable]
    public class KeyValueEntry
    {
        public string key;
        public float value;
    }

    [Serializable]
    public class RegionState
    {
        public string id;
        public string displayName;
        public int population;
        public float stability;
        public float technologyLevel;
        public KeyValueEntry[] resourceEntries;
    }

    [Serializable]
    public class WorldState
    {
        public string schemaVersion;
        public int seed;
        public int currentTimestamp;
        public int tick;
        public string[] activeScenarioIds;
        public RegionState[] regions;
        public KeyValueEntry[] globalMetrics;
    }
}
