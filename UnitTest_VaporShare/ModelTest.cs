using NUnit.Framework;
using System.Linq;
using VaporShare.Core.Models;

namespace UnitTest_VaporShare
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void IndexGameIds()
        {
            var ids = SteamScreenshots.IndexIDs(SteamScreenshots.ScreenshotDirectoryPath(@"C:\Program Files (x86)\Steam\"));
            
            if (ids == null)
            {
                Assert.Fail();
                return;
            }
            
            if (!ids.Contains(730))
            {
                Assert.Fail();
                return;
            }

            Assert.Pass();
        }

        [Test]
        public void ScreenshotPath()
        {
            var folder = SteamScreenshots.ScreenshotDirectoryPath(@"C:\Program Files (x86)\Steam\");

            System.Console.WriteLine(folder);

            if (!folder.Contains("remote"))
            {
                Assert.Fail();
                return;
            }

            Assert.Pass();
        }
    }
}