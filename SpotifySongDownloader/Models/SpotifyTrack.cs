using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifySongDownloader.Models
{
    public class SpotifyTrack
    {
        public string Name { get; set; }
        public int Duration_ms { get; set; }
        public Album Album { get; set; }
        public List<Artist> Artists { get; set; }
    }
   
}
