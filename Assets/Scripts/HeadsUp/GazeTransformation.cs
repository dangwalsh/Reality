namespace HeadsUp {

    using System;
    using UnityEngine;
    using HoloToolkit.Unity;
    using HoloToolkit.Unity.InputModule;

    public abstract class GazeTransformation : MonoBehaviour, IInputClickHandler {
        public event Action StartedTransformation;
        public event Action StoppedTransformation;

        [Tooltip("Transform that will be dragged. Defaults to the object of the component.")]
        public Transform HostTransform;

        [Tooltip("Scale by which hand movement in z is multipled to move the dragged object.")]
        public float DistanceScale = 2.0f;

        [Tooltip("Should the object be oriented towards the user as it is being dragged?")]
        public bool IsOrientTowardsUser = false;

        [Tooltip("Should the object be kept upright as it is being dragged?")]
        public bool IsKeepUpright = true;

        public static bool isTransforming;
        protected float objDistance;

        void StartTransform() {
            InputManager.Instance.PushModalInputHandler(HostTransform.gameObject);
            isTransforming = true;
            objDistance = Camera.main.transform.InverseTransformDirection(HostTransform.localPosition).z;
            StartedTransformation.RaiseEvent();
        }

        void UpdateTransform() {
            ApplyTransformation(Camera.main, Screen.width, Screen.height);
        }

        void StopTransform() {
            if (!isTransforming) return;
            InputManager.Instance.PopModalInputHandler();
            isTransforming = false;
            StoppedTransformation.RaiseEvent();
        }

        protected abstract void ApplyTransformation(Camera camera, float width, float height);

        void OnEnable() {
            StartTransform();
        }

        void Start() {
            if (HostTransform == null)
                HostTransform = transform;
        }

        void Update() {
            if (isTransforming)
                UpdateTransform();
        }

        public void OnInputClicked(InputEventData eventData) {
            StopTransform();
        }
    }
}