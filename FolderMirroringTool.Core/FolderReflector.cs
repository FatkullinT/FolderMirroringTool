using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using FolderMirroringTool.Core.ConfigSections;
using FolderMirroringTool.Core.ProjectNodes;
using FolderMirroringTool.Core.ProjectNodes.ItemGroups;
using FolderMirroringTool.Core.ProjectNodes.ItemGroups.ItemGroupContent;

namespace FolderMirroringTool.Core
{
    public class FolderReflector
    {
        public void FullMirroring(string projectPath)
        {
            var projectDirectory = Path.GetDirectoryName(projectPath) ?? string.Empty;
            var configFile = Path.Combine(projectDirectory, "FolderMirroring.xml.user");

            var maps = FolderMirroringMaps.Load(configFile);

            foreach (var map in maps.Items)
            {
                MoveFilesToExternalFolder(projectDirectory, map.FolderPath, map.FolderLink, true);
                AddAllFolderFilesAsLinks(projectPath, map.FolderPath, map.FolderLink);
            }
        }

        private void MoveFilesToExternalFolder(string projectDirectory, string folderPath, string folderLink, bool replace)
        {
            var files = Directory.GetFiles(Path.Combine(projectDirectory, folderLink), "*.cs", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                var sourceFileRelativePath = GetRelativePath(Path.Combine(projectDirectory, folderLink), file);
                var targetFilePath = Path.Combine(Path.Combine(projectDirectory, folderPath), sourceFileRelativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(targetFilePath) ?? string.Empty);
                File.Move(file, targetFilePath);
            }
        }

        public void AddAllFolderFilesAsLinks(string projectPath, string folderPath, string folderLink)
        {
            var projectDirectory = Path.GetDirectoryName(projectPath) ?? string.Empty;
            var fullFolderPath = Path.Combine(projectDirectory, folderPath);
            var files = Directory.GetFiles(fullFolderPath, "*.cs", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var fileFromFolderPath = GetRelativePath(fullFolderPath, file);
                var fileFromProjectPath = GetRelativePath(projectDirectory, file);
                AddFileAsLink(projectPath, fileFromProjectPath, Path.Combine(folderLink, fileFromFolderPath));
            }
        }

        private static string GetRelativePath(string folderPath, string file)
        {
            Uri fileUri = new Uri(file);

            Uri folderUri = new Uri( folderPath.EndsWith("\\") ? folderPath : folderPath + "\\");
            return Uri.UnescapeDataString(
                folderUri.MakeRelativeUri(fileUri)
                    .ToString()
                    .Replace('/', Path.DirectorySeparatorChar)
            );
        }

        public void AddFileAsLink(string projectPath, string filePath, string link)
        {
            var project = Project.Load(projectPath);

            if (LinkExists(project, filePath))
            {
                return;
            }

            var compile = new Compile(filePath) { Link = link };
            var itemGroup = FindOrCreateLinkItemGroup(project);

            itemGroup.AddContent(compile);

            project.Save(projectPath);

        }

        private ItemGroup FindOrCreateLinkItemGroup(Project project)
        {
            var itemGroup = project.ItemGroups
                .FirstOrDefault(group =>
                    group.Contents.Any(content =>
                        content is Compile compile
                        && compile.Link != null
                        && compile.Condition == null));

            if (itemGroup == null)
            {
                itemGroup = new ItemGroup();
                project.AddItemGroup(itemGroup);
            }

            return itemGroup;
        }

        private bool LinkExists(Project project, string filePath)
        {
            return project.ItemGroupContentExists<Compile>(filePath);
        }
    }
}
