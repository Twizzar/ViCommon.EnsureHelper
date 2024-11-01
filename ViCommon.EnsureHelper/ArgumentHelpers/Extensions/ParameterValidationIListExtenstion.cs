using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static ViCommon.EnsureHelper.ArgumentHelpers.ExceptionBuilders.DefaultExceptionBuilder;

#pragma warning disable CA1062 // Validate arguments of public methods
#pragma warning disable CA1303 // Do not pass literals as localized parameters

namespace ViCommon.EnsureHelper.ArgumentHelpers.Extensions
{
    /// <summary>
    /// Extenstion methods for the type IParameterValidator&lt;IList&gt;.
    /// </summary>
    public static class ParameterValidationIListExtenstion
    {
        /// <summary>
        /// Checks if the list is empty.
        /// </summary>
        /// <param name="parameterValidator">The parameter validator.</param>
        /// <returns>A new <see cref="IParameterValidator{TParam}"/> to chain  more checks or react to the validation.</returns>
        public static IParameterValidator<IList> IsNotEmpty(this IParameterValidator<IList> parameterValidator) =>
            IsNotEmpty(parameterValidator, ArgumentExceptionBuilder("Parameter is empty"));

        /// <summary>
        /// Checks if the list is empty.
        /// </summary>
        /// <param name="parameterValidator">The parameter validator.</param>
        /// <param name="customExceptionBuilder">Function which has as input the parameter name and builds a custom <see cref="Exception"/> to throw on failure.</param>
        /// <returns>On success returns itself on failure throws an <see cref="ArgumentException"/>.</returns>
        public static IParameterValidator<IList> IsNotEmpty(
            this IParameterValidator<IList> parameterValidator,
            Func<string, Exception> customExceptionBuilder)
        {
            EnsureHelper.GetDefault.Parameter(parameterValidator, nameof(parameterValidator)).ThrowWhenNull();
            return parameterValidator.IsFalse(l => l.Count == 0, customExceptionBuilder);
        }

        /// <summary>
        /// Checks if the list is empty.
        /// </summary>
        /// <param name="parameterValidator">The parameter validator.</param>
        /// <typeparam name="TParam">The type of the Parameter.</typeparam>
        /// <returns>A new <see cref="IParameterValidator{TParam}"/> to chain  more checks or react to the validation.</returns>
        public static IParameterValidator<IList<TParam>> IsNotEmpty<TParam>(
            this IParameterValidator<IList<TParam>> parameterValidator) =>
            IsNotEmpty(parameterValidator, ArgumentExceptionBuilder("parameter is empty"));

        /// <summary>
        /// Checks if the list is empty.
        /// </summary>
        /// <param name="parameterValidator">The parameter validator.</param>
        /// <param name="customExceptionBuilder">Function which has as input the parameter name and builds a custom <see cref="Exception"/> to throw on failure.</param>
        /// <typeparam name="TParam">The type of the Parameter.</typeparam>
        /// <returns>A new <see cref="IParameterValidator{TParam}"/> to chain  more checks or react to the validation.</returns>
        public static IParameterValidator<IList<TParam>> IsNotEmpty<TParam>(
            this IParameterValidator<IList<TParam>> parameterValidator,
            Func<string, Exception> customExceptionBuilder)
        {
            EnsureHelper.GetDefault.Parameter(parameterValidator, nameof(parameterValidator)).ThrowWhenNull();
            return parameterValidator.IsFalse(l => l.Count == 0, customExceptionBuilder);
        }

        /// <summary>
        /// Check of all element of the list fulfill the predicate.
        /// </summary>
        /// <typeparam name="TParam">The type of the Parameter.</typeparam>
        /// <param name="parameterValidator">The parameter validator.</param>
        /// <param name="predicate">Predicate all elements of the list must fulfill.</param>
        /// <param name="customExceptionBuilder">Function which has as input the parameter name and builds a custom <see cref="Exception"/> to throw on failure.</param>
        /// <returns>A new <see cref="IParameterValidator{TParam}"/> to chain  more checks or react to the validation.</returns>
        public static IParameterValidator<IList<TParam>> All<TParam>(
            this IParameterValidator<IList<TParam>> parameterValidator,
            Func<TParam, bool> predicate,
            Func<string, Exception> customExceptionBuilder)
        {
            EnsureHelper.GetDefault.Parameter(parameterValidator, nameof(parameterValidator)).ThrowWhenNull();
            return parameterValidator.IsTrue(list => list.All(predicate), customExceptionBuilder);
        }

        /// <summary>
        /// Checks if any element in the list in null.
        /// </summary>
        /// <typeparam name="TParam">The type of the Parameter.</typeparam>
        /// <param name="parameterValidator">The parameter validator.</param>
        /// <param name="customExceptionBuilder">Function which has as input the parameter name and builds a custom <see cref="Exception"/> to throw on failure.</param>
        /// <returns>A new <see cref="IParameterValidator{TParam}"/> to chain  more checks or react to the validation.</returns>
        public static IParameterValidator<IList<TParam>> NoneIsNull<TParam>(
            this IParameterValidator<IList<TParam>> parameterValidator,
            Func<string, Exception> customExceptionBuilder)
            where TParam : class
        {
            return All(parameterValidator, e => e != null, customExceptionBuilder);
        }

        /// <summary>
        /// Checks if any element in the list in null.
        /// </summary>
        /// <typeparam name="TParam">The type of the Parameter.</typeparam>
        /// <param name="parameterValidator">The parameter validator.</param>
        /// <returns>A new <see cref="IParameterValidator{TParam}"/> to chain  more checks or react to the validation.</returns>
        public static IParameterValidator<IList<TParam>> NoneIsNull<TParam>(
            this IParameterValidator<IList<TParam>> parameterValidator)
            where TParam : class
        {
            return All(
                parameterValidator,
                e => e != null,
                ArgumentExceptionBuilder("The sequence cannot contain elements with null reference"));
        }

        /// <summary>
        /// Checks if the list is null or empty.
        /// </summary>
        /// <param name="parameterValidator">The parameter validator.</param>
        /// <returns>A new <see cref="IParameterValidator{TParam}"/> to chain  more checks or react to the validation.</returns>
        public static IParameterValidator<IList> IsNotNullAndNotEmpty(this IParameterValidator<IList> parameterValidator) =>
            parameterValidator.IsNotNull().IsNotEmpty();

        /// <summary>
        /// Checks if the list is null or empty.
        /// </summary>
        /// <param name="parameterValidator">The parameter validator.</param>
        /// <typeparam name="TParam">The type of the Parameter.</typeparam>
        /// <returns>A new <see cref="IParameterValidator{TParam}"/> to chain  more checks or react to the validation.</returns>
        public static IParameterValidator<IList<TParam>> IsNotNullAndNotEmpty<TParam>(this IParameterValidator<IList<TParam>> parameterValidator) =>
            parameterValidator.IsNotNull().IsNotEmpty();
    }
}
