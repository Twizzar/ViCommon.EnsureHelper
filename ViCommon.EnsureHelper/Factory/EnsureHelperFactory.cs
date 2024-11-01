using System.Diagnostics.CodeAnalysis;

namespace ViCommon.EnsureHelper.Factory
{
    /// <summary>
    /// Factory for creating an <see cref="IEnsureHelperFactory"/> which logs on throw with the log level error.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class EnsureHelperFactory : IEnsureHelperFactory
    {
        /// <inheritdoc />
        public IEnsureHelper Create() =>
            new EnsureHelper();
    }
}
