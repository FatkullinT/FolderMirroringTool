using System.Linq;
using System.Xml.Linq;

namespace FolderMirroringTool.Core.ProjectNodes.PropertyGroups
{
    public class PropertyGroup : CsProjectNode
    {
        public override int Depth => 1;

        public PropertyGroup(XNode node) : base(node)
        {

        }

        /// <summary>
        /// Returns the text value of the element matching the index. To get the contents of the element, use GetChildElement() to get the Element object
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string this[string index]
        {
            get { return ((XElement)Node).Elements().First(x => x.Name.LocalName == index).Value; }
            set
            {
                AddOrUpdateElement(index, value);
            }
        }

    }
}