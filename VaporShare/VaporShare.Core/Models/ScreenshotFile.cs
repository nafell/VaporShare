using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VaporShare.Core.Models
{
    public class ScreenshotFile
    {
        public const string ThumbnailFolderName = "thumbnails";
        public string FilePath { get; private set; }
        public string ThumbnailPath { get; private set; }

        public FileInfo FileInfo { get; set; }

        public ScreenshotFile(string path)
        {
            FilePath = path;
            ThumbnailPath = Path.Combine(path, ThumbnailFolderName, Path.GetFileName(path));
            FileInfo = new FileInfo(path);
        }
    }
}
