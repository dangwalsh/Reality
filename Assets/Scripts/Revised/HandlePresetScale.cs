using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class HandlePresetScale : MonoBehaviour, IInputHandler {

    [Tooltip("The transform to be controlled.")]
    public Transform targetTransform;

    public void OnInputDown(InputEventData eventData) {

        var scale = ChooseScale(this.targetTransform);
        this.targetTransform.localScale = scale;
        eventData.Reset();
    }

    private Vector3 ChooseScale(Transform target) {

        Vector3 scale = Vector3.one;
        if (target.localScale == scale) {

            var initTransform = target.gameObject.GetComponent<InitTransform>();
            if (initTransform != null)
                scale = initTransform.ScaleToFit;
        }
        return scale;
    }

    public void OnInputUp(InputEventData eventData) {

        // do nothing
    }
}
