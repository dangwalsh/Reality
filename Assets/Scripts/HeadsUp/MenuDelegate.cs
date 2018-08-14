namespace HeadsUp
{
    using UnityEngine;
    using HoloToolkit.Unity.InputModule;

    [RequireComponent(typeof(HandRotate))]
    [RequireComponent(typeof(HandScale))]
    [RequireComponent(typeof(HandTranslate))]
    [RequireComponent(typeof(HandElevate))]
    public class MenuDelegate : MonoBehaviour, IInputClickHandler
    {
        public MenusManager MenusManager
        {
            get { return menusManager; }
            set
            {
                menusManager = value;
                mainMenuManager = menusManager.MainMenu.GetComponent<MainMenuManager>();
                heightMenuManager = menusManager.HeightMenu.GetComponent<HeightMenuManager>();
                rotateMenuManager = menusManager.RotateMenu.GetComponent<RotateMenuManager>();
                scaleMenuManager = menusManager.ScaleMenu.GetComponent<ScaleMenuManager>();
                translateMenuManager = menusManager.TranslateMenu.GetComponent<TranslateMenuManager>();
            }
        }

        MenusManager menusManager;
        MainMenuManager mainMenuManager;
        HeightMenuManager heightMenuManager;
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
                heightMenuManager.handController = GetComponent<HandElevate>();
                rotateMenuManager.handController = GetComponent<HandRotate>();
                scaleMenuManager.handController = GetComponent<HandScale>();
                translateMenuManager.handController = GetComponent<HandTranslate>();

                mainMenuManager.ShowMenu();
            }
            else
            {
                menusManager.ThisModel = null;
                heightMenuManager.handController = null;
                rotateMenuManager.handController = null;
                scaleMenuManager.handController = null;
                translateMenuManager.handController = null;

                mainMenuManager.DismissMenu();
            }

            eventData.Use();
        }
    }
}
