using UnityEngine;

public class InitCorner : MonoBehaviour {

    public enum Location { Min, Center, Max}

    [Tooltip("The object whose corner we want to find.")]
    public GameObject targetObject;

    [Tooltip("With what point does the object align?")]
    public Location location = Location.Min;

    /// <summary>
    /// Called once at instantiation.
    /// </summary>
	void Start () {

        var initBounds = this.targetObject.GetComponent<InitBounds>();
        var bounds = initBounds.Bounds;

        switch(this.location) {

            case Location.Min:
                this.transform.position = bounds.min;
                break;
            case Location.Center:
                this.transform.position = bounds.center;
                break;
            case Location.Max:
                this.transform.position = bounds.max;
                break;
            default:
                throw new System.Exception("There is no corresponding location.");
        } 
    }
}

