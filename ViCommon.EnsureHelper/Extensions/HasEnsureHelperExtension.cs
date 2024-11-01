using Microsoft;
using ViCommon.EnsureHelper.ArgumentHelpers;
using ViCommon.EnsureHelper.ArgumentHelpers.Extensions;

namespace ViCommon.EnsureHelper.Extensions
{
    /// <summary>
    /// Extension class for simple access to the <see cref="IEnsureHelper"/>.
    /// </summary>
    public static class HasEnsureHelperExtension
    {
        /// <summary>
        /// Gets a parameter validation helper for validating method arguments.
        /// </summary>
        /// <typeparam name="TParam">The type of the parameter.</typeparam>
        /// <param name="hasEnsureHelper">The <see cref="IEnsureHelper"/>.</param>
        /// <param name="argument">The argument value.</param>
        /// <param name="argumentName">The argument name.</param>
        /// <returns>An <see cref="IParameterValidator{TParam}"/>.</returns>
        public static IParameterValidator<TParam> EnsureParameter<TParam>(
            this IHasEnsureHelper hasEnsureHelper,
            [ValidatedNotNull] TParam argument,
            string argumentName) =>
            GetOrDefault(hasEnsureHelper).Parameter(argument, argumentName);

        /// <summary>
        /// Get a parameter validation helper for multiple parameters.
        /// </summary>
        /// <typeparam name="TParam">The type of all parameters.</typeparam>
        /// <param name="hasEnsureHelper">The <see cref="IEnsureHelper"/>.</param>
        /// <returns>An <see cref="ParameterValidatorCollection{TParam}"/>.</returns>
        public static ParameterValidatorCollection<TParam> EnsureMany<TParam>(this IHasEnsureHelper hasEnsureHelper) =>
            GetOrDefault(hasEnsureHelper).Many<TParam>();

        /// <summary>
        /// Get a parameter validation helper for multiple parameters.
        /// </summary>
        /// <remarks>Use <see cref="EnsureMany{TParam}"/> when all parameters have the same type.</remarks>
        /// <param name="hasEnsureHelper">The <see cref="IEnsureHelper"/>.</param>
        /// <returns>An <see cref="ParameterValidatorCollection{TParam}"/>.</returns>
        public static ParameterValidatorCollection<object> EnsureMany(this IHasEnsureHelper hasEnsureHelper) =>
            GetOrDefault(hasEnsureHelper).Many<object>();

        /// <summary>
        /// Ensures a Constructor parameter is not null and returns the parameter on success.
        /// </summary>
        /// <typeparam name="TParam">The type of the parameter.</typeparam>
        /// <param name="hasEnsureHelper">The <see cref="IHasEnsureHelper"/>.</param>
        /// <param name="argument">The argument value.</param>
        /// <param name="argumentName">The argument name.</param>
        /// <returns>An <see cref="IParameterValidator{TParam}"/>.</returns>
        public static TParam EnsureCtorParameterIsNotNull<TParam>(
            this IHasEnsureHelper hasEnsureHelper,
            [ValidatedNotNull] TParam argument,
            string argumentName)
            where TParam : class =>
            GetOrDefault(hasEnsureHelper).Parameter(argument, argumentName).IsNotNull().MatchOrThrow(() => argument);

        private static IEnsureHelper GetOrDefault(IHasEnsureHelper hasEnsureHelper) =>
            hasEnsureHelper?.EnsureHelper ?? EnsureHelper.GetDefault;
    }
}
