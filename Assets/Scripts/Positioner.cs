using UnityEngine;

public class Positioner : MonoBehaviour {

    public Transform Root;
    public Transform Model;
    public Transform Gizmo;

    private Bounds bounds;
    /// <summary>
    /// The bounds of the targeted object.
    /// </summary>
    public Bounds Bounds {
        get {
            return this.bounds;
        }
    }

    private void Start() {

        this.bounds = new Bounds();
        AggregateBounds(this.Model);
        this.Gizmo.position = this.bounds.min;
        this.Root.localScale /= (this.bounds.size.magnitude / 5);
        this.Root.position += new Vector3(0, 0, 10);
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
}
