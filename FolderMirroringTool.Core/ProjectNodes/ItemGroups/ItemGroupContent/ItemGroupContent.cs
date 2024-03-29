using System.Xml.Linq;

namespace FolderMirroringTool.Core.ProjectNodes.ItemGroups.ItemGroupContent
{
    /// <summary>
    /// Represents an untyped ItemGroup child element
    /// </summary>
    public class ItemGroupContent : CsProjectNode
    {
        public override int Depth => 2;

        private string _include;

        public override string ElementName {
            get { return ((XElement) Node)?.Name.LocalName ?? GetType().Name; }
        }

        public string Include
        {
            get { return _include; }
            set
            {
                AddOrUpdateAttribute("Include", value);
                _include = value;
            }
        }

        protected ItemGroupContent()
        {

        }

        public ItemGroupContent(XNode node) : base(node)
        {
            _include = Element.Attribute("Include")?.Value;
        }

        protected ItemGroupContent(string include)
        {
            Include = include;
        }

        public ItemGroupContent(string name, string include) : base(name)
        {
            Include = include;
        }
    }
}