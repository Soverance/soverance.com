using System.ComponentModel.DataAnnotations;

namespace soverance.com.Models
{
    public partial class MailConfig_Contact
    {
        public string Server { get; set; }

        public int Port { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        [Required]
        public string ContactName { get; set; }
        [Required]
        [EmailAddress]
        public string ContactEmail { get; set; }
        [Phone]
        public string ContactPhone { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
