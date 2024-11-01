using System;
using static ViCommon.EnsureHelper.ArgumentHelpers.ExceptionBuilders.DefaultExceptionBuilder;

namespace ViCommon.EnsureHelper.ArgumentHelpers.Extensions
{
    /// <summary>
    /// Extenstion methods for the type IParameterValidator&lt;T&gt; where T is implementing IComparable.
    /// </summary>
    public static class ParameterValidationIComparableExtenstion
    {
        /// <summary>
        /// Check if the argument is greater than an number.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="parameterValidator">The argument validator.</param>
        /// <param name="number">Number to compare against argument &gt; number.</param>
        /// <returns>A new <see cref="IParameterValidator{TParam}"/> to chain  more checks or react to the validation.</returns>
        public static IParameterValidator<T> IsGreaterThan<T>(this IParameterValidator<T> parameterValidator, T number)
            where T : IComparable =>
            IsGreaterThan(
                parameterValidator,
                number,
                ArgumentOutOfRangeExceptionBuilder($"argument is less equal than {number}"));

        /// <summary>
        /// Check if the argument is greater than an number.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="parameterValidator">The argument validator.</param>
        /// <param name="number">Number to compare against argument &gt; number.</param>
        /// <param name="customExceptionBuilder">Function which has as input the parameter name and builds a custom <see cref="Exception"/> to throw on failure.</param>
        /// <returns>A new <see cref="IParameterValidator{TParam}"/> to chain  more checks or react to the validation.</returns>
        public static IParameterValidator<T> IsGreaterThan<T>(
            this IParameterValidator<T> parameterValidator,
            T number,
            Func<string, Exception> customExceptionBuilder)
            where T : IComparable
        {
            EnsureHelper.GetDefault.Parameter(parameterValidator, nameof(parameterValidator)).ThrowWhenNull();
            return parameterValidator.IsTrue(i => IsGreaterThan(i, number), customExceptionBuilder);
        }

        /// <summary>
        /// Check if the argument is less than an number.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="parameterValidator">The argument validator.</param>
        /// <param name="number">Number to compare against argument &lt; number.</param>
        /// <returns>A new <see cref="IParameterValidator{TParam}"/> to chain  more checks or react to the validation.</returns>
        public static IParameterValidator<T> IsLessThan<T>(this IParameterValidator<T> parameterValidator, T number)
            where T : IComparable =>
            IsLessThan(
                parameterValidator,
                number,
                ArgumentOutOfRangeExceptionBuilder($"argument is greater equal than {number}"));

        /// <summary>
        /// Check if the argument is less than an number.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="parameterValidator">The argument validator.</param>
        /// <param name="number">Number to compare against argument &lt; number.</param>
        /// <param name="customExceptionBuilder">Function which has as input the parameter name and builds a custom <see cref="Exception"/> to throw on failure.</param>
        /// <returns>A new <see cref="IParameterValidator{TParam}"/> to chain  more checks or react to the validation.</returns>
        public static IParameterValidator<T> IsLessThan<T>(
            this IParameterValidator<T> parameterValidator,
            T number,
            Func<string, Exception> customExceptionBuilder)
            where T : IComparable
        {
            EnsureHelper.GetDefault.Parameter(parameterValidator, nameof(parameterValidator)).ThrowWhenNull();
            return parameterValidator.IsTrue(
                i => IsLessThan(i, number), customExceptionBuilder);
        }

        /// <summary>
        /// Check if the argument is greater equal than an number.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="parameterValidator">The argument parameterValidator.</param>
        /// <param name="number">Number to compare against argument &gt;= number.</param>
        /// <returns>A new <see cref="IParameterValidator{TParam}"/> to chain  more checks or react to the validation.</returns>
        public static IParameterValidator<T> IsGreaterEqualThan<T>(this IParameterValidator<T> parameterValidator, T number)
            where T : IComparable =>
            IsGreaterEqualThan(parameterValidator, number, ArgumentOutOfRangeExceptionBuilder($"argument is less than {number}"));

        /// <summary>
        /// Check if the argument is greater equal than an number.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="parameterValidator">The argument parameterValidator.</param>
        /// <param name="number">Number to compare against argument &gt;= number.</param>
        /// <param name="customExceptionBuilder">Function which has as input the parameter name and builds a custom <see cref="Exception"/> to throw on failure.</param>
        /// <returns>A new <see cref="IParameterValidator{TParam}"/> to chain  more checks or react to the validation.</returns>
        public static IParameterValidator<T> IsGreaterEqualThan<T>(
            this IParameterValidator<T> parameterValidator,
            T number,
            Func<string, Exception> customExceptionBuilder)
            where T : IComparable
        {
            EnsureHelper.GetDefault.Parameter(parameterValidator, nameof(parameterValidator)).ThrowWhenNull();
            return parameterValidator.IsTrue(i => IsGreaterEqualThan(i, number), customExceptionBuilder);
        }

        /// <summary>
        /// Check if the argument is less equal than an number.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="parameterValidator">The argument parameterValidator.</param>
        /// <param name="number">Number to compare against argument &lt;= number.</param>
        /// <returns>A new <see cref="IParameterValidator{TParam}"/> to chain  more checks or react to the validation.</returns>
        public static IParameterValidator<T> IsLessEqualThan<T>(this IParameterValidator<T> parameterValidator, T number)
            where T : IComparable =>
            IsLessEqualThan(
                parameterValidator,
                number,
                ArgumentOutOfRangeExceptionBuilder($"argument is greater than {number}"));

        /// <summary>
        /// Check if the argument is less equal than an number.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="parameterValidator">The argument parameterValidator.</param>
        /// <param name="number">Number to compare against argument &lt;= number.</param>
        /// <param name="customExceptionBuilder">Function which has as input the parameter name and builds a custom <see cref="Exception"/> to throw on failure.</param>
        /// <returns>A new <see cref="IParameterValidator{TParam}"/> to chain  more checks or react to the validation.</returns>
        public static IParameterValidator<T> IsLessEqualThan<T>(
            this IParameterValidator<T> parameterValidator,
            T number,
            Func<string, Exception> customExceptionBuilder)
            where T : IComparable
        {
            EnsureHelper.GetDefault.Parameter(parameterValidator, nameof(parameterValidator)).ThrowWhenNull();
            return parameterValidator.IsTrue(i => IsLessEqualThan(i, number), customExceptionBuilder);
        }

        /// <summary>
        /// Check if the argument x is in a certain range. (inclusiveStart &lt;= x &lt; exclusiveEnd).
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="parameterValidator">The argument parameterValidator.</param>
        /// <param name="inclusiveStart">The start of the range (inclusive).</param>
        /// <param name="exclusiveEnd">The end of the range (exclusive).</param>
        /// <returns>A new <see cref="IParameterValidator{TParam}"/> to chain  more checks or react to the validation.</returns>
        public static IParameterValidator<T> IsInRange<T>(this IParameterValidator<T> parameterValidator, T inclusiveStart, T exclusiveEnd)
            where T : IComparable =>
            IsInRange(
                parameterValidator,
                inclusiveStart,
                exclusiveEnd,
                ArgumentOutOfRangeExceptionBuilder($"Value is not in the range [{inclusiveStart}, {exclusiveEnd})"));

        /// <summary>
        /// Check if the argument x is in a certain range. (inclusiveStart &lt;= x &lt; exclusiveEnd).
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="parameterValidator">The argument parameterValidator.</param>
        /// <param name="inclusiveStart">The start of the range (inclusive).</param>
        /// <param name="exclusiveEnd">The end of the range (exclusive).</param>
        /// <param name="customExceptionBuilder">Function which has as input the parameter name and builds a custom <see cref="Exception"/> to throw on failure.</param>
        /// <returns>A new <see cref="IParameterValidator{TParam}"/> to chain  more checks or react to the validation.</returns>
        public static IParameterValidator<T> IsInRange<T>(
            this IParameterValidator<T> parameterValidator,
            T inclusiveStart,
            T exclusiveEnd,
            Func<string, Exception> customExceptionBuilder)
            where T : IComparable
        {
            EnsureHelper.GetDefault.Parameter(inclusiveStart, nameof(inclusiveStart))
                .IsLessThan(
                    exclusiveEnd,
                    ArgumentOutOfRangeExceptionBuilder($"{nameof(inclusiveStart)} should be smaller than {nameof(exclusiveEnd)}"))
                .ThrowOnFailure();

            return parameterValidator
                            .IsGreaterEqualThan(inclusiveStart, customExceptionBuilder)
                            .IsLessThan(exclusiveEnd, customExceptionBuilder);
        }

        private static bool IsGreaterThan<T>(this T value, T other)
            where T : IComparable =>
            value.CompareTo(other) > 0;

        private static bool IsLessThan<T>(this T value, T other)
            where T : IComparable =>
            value.CompareTo(other) < 0;

        private static bool IsGreaterEqualThan<T>(this T value, T other)
            where T : IComparable =>
            value.CompareTo(other) >= 0;

        private static bool IsLessEqualThan<T>(this T value, T other)
            where T : IComparable =>
            value.CompareTo(other) <= 0;
    }
}
