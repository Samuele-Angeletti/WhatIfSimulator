using System.IO;
using NUnit.Framework;
using UnityEngine;

namespace WhatIfSimulator.Tests.EditMode
{
    public class SchemaContractExamplesTests
    {
        [Test]
        public void RequiredContractFilesExist()
        {
            Assert.That(File.Exists(ProjectPath("Assets/Data/Contracts/WorldState.schema.md")), Is.True);
            Assert.That(File.Exists(ProjectPath("Assets/Data/Contracts/SimulationEvent.schema.md")), Is.True);
            Assert.That(File.Exists(ProjectPath("Assets/Data/Contracts/Action.schema.md")), Is.True);
            Assert.That(File.Exists(ProjectPath("Assets/Data/Contracts/WhatIfScenario.schema.md")), Is.True);
            Assert.That(File.Exists(ProjectPath("Assets/Data/Contracts/ChunkSummary.schema.md")), Is.True);
            Assert.That(File.Exists(ProjectPath("Assets/Data/Contracts/SchemaValidationRules.md")), Is.True);
        }

        [Test]
        public void ContractDocumentsContainRequiredSections()
        {
            AssertDocumentContainsSections(ProjectPath("Assets/Data/Contracts/WorldState.schema.md"));
            AssertDocumentContainsSections(ProjectPath("Assets/Data/Contracts/SimulationEvent.schema.md"));
            AssertDocumentContainsSections(ProjectPath("Assets/Data/Contracts/Action.schema.md"));
            AssertDocumentContainsSections(ProjectPath("Assets/Data/Contracts/WhatIfScenario.schema.md"));
            AssertDocumentContainsSections(ProjectPath("Assets/Data/Contracts/ChunkSummary.schema.md"));
        }

        [Test]
        public void WorldStateExample_DeserializesAndContainsRequiredMetadata()
        {
            var dto = JsonUtility.FromJson<WorldStateContractDto>(ReadProjectFile("Assets/Data/WorldStates/world-state.example.json"));

            Assert.That(dto.schemaVersion, Is.Not.Empty);
            Assert.That(dto.tick, Is.GreaterThanOrEqualTo(0));
            Assert.That(dto.regions, Is.Not.Null.And.Length.GreaterThan(0));
        }

        [Test]
        public void SimulationEventExample_UsesImpactEntriesInsteadOfRawMap()
        {
            var path = ProjectPath("Assets/Data/Events/simulation-event.example.json");
            var json = File.ReadAllText(path);
            var dto = JsonUtility.FromJson<SimulationEventContractDto>(json);

            Assert.That(dto.impactEntries, Is.Not.Null.And.Length.GreaterThan(0));
            Assert.That(dto.description, Is.Not.Empty);
            Assert.That(json.Contains("\"impact\":"), Is.False);
        }

        [Test]
        public void ActionExample_KeepsIntensityWithinNormalizedRange()
        {
            var dto = JsonUtility.FromJson<ActionContractDto>(ReadProjectFile("Assets/Data/Actions/action.example.json"));

            Assert.That(dto.action, Is.Not.Empty);
            Assert.That(dto.target, Is.Not.Empty);
            Assert.That(dto.region, Is.Not.Empty);
            Assert.That(dto.intensity, Is.InRange(0f, 1f));
        }

        [Test]
        public void WhatIfScenarioExample_UsesKnownModificationTypeAndSinglePayload()
        {
            var dto = JsonUtility.FromJson<WhatIfScenarioContractDto>(ReadProjectFile("Assets/Data/WhatIf/no-dinosaur-extinction.example.json"));

            Assert.That(dto.modifications, Is.Not.Null.And.Length.GreaterThan(0));

            foreach (var modification in dto.modifications)
            {
                Assert.That(IsSupportedModificationType(modification.modificationType), Is.True);
                Assert.That(CountPayloads(modification), Is.EqualTo(1));
            }
        }

        [Test]
        public void ChunkSummaryExample_ContainsFastForwardSummaryFields()
        {
            var dto = JsonUtility.FromJson<ChunkSummaryContractDto>(ReadProjectFile("Assets/Data/ChunkSummaries/chunk-summary.example.json"));

            Assert.That(dto.endTimestamp, Is.GreaterThan(dto.startTimestamp));
            Assert.That(dto.events, Is.Not.Null);
            Assert.That(dto.stateDeltaEntries, Is.Not.Null);
            Assert.That(dto.narrativePlaceholder, Is.Not.Empty);
        }

        private static void AssertDocumentContainsSections(string path)
        {
            var content = File.ReadAllText(path);

            Assert.That(content.Contains("## Required fields"), Is.True);
            Assert.That(content.Contains("## Optional fields"), Is.True);
            Assert.That(content.Contains("## Constraints"), Is.True);
            Assert.That(content.Contains("## Determinism"), Is.True);
            Assert.That(content.Contains("## JsonUtility"), Is.True);
        }

        private static int CountPayloads(ScenarioModificationContractDto modification)
        {
            var payloadCount = 0;
            payloadCount += modification.removeEvent != null && !string.IsNullOrWhiteSpace(modification.removeEvent.targetEventId) ? 1 : 0;
            payloadCount += modification.addEvent != null && !string.IsNullOrWhiteSpace(modification.addEvent.eventId) ? 1 : 0;
            payloadCount += modification.modifyParameter != null && !string.IsNullOrWhiteSpace(modification.modifyParameter.parameterId) ? 1 : 0;
            payloadCount += modification.injectRule != null && !string.IsNullOrWhiteSpace(modification.injectRule.ruleId) ? 1 : 0;
            payloadCount += modification.alterInitialConditions != null && !string.IsNullOrWhiteSpace(modification.alterInitialConditions.conditionId) ? 1 : 0;
            return payloadCount;
        }

        private static bool IsSupportedModificationType(string modificationType)
        {
            return modificationType == "remove_event"
                || modificationType == "add_event"
                || modificationType == "modify_parameter"
                || modificationType == "inject_rule"
                || modificationType == "alter_initial_conditions";
        }

        private static string ReadProjectFile(string relativePath)
        {
            return File.ReadAllText(ProjectPath(relativePath));
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
