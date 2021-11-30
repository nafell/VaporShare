using Epoxy;
using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VaporShare.Core.Models;

namespace VaporShare.ViewModels
{
    [ViewModel]
    public sealed class ScreenshotFileViewModel
    {
        public string ScreenshotFilePath { get; set; }
        public ImageSource? Thumbnail { get; set; }
        public string ThumbnailFilePath { get; set; }
        public int GameId { get; set; }
        public string GameName { get; set; }
        public DateTime DateTaken { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public long FileSize { get; set; }
        

        public ScreenshotFileViewModel(ScreenshotFile screenshotFile, int gameId, IEnumerable<SteamAppIdNamePair> catalog)
        {
            ScreenshotFilePath = screenshotFile.FilePath;
            ThumbnailFilePath = screenshotFile.ThumbnailPath;
            Thumbnail = ShellFile.FromFilePath(ScreenshotFilePath).Thumbnail.BitmapSource;
            GameId = gameId;
            DateTaken = screenshotFile.FileInfo.CreationTime;
            GameName = catalog.FirstOrDefault(game => game.appid == gameId).name;
        }
    }
}
