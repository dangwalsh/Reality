namespace HeadsUp {

    using UnityEngine;
    using HoloToolkit.Unity.InputModule;

    public class MenuDelegate : MonoBehaviour, IInputClickHandler {

        public GameObject MenuSystem;

        public void OnInputClicked(InputEventData eventData) {
            if (!MenuSystem.activeSelf) {
                MenuSystem.SetActive(!MenuSystem.activeSelf);
            }
            else {
                MenuSystem.GetComponent<MainMenuManager>().DismissMenu();
            }
        }
    }
}
