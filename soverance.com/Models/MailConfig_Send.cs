using System.ComponentModel.DataAnnotations;

namespace soverance.com.Models
{
    public partial class MailConfig_Send
    {
        public string Server { get; set; }

        public int Port { get; set; }

        public string User { get; set; }

        public string Password { get; set; }
        
        [Required]
        [EmailAddress]
        public string Recipient { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
