namespace ViCommon.EnsureHelper
{
    /// <summary>
    /// Information about the caller.
    /// </summary>
    public record CallerInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallerInformation"/> class.
        /// </summary>
        /// <param name="callerMemberName">The caller member name.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="line">The line.</param>
        public CallerInformation(string callerMemberName, string filePath, int line)
        {
            this.CallerMemberName = callerMemberName;
            this.FilePath = filePath;
            this.Line = line;
        }

        /// <summary>
        /// Gets the caller member name.
        /// </summary>
        public string CallerMemberName { get; }

        /// <summary>
        /// Gets the file path of the caller.
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// Gets the line of the caller member.
        /// </summary>
        public int Line { get; }
    }
}