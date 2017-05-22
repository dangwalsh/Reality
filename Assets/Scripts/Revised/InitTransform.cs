using UnityEngine;

public class InitTransform : MonoBehaviour {

    [Tooltip("Multiplier for scale")]
    public float sizeAdjustment = 0.2f;

    /// <summary>
    /// Called once at instantiation.
    /// </summary>
    private void Start () {

        var bounds = this.gameObject.GetComponent<InitBounds>().Bounds;
        var sizeFactor = bounds.extents.magnitude * this.sizeAdjustment;

        this.transform.localScale /= sizeFactor;
        this.transform.position += new Vector3(0, 0, 20);
    }
}
