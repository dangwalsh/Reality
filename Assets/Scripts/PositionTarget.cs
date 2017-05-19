using UnityEngine;

public class PositionTarget : MonoBehaviour {

    public Transform root;
    public Transform model;
    public Transform target;

    private float sizeFactor;
    private Vector3 initialScale;

    private Bounds bounds = new Bounds();
    /// <summary>
    /// The bounds of the targeted object.
    /// </summary>
    public Bounds Bounds {
        get {
            return this.bounds;
        }
    }

    /// <summary>
    /// Called once after instantiation.
    /// </summary>
    private void Start() {

        AggregateBounds(this.model);

        this.initialScale = this.target.localScale;
        this.sizeFactor = this.bounds.size.magnitude / 5;
        this.target.position = this.bounds.min;
        this.root.localScale /= this.sizeFactor;
        this.root.position += new Vector3(0, 0, 10);
    }

    /// <summary>
    /// Called once per frame.
    /// </summary>
    private void Update() {

        this.target.localScale = UpdateScale();
    }

    /// <summary>
    /// Scale independent of perspetive distance and object size.
    /// </summary>
    /// <returns></returns>
    private Vector3 UpdateScale() {

        var perspectiveFactor = CalculatePerspectiveFactor();
        var normalizedScale = perspectiveFactor * this.sizeFactor * this.initialScale;
        return normalizedScale;
    }

    /// <summary>
    /// Recursively walk the scene root expanding the bounds of the gizmo.
    /// </summary>
    /// <param name="parent"></param>
    private void AggregateBounds(Transform parent) {

        foreach (Transform child in parent) {
            var filter = child.GetComponent<MeshFilter>();
            if (filter != null) {
                var mesh = filter.mesh;
                if (mesh == null) continue;
                this.bounds.Encapsulate(mesh.bounds);
            }
            AggregateBounds(child);
        }
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
