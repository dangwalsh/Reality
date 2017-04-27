using UnityEngine;

public class ModelImport : MonoBehaviour {
    [Tooltip("GameObject container for a newly imported geometry file.")]
    public GameObject TargetObject;

    public void ImportModel(string path) {
#if !UNITY_EDITOR
        Geometry.Initialize(path, this.TargetObject);
#endif
    }
}
