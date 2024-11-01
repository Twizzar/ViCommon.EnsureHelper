using System;
using System.Collections.Generic;

namespace ViCommon.EnsureHelper.ResultHelpers
{
    /// <summary>
    /// Interface for a Result.
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// Gets a value indicating whether the result is a success.
        /// </summary>
        bool IsSuccess { get;  }

        /// <summary>
        /// Gets a value indicating whether the result is a failure.
        /// </summary>
        bool IsFailure { get; }

        /// <summary>
        /// Invokes onSuccess on success or onFailure on failure and returns the output of the function.
        /// </summary>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="onSuccess">Function invoked on success.</param>
        /// <param name="onFailure">Function invoked on failure.</param>
        /// <returns>The result of the function.</returns>
        TResult Match<TResult>(Func<TResult> onSuccess, Func<Exception, TResult> onFailure);

        /// <summary>
        /// Invokes an action when is failure.
        /// </summary>
        /// <param name="action">Action to invoke.</param>
        void OnFailure(Action<Exception> action);

        /// <summary>
        /// Returns all exception.
        /// </summary>
        /// <returns>An empty sequence whe is success.
        /// When the exception is of type <see cref="AggregateException"/> returns <see cref="Exception.InnerException"/> else
        /// returns a sequence with one element.</returns>
        IEnumerable<Exception> GetExceptions();
    }
}
