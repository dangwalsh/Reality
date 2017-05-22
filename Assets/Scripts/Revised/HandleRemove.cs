using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class HandleRemove : MonoBehaviour, IInputHandler {

    [Tooltip("The GameObject to be removed")]
    public GameObject targetObject;

    /// <summary>
    /// Called when AirTap gesture is started.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnInputDown(InputEventData eventData) {

        Destroy(this.targetObject);
    }

    /// <summary>
    /// Called when AirTap gesture is completed.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnInputUp(InputEventData eventData) {

        // do nothing
    }
}
