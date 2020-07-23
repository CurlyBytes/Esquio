namespace Esquio.UI.Host.Infrastructure.Options
{
    public class Api
    {
        public string Audience { get; set; }
        public string Authority { get; set; }
        public string RequireHttpsMetadata { get; set; }
        public bool ValidateEndpoints { get; set; }
    }
}
