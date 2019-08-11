using System;
using FolderMirroringTool.Core;

namespace FolderMirroringTool.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            new FolderReflector().FullMirroring(
                @"C:\Users\fatku\source\repos\FolderMirroringTool\FolderMirroringTool.TestLib\FolderMirroringTool.TestLib.csproj");
        }
    }
}
