namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// API model for warning or error
    /// </summary>
    public class IssueApiModel
    {
        public int Status { get; set; }
        public int Code { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public IssueSourceApiModel Source { get; set; }
    }

    /// <summary>
    /// API model for issue source
    /// </summary>
    public class IssueSourceApiModel
    {
        public string Parameter { get; set; }
        public string Example { get; set; }
    }
}