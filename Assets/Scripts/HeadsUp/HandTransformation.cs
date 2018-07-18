namespace HeadsUp {

    using System;
    using UnityEngine;
    using HoloToolkit.Unity;
    using HoloToolkit.Unity.InputModule;
    using HoloToolkit.Unity.SpatialMapping;

    public abstract class HandTransformation : MonoBehaviour, IFocusable, IInputHandler, ISourceStateHandler {
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




        //[Tooltip("Supply a friendly name for the anchor as the key name for the WorldAnchorStore.")]
        //public string SavedAnchorFriendlyName = "SavedAnchorFriendlyName";

        //[Tooltip("Place parent on tap instead of current game object.")]
        //public bool PlaceParentOnTap;

        //[Tooltip("Specify the parent game object to be moved on tap, if the immediate parent is not desired.")]
        //public GameObject ParentGameObjectToPlace;

        [Tooltip("Setting this to true will enable the user to move and place the object in the scene without needing to tap on the object. Useful when you want to place an object immediately.")]
        public bool IsBeingPlaced;




        public float QuantizeValue {
            get;
            set;
        }

        public float TransformValue { get; set; }

        private float handValue;
        public float HandValue {
            get {
                return handValue;
            }
            protected set {
                var temp = value;
                temp = (temp > 1.0f) ? 1.0f : temp;
                temp = (temp < -1.0f) ? -1.0f : temp;
                handValue = temp;
            }
        }

        private bool isGazed;
        private bool isTransforming;
        private uint currentInputSourceId;
        private IInputSource currentInputSource;
        private Quaternion gazeAngularOffset;
        private float handRefDistance;
        private float objRefDistance;

        protected Camera mainCamera;
        protected Vector3 objRefGrabPoint;
        protected Vector3 draggingPosition;
        protected Vector3 objRefForward;
        protected Quaternion draggingRotation;
        //protected WorldAnchorManager anchorManager;
        //protected SpatialMappingManager spatialMappingManager;
        protected ModelAnchorManager modelAnchorManager;

        public void OnFocusEnter() {
            if (isGazed)
                return;
            isGazed = true;
        }

        public void OnFocusExit() {
            isGazed = false;
        }

        public void OnInputDown(InputEventData eventData) {
            currentInputSource = eventData.InputSource;
            currentInputSourceId = eventData.SourceId;
            StartTransform();
        }

        public void OnInputUp(InputEventData eventData) {

            StopTransform();
        }

        public void OnSourceDetected(SourceStateEventData eventData) {
            // do nothing
        }

        public void OnSourceLost(SourceStateEventData eventData) {
            StopTransform();
        }




        public void OnClick() {

            // On each tap gesture, toggle whether the user is in placing mode.
            IsBeingPlaced = !IsBeingPlaced;

            // If the user is in placing mode, display the spatial mapping mesh.
            if (IsBeingPlaced) {
                modelAnchorManager.SpatialMappingManager.DrawVisualMeshes = true;

                Debug.Log(gameObject.name + " : Removing existing world anchor if any.");

                modelAnchorManager.AnchorManager.RemoveAnchor(gameObject);
            }
            // If the user is not in placing mode, hide the spatial mapping mesh.
            else {
                modelAnchorManager.SpatialMappingManager.DrawVisualMeshes = false;
                // Add world anchor when object placement is done.
                modelAnchorManager.AnchorManager.AttachAnchor(gameObject, modelAnchorManager.SavedAnchorFriendlyName);
            }
        }




        private void StartTransform() {
            InputManager.Instance.PushModalInputHandler(gameObject);
            isTransforming = true;

            //StatsUpdater.TargetTransform = this.transform;

            Vector3 gazeHitPosition = GazeManager.Instance.HitInfo.point;

            Vector3 handPosition;

            currentInputSource.TryGetGripPosition(currentInputSourceId, out handPosition);

            Vector3 pivotPosition = GetHandPivotPosition();
            handRefDistance = Vector3.Magnitude(handPosition - pivotPosition);
            objRefDistance = Vector3.Magnitude(gazeHitPosition - pivotPosition);

            objRefGrabPoint = mainCamera.transform.InverseTransformDirection(HostTransform.position - gazeHitPosition);

            Vector3 objForward = HostTransform.forward;
            Vector3 objDirection = Vector3.Normalize(gazeHitPosition - pivotPosition);
            Vector3 handDirection = Vector3.Normalize(handPosition - pivotPosition);

            objForward = mainCamera.transform.InverseTransformDirection(objForward);
            objDirection = mainCamera.transform.InverseTransformDirection(objDirection);
            handDirection = mainCamera.transform.InverseTransformDirection(handDirection);

            objRefForward = objForward;

            gazeAngularOffset = Quaternion.FromToRotation(handDirection, objDirection);
            draggingPosition = gazeHitPosition;

            StartedTransformation.RaiseEvent();
        }

        private void UpdateTransform() {
            // get the hand's current position vector
            Vector3 newHandPosition;

            currentInputSource.TryGetGripPosition(currentInputSourceId, out newHandPosition);
            // this estimates where your head is by using the camera's position
            Vector3 pivotPosition = GetHandPivotPosition();
            // calculate the hand's direction vector relative to the head by subtracting
            // the above two vectors and normalizing
            Vector3 newHandDirection = Vector3.Normalize(newHandPosition - pivotPosition);
            // change that direction from world space to local camera space that way 
            // it doesn't matter which way the head is pointed
            newHandDirection = mainCamera.transform.InverseTransformDirection(newHandDirection);
            // normalize to get just the direction
            Vector3 targetDirection = Vector3.Normalize(gazeAngularOffset * newHandDirection);
            targetDirection = mainCamera.transform.TransformDirection(targetDirection);

            // find the distance the hand is from the head
            float currenthandDistance = Vector3.Magnitude(newHandPosition - pivotPosition);
            // find the ratio of the current hand distance to the original distance
            float distanceRatio = currenthandDistance / handRefDistance;
            // scale the movement based on how close or far the hand is (probably adds stability)
            float distanceOffset = distanceRatio > 0 ? (distanceRatio - 1f) * DistanceScale : 0;
            // a measure of the the distance of the object from the head plus the stabilizing offset
            float targetDistance = objRefDistance + distanceOffset;

            // multiply target direction vector by target magnitude to get target position
            // then add to pivot position to put back in world position
            Vector3 newDraggingPosition = pivotPosition + (targetDirection * targetDistance);
            // change that direction from world space to local camera space
            // that way it doesn't matter which way the head is pointed
            Vector3 delta = mainCamera.transform.InverseTransformDirection(draggingPosition - newDraggingPosition);

            ApplyTransformation(pivotPosition, newDraggingPosition, delta);
        }

        protected abstract void ApplyTransformation(Vector3 pivotPosition, Vector3 newPosition, Vector3 delta, bool isPerpetual = true);

        private void StopTransform() {
            if (!isTransforming)
                return;

            InputManager.Instance.PopModalInputHandler();
            //StatsUpdater.TargetTransform = null;
            isTransforming = false;
            currentInputSource = null;
            StoppedTransformation.RaiseEvent();
        }

        private Vector3 GetHandPivotPosition() {
            Vector3 pivot = Camera.main.transform.position + new Vector3(0, -0.2f, 0) - Camera.main.transform.forward * 0.2f; // a bit lower and behind
            return pivot;
        }

        private void Start() {
            if (HostTransform == null) {
                HostTransform = transform;
            }
            mainCamera = Camera.main;

            modelAnchorManager = GetComponent<ModelAnchorManager>();

            //// Make sure we have all the components in the scene we need.
            //anchorManager = WorldAnchorManager.Instance;
            //if (anchorManager == null) {
            //    Debug.LogError("This script expects that you have a WorldAnchorManager component in your scene.");
            //}

            //spatialMappingManager = SpatialMappingManager.Instance;
            //if (spatialMappingManager == null) {
            //    Debug.LogError("This script expects that you have a SpatialMappingManager component in your scene.");
            //}

            //if (anchorManager != null && spatialMappingManager != null) {
            //    anchorManager.AttachAnchor(gameObject, SavedAnchorFriendlyName);
            //}
            //else {
            //    // If we don't have what we need to proceed, we may as well remove ourselves.
            //    Destroy(this);
            //}

            ////if (PlaceParentOnTap) {
            ////    if (ParentGameObjectToPlace != null && !gameObject.transform.IsChildOf(ParentGameObjectToPlace.transform)) {
            ////        Debug.LogError("The specified parent object is not a parent of this object.");
            ////    }

            ////    DetermineParent();
            ////}



        }

        private void Update() {
            if (isTransforming)
                UpdateTransform();
        }

        private void OnEnable() {
            this.HandValue = 0.0f;
        }




        //private void DetermineParent() {
        //    if (ParentGameObjectToPlace == null) {
        //        if (gameObject.transform.parent == null) {
        //            Debug.LogError("The selected GameObject has no parent.");
        //            PlaceParentOnTap = false;
        //        }
        //        else {
        //            Debug.LogError("No parent specified. Using immediate parent instead: " + gameObject.transform.parent.gameObject.name);
        //            ParentGameObjectToPlace = gameObject.transform.parent.gameObject;
        //        }
        //    }
        //}




    }
}
