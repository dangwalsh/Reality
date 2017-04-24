using System;
using UnityEngine;

public class ModelManager : MonoBehaviour {
    [Tooltip("GameObject container for a newly imported geometry file.")]
    public GameObject Model;

    public void ImportModel(string path) {
#if !UNITY_EDITOR
        Geometry.Initialize(path, this.Model);
#endif
    }
}
