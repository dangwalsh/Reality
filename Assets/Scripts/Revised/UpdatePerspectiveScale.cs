using UnityEngine;

public class UpdatePerspectiveScale : MonoBehaviour {

    [Tooltip("Multiplier for scale")]
    public float sizeAdjustment = 0.2f;

    [Tooltip("Parent object from which to derive scale.")]
    public GameObject targetObject;

    private Vector3 initialScale;
    private float sizeFactor;

    /// <summary>
    /// Called once after instantiation.
    /// </summary>
    private void Start() {

        var initBounds = this.targetObject.GetComponent<InitBounds>();
        var bounds = initBounds.Bounds;

        this.sizeFactor = bounds.extents.magnitude * this.sizeAdjustment;
        this.initialScale = this.transform.localScale;
        this.transform.position = bounds.min;
    }

    /// <summary>
    /// Called once per frame.
    /// </summary>
    private void Update() {
        
        var invertCam = InvertCameraMatrix();
        this.transform.localScale = invertCam;
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

        float distance = (Camera.main.transform.position - this.transform.position).magnitude;
        return distance;
    }

    /// <summary>
    /// Returns a scalar value based on the size of the screen.
    /// </summary>
    /// <returns></returns>
    private float CalculateScreenSize() {

        float size = Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height;
        return size;
    }
}
