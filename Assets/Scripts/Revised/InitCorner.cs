using UnityEngine;

public class InitCorner : MonoBehaviour {

    [Tooltip("The object whose corner we want to find.")]
    public GameObject targetObject;

    /// <summary>
    /// Called once at instantiation.
    /// </summary>
	void Start () {

        var initBounds = this.targetObject.GetComponent<InitBounds>();
        var bounds = initBounds.Bounds;
        this.transform.position = bounds.min;
    }
}
