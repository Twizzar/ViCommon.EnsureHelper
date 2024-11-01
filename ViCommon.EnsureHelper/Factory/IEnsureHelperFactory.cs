namespace ViCommon.EnsureHelper.Factory
{
    /// <summary>
    /// Factory for creating <see cref="IEnsureHelper"/>.
    /// </summary>
    public interface IEnsureHelperFactory
    {
        /// <summary>
        /// Create a new instance of <see cref="IEnsureHelper"/>.
        /// </summary>
        /// <returns>An <see cref="IEnsureHelper"/>.</returns>
        IEnsureHelper Create();
    }
}
