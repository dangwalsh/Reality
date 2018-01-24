namespace HeadsUp {

    using System.Collections;
    using UnityEngine;

    public class ModelInstance : MonoBehaviour {

        [Tooltip("The GameObject displayed while loading content.")]
        public GameObject LoadingScreen;

        #region MonoBehaviour Members
        void Start() {
            LoadingScreen.SetActive(true);
            Time.timeScale = 0.0f;

            AttachMenuSystem();
            var path = UnityEngine.WSA.Application.arguments.Replace("File=", "");        
            var adapter = gameObject.GetComponent<GeometryAdapter>();
            adapter.Initialize(path, gameObject);

            StartCoroutine(RemoveLoadingScreen());
        }
        #endregion

        void AttachMenuSystem() {
            var menuDelegate = gameObject.GetComponent<MenuDelegate>();
            var menusManagerObject = GameObject.Find("MenusManager");
            var menusManager = menusManagerObject.GetComponent<MenusManager>();
            menuDelegate.MenusManager = menusManager;
        }

        IEnumerator RemoveLoadingScreen() {
            yield return new WaitForSecondsRealtime(1);
            LoadingScreen.SetActive(false);

            Time.timeScale = 1.0f;
        }
    }
}

