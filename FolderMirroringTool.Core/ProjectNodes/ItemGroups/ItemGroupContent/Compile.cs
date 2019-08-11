using System.Xml.Linq;

namespace FolderMirroringTool.Core.ProjectNodes.ItemGroups.ItemGroupContent
{
    public class Compile : ItemGroupFileContent
    {
        public Compile() 
        {

        }

        public Compile(XNode node) : base(node)
        {
            _link = Element.Attribute("Link")?.Value;
        }

        public Compile(string include) : base(include)
        {
            
        }

        public Compile(string include, string dependentUpon) : base(include, dependentUpon)
        {

        }

        public Compile(string include, string dependentUpon, string subType) : base(include, dependentUpon, subType)
        {

        }

        private string _link;

        public string Link
        {
            get => _link;
            set
            {
                AddOrUpdateAttribute("Link", value);
                _link = value;
            }
        }
    }
}