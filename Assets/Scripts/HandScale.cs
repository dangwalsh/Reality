using UnityEngine;

namespace HoloToolkit.Unity.InputModule
{
    public class HandScale : MonoBehaviour, IFocusable, IInputHandler, ISourceStateHandler
    {
        public Transform HostTransform;
        public float Factor = 5.0f;
        public float Minimum = 0.01f;
        public float Maximum = 100.0f;

        bool isGazed;
        bool isTransforming;
        uint currentInputSourceId;
        IInputSource currentInputSource;
        Vector3 currentHandPosition;
        Vector3 newTransform;
        Vector3 currentTransform;

        public void OnFocusEnter()
        {
            if (isGazed)
                return;
            isGazed = true;
        }

        public void OnFocusExit()
        {
            isGazed = false;
        }

        public void OnInputDown(InputEventData eventData)
        {
            currentInputSource = eventData.InputSource;
            currentInputSourceId = eventData.SourceId;
            StartTransform();
        }

        public void OnInputUp(InputEventData eventData)
        {
            StopTransform();
        }

        public void OnSourceDetected(SourceStateEventData eventData)
        {
            // do nothing
        }

        public void OnSourceLost(SourceStateEventData eventData)
        {
            StopTransform();
        }

        void StartTransform()
        {
            InputManager.Instance.PushModalInputHandler(gameObject);
            isTransforming = true;
            Vector3 handPosition;
            currentInputSource.TryGetPosition(currentInputSourceId, out handPosition);
            currentHandPosition = handPosition;
            newTransform = HostTransform.localScale;
        }

        void UpdateTransform()
        {
            Vector3 newHandPosition;
            currentInputSource.TryGetPosition(currentInputSourceId, out newHandPosition);
            Vector3 delta = newHandPosition - currentHandPosition;
            newTransform = newTransform + new Vector3(delta.x * Factor, delta.x * Factor, delta.x * Factor);

            if (newTransform.x <= Minimum)
                newTransform = currentTransform;

            HostTransform.localScale = newTransform;
            currentHandPosition = newHandPosition;
            currentTransform = newTransform;
        }

        void StopTransform()
        {
            if (!isTransforming)
                return;

            InputManager.Instance.PopModalInputHandler();
            isTransforming = false;
            currentInputSource = null;
        }

        void Start()
        {
            if (HostTransform == null)
                HostTransform = transform;
        }

        void Update()
        {
            if (isTransforming)
                UpdateTransform();
        }
    }
}



