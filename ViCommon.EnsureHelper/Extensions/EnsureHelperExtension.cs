using System;
using Microsoft;
using ViCommon.EnsureHelper.ArgumentHelpers;
using ViCommon.EnsureHelper.ArgumentHelpers.Extensions;

namespace ViCommon.EnsureHelper.Extensions
{
    /// <summary>
    /// Extension methods for the ensure Helper.
    /// </summary>
    public static class EnsureHelperExtension
    {
        /// <summary>
        /// Ensures a parameter is not null and returns the parameter on success.
        /// </summary>
        /// <typeparam name="TParam">The type of the parameter.</typeparam>
        /// <param name="ensureHelper">The <see cref="IEnsureHelper"/>.</param>
        /// <param name="argument">The argument value.</param>
        /// <param name="argumentName">The argument name.</param>
        /// <returns>An <see cref="IParameterValidator{TParam}"/>.</returns>
        public static TParam EnsureParameterIsNotNullThenReturn<TParam>(
            this IEnsureHelper ensureHelper,
            [ValidatedNotNull] TParam argument,
            string argumentName)
            where TParam : class =>
            ensureHelper?.Parameter(argument, argumentName).IsNotNull().MatchOrThrow(() => argument)
                ?? throw new ArgumentNullException(nameof(ensureHelper));
    }
}
