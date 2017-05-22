using UnityEngine;

public class UpdateScene : MonoBehaviour {

    [Tooltip("The Prefab that gets instantiated to hold a new model.")]
    public GameObject modelPrefab;

    private string args = "";

    /// <summary>
    /// Called once per frame.
    /// </summary>
    private void Update() {

        var arguments = UnityEngine.WSA.Application.arguments;
        if (arguments == args || arguments == "") return;
        Instantiate(this.modelPrefab, Vector3.zero, Quaternion.identity, this.transform);
        this.args = arguments;
    }
}
