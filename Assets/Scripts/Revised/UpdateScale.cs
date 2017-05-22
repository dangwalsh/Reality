using UnityEngine;

public class UpdateScale : MonoBehaviour {

    [Tooltip("Multiplier for scale")]
    public float sizeAdjustment = 0.2f;

    [Tooltip("Parent object from which to derive scale.")]
    public GameObject targetObject;

    private Vector3 initialScale;
    private Vector3 oldTargetScale;
    private float sizeFactor;

    /// <summary>
    /// Called once after instantiation.
    /// </summary>
    private void Start() {

        var initBounds = this.targetObject.GetComponent<InitBounds>();
        var bounds = initBounds.Bounds;

        this.oldTargetScale = this.targetObject.transform.localScale;
        this.sizeFactor = bounds.extents.magnitude * this.sizeAdjustment;
        this.initialScale = this.transform.localScale;
        this.transform.position = bounds.min;
    }

    /// <summary>
    /// Called once per frame.
    /// </summary>
    private void Update() {

        var invertTrans = InvertTransformMatrix(this.oldTargetScale, this.targetObject.transform.localScale);
        this.transform.localScale = Vector3.Scale(this.initialScale, invertTrans);
        
        var invertCam = InvertCameraMatrix();
        this.transform.localScale = invertCam;

        this.oldTargetScale = this.targetObject.transform.localScale;
    }

    private Vector3 InvertTransformMatrix(Vector3 oldScale, Vector3 newScale) {

        var inverted = new Vector3(oldScale.x / newScale.x, oldScale.y / newScale.y, oldScale.z / newScale.z);
        return inverted;
    }

    /// <summary>
    /// Scale independent of perspetive distance and object size.
    /// </summary>
    /// <returns></returns>
    private Vector3 InvertCameraMatrix() {

        var distance = CalculateDistance();
        var normalizedScale = distance * this.sizeFactor * this.initialScale;
        return normalizedScale;
    }

    /// <summary>
    /// Returns a scalar value based on the distance of the camera to the object.
    /// </summary>
    /// <returns></returns>
    private float CalculateDistance() {

        float scale = (Camera.main.transform.position - this.transform.position).magnitude;
        return scale;
    }

    /// <summary>
    /// Returns a scalar value based on the size of the screen.
    /// </summary>
    /// <returns></returns>
    private float CalculateScreenSize() {

        float scale = Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height;
        return scale;
    }
}
