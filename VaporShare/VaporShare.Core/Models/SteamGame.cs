using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VaporShare.Core.Models
{
    public class SteamGame
    {
        public const string SubfolderName = "screenshots";

        public int Id { get; private set; }
        public string Name { get; private set; }
        public IEnumerable<ScreenshotFile> Files { get; private set; }


        public SteamGame(int id, string path)
        {
            Console.WriteLine(id);
            Id = id;

            Name = FetchGameNameFromId(id);

            var folder = Path.Combine(path, id.ToString(), SubfolderName);

            var filePaths = Directory.EnumerateFiles(folder, "*.*", SearchOption.TopDirectoryOnly);
            Files = filePaths.Select(file => new ScreenshotFile(file));
        }

        public static string FetchGameNameFromId(int id)
        {
            return "to be implemented";
        }

        public override string ToString()
        {
            return $"{Id}: {Name}";
        }
    }
}
