using System.IO;
using NUnit.Framework;
using UnityEngine;

namespace WhatIfSimulator.Tests.EditMode
{
    public class RepositoryGovernanceDocsTests
    {
        [Test]
        public void Readme_UsesCanonicalNestedUnityPaths()
        {
            var content = File.ReadAllText(RepoPath("README.md"));

            Assert.That(content.Contains("WhatIfSimulator/Assets/Core"), Is.True);
            Assert.That(content.Contains("WhatIfSimulator/Assets/Data"), Is.True);
            Assert.That(content.Contains("WhatIfSimulator/Assets/LLM"), Is.True);
            Assert.That(content.Contains("WhatIfSimulator/Assets/UI"), Is.True);
        }

        [Test]
        public void Contributing_ContainsBranchProtectionAndLibraryApprovalGuidance()
        {
            var content = File.ReadAllText(RepoPath("CONTRIBUTING.md"));

            Assert.That(content.Contains("Branch protection on `master`"), Is.True);
            Assert.That(content.Contains("Library name and version"), Is.True);
            Assert.That(content.Contains("Unity-native alternative"), Is.True);
        }

        [Test]
        public void IssueTemplatesExist_ForBugFeatureAndLibraryApproval()
        {
            Assert.That(File.Exists(RepoPath(".github/ISSUE_TEMPLATE/bug_report.md")), Is.True);
            Assert.That(File.Exists(RepoPath(".github/ISSUE_TEMPLATE/feature_request.md")), Is.True);
            Assert.That(File.Exists(RepoPath(".github/ISSUE_TEMPLATE/library_approval_request.md")), Is.True);
            Assert.That(File.Exists(RepoPath(".github/ISSUE_TEMPLATE/config.yml")), Is.True);
        }

        [Test]
        public void DataReadme_ExplainsJsonUtilityCompatibility()
        {
            var content = File.ReadAllText(ProjectPath("Assets/Data/README.md"));

            Assert.That(content.Contains("JsonUtility"), Is.True);
            Assert.That(content.Contains("explicit objects and arrays"), Is.True);
        }

        private static string RepoPath(string relativePath)
        {
            return Path.Combine(RepoRootPath(), relativePath.Replace('/', Path.DirectorySeparatorChar));
        }

        private static string ProjectPath(string relativePath)
        {
            return Path.Combine(ProjectRootPath(), relativePath.Replace('/', Path.DirectorySeparatorChar));
        }

        private static string RepoRootPath()
        {
            return Path.GetFullPath(Path.Combine(ProjectRootPath(), ".."));
        }

        private static string ProjectRootPath()
        {
            return Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        }
    }
}
