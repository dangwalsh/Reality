using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System.Collections;

public class RemoveTarget : MonoBehaviour, IInputClickHandler {

    [Tooltip("The GameObject to be removed")]
    public GameObject TargetObject;

    private int count = 0;

    public void OnInputClicked(InputEventData eventData) {
        Destroy(this.TargetObject);
    }

    private IEnumerator ClickCount() {
        WaitForSeconds wait = new WaitForSeconds(0.05f);
        for (int i = 0; i < 2; i++) {
            this.count++;
            yield return wait;
        }
        this.count = 0;
    }
}
