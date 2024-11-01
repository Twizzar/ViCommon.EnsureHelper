using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

namespace ViCommon.EnsureHelper.UnitTest.Ensure.Performance
{
    /// <summary>
    /// Class for measuring performance.
    /// </summary>
    public class PerformanceStatistics
    {
        /// <summary>
        /// Contains the result samples of the performance test.
        /// </summary>
        public class MeasurementsResults
        {
            /// <summary>
            /// Number of samples
            /// </summary>
            public readonly int Samples;
            /// <summary>
            /// How many executions where made per samples.
            /// </summary>
            public readonly int NumberOfExecutions;
            /// <summary>
            /// Average runtime.
            /// </summary>
            public readonly double Avg;
            /// <summary>
            /// The minimum runtime.
            /// </summary>
            public readonly double Min;
            /// <summary>
            /// The maximum runtime.
            /// </summary>
            public readonly double Max;
            /// <summary>
            /// The standard deviation of the runtime.
            /// </summary>
            public readonly double Std;
            /// <summary>
            /// Time in milliseconds of all the measurements.
            /// </summary>
            public readonly List<double> Measurements;

            internal MeasurementsResults(IEnumerable<double> measurements, int numberOfExecutions)
            {
                this.Measurements = measurements.ToList();
                var min = double.PositiveInfinity;
                var max = double.NegativeInfinity;
                var sum = 0.0;
                this.NumberOfExecutions = numberOfExecutions;
                this.Samples = this.Measurements.Count;

                foreach (var x in this.Measurements)
                {
                    if (x < min)
                        min = x;

                    if (x > max)
                        max = x;

                    sum += x;
                }

                this.Avg = sum / this.Samples;
                this.Min = min;
                this.Max = max;
                this.Std = StandardDeviation(this.Measurements, this.Avg);
            }

            /// <summary>
            /// Get how many times this measurement average is slower.
            /// </summary>
            /// <param name="otherAvg">The avg to compare.</param>
            /// <returns></returns>
            public double GetAvgFactor(double otherAvg) => this.Avg / otherAvg;

            private static string Format(string title, double value) =>
                $"{title,-40}{value, 10:#0.0000 ms}";

            /// <summary>
            /// Get the results as an string.
            /// </summary>
            /// <returns>A string.</returns>
            public override string ToString()
            {
                var sb = new StringBuilder();
                sb.AppendFormat("Tested with {0} samples for each was executed {1} times. \n", this.Samples, this.NumberOfExecutions);
                sb.AppendLine(Format("Average runtime:", this.Avg));
                sb.AppendLine(Format("Average runtime for 1 action:", this.Avg / this.NumberOfExecutions));
                sb.AppendLine(Format("Maximum:", this.Max));
                sb.AppendLine(Format("Minimum:", this.Min));
                sb.AppendLine(Format("Standard Deviation:", this.Std));

                return sb.ToString();
            }

            /// <summary>
            /// Get the results as an dictionary.
            /// </summary>
            /// <returns>Dictionary with a meaningful name as keys and the measurements value as values.</returns>
            public Dictionary<string, double> ToDict() =>
                new Dictionary<string, double>()
                {
                    {"samples", this.Samples },
                    {"number of executions", this.NumberOfExecutions},
                    {"average runtime", this.Avg},
                    {"Average runtime for one action", this.Avg / this.NumberOfExecutions},
                    {"Maximum", this.Max},
                    {"Minimum", this.Min},
                    {"Standard Deviation", this.Std},
                };

            /// <summary>
            /// Get the results in a csv format.
            /// </summary>
            /// <param name="withHeader">Also include a header.</param>
            /// <param name="horizontal">Print it horizontal 1 row the headers 1 row the values,
            /// or vertical 1 column the header 1 column the values.</param>
            /// <returns>String in a csv format.</returns>
            public string AsCsvFormat(bool withHeader = true, bool horizontal = false)
            {
                var sb = new StringBuilder();

                if (horizontal)
                {
                    if (withHeader)
                    {
                        sb.AppendLine(
                            this.ToDict().Keys
                                .Aggregate("", (s, s1) => s + "," + s1));
                    }

                    sb.AppendLine(
                        this.ToDict().Values
                            .Aggregate("", (s, s1) => s + "," + s1));
                }
                else
                {
                    foreach (var (key, value) in this.ToDict())
                    {
                        if (withHeader)
                        {
                            sb.Append($"{key}, ");
                        }
                        sb.AppendLine(value.ToString(CultureInfo.InvariantCulture));
                    }
                }

                return sb.ToString();
            }
        }

        private readonly Action _action;
        private readonly bool _preventGarbageCollection;
        /// <summary>
        /// When the option is active to prevent Garbage Collection this value indicates what the Max size to prevent is.
        /// </summary>
        public long MaxGcSizeToPrevent { get; set; } = (long)1E8;

        /// <summary>
        /// Create a new instance of <see cref="PerformanceStatistics"/>.
        /// </summary>
        /// <param name="actionToMeasure">The action to measure.</param>
        /// <param name="preventGarbageCollection">Garbage collection is prevented in the actionToMeasure when this is ture.</param>
        public PerformanceStatistics(Action actionToMeasure, bool preventGarbageCollection = true)
        {
            this._action = actionToMeasure;
            this._preventGarbageCollection = preventGarbageCollection;
        }

        /// <summary>
        /// Do the action till the given microseconds is reached. For warming up the system.
        /// </summary>
        /// <param name="milliseconds">How long to do the warmup.</param>
        public void Warmup(int milliseconds)
        {
            var timer = Stopwatch.StartNew();
            while (timer.ElapsedMilliseconds < milliseconds)
            {
                this._action();
            }
        }
        /// <summary>
        /// Measure the performance.
        /// </summary>
        /// <param name="numberOfActions">How many action 1 sample should contain.</param>
        /// <param name="samples">Number of samples to collect.</param>
        /// <returns>A <see cref="MeasurementsResults"/>.</returns>
        public MeasurementsResults Measure(int numberOfActions, int samples)
        {
            return new MeasurementsResults(this.MeasureTime(this._action, numberOfActions, samples)
                .Select(span => span.TotalMilliseconds), numberOfActions);
        }

        private TimeSpan MeasureTime(Action toTime)
        {
            if (this._preventGarbageCollection && !GC.TryStartNoGCRegion(this.MaxGcSizeToPrevent))
            {
                throw new Exception("Cannot disable GC collection");
            }

            var timer = Stopwatch.StartNew();
            toTime();
            timer.Stop();

            if(this._preventGarbageCollection)
                GC.EndNoGCRegion();
            return timer.Elapsed;
        }

        private TimeSpan MeasureTime(Action toTime, int n)
        {
            return this.MeasureTime(() =>
            {
                for (int i = 0; i < n; i++)
                {
                    toTime();
                }
            });
        }

        private IEnumerable<TimeSpan> MeasureTime(Action toTime, int n, int samples)
        {
            for (int i = 0; i < samples; i++)
            {
                yield return this.MeasureTime(toTime, n);
            }
        }

        private static double StandardDeviation(IEnumerable<double> values, double mean)
        {
            return Math.Sqrt(Variance(values, mean));

        }

        private static double Variance(IEnumerable<double> values, double mean)
        {
            var vs = values.ToList();
            var n = vs.Count;
            return 1f / n *
                   vs
                       .Select(x => Math.Pow(x - mean, 2))
                       .Sum();
        }


    }
}
