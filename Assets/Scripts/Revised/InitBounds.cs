using UnityEngine;

public class InitBounds : MonoBehaviour {

    
    private readonly Vector3[] CornerScales = new Vector3[] {
        new Vector3(  1, 1, 1 ),
        new Vector3( -1, 1, 1 ),
        new Vector3( -1, 1,-1 ),
        new Vector3(  1, 1,-1 )
    };

    private Vector3[,] corners;
    /// <summary>
    /// Exposes the calculated corner points of this instance.
    /// </summary>
    public Vector3[,] Corners {
        get { return this.corners; }
    }

    private Bounds bounds = new Bounds();
    /// <summary>
    /// Exposes the calculated bounds instance.
    /// </summary>
    public Bounds Bounds {
        get { return this.bounds; }
    }

    /// <summary>
    /// Called once at instantiation.
    /// </summary>
    private void Awake() {
 
        AggregateBounds(this.transform);
        LocateCorners(this.bounds);
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
    /// Method to derive the corner points of the bounding box based 
    /// on the target object's bounds
    /// </summary>
    /// <param name="bounds"></param>
    /// <returns></returns>
    private Vector3[,] LocateCorners(Bounds bounds) {
        var sign = 1;
        this.corners = new Vector3[2, 4];

        for (int i = 0; i < 2; ++i) {
            for (int j = 0; j < 4; ++j) {
                int k = j + i * 2;
                k = (k > 3) ? k - 4 : k;
                var vector = Vector3.Scale(CornerScales[j], bounds.extents) * sign;
                this.corners[i, k] = CreatePoint(bounds.center, vector);
            }
            sign *= -1;
        }

        return corners;
    }

    /// <summary>
    /// Method to generate a vector from two other vectors.
    /// </summary>
    /// <param name="center"></param>
    /// <param name="vector"></param>
    /// <returns></returns>
    private static Vector3 CreatePoint(Vector3 center, Vector3 vector) {
        return center + vector;
    }
}
