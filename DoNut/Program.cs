using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;

namespace DoNut
{
    internal class Program
    {
        private static string _patchName = string.Empty;

        private static readonly List<string> _ignoreFolders = new List<string>() { "script, config" };

        private static void Main(string[] args)
        {
            _patchName = Prompt("Patch folder name (for example, \"c0patch\"): ");

            string mainJsonPath = $"{_patchName}_info.json";

            UpdateJson(mainJsonPath, GetFileInfo());
        }

        private static ArchiveInfo GetFileInfo()
        {
            var archiveInfo = new ArchiveInfo();

            string[] directories = Directory.GetDirectories(_patchName);

            foreach (string directory in directories)
            {
                if (_ignoreFolders.Contains(directory))
                {
                    continue;
                }

                string[] files = Directory.GetFiles(directory, "*.m");

                foreach (string file in files)
                {
                    string token = string.Concat(Path.GetFileName(directory), "/", Path.GetFileName(file));

                    if (!archiveInfo.file_info.ContainsKey(token))
                    {
                        archiveInfo.file_info.Add(token, new int[] { 0, 0 });
                    }
                }
            }

            return archiveInfo;
        }

        private static void UpdateJson(string jsonPath, ArchiveInfo archiveInfo)
        {
            File.WriteAllText(jsonPath, JsonConvert.SerializeObject(archiveInfo, Formatting.Indented));
        }

        private static string Prompt(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        public class ArchiveInfo
        {
            public List<string> expire_suffix_list { get; set; } = new List<string>() { ".psb.m" };

            public Dictionary<string, int[]> file_info { get; set; } = new Dictionary<string, int[]>();

            public string info { get; set; } = "archive";

            public double version { get; set; } = 1.0;
        }
    }
}
