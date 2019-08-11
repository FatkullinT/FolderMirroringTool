using System;
using System.Collections.Generic;
using System.Linq;
using FolderMirroringTool.Core.ProjectNodes.ItemGroups.ItemGroupContent;

namespace FolderMirroringTool.Core.ProjectNodes
{
    public static class Reflection
    {
        private static readonly Dictionary<string, Type> _typelookup;

        static Reflection()
        {
            _typelookup = typeof(ItemGroupContent).Assembly.GetTypes().Where(x => typeof(ItemGroupContent).IsAssignableFrom(x)).ToDictionary(x => x.Name);
        }

        public static Type GetItemGroupContentTypeFromName(string name)
        {
            if (_typelookup.TryGetValue(name, out var type))
                return type;
            
            return typeof(ItemGroupContent);
        }
    }
}