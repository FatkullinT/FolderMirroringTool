using System;
using FolderMirroringTool.Core;

namespace FolderMirroringTool.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            new FolderReflector().FullMirroring(
                @"c:\Projects\INNF\epm-innf\Backend\InnerFocus.Backend.Web\InnerFocus.Backend.Web.csproj");
        }
    }
}
