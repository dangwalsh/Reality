using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class RemoveTarget : MonoBehaviour, IHoldHandler {

    [Tooltip("The GameObject to be removed")]
    public GameObject TargetObject;

    public void OnHoldCanceled(HoldEventData eventData) {
        //throw new NotImplementedException();
    }

    public void OnHoldCompleted(HoldEventData eventData) {
        InputManager.Instance.PopModalInputHandler();
        Destroy(this.TargetObject);
    }

    public void OnHoldStarted(HoldEventData eventData) {
        //throw new NotImplementedException();
    }

}
