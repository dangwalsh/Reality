namespace HeadsUp
{
    using UnityEngine;
    using HoloToolkit.Unity.InputModule;

    public class MenuManagerTranslate : MonoBehaviour
    {

        public MenuManager menuController;
        public HandTranslate handController;
        public Transform mainMenu;

        LayerMask[] spatialMappingMask = { 1 << 31 };
        LayerMask[] defaultMask = { 1 << 0 | 1 << 1 | 1 << 4 | 1 << 5 | 1 << 31 };

        //bool isTransforming;

        private void OnEnable()
        {
            if (handController == null) return;

            handController.StartedTransformation += OnTransformationStarted;
            handController.StoppedTransformation += OnTransformationStopped;
            handController.enabled = true;

            gameObject.transform.position = mainMenu.position;
            gameObject.transform.rotation = mainMenu.rotation;
        }

        private void OnDisable()
        {
            if (handController == null) return;

            handController.StartedTransformation -= OnTransformationStarted;
            handController.StoppedTransformation -= OnTransformationStopped;
            handController.enabled = false;
        }

        private void OnTransformationStarted()
        {
            //isTransforming = true;
            GazeManager.Instance.RaycastLayerMasks = spatialMappingMask;
            handController.OnClick();
        }

        private void OnTransformationStopped()
        {
            //isTransforming = false;
            GazeManager.Instance.RaycastLayerMasks = defaultMask;
            handController.OnClick();
            menuController.ReturnHome(gameObject);
        }
    }
}
