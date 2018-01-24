using UnityEngine;

public class SceneManager : MonoBehaviour {

    [Tooltip("The Prefab that gets instantiated to hold a new model.")]
    public GameObject ModelManagerPrefab;
   
    private string args = "";

    #region MonoBehaviour Members
    private void Update() {
        
        var arguments = UnityEngine.WSA.Application.arguments;
        if (arguments == args || arguments == "") return;
        args = UnityEngine.WSA.Application.arguments;
        Instantiate(ModelManagerPrefab, Vector3.zero, Quaternion.identity, transform);
    }
    #endregion
}
