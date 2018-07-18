namespace HeadsUp
{
    using UnityEngine;
    using UnityEngine.WSA;

    [RequireComponent(typeof(GeometryAdapter))]
    public class ModelInstance : MonoBehaviour
    {

        //[Tooltip("The GameObject displayed while loading content.")]
        //public GameObject LoadingScreen;

        #region MonoBehaviour Members
        void Start()
        {
            //AttachMenuSystem();
            //var path = UnityEngine.WSA.Application.arguments.Replace("File=", "");
            var adapter = gameObject.GetComponent<GeometryAdapter>();

#if ENABLE_WINMD_SUPPORT
            UnityEngine.WSA.Application.InvokeOnUIThread(new AppCallbackItem(() => { adapter.OpenFileAsync(); }), false);
#endif
            //adapter.Initialize(path, gameObject);
        }
        #endregion

        //void AttachMenuSystem()
        //{
        //    var menuDelegate = gameObject.GetComponent<MenuDelegate>();
        //    var menusManagerObject = GameObject.Find("MenusManager");
        //    var menusManager = menusManagerObject.GetComponent<MenusManager>();
        //    menuDelegate.MenusManager = menusManager;
        //}

        //IEnumerator RemoveLoadingScreen()
        //{
        //    yield return new WaitForSecondsRealtime(1);
        //    LoadingScreen.SetActive(false);

        //    Time.timeScale = 1.0f;
        //}
    }
}

