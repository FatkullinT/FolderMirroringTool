using System.Configuration;
using System.Xml.Linq;

namespace FolderMirroringTool.Core.ConfigSections
{
    public class FolderMirroringMap
    {
        private readonly XElement _node;

        public FolderMirroringMap(XElement node)
        {
            _node = node;
        }

        public string FolderPath => (string)_node.Attribute(XName.Get("FolderPath"));

        public string FolderLink => (string)_node.Attribute(XName.Get("FolderLink"));
    }
}