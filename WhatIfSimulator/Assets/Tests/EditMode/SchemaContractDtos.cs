using System;

namespace WhatIfSimulator.Tests.EditMode
{
    [Serializable]
    public class KeyValueEntryDto
    {
        public string key;
        public float value;
    }

    [Serializable]
    public class RegionStateDto
    {
        public string id;
        public string displayName;
        public int population;
        public float stability;
        public float technologyLevel;
        public KeyValueEntryDto[] resourceEntries;
    }

    [Serializable]
    public class WorldStateContractDto
    {
        public string schemaVersion;
        public int seed;
        public int currentTimestamp;
        public int tick;
        public string[] activeScenarioIds;
        public RegionStateDto[] regions;
        public KeyValueEntryDto[] globalMetrics;
    }

    [Serializable]
    public class ImpactEntryDto
    {
        public string key;
        public float value;
    }

    [Serializable]
    public class SimulationEventContractDto
    {
        public string schemaVersion;
        public string id;
        public int timestamp;
        public string eventType;
        public string description;
        public bool chunkGenerated;
        public ImpactEntryDto[] impactEntries;
        public string[] sourceScenarioIds;
    }

    [Serializable]
    public class ActionContractDto
    {
        public string schemaVersion;
        public string action;
        public string target;
        public string region;
        public float intensity;
        public string reason;
    }

    [Serializable]
    public class RemoveEventPayloadDto
    {
        public string targetEventId;
    }

    [Serializable]
    public class AddEventPayloadDto
    {
        public string eventId;
    }

    [Serializable]
    public class ModifyParameterPayloadDto
    {
        public string parameterId;
        public float value;
    }

    [Serializable]
    public class InjectRulePayloadDto
    {
        public string ruleId;
    }

    [Serializable]
    public class AlterInitialConditionsPayloadDto
    {
        public string conditionId;
    }

    [Serializable]
    public class ScenarioModificationContractDto
    {
        public string modificationType;
        public RemoveEventPayloadDto removeEvent;
        public AddEventPayloadDto addEvent;
        public ModifyParameterPayloadDto modifyParameter;
        public InjectRulePayloadDto injectRule;
        public AlterInitialConditionsPayloadDto alterInitialConditions;
    }

    [Serializable]
    public class WhatIfScenarioContractDto
    {
        public string schemaVersion;
        public string id;
        public string name;
        public string description;
        public string[] tags;
        public ScenarioModificationContractDto[] modifications;
    }

    [Serializable]
    public class ChunkEventReferenceDto
    {
        public string eventId;
        public string summary;
    }

    [Serializable]
    public class ChunkSummaryContractDto
    {
        public string schemaVersion;
        public int startTimestamp;
        public int endTimestamp;
        public string resolution;
        public ChunkEventReferenceDto[] events;
        public KeyValueEntryDto[] stateDeltaEntries;
        public string[] divergenceNotes;
        public string narrativePlaceholder;
    }
}
