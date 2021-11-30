using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VaporShare.Core.Models
{
    public static class SteamCatalog
    {
        private const string CatalogUri = @"http://api.steampowered.com/ISteamApps/GetAppList/v0002/";

        private const string LocalCatalogFileName = @"catalog.json";

        private static string LocalCatalogPath { get; }

        static SteamCatalog()
        {
            LocalCatalogPath = Path.GetFullPath(LocalCatalogFileName);
        }

        public static async Task<IEnumerable<SteamAppIdNamePair>> FetchNeededCatalog(IEnumerable<int> idsNeeded)
        {
            var catalogAll = await LoadLocalCatalog();

            // || ids.Any(id => !catalogAll.Any(app => app.appid == id))
            if (catalogAll == null)
            {
                await DowanloadGameCatalog();
            }

            catalogAll = await LoadLocalCatalog();

            //???? memory leak:
            //var catalog = catalogAll.Where(app => idsNeeded.Contains(app.appid));

            var a = catalogAll.First(app => app.appid == 730);

            return catalogAll;
        }

        private static async Task<IEnumerable<SteamAppIdNamePair>?> LoadLocalCatalog()
        {
            if (!File.Exists(LocalCatalogPath))
            {
                await DowanloadGameCatalog();
            }

            var file = "";
            using (var reader = File.OpenText(LocalCatalogPath))
            {
                file = await reader.ReadToEndAsync();
            }

            var serialized = JsonConvert.DeserializeObject<CatalogJson>(file);

            return serialized.applist.apps;
        }

        private static async Task DowanloadGameCatalog()
        {
            var client = new WebClient();
            await client.DownloadFileTaskAsync(CatalogUri, LocalCatalogPath);
        }
    }

    public struct SteamAppIdNamePair 
    {
        public int appid;
        public string name;
    }

    public struct Applist 
    {
        public List<SteamAppIdNamePair> apps;
    }

    public struct CatalogJson
    {
        public Applist applist;
    }


}
