namespace HeadsUp {

    using UnityEngine;

    public class TranslateMenuManager : MonoBehaviour {

        public MenusManager menuController;
        public GazeTransformation gazeController;
        public Transform mainMenu;
        public PlacementController placement;

        bool isTransforming;

        private void OnEnable() {
            if (gazeController == null) return;

            gazeController.StartedTransformation += OnTransformationStarted;
            gazeController.StoppedTransformation += OnTransformationStopped;
            gazeController.enabled = true;

            gameObject.transform.position = mainMenu.position;
            gameObject.transform.rotation = mainMenu.rotation;
        }

        private void OnDisable() {
            if (gazeController == null) return;

            gazeController.StartedTransformation -= OnTransformationStarted;
            gazeController.StoppedTransformation -= OnTransformationStopped;
            gazeController.enabled = false;
        }

        private void OnTransformationStarted() {
            isTransforming = true;
            placement.OnClick();
        }

        private void OnTransformationStopped() {
            isTransforming = false;
            placement.OnClick();
            menuController.ReturnHome(gameObject);
        }
    }
}
