namespace HeadsUp
{

    using UnityEngine;
    using HoloToolkit.Unity.InputModule;

    [RequireComponent(typeof(HandRotate))]
    [RequireComponent(typeof(HandScale))]
    [RequireComponent(typeof(HandTranslate))]
    public class MenuDelegate : MonoBehaviour, IInputClickHandler
    {

        public MenusManager MenusManager
        {
            get { return menusManager; }
            set
            {
                menusManager = value;
                mainMenuManager = menusManager.MainMenu.GetComponent<MainMenuManager>();
                rotateMenuManager = menusManager.RotateMenu.GetComponent<RotateMenuManager>();
                scaleMenuManager = menusManager.ScaleMenu.GetComponent<ScaleMenuManager>();
                translateMenuManager = menusManager.TranslateMenu.GetComponent<TranslateMenuManager>();
            }
        }

        MenusManager menusManager;
        MainMenuManager mainMenuManager;
        RotateMenuManager rotateMenuManager;
        ScaleMenuManager scaleMenuManager;
        TranslateMenuManager translateMenuManager;

        #region MonoBehaviour Members
        void Start()
        {
            MenusManager = GameObject.Find("MenusManager").GetComponent<MenusManager>();
        }
        #endregion

        public void OnInputClicked(InputClickedEventData eventData)
        {
            if (eventData.used) return;

            if (!menusManager.MainMenu.activeSelf)
            {
                menusManager.ThisModel = gameObject;
                rotateMenuManager.handController = GetComponent<HandRotate>();
                scaleMenuManager.handController = GetComponent<HandScale>();
                translateMenuManager.handController = GetComponent<HandTranslate>();

                mainMenuManager.ShowMenu();
            }
            else
            {
                menusManager.ThisModel = null;
                rotateMenuManager.handController = null;
                scaleMenuManager.handController = null;
                translateMenuManager.handController = null;

                mainMenuManager.DismissMenu();
            }

            eventData.Use();
        }
    }
}
