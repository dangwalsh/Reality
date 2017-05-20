using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class HandleRemove : MonoBehaviour, IInputClickHandler {

    [Tooltip("The GameObject to be removed")]
    public GameObject targetObject;

    /// <summary>
    /// Destroy the target when clicked.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnInputClicked(InputEventData eventData) {

        Destroy(this.targetObject);
    }
}
