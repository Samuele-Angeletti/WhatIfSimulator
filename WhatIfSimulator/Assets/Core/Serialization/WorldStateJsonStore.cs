using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

using WhatIfSimulator.Core.Models;

namespace WhatIfSimulator.Core.Serialization
{
    public sealed class WorldStateLoadResult
    {
        private WorldStateLoadResult(bool success, WorldState worldState, string errorCode, string message)
        {
            Success = success;
            WorldState = worldState;
            ErrorCode = errorCode;
            Message = message;
        }

        public bool Success { get; }

        public WorldState WorldState { get; }

        public string ErrorCode { get; }

        public string Message { get; }

        public static WorldStateLoadResult FromSuccess(WorldState worldState)
        {
            return new WorldStateLoadResult(true, worldState, string.Empty, string.Empty);
        }

        public static WorldStateLoadResult FromFailure(string errorCode, string message)
        {
            return new WorldStateLoadResult(false, null, errorCode, message);
        }
    }

    public sealed class WorldStateJsonStore
    {
        public WorldStateLoadResult TryLoadFromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return WorldStateLoadResult.FromFailure("WORLDSTATE_JSON_EMPTY", "WorldState JSON cannot be empty.");
            }

            WorldState worldState;

            try
            {
                worldState = JsonUtility.FromJson<WorldState>(json);
            }
            catch (Exception exception)
            {
                return WorldStateLoadResult.FromFailure("WORLDSTATE_DESERIALIZATION_FAILED", exception.Message);
            }

            if (worldState == null)
            {
                return WorldStateLoadResult.FromFailure("WORLDSTATE_DESERIALIZATION_FAILED", "JsonUtility returned null for the provided payload.");
            }

            Normalize(worldState);

            if (string.IsNullOrWhiteSpace(worldState.schemaVersion))
            {
                return WorldStateLoadResult.FromFailure("WORLDSTATE_SCHEMA_VERSION_MISSING", "schemaVersion is required.");
            }

            if (worldState.tick < 0)
            {
                return WorldStateLoadResult.FromFailure("SCHEMA_INVALID_RANGE", "tick must be greater than or equal to zero.");
            }

            if (worldState.regions.Length == 0)
            {
                return WorldStateLoadResult.FromFailure("WORLDSTATE_REGION_COLLECTION_EMPTY", "At least one region is required.");
            }

            var regionIds = new HashSet<string>(StringComparer.Ordinal);

            for (var index = 0; index < worldState.regions.Length; index++)
            {
                var region = worldState.regions[index];
                if (region == null)
                {
                    return WorldStateLoadResult.FromFailure("SCHEMA_REQUIRED_FIELD_MISSING", "Region entries cannot be null.");
                }

                region.resourceEntries = region.resourceEntries ?? Array.Empty<KeyValueEntry>();

                if (string.IsNullOrWhiteSpace(region.id))
                {
                    return WorldStateLoadResult.FromFailure("SCHEMA_REQUIRED_FIELD_MISSING", "Each region requires a non-empty id.");
                }

                if (!regionIds.Add(region.id))
                {
                    return WorldStateLoadResult.FromFailure("WORLDSTATE_DUPLICATE_REGION_ID", $"Duplicate region id '{region.id}' detected.");
                }
            }

            return WorldStateLoadResult.FromSuccess(worldState);
        }

        public WorldStateLoadResult TryLoadFromFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return WorldStateLoadResult.FromFailure("WORLDSTATE_JSON_EMPTY", "A valid path is required.");
            }

            if (!File.Exists(path))
            {
                return WorldStateLoadResult.FromFailure("SCHEMA_REFERENCE_NOT_FOUND", $"WorldState file '{path}' was not found.");
            }

            return TryLoadFromJson(File.ReadAllText(path));
        }

        public string ToJson(WorldState worldState, bool prettyPrint)
        {
            if (worldState == null)
            {
                throw new ArgumentNullException(nameof(worldState));
            }

            var normalized = Clone(worldState);
            return JsonUtility.ToJson(normalized, prettyPrint);
        }

        public void SaveToFile(string path, WorldState worldState, bool prettyPrint)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("A valid path is required.", nameof(path));
            }

            File.WriteAllText(path, ToJson(worldState, prettyPrint));
        }

        public WorldState Clone(WorldState worldState)
        {
            if (worldState == null)
            {
                throw new ArgumentNullException(nameof(worldState));
            }

            var json = JsonUtility.ToJson(worldState, false);
            var clone = JsonUtility.FromJson<WorldState>(json);
            Normalize(clone);
            return clone;
        }

        private static void Normalize(WorldState worldState)
        {
            worldState.activeScenarioIds = worldState.activeScenarioIds ?? Array.Empty<string>();
            worldState.regions = worldState.regions ?? Array.Empty<RegionState>();
            worldState.globalMetrics = worldState.globalMetrics ?? Array.Empty<KeyValueEntry>();

            for (var index = 0; index < worldState.regions.Length; index++)
            {
                if (worldState.regions[index] == null)
                {
                    continue;
                }

                worldState.regions[index].resourceEntries = worldState.regions[index].resourceEntries ?? Array.Empty<KeyValueEntry>();
            }
        }
    }
}
