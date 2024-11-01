using System;
using System.Collections.Generic;
using System.Linq;
using ViCommon.EnsureHelper.ArgumentHelpers.Extensions;

namespace ViCommon.EnsureHelper.ResultHelpers
{
    /// <summary>
    /// A result class which can be successful or can fail.
    /// </summary>
    public class Result : IResult
    {
        private readonly Exception _exception;

        private Result(Exception exception)
        {
            this._exception = exception;
            this.IsSuccess = false;
        }

        private Result() =>
            this.IsSuccess = true;

        /// <inheritdoc />
        public bool IsSuccess { get; }

        /// <inheritdoc />
        public bool IsFailure => !this.IsSuccess;

        /// <summary>
        /// Construct a result which indicates a success status.
        /// </summary>
        /// <returns>A result.</returns>
        public static Result Success() => new();

        /// <summary>
        /// Constructs a result which indicates a failure status.
        /// </summary>
        /// <param name="exception">The exception of the failure.</param>
        /// <returns>A result.</returns>
        public static Result Failure(Exception exception) => new(exception);

        /// <summary>
        /// Combine two results to one.
        /// </summary>
        /// <param name="a">The first result.</param>
        /// <param name="b">The second result.</param>
        /// <returns>When both results are a success returns a success result (<see cref="Success"/>).
        /// When one or booth are failure returns a failure result (<see cref="Failure"/>) with an <see cref="AggregateException"/>.</returns>
        public static Result Combine(IResult a, IResult b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            var exp = a.GetExceptions().Concat(b.GetExceptions()).ToList();
            return exp.Count switch
            {
                1 => Failure(new AggregateException(exp)),
                var i when i > 1 => Failure(new AggregateException(exp)),
                _ => Success()
            };
        }

        /// <inheritdoc />
        public IEnumerable<Exception> GetExceptions() =>
            this.IsSuccess switch
            {
                true => new List<Exception>(),
                false => (this._exception is AggregateException aggregateException)
                    ? aggregateException.InnerExceptions.ToList()
                    : new List<Exception>() { this._exception }
            };

        /// <inheritdoc />
        public TResult Match<TResult>(Func<TResult> onSuccess, Func<Exception, TResult> onFailure)
        {
            EnsureHelper.GetDefault.Many()
                .Parameter(onSuccess, nameof(onSuccess))
                .Parameter(onFailure, nameof(onFailure))
                .ThrowWhenNull();
            return this.IsSuccess ? onSuccess() : onFailure(this._exception);
        }

        /// <inheritdoc />
        public void OnFailure(Action<Exception> action)
        {
            if (this.IsFailure)
            {
                action?.Invoke(this._exception);
            }
        }
    }
}
