using UnityEngine;

public class GizmoManager : MonoBehaviour {

    [Tooltip("Tweaks the size of the gizmo.")]
    public float scaleFactor;

    [Tooltip("The object representing the Rotate handle.")]
    public GameObject rotateGizmo;

    [Tooltip("The object representing the Scale handle.")]
    public GameObject scale1Gizmo;

    [Tooltip("The object representing the Scale handle.")]
    public GameObject scale2Gizmo;

    [Tooltip("The object representing the Translate handle.")]
    public GameObject translateGizmo;

    [Tooltip("The object representing the Remove handle.")]
    public GameObject removeGizmo;

    [Tooltip("The global material for the Gizmo.")]
    public Material material;

    private float magnitude;
    private float unit;
    private Vector3 center;
    private Vector3 size;
    private Vector3 xVec;
    private Vector3 yVec;
    private Vector3 zVec;
    private GameObject xAxis;
    private GameObject yAxis;
    private GameObject zAxis;

    private GameObject translate;
    private GameObject rotate;
    private GameObject scale1;
    private GameObject scale2;
    private GameObject remove;

    /// <summary>
    /// Called once at instantiation.
    /// </summary>
    private void Awake() {

        UpdateVectors();

        xAxis = new GameObject("X Axis");
        yAxis = new GameObject("Y Axis");
        zAxis = new GameObject("Z Axis");

        CreateLine(this.xAxis, this.center, this.center + this.xVec, this.unit, this.material);
        CreateLine(this.yAxis, this.center, this.center + this.yVec, this.unit, this.material);
        CreateLine(this.zAxis, this.center, this.center + this.zVec, this.unit, this.material);

        this.translate = Instantiate<GameObject>(this.translateGizmo, this.center, Quaternion.identity);
        this.rotate = Instantiate<GameObject>(this.rotateGizmo, this.center + this.yVec / 2.0f, Quaternion.identity);
        this.scale1 = Instantiate<GameObject>(this.scale1Gizmo, this.center + this.yVec, Quaternion.identity);
        this.scale2 = Instantiate<GameObject>(this.scale1Gizmo, this.center + this.xVec, Quaternion.identity);
        this.remove = Instantiate<GameObject>(this.scale1Gizmo, this.center + this.zVec, Quaternion.identity);
    }

    /// <summary>
    /// Called every frame.
    /// </summary>
    private void Update() {

        UpdateVectors();

        this.translate.transform.position = this.center;
        this.rotate.transform.position = this.center + this.yVec / 2.0f;
        this.scale1.transform.position = this.center + this.xVec;
        this.scale2.transform.position = this.center + this.yVec;
        this.remove.transform.position = this.center + this.zVec;
    }

    /// <summary>
    /// Generates all of the vectors needed to draw the Gizmo.
    /// </summary>
    private void UpdateVectors() {

        this.center = this.transform.position;
        this.magnitude = CalculateMagnitude();
        this.unit = this.magnitude / 10.0f;

        this.size = new Vector3(this.unit, this.unit, this.unit);
        this.xVec = new Vector3(this.magnitude, 0, 0);
        this.yVec = new Vector3(0, this.magnitude, 0);
        this.zVec = new Vector3(0, 0, this.magnitude);
    }

    /// <summary>
    /// Adds a new new instance of LineRenderer to the Gizmo.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="width"></param>
    /// <param name="material"></param>
    private void CreateLine(GameObject lineObject, Vector3 from, Vector3 to, float width, Material material) {

        var line = lineObject.AddComponent<LineRenderer>();
        line.material = material;
        line.startWidth = width;
        line.endWidth = width;
        line.useWorldSpace = false;
        line.numPositions = 2;
        line.SetPositions(new Vector3[] { from, to });
        line.transform.parent = this.transform;
    }

    /// <summary>
    /// Returns a scalar value relating the size of the gizmo either to its location or the screen.
    /// </summary>
    /// <returns></returns>
    private float CalculateMagnitude() {

        float magnitude;
#if SCREEN_REL
        magnitude = Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height;
#else
        magnitude = (Camera.main.transform.position - this.transform.position).magnitude;
#endif
        return magnitude / this.scaleFactor;
    }
}
