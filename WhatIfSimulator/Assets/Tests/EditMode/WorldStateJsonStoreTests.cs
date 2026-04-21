using System.IO;

using NUnit.Framework;
using UnityEngine;

using WhatIfSimulator.Core.Serialization;

namespace WhatIfSimulator.Tests.EditMode
{
    public class WorldStateJsonStoreTests
    {
        [Test]
        public void WorldStateJsonStore_LoadsCanonicalExample()
        {
            var store = new WorldStateJsonStore();
            var result = store.TryLoadFromFile(ProjectPath("Assets/Data/WorldStates/world-state.example.json"));

            Assert.That(result.Success, Is.True);
            Assert.That(result.WorldState.schemaVersion, Is.EqualTo("1.0.0"));
            Assert.That(result.WorldState.regions.Length, Is.GreaterThan(0));
        }

        [Test]
        public void WorldStateJsonStore_RoundTripsWithoutLosingRequiredFields()
        {
            var store = new WorldStateJsonStore();
            var loaded = store.TryLoadFromFile(ProjectPath("Assets/Data/WorldStates/world-state.example.json"));

            Assert.That(loaded.Success, Is.True);

            var firstJson = store.ToJson(loaded.WorldState, false);
            var reloaded = store.TryLoadFromJson(firstJson);
            var secondJson = store.ToJson(reloaded.WorldState, false);

            Assert.That(reloaded.Success, Is.True);
            Assert.That(secondJson, Is.EqualTo(firstJson));
        }

        [Test]
        public void WorldStateJsonStore_RejectsEmptyJson()
        {
            var store = new WorldStateJsonStore();
            var result = store.TryLoadFromJson(string.Empty);

            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorCode, Is.EqualTo("WORLDSTATE_JSON_EMPTY"));
        }

        [Test]
        public void WorldStateJsonStore_RejectsMissingSchemaVersionOrRegions()
        {
            var store = new WorldStateJsonStore();
            var missingSchemaVersion = store.TryLoadFromJson("{\"seed\":1,\"currentTimestamp\":0,\"tick\":0,\"regions\":[{\"id\":\"global\"}]}");
            var missingRegions = store.TryLoadFromJson("{\"schemaVersion\":\"1.0.0\",\"seed\":1,\"currentTimestamp\":0,\"tick\":0}");

            Assert.That(missingSchemaVersion.Success, Is.False);
            Assert.That(missingSchemaVersion.ErrorCode, Is.EqualTo("WORLDSTATE_SCHEMA_VERSION_MISSING"));
            Assert.That(missingRegions.Success, Is.False);
            Assert.That(missingRegions.ErrorCode, Is.EqualTo("WORLDSTATE_REGION_COLLECTION_EMPTY"));
        }

        private static string ProjectPath(string relativePath)
        {
            return Path.Combine(ProjectRootPath(), relativePath.Replace('/', Path.DirectorySeparatorChar));
        }

        private static string ProjectRootPath()
        {
            return Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        }
    }
}
