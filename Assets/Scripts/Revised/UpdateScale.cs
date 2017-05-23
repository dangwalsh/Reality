using UnityEngine;

public class UpdateScale : MonoBehaviour {

    [Tooltip("Tweak the base scale of the target object.")]
    public float ScaleAdjust = 0.2f;

    /// <summary>
    /// Called once per frame.
    /// </summary>
    private void Update () {

        var newScale = transform.parent.parent.localScale;
        this.transform.localScale = 
            new Vector3(this.ScaleAdjust / newScale.x,
                        this.ScaleAdjust / newScale.y,
                        this.ScaleAdjust / newScale.z);

	}
}
