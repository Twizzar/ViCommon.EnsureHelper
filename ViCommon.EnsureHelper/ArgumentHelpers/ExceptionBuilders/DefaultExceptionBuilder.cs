using System;

namespace ViCommon.EnsureHelper.ArgumentHelpers.ExceptionBuilders
{
    /// <summary>
    /// Some default Exception Builder.
    /// </summary>
    public static class DefaultExceptionBuilder
    {
        /// <summary>
        /// Create a <see cref="ArgumentException"/>.
        /// </summary>
        /// <param name="message">Message for the Exception.</param>
        /// <returns>A <see cref="ArgumentException"/>.</returns>
        public static Func<string, Exception> ArgumentExceptionBuilder(string message) =>
            paramName => new ArgumentException(message, paramName);

        /// <summary>
        /// Create a <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="message">Message for the Exception.</param>
        /// <returns>A <see cref="ArgumentNullException"/>.</returns>
        public static Func<string, Exception> ArgumentNullExceptionBuilder(string message) =>
            paramName => new ArgumentNullException(paramName, message);

        /// <summary>
        /// Create a <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <returns>A <see cref="ArgumentNullException"/>.</returns>
        public static Func<string, Exception> ArgumentNullExceptionBuilder() =>
            paramName => new ArgumentNullException(paramName);

        /// <summary>
        /// Create a <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name="message">Message for the Exception.</param>
        /// <returns>A <see cref="ArgumentOutOfRangeException"/>.</returns>
        public static Func<string, Exception> ArgumentOutOfRangeExceptionBuilder(string message) =>
            paramName => new ArgumentOutOfRangeException(paramName, message);
    }
}
