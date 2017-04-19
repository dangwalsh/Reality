using UnityEngine;


public class HandScale : HandTransformation
{
    protected override void ApplyTransformation(Vector3 pivotPosition, Vector3 newDraggingPosition, Vector3 delta)
    {
        // simply sum all of the components of the delta vector to get a unified scale factor
        float scaleFactor = (delta.x + delta.y + delta.z);
        HostTransform.localScale += new Vector3(scaleFactor, scaleFactor, scaleFactor);
        draggingPosition = newDraggingPosition;
    }
}
