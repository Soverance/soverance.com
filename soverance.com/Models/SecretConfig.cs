using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace soverance.com.Models
{
    public partial class SecretConfig
    {
        public string AzureMapsKey { get; set; }

        public string GoogleApiKey { get; set; }
    }
}
