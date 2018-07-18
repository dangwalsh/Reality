namespace HeadsUp
{
    using UnityEngine;
    using HoloToolkit.Unity.InputModule;

    public class SceneManager : MonoBehaviour, IInputClickHandler
    {
        public MenusManager MenusManager
        {
            get { return menusManager; }
            set
            {
                menusManager = value;
                mainMenuManager = menusManager.MainMenu.GetComponent<MainMenuManager>();
            }
        }

        MenusManager menusManager;
        MainMenuManager mainMenuManager;

        #region MonoBehaviour Members
        void Awake()
        {
            MenusManager = GameObject.Find("MenusManager").GetComponent<MenusManager>();
        }

        void OnEnable()
        {
            InputManager.Instance.PushFallbackInputHandler(gameObject);
        }

        void OnDisable()
        {
            InputManager.Instance.PopFallbackInputHandler();
        }
        #endregion

        public void OnInputClicked(InputClickedEventData eventData)
        {
            if (eventData.used) return;

            if (mainMenuManager.isActiveAndEnabled)
                mainMenuManager.DismissMenu();
            else
                mainMenuManager.ShowMenu();

            eventData.Use();
        }

        #region Open from file association
        //[Tooltip("The Prefab that gets instantiated to hold a new model.")]
        //public GameObject ModelManagerPrefab;
        //private string args = "";

        //void Update()
        //{
        //   var arguments = UnityEngine.WSA.Application.arguments;
        //    if (arguments == args || arguments == "") return;
        //    args = UnityEngine.WSA.Application.arguments;
        //    Instantiate(ModelManagerPrefab, Vector3.zero, Quaternion.identity, transform);
        //}
        #endregion
    }
}