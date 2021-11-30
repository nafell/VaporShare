using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaporShare.Core.Models
{
    public class SteamScreenshots
    {
        //private const string steamFilepath = @"C:\Program Files (x86)\Steam\userdata\151574282\760\remote\";
        private const string steamInstallationPath = @"C:\Program Files (x86)\Steam\";

        public IEnumerable<SteamGame> Games { get; private set; }

        public SteamScreenshots()
        {
            var directory = ScreenshotDirectoryPath(steamInstallationPath);
            var ids = IndexIDs(directory);

            var games = ids.Select(id => new SteamGame(id, directory));
            Games = games;
        }

        public static string ScreenshotDirectoryPath(string steamInstallLocation)
        {
            var userdataPath = Path.Combine(steamInstallLocation, "userdata");
            var userIdPath = FirstSubDirectoryPath(userdataPath);
            var screenshotPath = Path.Combine(userIdPath, @"760\remote");

            return screenshotPath;
        }

        //userid folder; change this implementation to include all users' screenshots
        public static string FirstSubDirectoryPath(string path)
        {
            var folders = Directory.EnumerateDirectories(path);

            if (!folders.Any())
                throw new InvalidOperationException("The directory did not contain any folder");

            return folders.First();
        }

        public static IEnumerable<int> IndexIDs(string path)
        {
            var folders = Directory.EnumerateDirectories(path, "", SearchOption.TopDirectoryOnly);
            
            return folders.Select(folder => int.Parse(Path.GetFileName(folder)));
        }
    }
}
