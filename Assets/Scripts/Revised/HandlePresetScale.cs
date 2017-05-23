using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

public class HandlePresetScale : MonoBehaviour, IInputHandler {

    [Tooltip("The transform to be controlled.")]
    public Transform targetTransform;

    public void OnInputDown(InputEventData eventData) {

        var scale = ChooseScale(this.targetTransform);
        this.targetTransform.localScale = scale;
        eventData.Reset();
    }

    public void OnInputUp(InputEventData eventData) {

        // do nothing
    }

    /// <summary>
    /// Logic behind the scale toggle action.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private Vector3 ChooseScale(Transform target) {

        UpdateCursor(CursorTransformEnum.Fit);
        Vector3 scale = Vector3.one;
        if (target.localScale == scale) {

            var initTransform = target.gameObject.GetComponent<InitTransform>();
            if (initTransform != null) {
                scale = initTransform.ScaleToFit;
                UpdateCursor(CursorTransformEnum.OneToOne);
            }
        }
        return scale;
    }

    /// <summary>
    /// Changes the CursorType of this gameObject
    /// </summary>
    /// <param name="cursorType"></param>
    private void UpdateCursor(CursorTransformEnum cursorType) {

        var cursor = this.gameObject.GetComponent<CursorTransform>();
        cursor.CursorType = cursorType;
    }
}
