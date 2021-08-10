namespace Ordering.Application.Models
{
    public class EmailSettingsOptions
    {
        public static readonly string EmailSettings = "EmailSettings";
        public string ApiKey { get; set; }

        public string FromAddress { get; set; }

        public string FromName { get; set; }
    }
}
