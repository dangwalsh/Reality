﻿using UnityEngine;

public class InitBox : MonoBehaviour {

    [Tooltip("The width of the lines")]
    public float widthMultipler = 0.02f;

    [Tooltip("The box material")]
    public Material material;

    /// <summary>
    /// Keeps track of which face we are addressing.
    /// </summary>
    private enum BoxFace {
        Top,
        Bottom,
        Side
    }

    private void Start() {

        var initBounds = this.gameObject.GetComponent<InitBounds>();
        var corners = initBounds.Corners;
            
        CreateFrame("Edge", corners);
    }

    /// <summary>
    /// Method to build the outline of the bouding box
    /// </summary>
    /// <returns></returns>
    private void CreateFrame(string name, Vector3[,] corners) {

        CreateLoop(this.gameObject, corners, name, BoxFace.Top);
        CreateLoop(this.gameObject, corners, name, BoxFace.Bottom);
        CreateRing(this.gameObject, corners, name, BoxFace.Side);
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
        renderer.widthMultiplier = this.widthMultipler;
        renderer.useWorldSpace = false;
        renderer.numPositions = 2;
        renderer.material = this.material;
        return renderer;
    }
}
