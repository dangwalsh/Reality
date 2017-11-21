namespace HeadsUp {

    using UnityEngine;
    using HoloToolkit.Unity.InputModule;

    public class MenuDelegate : MonoBehaviour, IInputClickHandler {

        public GameObject MenusManagerObject {
            get { return menusManagerObject; }
            set {
                menusManagerObject = value;
                menusManager = menusManagerObject.GetComponent<MenusManager>();
                mainMenuObject = menusManager.MainMenu;
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
        GameObject menusManagerObject;
        GameObject mainMenuObject;

        public void OnInputClicked(InputEventData eventData) {

            if (!mainMenuObject.activeSelf) {
     
                rotateMenuManager.handController = GetComponent<HandRotate>();
                scaleMenuManager.handController = GetComponent<HandScale>();
                translateMenuManager.gazeController = GetComponent<GazeTranslate>();

                mainMenuObject.SetActive(!mainMenuObject.activeSelf);
            }
            else {

                mainMenuManager.DismissMenu();
            }
        }
    }
}
