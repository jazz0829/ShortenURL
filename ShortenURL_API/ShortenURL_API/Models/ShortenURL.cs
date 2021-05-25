using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShortenURL_API.Models
{
    public class ShortenURL
    {
        public string OriginalURL { get; set; }
        public string ResultURL { get; set; }
    }
}