using UnityEngine;
using HoloToolkit.Unity.InputModule;

public interface ICursorTransform
{
    void GetModifiedCursorType(ICursor cursor, out CursorTransformEnum type);
}

