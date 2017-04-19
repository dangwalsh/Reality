using UnityEngine;

public class HandRotate : HandTransformation
{
    protected override void ApplyTransformation(Vector3 pivotPosition, Vector3 newDraggingPosition, Vector3 delta)
    {
        // sum the components of the delta vector to get a unified rotation magnitude and convert to degrees
        float rotationFactor = Mathf.Rad2Deg * (delta.x + delta.y + delta.z);
        HostTransform.Rotate(new Vector3(0, rotationFactor, 0));
        draggingPosition = newDraggingPosition;
    }
}


