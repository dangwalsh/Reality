using UnityEngine;

public class InitPosition : MonoBehaviour {

    /// <summary>
    /// Called once at instantiation.
    /// </summary>
    void Start() {
        this.transform.position += new Vector3(0, 0, 20);
    }
}
