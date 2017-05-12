using System;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

[RequireComponent(typeof(AudioSource))]
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

    private bool isGazed;
    private bool isTransforming;
    private uint currentInputSourceId;
    private IInputSource currentInputSource;
    private Quaternion gazeAngularOffset;
    private float handRefDistance;
    private float objRefDistance;
    private AudioSource audioSource;
    private AudioClip inputDown;

    protected Camera mainCamera;
    protected Vector3 objRefGrabPoint;
    protected Vector3 draggingPosition;
    protected Vector3 objRefForward;
    protected Quaternion draggingRotation;

    public void OnFocusEnter() {
        if (isGazed)
            return;
        isGazed = true;
    }

    public void OnFocusExit() {
        isGazed = false;
    }

    public void OnInputDown(InputEventData eventData) {
        audioSource.PlayOneShot(inputDown);
        currentInputSource = eventData.InputSource;
        currentInputSourceId = eventData.SourceId;
        StartTransform();
    }

    public void OnInputUp(InputEventData eventData) {
        audioSource.PlayOneShot(inputDown);
        StopTransform();
    }

    public void OnSourceDetected(SourceStateEventData eventData) {
        // do nothing
    }

    public void OnSourceLost(SourceStateEventData eventData) {
        audioSource.PlayOneShot(inputDown);
        StopTransform();
    }

    private void StartTransform() {
        InputManager.Instance.PushModalInputHandler(gameObject);
        isTransforming = true;

        StatsUpdater.TargetTransform = this.transform;

        Vector3 gazeHitPosition = GazeManager.Instance.HitInfo.point;

        Vector3 handPosition;
        currentInputSource.TryGetPosition(currentInputSourceId, out handPosition);

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
        currentInputSource.TryGetPosition(currentInputSourceId, out newHandPosition);
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

    protected abstract void ApplyTransformation(Vector3 pivotPosition, Vector3 newPosition, Vector3 delta);

    private void StopTransform() {
        if (!isTransforming)
            return;

        InputManager.Instance.PopModalInputHandler();
        StatsUpdater.TargetTransform = null;
        isTransforming = false;
        currentInputSource = null;
        StoppedTransformation.RaiseEvent();
    }

    private Vector3 GetHandPivotPosition() {
        Vector3 pivot = Camera.main.transform.position + new Vector3(0, -0.2f, 0) - Camera.main.transform.forward * 0.2f; // a bit lower and behind
        return pivot;
    }

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1.0f;
    }

    private void Start() {
        inputDown = Resources.Load<AudioClip>("Sounds/Clap Simple Snap");
        if (HostTransform == null) {
            HostTransform = transform;
        }
        mainCamera = Camera.main;
    }

    private void Update() {
        if (isTransforming)
            UpdateTransform();
    }
}
