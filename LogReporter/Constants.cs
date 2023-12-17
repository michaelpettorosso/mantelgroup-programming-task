namespace LogReporter;

internal static class Constants
{
    /// <summary>
    /// List of Group names used to create data from logfile
    /// </summary>
    internal static class Group
    {
        public const string IpAddress = nameof(IpAddress);
        public const string User = nameof(User);
        public const string DateTime = nameof(DateTime);
        public const string Method = nameof(Method);
        public const string Url = nameof(Url);
        public const string Protocol = nameof(Protocol);
        public const string StatusCode = nameof(StatusCode);
        public const string UserAgent = nameof(UserAgent);
    }

}
