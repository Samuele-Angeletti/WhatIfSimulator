using System;
using System.Collections.Generic;
using System.IO;

using UnityEditor;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;

using ApiTestMode = UnityEditor.TestTools.TestRunner.Api.TestMode;

namespace WhatIfSimulator.EditorTools
{
    public static class CommandLineEditModeTestRunner
    {
        public static void Run()
        {
            var resultsPath = ResolveResultsPath();
            var callbacks = new RunCallbacks(resultsPath);
            var testRunnerApi = ScriptableObject.CreateInstance<TestRunnerApi>();
            var executionSettings = new ExecutionSettings(new Filter
            {
                assemblyNames = new[] { "WhatIfSimulator.EditModeTests" },
                testMode = ApiTestMode.EditMode
            })
            {
                runSynchronously = true
            };

            testRunnerApi.RegisterCallbacks(callbacks);
            testRunnerApi.Execute(executionSettings);

            if (!callbacks.Completed)
            {
                callbacks.WriteUnexpectedFailure("Runner did not complete synchronously.");
                EditorApplication.Exit(1);
            }
        }

        private static string ResolveResultsPath()
        {
            var commandLineArgs = Environment.GetCommandLineArgs();

            for (var index = 0; index < commandLineArgs.Length - 1; index++)
            {
                if (string.Equals(commandLineArgs[index], "-utfResults", StringComparison.OrdinalIgnoreCase))
                {
                    return commandLineArgs[index + 1];
                }
            }

            return Path.Combine(Directory.GetCurrentDirectory(), "EditModeResults.json");
        }

        private sealed class RunCallbacks : ICallbacks
        {
            private readonly string resultsPath;
            private readonly List<FailureSummary> failures = new List<FailureSummary>();

            public RunCallbacks(string resultsPath)
            {
                this.resultsPath = resultsPath;
            }

            public bool Completed { get; private set; }

            public void RunStarted(ITestAdaptor testsToRun)
            {
                Debug.Log($"Running EditMode tests in assembly {testsToRun.Name}.");
            }

            public void RunFinished(ITestResultAdaptor result)
            {
                Completed = true;

                var summary = new RunSummary
                {
                    total = result.PassCount + result.FailCount + result.SkipCount + result.InconclusiveCount,
                    passed = result.PassCount,
                    failed = result.FailCount,
                    skipped = result.SkipCount,
                    inconclusive = result.InconclusiveCount,
                    failures = failures.ToArray()
                };

                var directory = Path.GetDirectoryName(resultsPath);
                if (!string.IsNullOrWhiteSpace(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(resultsPath, JsonUtility.ToJson(summary, true));
                Debug.Log($"Finished EditMode tests. Failed: {summary.failed}. Results written to {resultsPath}");
                EditorApplication.Exit(summary.failed > 0 ? 1 : 0);
            }

            public void TestStarted(ITestAdaptor test)
            {
            }

            public void TestFinished(ITestResultAdaptor result)
            {
                if (result.TestStatus != TestStatus.Failed)
                {
                    return;
                }

                failures.Add(new FailureSummary
                {
                    name = result.FullName,
                    message = result.Message,
                    stackTrace = result.StackTrace
                });
            }

            public void WriteUnexpectedFailure(string message)
            {
                var summary = new RunSummary
                {
                    total = 0,
                    passed = 0,
                    failed = 1,
                    skipped = 0,
                    inconclusive = 0,
                    failures = new[]
                    {
                        new FailureSummary
                        {
                            name = "CommandLineEditModeTestRunner",
                            message = message,
                            stackTrace = string.Empty
                        }
                    }
                };

                File.WriteAllText(resultsPath, JsonUtility.ToJson(summary, true));
            }
        }

        [Serializable]
        private sealed class RunSummary
        {
            public int total;
            public int passed;
            public int failed;
            public int skipped;
            public int inconclusive;
            public FailureSummary[] failures;
        }

        [Serializable]
        private sealed class FailureSummary
        {
            public string name;
            public string message;
            public string stackTrace;
        }
    }
}
