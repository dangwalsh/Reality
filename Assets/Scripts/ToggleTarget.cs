using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class ToggleTarget : MonoBehaviour, IInputHandler {
    [Tooltip("The GameObject to be toggled ON.")]
    public GameObject targetObject;

    public void OnInputDown(InputEventData eventData) {
        this.targetObject.SetActive(!this.targetObject.activeSelf);
    }

    public void OnInputUp(InputEventData eventData) {
        var statsObject = GameObject.Find("StatsManager");
        var stats = statsObject.GetComponent<StatsUpdater>();
        stats.TargetTransform = targetObject.transform;
    }
}
