using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FolderMirroringTool.Core.ProjectNodes.ItemGroups
{
    public class ItemGroup : CsProjectNode
    {
        public override int Depth => 1;

        public ItemGroup()
        {
        }

        public ItemGroup(XNode node) : base(node)
        {

        }

        public void AddContent(ItemGroupContent.ItemGroupContent content)
        {
            AddElement(content.Node);
        }

        public IEnumerable<ItemGroupContent.ItemGroupContent> Contents
        {
            get
            {
                // I know creating new objects every time you access the contents is very inefficient
                return Element.Elements()
                    .Select(x =>
                    {
                        var type = Reflection.GetItemGroupContentTypeFromName(x.Name.LocalName);
                        return (ItemGroupContent.ItemGroupContent) Activator.CreateInstance(type, x);
                    });
            }
        }

    }
}