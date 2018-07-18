namespace HeadsUp
{
    using UnityEngine;

    public class MenusManager : MonoBehaviour
    {
        public GameObject MainMenu;
        public GameObject RotateMenu;
        public GameObject ScaleMenu;
        public GameObject TranslateMenu;
        public GameObject ThisModel;
        public GameObject ModelManagerPrefab;

        public void ReturnHome(GameObject pSender)
        {

            //MainMenu.SetActive(true);
            pSender.SetActive(false);
            //MainMenu.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.4f, 1.0f));
        }

        public void ShowRotateMenu()
        {

            this.RotateMenu.SetActive(true);
        }

        public void ShowScaleMenu()
        {

            this.ScaleMenu.SetActive(true);
        }

        public void ShowPositionMenu()
        {

            this.TranslateMenu.SetActive(true);
        }

        public void RemoveThisModel()
        {

            Destroy(ThisModel);
        }

        public void OpenNewFile()
        {
            Instantiate(ModelManagerPrefab, Vector3.zero, Quaternion.identity, transform);
            //#if ENABLE_WINMD_SUPPORT
            //            UnityEngine.WSA.Application.InvokeOnUIThread(OpenFileAsync, false);
            //#endif
        }

//#if ENABLE_WINMD_SUPPORT
//        public async void OpenFileAsync()
//        {
//            Debug.LogFormat("OpenFileAsync() on Thread: {0}", Thread.CurrentThread.ManagedThreadId);

//            openPicker = new FileOpenPicker();

//            openPicker.FileTypeFilter.Add(".zip");

//            var file = await openPicker.PickSingleFileAsync();

//            byte[] fileBytes = null;
//            using (var stream = await file.OpenReadAsync())
//            {
//                fileBytes = new byte[stream.Size];
//                using (var reader = new DataReader(stream))
//                {
//                    await reader.LoadAsync((uint)stream.Size);
//                    reader.ReadBytes(fileBytes);
//                }
//            }

//            UnityEngine.WSA.Application.InvokeOnAppThread(new AppCallbackItem(() => { models = SaveToTemporaryFileAsync(file.Name, fileBytes); }), false);
//            UnityEngine.WSA.Application.InvokeOnAppThread(new AppCallbackItem(ImportData), false);
//        }
//#endif

//#if ENABLE_WINMD_SUPPORT
//        public void ImportData()
//        {
//            models.ContinueWith((pathList) =>
//            {
//                foreach (string objPath in pathList.Result)
//                {
//                    Debug.Log("OBJ Path: " + objPath);
//                    //int count = Facade.ImportObjects(objPath);

//                    //var verts = ConvertVertices();
//                    //var uvs = ConvertUVs();

//                    //CreateObjects(count, verts, uvs, root);
//                }
//            });
//        }
//#endif

//#if ENABLE_WINMD_SUPPORT
//        public async static Task<List<string>> SaveToTemporaryFileAsync(string fileName, byte[] fileBytes)
//        {
//            var file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
//            await FileIO.WriteBytesAsync(file, fileBytes);

//            //tempFilePath = string.Format("ms-appdata:///temp/{0}", file.Name);

//            var zipPath = Path.Combine(ApplicationData.Current.TemporaryFolder.Path, file.Name);
//            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
//            {
//                archive.ExtractToDirectory(ApplicationData.Current.TemporaryFolder.Path, true);
//            }

//            var files = await ApplicationData.Current.TemporaryFolder.GetFilesAsync();
//            var models = new List<string>();

//            foreach (var item in files)
//            {
//                if (Path.GetExtension(item.Path) != ".obj") continue;
//                models.Add(item.Path);
//            }

//            if (models == null)
//                throw new FileNotFoundException();

//            return models;
//        }
//#endif

        private void Update()
        {
            SetMenuRotation(this.MainMenu);
        }

        private void SetMenuRotation(GameObject menu)
        {
            var pivotPosition = GetHandPivotPosition();
            var draggingRotation = Quaternion.LookRotation(menu.transform.position - pivotPosition);
            menu.transform.rotation = draggingRotation;
        }

        private Vector3 GetHandPivotPosition()
        {
            var pivot = Camera.main.transform.position + new Vector3(0, -0.2f, 0) - Camera.main.transform.forward * 0.2f;
            return pivot;
        }
    }
}

