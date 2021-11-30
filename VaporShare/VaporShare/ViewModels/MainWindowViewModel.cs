////////////////////////////////////////////////////////////////////////////
//
// Epoxy template source code.
// Write your own copyright and note.
// (You can use https://github.com/rubicon-oss/LicenseHeaderManager)
//
////////////////////////////////////////////////////////////////////////////

using Epoxy;
using Epoxy.Synchronized;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VaporShare.Core.Models;

namespace VaporShare.ViewModels
{
    [ViewModel]
    public sealed class MainWindowViewModel : ViewModel
    {

        public MainWindowViewModel()
        {
            PreviewPanelVisibility = true;

            StatusBarNotificationText = "Notification";
            StatusBarNotificationPile = PileFactory.Create<Border>();

            ScreenshotFiles = new ObservableCollection<ScreenshotFileViewModel>();
            ScreenshotFiles.CollectionChanged += (sender, eventargs) =>
            {
                ScreenshotCount = ScreenshotFiles.Count;
            };
            
            Task nowait = LoadList();

            ItemDoubleClick = CommandFactory.Create<EventArgs>(async _ => { OpenImage(); });
            CopyPath = CommandFactory.Create<EventArgs>(async _ =>
            {
                try
                {
                    if (SelectedFile == null)
                        return;

                    Clipboard.SetText(SelectedFile.ScreenshotFilePath);

                    NotifyOnStatusbar("Copied file path!");
                }
                catch (Exception ex)
                {
                    NotifyOnStatusbar("Failed to copy file path.", ex);
                }
            });
            CopyImage = CommandFactory.Create<EventArgs>(async _ =>
            {
                try
                {
                    if (SelectedFile == null)
                        return;

                    Clipboard.SetImage(new BitmapImage(new Uri(SelectedFile.ScreenshotFilePath)));

                    NotifyOnStatusbar("Copied image!");
                }
                catch (Exception ex)
                {
                    NotifyOnStatusbar("Failed to copy image.", ex);
                }
            });
            ShowOnExplorer = CommandFactory.Create<EventArgs>(async _ =>
            {
                try
                {
                    if (SelectedFile == null)
                        return;

                    Process.Start("EXPLORER.EXE", @$"/select, ""{SelectedFile.ScreenshotFilePath}""");

                    NotifyOnStatusbar("Opened file location!");
                }
                catch (Exception ex)
                {
                    NotifyOnStatusbar("Failed to open file location.", ex);
                }
            });

        }

        private async Task LoadList()
        {
            ScreenshotFiles.Clear();

            var since = DateTime.Now - new TimeSpan(7, 0, 0, 0);

            var list = new SteamScreenshots();

            if (list == null)
                return;

            var catalog = await SteamCatalog.FetchNeededCatalog(list.Games.Select(game => game.Id));

            var unsortedFiles = new List<ScreenshotFileViewModel>();

            foreach (var game in list.Games)
            {
                foreach (var file in game.Files)
                {
                    if (file.FileInfo.CreationTime < since)
                        continue;
                    unsortedFiles.Add(
                        new ScreenshotFileViewModel(file, game.Id, catalog)
                        );
                }
            }
            unsortedFiles = unsortedFiles.OrderByDescending(file => file.DateTaken).ToList();
            foreach (var file in unsortedFiles)
            {
                ScreenshotFiles.Add(file);
            }
        }

        private void OpenImage()
        {
            try
            {
                var psi = new ProcessStartInfo(SelectedFile.ScreenshotFilePath);
                psi.UseShellExecute = true;

                Process.Start(psi);

                NotifyOnStatusbar("Image opened!");
            }
            catch (Exception ex)
            {
                NotifyOnStatusbar("Failed to open image.", ex);
            }
        }
        private async void NotifyOnStatusbar(string message, Exception error = default)
        {
            if (error != null)
            {
                IsErrorNotification = true;
                //Click for more details
            }

            if (StatusBarNotificationPile == null)
                return;

            StatusBarNotificationText = message;

            await StatusBarNotificationPile.RentAsync(async border =>
            {
                var eventArgs = new RoutedEventArgs(UIElement.GotFocusEvent);
                border.RaiseEvent(eventArgs);
            });
        }

        public ObservableCollection<ScreenshotFileViewModel>? ScreenshotFiles { get; set; }
        public ScreenshotFileViewModel? SelectedFile { get; set; }
        public int? ScreenshotCount { get; set; }
        public Command? ItemDoubleClick { get; private set; }
        public Command? CopyImage { get; private set; }
        public Command? CopyPath { get; private set; }
        public Command? ShowOnExplorer { get; private set; }
        public bool PreviewPanelVisibility { get; set; }
        public string? StatusBarNotificationText { get; set; }
        public Pile<Border>? StatusBarNotificationPile { get; set; }
        public bool IsErrorNotification { get; set; }
    }
}
