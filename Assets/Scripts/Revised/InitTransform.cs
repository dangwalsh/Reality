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

    /// <summary>
    /// Called once at instantiation.
    /// </summary>
    private void Start () {

        var bounds = this.gameObject.GetComponent<InitBounds>().Bounds;
        var sizeFactor = bounds.extents.magnitude * this.sizeAdjustment;

        this.scaleToFit = this.transform.localScale / sizeFactor;
        this.transform.localScale = this.scaleToFit;
        this.transform.position += new Vector3(0, 0, 20);
    }
}
