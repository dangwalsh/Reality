using UnityEngine;

public class UpdateScale : MonoBehaviour {

    [Tooltip("Multiplier for scale")]
    public float sizeAdjustment = 0.2f;

    private Vector3 initialScale;
    private float sizeFactor;

    /// <summary>
    /// Called once after instantiation.
    /// </summary>
    private void Start() {

        var model = this.transform.parent.gameObject;
        var initBounds = model.GetComponent<InitBounds>();
        var bounds = initBounds.Bounds;

        this.sizeFactor = bounds.extents.magnitude * this.sizeAdjustment;
        this.initialScale = this.transform.localScale;
        this.transform.position = bounds.min;
    }

    /// <summary>
    /// Called once per frame.
    /// </summary>
    private void Update() {

        this.transform.localScale = SetScale();
    }

    /// <summary>
    /// Scale independent of perspetive distance and object size.
    /// </summary>
    /// <returns></returns>
    private Vector3 SetScale() {

        var perspectiveFactor = CalculatePerspectiveFactor();
        var normalizedScale = perspectiveFactor * this.sizeFactor * this.initialScale;
        return normalizedScale;
    }

    /// <summary>
    /// Returns a scalar value based on the distance of the camera to the object.
    /// </summary>
    /// <returns></returns>
    private float CalculatePerspectiveFactor() {

        float scale = (Camera.main.transform.position - this.transform.position).magnitude;
        return scale;
    }

    /// <summary>
    /// Returns a scalar value based on the size of the screen.
    /// </summary>
    /// <returns></returns>
    private float CalculateScreenFactor() {

        float scale = Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height;
        return scale;
    }
}
