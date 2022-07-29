using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifySongDownloader.Models
{
    public class Track
    {
        public string? Name { get; set; }
        public string[]? Artists { get; set; }
        public string? ImageUrl { get; set; }
        public int Duration { get; set; }
    }
}
