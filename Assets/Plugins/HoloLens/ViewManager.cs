#if !UNITY_EDITOR
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.Collections.Generic;

namespace Reality.HoloLens
{
    public delegate void FileChangedEventHandler(object sender, EventArgs e);
    public static class ViewManager
    {
        public static event FileChangedEventHandler FileChangedEvent;

        public static string FilePath
        {
            get { return filePath; }
            set
            {
                filePath = value;
                OnFileChangedEvent(FilePath, new EventArgs() { });
            }
        }
        public static async Task SwitchViews()
        {
            if (view3d == null) view3d = CoreApplication.MainView;
            if (view2d == null) view2d = CoreApplication.CreateNewView();

            await RunOnDispatcherAsync(view2d, SwitchViewsAndOpenFileAsync);
        }

        static async Task RunOnDispatcherAsync(CoreApplicationView view, Func<Task> action)
        {
            await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
        }

        static async Task SwitchViewsAndOpenFileAsync()
        {
            var view = ApplicationView.GetForCurrentView();
            await ApplicationViewSwitcher.SwitchAsync(view.Id);

            // Uncomment to show parent window
            // Window.Current.Content = new MainPage();
            // Window.Current.Activate();

            await OpenFilesAsync();
        }

        static async Task SwitchViewsAsync()
        {
            var view = ApplicationView.GetForCurrentView();
            await ApplicationViewSwitcher.SwitchAsync(view.Id);
            Window.Current.Activate();
        }

        static async Task OpenOneFileAsync()
        {
            var openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.Objects3D;
            openPicker.FileTypeFilter.Add(".obj");

            StorageFile inFile = await openPicker.PickSingleFileAsync();
            if (inFile != null)
            {
                string text = await FileIO.ReadTextAsync(inFile);
                var folder = ApplicationData.Current.LocalFolder;
                var outFile = await folder.CreateFileAsync(
                    inFile.Name,
                    CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(outFile, text);
                FilePath = outFile.Path;
                await RunOnDispatcherAsync(CoreApplication.MainView, SwitchViewsAsync);
            }
        }

        static async Task OpenFilesAsync()
        {
            var filePicker = new FileOpenPicker();
            filePicker.ViewMode = PickerViewMode.Thumbnail;
            filePicker.SuggestedStartLocation = PickerLocationId.Objects3D;
            filePicker.FileTypeFilter.Add(".obj");
            filePicker.FileTypeFilter.Add(".mtl");
            filePicker.FileTypeFilter.Add(".jpg");
            filePicker.FileTypeFilter.Add(".png");
            var inFiles = await filePicker.PickMultipleFilesAsync();
            var outFolder = ApplicationData.Current.LocalCacheFolder;

            foreach (var inFile in inFiles)
            {
                var buffer = await FileIO.ReadBufferAsync(inFile);
                var outFile = await outFolder.CreateFileAsync(
                    inFile.Name,
                    CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteBufferAsync(outFile, buffer);
                if (inFile.FileType == ".obj")
                {
                    FilePath = outFile.Path;
                }
            }
            await RunOnDispatcherAsync(CoreApplication.MainView, SwitchViewsAsync);
        }

        static async Task OpenFolderAsync()
        {
            var folderPicker = new FolderPicker();
            folderPicker.ViewMode = PickerViewMode.Thumbnail;
            folderPicker.SuggestedStartLocation = PickerLocationId.Objects3D;
            var pickedFolder = await folderPicker.PickSingleFolderAsync();

            IEnumerable<StorageFile> inFiles = await pickedFolder.GetFilesAsync();
            var outFolder = ApplicationData.Current.LocalFolder;

            foreach (StorageFile inFile in inFiles)
            {
                var buffer = await FileIO.ReadBufferAsync(inFile);
                var outFile = await outFolder.CreateFileAsync(
                    inFile.Name,
                    CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteBufferAsync(outFile, buffer);
                if (inFile.FileType == ".obj")
                {
                    FilePath = outFile.Path;
                }
                await RunOnDispatcherAsync(CoreApplication.MainView, SwitchViewsAsync);
            }
        }

        static void OnFileChangedEvent(object sender, EventArgs e)
        {
            FileChangedEvent?.Invoke(sender, e);
        }

        static CoreApplicationView view3d;
        static CoreApplicationView view2d;
        static string filePath = "";
    }
}
#endif