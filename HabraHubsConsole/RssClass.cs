using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HabraHubsConsole
{
    [XmlRoot(ElementName = "rss")]
    public class RssClass
    {
        [XmlElement(ElementName = "channel")]
        public ChannelRss channel { set; get; }
    }

    public class ChannelRss
    {
        [XmlElement(ElementName = "item")]
        public List<ItemRss> Items { set; get; }

        [XmlElement(ElementName = "title")]
        public string Title { set; get; }
    }

    public class ItemRss
    {
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }

        [XmlElement(ElementName = "link")]
        public string Link { get; set; }

    }
}
