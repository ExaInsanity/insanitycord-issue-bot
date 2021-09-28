using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueBot
{
    public class Config
    {
        public String Token { get; set; }
        public List<ChannelData> AssociatedChannels { get; set; }
        public Dictionary<String, String> RepositoryIds { get; set; }
    }
}
