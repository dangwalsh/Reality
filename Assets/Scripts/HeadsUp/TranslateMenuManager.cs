namespace HeadsUp {

    using UnityEngine;

    public class TranslateMenuManager : MonoBehaviour {

        public MenuController menuController;
        public GazeTransformation gazeController;
        public Transform mainMenu;

        bool isTransforming;

        private void OnEnable() {
            gazeController.enabled = true;
            gameObject.transform.position = mainMenu.position;
            gameObject.transform.rotation = mainMenu.rotation;
        }

        private void OnDisable() {
            if (gazeController == null) return;
            gazeController.enabled = false;
        }

        private void OnDestroy() {
            gazeController.StartedTransformation -= OnTransformationStarted;
            gazeController.StoppedTransformation -= OnTransformationStopped;
        }

        private void Start() {
            gazeController.StartedTransformation += OnTransformationStarted;
            gazeController.StoppedTransformation += OnTransformationStopped;
        }

        private void OnTransformationStarted() {
            isTransforming = true;
        }

        private void OnTransformationStopped() {
            isTransforming = false;
            menuController.ReturnHome(gameObject);
        }
    }
}
