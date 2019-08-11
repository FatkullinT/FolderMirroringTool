using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using FolderMirroringTool.Core.ProjectNodes.ItemGroups;
using FolderMirroringTool.Core.ProjectNodes.ItemGroups.ItemGroupContent;
using FolderMirroringTool.Core.ProjectNodes.PropertyGroups;

namespace FolderMirroringTool.Core.ProjectNodes
{
    public class Project
    {
        private readonly XDocument _source;

        /// <summary>
        /// Instantiates a Project from an XML document
        /// </summary>
        /// <param name="source"></param>
        public Project(XDocument source)
        {
            _source = source;
        }

        /// <summary>
        /// Loads a .csporj file and returns a new Project instance
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static Project Load(string file)
        {
            var xml = XDocument.Load(file, LoadOptions.PreserveWhitespace);
            return new Project(xml);
        }


        public IEnumerable<ItemGroup> ItemGroups
        {
            get
            {
                return _source.Root.Elements().Where(x => x.Name.LocalName == "ItemGroup").Select(x => new ItemGroup(x));
            }
        }

        public IEnumerable<PropertyGroup> PropertyGroups
        {
            get
            {
                return _source.Root.Elements().Where(x => x.Name.LocalName == "PropertyGroup").Select(x => new PropertyGroup(x));
            }
        }


        /// <summary>
        /// Returns a <see cref="ItemGroupContent"/> instance with an Include matching the given string, or null otherwise
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="include"></param>
        /// <returns></returns>
        public T FindItemGroupContent<T>(string include) where T : ItemGroupContent, new()
        {
            var node =
                _source.DescendantNodes()
                    .Where(x => x is XElement)
                    .FirstOrDefault(
                        x =>
                            ((XElement)x).Name.LocalName == typeof(T).Name &&
                            ((XElement)x).Attributes().First(y => y.Name.LocalName == "Include").Value == include);

            if (node == null) return default(T);

            return (T)Activator.CreateInstance(typeof(T), new object[] { node });
        }

        /// <summary>
        /// Returns true if an <see cref="ItemGroupContent"/> with a matching Include exists in the Project
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="include"></param>
        /// <returns></returns>
        public bool ItemGroupContentExists<T>(string include) where T : ItemGroupContent
        {
            return _source.DescendantNodes()
                       .Where(x => x is XElement)
                       .FirstOrDefault(
                           x =>
                               string.Equals(((XElement) x).Name.LocalName, typeof(T).Name,
                                   StringComparison.InvariantCultureIgnoreCase) &&
                               string.Equals(
                                   ((XElement) x).Attributes().First(y => y.Name.LocalName == "Include").Value, include,
                                   StringComparison.InvariantCultureIgnoreCase)) != null;
        }

        /// <summary>
        /// Returns a <see cref="ItemGroupContent"/> instance with an Include matching the given string, or null otherwise
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="include"></param>
        /// <returns></returns>
        public bool ProjectReferenceExists(string include)
        {
            return _source.DescendantNodes()
                .Where(x => x is XElement)
                .FirstOrDefault(
                    x =>
                        ((XElement)x).Name.LocalName == "ProjectReference" &&
                        ((XElement)x).Attributes().First(y => y.Name.LocalName == "Include").Value == include) != null;
        }

        /// <summary>
        /// Returns a <see cref="ProjectReference"/> instance with an Include matching the given string, or null otherwise
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="include"></param>
        /// <returns></returns>
        public ProjectReference FindProjectReference(string include)
        {
            var node =
                _source.DescendantNodes()
                    .Where(x => x is XElement)
                    .FirstOrDefault(
                        x =>
                            ((XElement)x).Name.LocalName == "ProjectReference" &&
                            ((XElement)x).Attributes().First(y => y.Name.LocalName == "Include").Value == include);

            if (node == null) return null;

            return new ProjectReference(node);
        }

        public void AddItemGroup(ItemGroup itemGroup)
        {
            var lastNode = (CsProjectNode)ItemGroups.LastOrDefault() ?? PropertyGroups.Last();
            lastNode.AddAfterSelf(itemGroup);
        }

        /// <summary>
        /// Saves the Project as an Xml Document
        /// </summary>
        /// <param name="file"></param>
        public void Save(string file)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                OmitXmlDeclaration = true
            };

            using (XmlWriter writer = XmlWriter.Create(file, settings))
            {
                _source.Save(writer);
            }
        }

        /// <summary>
        /// Saves the Project to the given Stream
        /// </summary>
        /// <param name="stream"></param>
        public void Save(Stream stream)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
            };

            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                _source.Save(writer);
            }
        }

        /// <summary>
        /// Returns the underlying Xml Document
        /// </summary>
        /// <returns></returns>
        public XDocument ToXml()
        {
            return _source;
        }
    }
}