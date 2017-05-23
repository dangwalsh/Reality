using UnityEngine;

public class UpdateTransformScale : MonoBehaviour {

    [Tooltip("Tweak the base scale of the target object.")]
    public float ScaleAdjust = 0.2f;

    private void Update () {

        var newScale = transform.parent.parent.localScale;
        this.transform.localScale = 
            new Vector3(this.ScaleAdjust / newScale.x,
                        this.ScaleAdjust / newScale.y,
                        this.ScaleAdjust / newScale.z);

	}
}
