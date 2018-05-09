namespace HeadsUp {

    using UnityEngine;

    public class TranslateMenuManager : MonoBehaviour {

        public MenusManager menuController;
        public HandTranslate handController;
        public Transform mainMenu;

        bool isTransforming;

        private void OnEnable() {
            if (handController == null) return;

            handController.StartedTransformation += OnTransformationStarted;
            handController.StoppedTransformation += OnTransformationStopped;
            handController.enabled = true;

            gameObject.transform.position = mainMenu.position;
            gameObject.transform.rotation = mainMenu.rotation;
        }

        private void OnDisable() {
            if (handController == null) return;

            handController.StartedTransformation -= OnTransformationStarted;
            handController.StoppedTransformation -= OnTransformationStopped;
            handController.enabled = false;
        }

        private void OnTransformationStarted() {
            isTransforming = true;
            handController.OnClick();
        }

        private void OnTransformationStopped() {
            isTransforming = false;
            handController.OnClick();
            menuController.ReturnHome(gameObject);
        }
    }
}
