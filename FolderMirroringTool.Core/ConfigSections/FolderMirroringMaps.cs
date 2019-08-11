using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace FolderMirroringTool.Core.ConfigSections
{
    public class FolderMirroringMaps
    {
        private readonly XElement _node;
        public FolderMirroringMap[] Items { get; }

        private FolderMirroringMaps(XElement node)
        {
            _node = node;
            Items = _node.Elements(XName.Get("FolderMirroringMap")).Select(element => new FolderMirroringMap(element)).ToArray();
        }

        private FolderMirroringMaps()
        {
            _node = new XElement(XName.Get("FolderMirroringMaps"));
            Items = Array.Empty<FolderMirroringMap>();
        }


        public static FolderMirroringMaps Load(string filePath)
        {
            if (!File.Exists(filePath))
                return new FolderMirroringMaps();

            using (var stream = File.OpenRead(filePath))
            {
                var configDocument = XDocument.Load(stream);
                return new FolderMirroringMaps(configDocument.Root);
            }
        }
    }
}