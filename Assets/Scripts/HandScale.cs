using UnityEngine;


public class HandScale : HandTransformation
{
    protected override void ApplyTransformation(Vector3 pivotPosition, Vector3 newDraggingPosition, Vector3 delta)
    {
        // simply sum all of the components of the delta vector to get a unified scale factor
        float scaleFactor = (delta.x + delta.y + delta.z);
        Vector3 currentScale = HostTransform.localScale;
        Vector3 newScale = scaleFactor * currentScale;
        HostTransform.localScale += newScale;
        //HostTransform.localScale += Vector3.Scale(new Vector3(scaleFactor, scaleFactor, scaleFactor), currentScale);
        draggingPosition = newDraggingPosition;
    }
}
