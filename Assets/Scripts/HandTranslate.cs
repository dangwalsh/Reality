using UnityEngine;

public class HandTranslate : HandTransformation
{
    protected override void ApplyTransformation(Vector3 pivotPosition, Vector3 newDraggingPosition, Vector3 delta)
    {
        if (IsOrientTowardsUser)
        {
            draggingRotation = Quaternion.LookRotation(HostTransform.position - pivotPosition);
        }
        else
        {
            Vector3 objForward = mainCamera.transform.TransformDirection(objRefForward);
            draggingRotation = Quaternion.LookRotation(objForward);
        }

        HostTransform.position = newDraggingPosition + mainCamera.transform.TransformDirection(objRefGrabPoint);
        HostTransform.rotation = draggingRotation;

        if (IsKeepUpright)
        {
            Quaternion upRotation = Quaternion.FromToRotation(HostTransform.up, Vector3.up);
            HostTransform.rotation = upRotation * HostTransform.rotation;
        }
    }
}

