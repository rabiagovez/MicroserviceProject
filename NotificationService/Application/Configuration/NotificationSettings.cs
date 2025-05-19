namespace NotificationService.Application.Configuration
{
    public class NotificationSettings
    {
        public string SmtpEmail { get; set; } = string.Empty;
        public string SmtpPassword { get; set; } = string.Empty;
        public string SmtpHost { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string FromEmail { get; set; } = string.Empty;

        public string TwilioSid { get; set; } = string.Empty;
        public string TwilioToken { get; set; } = string.Empty;
        public string TwilioFrom { get; set; } = string.Empty;
    }

}
