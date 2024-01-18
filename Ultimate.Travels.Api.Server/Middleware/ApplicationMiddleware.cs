namespace Ultimate.Travels.Api.Server
{
    public class ApplicationMiddleware
    {
        private readonly RequestDelegate next;

        public ApplicationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
    }
}