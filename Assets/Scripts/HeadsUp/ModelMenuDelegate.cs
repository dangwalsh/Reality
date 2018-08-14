namespace HeadsUp
{
    using UnityEngine;
    using HoloToolkit.Unity.InputModule;

    [RequireComponent(typeof(HandRotate))]
    [RequireComponent(typeof(HandScale))]
    [RequireComponent(typeof(HandTranslate))]
    [RequireComponent(typeof(HandElevate))]
    public class ModelMenuDelegate : MonoBehaviour, IInputClickHandler
    {
        public MenuManager MenusManager
        {
            get { return menusManager; }
            set
            {
                menusManager = value;
                mainMenuManager = menusManager.MainMenu.GetComponent<MenuManagerMain>();
                heightMenuManager = menusManager.HeightMenu.GetComponent<MenuManagerHeight>();
                rotateMenuManager = menusManager.RotateMenu.GetComponent<MenuManagerRotate>();
                scaleMenuManager = menusManager.ScaleMenu.GetComponent<MenuManagerScale>();
                translateMenuManager = menusManager.TranslateMenu.GetComponent<MenuManagerTranslate>();
            }
        }

        MenuManager menusManager;
        MenuManagerMain mainMenuManager;
        MenuManagerHeight heightMenuManager;
        MenuManagerRotate rotateMenuManager;
        MenuManagerScale scaleMenuManager;
        MenuManagerTranslate translateMenuManager;

        #region MonoBehaviour Members
        void Start()
        {
            MenusManager = GameObject.Find("MenusManager").GetComponent<MenuManager>();
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
