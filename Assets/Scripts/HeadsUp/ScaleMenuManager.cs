namespace HeadsUp {
    using UnityEngine;

    public class ScaleMenuManager : MonoBehaviour {
        public GameObject leftIndicator;
        public GameObject rightIndicator;
        public GameObject leftControl;
        public GameObject rightControl;
        public TextMesh textField;

        public MenusManager menuController;
        public HandScale handController;
        public Camera sceneCamera;
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

        private void Start() {

            ResetControls();
        }

        private void Update() {

            if (handController.HandValue < 0) {
                leftControl.SetActive(false);
                leftIndicator.SetActive(false);
                rightControl.SetActive(true);
                rightIndicator.SetActive(true);
                rightControl.transform.localPosition = new Vector3(Mathf.Abs(handController.HandValue), 0, 0);
            }
            else if (handController.HandValue > 0) {
                rightControl.SetActive(false);
                rightIndicator.SetActive(false);
                leftControl.SetActive(true);
                leftIndicator.SetActive(true);
                leftControl.transform.localPosition = new Vector3(Mathf.Abs(handController.HandValue), 0, 0);
            }
            else {
                ResetControls();
            }

            textField.text = handController.TransformValue.ToString("n3");
        }


        private void OnTransformationStarted() {

            isTransforming = true;
        }

        private void OnTransformationStopped() {

            isTransforming = false;
            menuController.ReturnHome(gameObject);
        }

        private void ResetControls() {

            if (gameObject.activeInHierarchy) handController.enabled = true;

            leftControl.transform.localPosition = Vector3.zero;
            rightControl.transform.localPosition = Vector3.zero;
            leftControl.SetActive(false);
            leftIndicator.SetActive(false);
            rightControl.SetActive(false);
            rightIndicator.SetActive(false);
        }
    }
}
