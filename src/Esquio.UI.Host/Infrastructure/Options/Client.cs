namespace Esquio.UI.Host.Infrastructure.Options
{
    public class Client
    {
        public string ClientId { get; set; }
        public string Audience { get; set; }
        public string Authority { get; set; }
        public string ResponseType { get; set; }
        public string RequireHttpsMetadata { get; set; }
    }
}
