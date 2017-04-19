using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class CursorTransform : MonoBehaviour, ICursorTransform
{
    [Tooltip("The type of cursor that should be shown when hovering over this object.")]
    public CursorTransformEnum CursorType;

    public void GetModifiedCursorType(ICursor cursor, out CursorTransformEnum type)
    {
        type = GetModifiedType(cursor);
    }

    CursorTransformEnum GetModifiedType(ICursor cursor)
    {
        return CursorType;
    }
}
