using System;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class TransformGizmo : MonoBehaviour {
    [Tooltip("The Transform to be controlled by the Gizmo")]
    public Transform RootTransform;

    [Tooltip("The GameObject to be controlled by the Gizmo")]
    public Transform SceneTransform;

    [Tooltip("The GameObject to be attached to the Scale handle")]
    public GameObject ScalePrefab;

    [Tooltip("The GameObject to be attached to the Rotate handle")]
    public GameObject RotatePrefab;

    [Tooltip("The Material assigned to the Box")]
    public Material Material;

    /// <summary>
    /// Keeps track of which face we are addressing.
    /// </summary>
    private enum BoxFace {
        Top,
        Bottom,
        Side
    }

    /// <summary>
    /// A matrix of vectors for translating one point to four corners.
    /// </summary>
    private static readonly Vector3[] CornerScales = new Vector3[]
    {
        new Vector3(  1, 1, 1 ),
        new Vector3( -1, 1, 1 ),
        new Vector3( -1, 1,-1 ),
        new Vector3(  1, 1,-1 )
    };

    /// <summary>
    /// The calculated corners of the bounding box.
    /// </summary>
    private Vector3[,] corners;

    /// <summary>
    /// The bounds of the targeted object.
    /// </summary>
    private Bounds bounds;

    /// <summary>
    /// Holds a reference to the FileManager instance.
    /// </summary>
    private SceneManager fileManager;

    #region MonoBehaviour Members

    private void Start() {
        this.fileManager = this.GetComponentInParent<SceneManager>();
        this.fileManager.ImportCompleted += OnImportCompleted;
    }

    #endregion

    /// <summary>
    /// Event handler for for ImportCompletion.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void OnImportCompleted(object sender, EventArgs e) {
        this.bounds = new Bounds();

        ExpandBounds(this.RootTransform);

        this.corners = LocateCorners(this.bounds);

        CreateTransformBox();

        //this.transform.localPosition = SceneTransform.localPosition;
        //this.transform.localRotation = SceneTransform.localRotation;
        //this.transform.localScale = SceneTransform.localScale;

        //this.targetObject.transform.parent = this.transform;
    }

    /// <summary>
    /// Recursively walk the scene root expanding the bounds of the gizmo.
    /// </summary>
    /// <param name="parent"></param>
    private void ExpandBounds(Transform parent) {
        foreach (Transform child in parent) {
            var filter = child.GetComponent<MeshFilter>();
            if (filter != null) {
                var mesh = filter.mesh;
                if (mesh == null) continue;
                this.bounds.Encapsulate(mesh.bounds);
            }
            ExpandBounds(child);
        }
    }

    /// <summary>
    /// Method to remove scaling from an object.
    /// </summary>
    /// <param name="targetObject"></param>
    private void NormalizeObject(GameObject targetObject) {
        var targetSize = targetObject.GetComponent<MeshFilter>().mesh.bounds.size;
        var maxSize = 1 / Mathf.Max(targetSize.x, Mathf.Max(targetSize.y, targetSize.z));
        targetObject.transform.localScale = new Vector3(maxSize, maxSize, maxSize);
    }


    /// <summary>
    /// Method to create subcomponents of TransformBox.
    /// </summary>
    private void CreateTransformBox() {
        CreateFrame("Edge");
        CreateTranslateGizmo("TranslateGizmo");
        CreateScaleGizmos("ScaleGizmo", BoxFace.Top);
        CreateScaleGizmos("ScaleGizmo", BoxFace.Bottom);
        CreateRotateGizmos("RotateGizmo", BoxFace.Side);
    }

    /// <summary>
    /// Method to add components to the TransformGizmo to enable Translation.
    /// </summary>
    /// <param name="name"></param>
    private void CreateTranslateGizmo(string name) {
        this.gameObject.AddComponent<BoxCollider>().size = this.bounds.size;
        this.gameObject.AddComponent<CursorTransform>().CursorType = CursorTransformEnum.Translate;
        this.gameObject.AddComponent<HandTranslate>().HostTransform = this.RootTransform;
    }

    /// <summary>
    /// Method to generate rotate gizmos for the four sides of the TransformBox
    /// </summary>
    /// <param name="name"></param>
    /// <param name="face"></param>
    void CreateRotateGizmos(string name, BoxFace face) {
        var gizmo = this.RotatePrefab;

        for (int i = 0; i < 4; ++i) {
            var position = (this.corners[0, i] + this.corners[1, i]) / 2;
            var instance = Instantiate(gizmo, position, Quaternion.identity);
            ConfigureGizmo(instance);
            instance.AddComponent<HandRotate>().HostTransform = this.RootTransform;
            instance.name = name;
        }
    }

    /// <summary>
    /// Method to generate scale gizmos for the four sides of the TransformBox
    /// </summary>
    /// <param name="name"></param>
    /// <param name="face"></param>
    void CreateScaleGizmos(string name, BoxFace face) {
        var gizmo = this.ScalePrefab;

        for (int i = 0; i < 4; ++i) {
            var position = this.corners[(int)face, i];
            var instance = Instantiate(gizmo, position, Quaternion.identity);
            ConfigureGizmo(instance);
            instance.AddComponent<HandScale>().HostTransform = this.RootTransform;
            instance.name = name;
        }
    }

    /// <summary>
    /// Method to set the Transforms and Components of each gizmo
    /// </summary>
    /// <param name="gizmo"></param>
    /// <param name="action"></param>
    private void ConfigureGizmo(GameObject gizmo) {
        gizmo.transform.localScale = Vector3.one * (this.bounds.size.magnitude / 100);
        gizmo.transform.parent = this.transform;
        gizmo.AddComponent<MeshRenderer>().material = this.Material;
    }

    /// <summary>
    /// Method to build the outline of the bouding box
    /// </summary>
    /// <returns></returns>
    private void CreateFrame(string name) {
        CreateLoop(this.gameObject, this.corners, name, BoxFace.Top);
        CreateLoop(this.gameObject, this.corners, name, BoxFace.Bottom);
        CreateRing(this.gameObject, this.corners, name, BoxFace.Side);
    }

    /// <summary>
    /// Method to generate the lines connecting faces
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="corners"></param>
    /// <param name="name"></param>
    /// <param name="face"></param>
    private void CreateRing(GameObject parent, Vector3[,] corners, string name, BoxFace face) {
        for (int j = 0; j < 4; ++j) {
            var vectors = new Vector3[] { corners[0, j], corners[1, j] };
            var line = new GameObject(name);
            AddLineRenderer(line).SetPositions(vectors);
            line.transform.parent = parent.transform;
        }
    }

    /// <summary>
    /// Method to generate the closed polygons for faces
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="corners"></param>
    /// <param name="name"></param>
    /// <param name="face"></param>
    private void CreateLoop(GameObject parent, Vector3[,] corners, string name, BoxFace face) {
        for (int j = 0; j < 4; ++j) {
            int k = ((j + 1) % 4 == 0) ? 0 : (j + 1);
            var vectors = new Vector3[] { corners[(int)face, j], corners[(int)face, k] };
            var line = new GameObject(name);
            AddLineRenderer(line).SetPositions(vectors);
            line.transform.parent = parent.transform;
        }
    }

    /// <summary>
    /// Method to create a LineRenderer component
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    private LineRenderer AddLineRenderer(GameObject line) {
        var renderer = line.AddComponent<LineRenderer>();
        renderer.widthMultiplier = 0.0125f;
        renderer.useWorldSpace = false;
        renderer.numPositions = 2;
        renderer.material = this.Material;
        return renderer;
    }

    /// <summary>
    /// Method to derive the corner points of the bounding box based 
    /// on the target object's bounds
    /// </summary>
    /// <param name="bounds"></param>
    /// <returns></returns>
    private static Vector3[,] LocateCorners(Bounds bounds) {
        var sign = 1;
        var corners = new Vector3[2, 4];

        for (int i = 0; i < 2; ++i) {
            for (int j = 0; j < 4; ++j) {
                int k = j + i * 2;
                k = (k > 3) ? k - 4 : k;
                var vector = Vector3.Scale(CornerScales[j], bounds.extents) * sign;
                corners[i, k] = CreatePoint(bounds.center, vector);
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
