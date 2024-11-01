using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViCommon.EnsureHelper.ArgumentHelpers.Extensions;
using XPlot.Plotly;

namespace ViCommon.EnsureHelper.UnitTest.Ensure.Performance
{
    [TestClass]
    public class PerformanceTest
    {
        [TestMethod]
        public void NotNullEnsureHelperPerformanceIsNotLessThanTwoTimesWorseThanDefault()
        {
            // arrange
            const int n = 1000;
            const int samples = 1000;
            const TestDataType testDataType = TestDataType.RandomWithNull;
            const bool doWarmup = true;

            // act
            var ensureHelper = new EnsureHelper();
            var defaultResults = this.GetMeasurementsResults(this.DefaultNotNullCheck,
                testDataType,
                doWarmup,
                n,
                samples);
            var ensureResults = this.GetMeasurementsResults(s => this.EnsureNotNullCheck(s, ensureHelper),
                testDataType,
                doWarmup,
                n,
                samples);

            Console.WriteLine("Default Null Check");
            Console.WriteLine(defaultResults);

            Console.WriteLine("Ensure Helper Null Check");
            Console.WriteLine(ensureResults);
            Console.WriteLine($"Ensure is {ensureResults.GetAvgFactor(defaultResults.Avg):F} times slower");

            // assert
            var factor = ensureResults.GetAvgFactor(defaultResults.Avg);
            factor.Should().BeLessThan(2);
        }

        [TestMethod]
        [Ignore("Shows the performance of the EnsureHelper. Only run this if you want to performance test the EnsureHelper.")]
        [DataRow(TestDataType.RandomWithNull, true)]

        public void NotNullPerformance(TestDataType testDataType, bool doWarmup)
        {
            const int n = 1000;
            const int samples = 1000;

            var ensureHelper = new EnsureHelper();
            var defaultResults = this.GetMeasurementsResults(this.DefaultNotNullCheck,
                testDataType,
                doWarmup,
                n,
                samples);
            var ensureResults = this.GetMeasurementsResults(s => this.EnsureNotNullCheck(s, ensureHelper),
                testDataType,
                doWarmup,
                n,
                samples);

            Console.WriteLine("Default Null Check");
            Console.WriteLine(defaultResults);

            Console.WriteLine("Ensure Helper Null Check");
            Console.WriteLine(ensureResults);

            Console.WriteLine($"Ensure is {ensureResults.GetAvgFactor(defaultResults.Avg):F} times slower");

            var scatterChart = Chart.Plot(
                new List<Graph.Scatter>()
                {
                    new Graph.Scatter()
                    {
                        y = defaultResults.Measurements,
                        name = "default"
                    },
                    new Graph.Scatter()
                    {
                        y = ensureResults.Measurements,
                        name = "ensure"
                    }
                }
            );

            var boxChart = Chart.Plot(new List<Graph.Box>
            {
                new Graph.Box
                {
                    y = defaultResults.Measurements,
                    name = "Default",
                    boxpoints = "all",
                },
                new Graph.Box
                {
                    y = ensureResults.Measurements,
                    name = "Ensure",
                    boxpoints = "all"
                },
            });

            Chart.ShowAll(new List<PlotlyChart>() { scatterChart, boxChart });
        }

        private void DefaultNotNullCheck(string s)
        {
            try
            {
                if (s == null)
                {
                    throw new ArgumentNullException(nameof(s));
                }
            }
            catch (ArgumentNullException)
            {
                // Do Noting
            }
        }

        private void EnsureNotNullCheck(string s, IEnsureHelper ensure)
        {
            try
            {
                ensure.Parameter(s, nameof(s)).IsNotNull().ThrowOnFailure();
            }
            catch (ArgumentNullException)
            {
                // Do Noting
            }
        }

        public enum TestDataType
        {
            Constant,
            OnlyNull,
            RandomWithoutNull,
            RandomWithNull
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1854:Unused assignments should be removed", Justification = "Is used in inner Method")]
        private IEnumerable<string> GenerateTestData(TestDataType dataType, int n, int samples)
        {
            var rnd = new Random(42);

            string GenerateString() =>
                dataType switch
                {
                    TestDataType.Constant => "Test",
                    TestDataType.OnlyNull => null,
                    TestDataType.RandomWithoutNull => Guid.NewGuid().ToString(),
                    TestDataType.RandomWithNull => (rnd.NextDouble()) < 0.8 ? Guid.NewGuid().ToString() : null,
                    _ => throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null)
                };

            return Enumerable
                .Range(0, n * samples)
                .Select(i => GenerateString());
        }

        private PerformanceStatistics.MeasurementsResults GetMeasurementsResults(Action<string> action, TestDataType testDataType, bool doWarmup, int n, int samples)
        {
            var rnds = this.GenerateTestData(testDataType, n, samples).ToList();
            var rndCount = 0;

#pragma warning disable S1121 // Assignments should not be made from within sub-expressions
            var performanceDefault = new PerformanceStatistics(
                () => action(rnds[rndCount = (rndCount + 1) % rnds.Count]));
#pragma warning restore S1121 // Assignments should not be made from within sub-expressions
            if (doWarmup)
            {
                performanceDefault.Warmup(100);
                rndCount = 0;
            }

            // force GC to collect
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            return performanceDefault.Measure(n, samples);
        }
    }
}
