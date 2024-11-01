namespace ViCommon.EnsureHelper
{
    /// <summary>
    /// Class which uses the <see cref="IEnsureHelper"/>.
    /// </summary>
    public interface IHasEnsureHelper
    {
        /// <summary>
        /// Gets or sets the <see cref="IEnsureHelper"/>.
        /// </summary>
        IEnsureHelper EnsureHelper { get; set; }
    }
}
