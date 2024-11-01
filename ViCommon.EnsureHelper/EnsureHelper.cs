using System.Collections.Generic;
using Microsoft;
using ViCommon.EnsureHelper.ArgumentHelpers;

namespace ViCommon.EnsureHelper
{
    /// <summary>
    /// Helper class for providing validation helpers.
    /// </summary>
    public class EnsureHelper : IEnsureHelper
    {
        #region static fields and constants

        private static IEnsureHelper _defaultEnsureHelper = new EnsureHelper();

        #endregion

        #region fields

        private readonly List<IEnsureHelper.EnsureHelperFailCallback> _onFailureCallbacks = new();
        private readonly List<IEnsureHelper.EnsureHelperThrowCallback> _onThrowCallbacks = new();

        #endregion

        #region properties

        /// <summary>
        /// Gets an simple ensure helper without any callbacks.
        /// </summary>
        public static IEnsureHelper GetDefault => _defaultEnsureHelper;

        private IEnsureHelper.EnsureHelperFailCallback OnFailureThrowCallback =>
            exception => this._onFailureCallbacks.ForEach(action => action?.Invoke(exception));

        private IEnsureHelper.EnsureHelperThrowCallback OnThrowThrowCallback =>
            (exception, stackTrace) => this._onThrowCallbacks.ForEach(action => action?.Invoke(exception, stackTrace));

        #endregion

        #region members

        /// <summary>
        /// Configure the return type of the <see cref="GetDefault"/>.
        /// </summary>
        /// <param name="ensureHelper">The new default ensure helper.</param>
        public static void ConfigureDefault(IEnsureHelper ensureHelper) =>
            _defaultEnsureHelper = ensureHelper;

        /// <inheritdoc/>
        public void RegisterOnFailureCallbacks(IEnsureHelper.EnsureHelperFailCallback failCallback) =>
            this._onFailureCallbacks.Add(failCallback);

        /// <inheritdoc/>
        public void RegisterOnThrowCallbacks(IEnsureHelper.EnsureHelperThrowCallback throwCallback) =>
            this._onThrowCallbacks.Add(throwCallback);

        /// <inheritdoc/>
        public IParameterValidator<TParam> Parameter<TParam>([ValidatedNotNull] TParam argument, string argumentName) =>
            new SuccessParameterValidator<TParam>(argument, argumentName, this.OnFailureThrowCallback, this.OnThrowThrowCallback);

        /// <inheritdoc/>
        public ParameterValidatorCollection<TParam> Many<TParam>() =>
            new(this.OnFailureThrowCallback, this.OnThrowThrowCallback);

        /// <inheritdoc/>
        public ParameterValidatorCollection<object> Many() =>
            new(this.OnFailureThrowCallback, this.OnThrowThrowCallback);

        #endregion
    }
}
