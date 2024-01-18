namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The base data model for all othe data models
    /// </summary>
    public class BaseDataModel
    {
        /// <summary>
        /// The unique id for all entires
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// The point in time data was created
        /// </summary>
        public DateTimeOffset DateCreated { get; set; }

        /// <summary>
        /// The point in time data was modified
        /// </summary>
        public DateTimeOffset DateModified { get; set; }
    }
}