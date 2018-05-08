﻿namespace HeadsUp {

    using UnityEngine;
    using HoloToolkit.Unity.InputModule;

    [RequireComponent(typeof(HandRotate))]
    [RequireComponent(typeof(HandScale))]
    [RequireComponent(typeof(GazeTranslate))]
    [RequireComponent(typeof(PlacementController))]
    public class MenuDelegate : MonoBehaviour, IInputClickHandler {

        public MenusManager MenusManager{
            get { return menusManager; }
            set {
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

        public void OnInputClicked(InputEventData eventData) {

            if (!menusManager.MainMenu.activeSelf) {

                menusManager.ThisModel = gameObject;
                rotateMenuManager.handController = GetComponent<HandRotate>();
                scaleMenuManager.handController = GetComponent<HandScale>();
                translateMenuManager.gazeController = GetComponent<GazeTranslate>();
                translateMenuManager.placement = GetComponent<PlacementController>();
                mainMenuManager.ShowMenu();
            }
            else {

                mainMenuManager.DismissMenu();
            }
        }
    }
}
