using UnityEngine;

public class InitTransform : MonoBehaviour {

    [Tooltip("Multiplier for scale")]
    public float sizeAdjustment = 0.2f;

    private Vector3 scaleToFit;
    /// <summary>
    /// Exposes the Fit Scale.
    /// </summary>
    public Vector3 ScaleToFit {
        get { return this.scaleToFit; }
    }

    private float sizeFactor;
    /// <summary>
    /// Exposes the size factor
    /// </summary>
    public float SizeFactor {
        get { return this.sizeFactor; }
    }

    /// <summary>
    /// Called once at instantiation.
    /// </summary>
    private void Start () {

        var bounds = this.gameObject.GetComponent<InitBounds>().Bounds;
        this.sizeFactor = bounds.extents.magnitude * this.sizeAdjustment;

        this.scaleToFit = this.transform.localScale / this.sizeFactor;
        this.transform.localScale = this.scaleToFit;
        this.transform.position += new Vector3(0, 0, 20);
    }
}
