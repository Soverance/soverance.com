using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace soverance.com.Models
{
    public partial class MailConfig
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
        public string ContactPhone { get; set; }
        public string Subject { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
