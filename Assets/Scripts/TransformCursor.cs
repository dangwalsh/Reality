// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using UnityEngine;

/// <summary>
/// The object cursor can switch between different game objects based on its state.
/// It simply links the game object to set to active with its associated cursor state.
/// </summary>
public class TransformCursor : Cursor {
    [Serializable]
    public struct ObjectCursorDatum {
        public string Name;
        public CursorStateEnum CursorState;
        public GameObject CursorObject;
    }

    [Serializable]
    public struct CursorActionDatum {
        public string Name;
        public CursorTransformEnum CursorType;
        public GameObject CursorSecondary;
    }

    [SerializeField]
    public ObjectCursorDatum[] CursorStateData;

    [SerializeField]
    public CursorActionDatum[] CursorActionData;

    /// <summary>
    /// Sprite renderer to change.  If null find one in children
    /// </summary>
    public Transform ParentTransform;

    /// <summary>
    /// On enable look for a sprite renderer on children
    /// </summary>
    protected override void OnEnable() {
        if (ParentTransform == null) {
            ParentTransform = transform;
        }
        for (int i = 0; i < ParentTransform.childCount; i++) {
            ParentTransform.GetChild(i).gameObject.SetActive(false);
        }
        base.OnEnable();
    }

    /// <summary>
    /// Override OnCursorType change to set the correct animation
    /// </summary>
    /// <param name="type"></param>
    public override void OnCursorActionChange(CursorTransformEnum type) {
        base.OnCursorActionChange(type);

        var newActive = new CursorActionDatum();
        for (int cursorIndex = 0; cursorIndex < CursorActionData.Length; cursorIndex++) {
            CursorActionDatum cursor = CursorActionData[cursorIndex];
            if (cursor.CursorType == type) {
                newActive = cursor;
                break;
            }
        }

        if (newActive.Name == null) {
            return;
        }

        for (int cursorIndex = 0; cursorIndex < CursorActionData.Length; cursorIndex++) {
            CursorActionDatum cursor = CursorActionData[cursorIndex];
            if (cursor.CursorSecondary.activeSelf) {
                cursor.CursorSecondary.SetActive(false);
                break;
            }
        }

        newActive.CursorSecondary.SetActive(true);
    }

    /// <summary>
    /// Override OnCursorState change to set the correct animation
    /// state for the cursor
    /// </summary>
    /// <param name="state"></param>
    public override void OnCursorStateChange(CursorStateEnum state) {
        base.OnCursorStateChange(state);
        if (state != CursorStateEnum.Contextual) {

            // First, try to find a cursor for the current state
            var newActive = new ObjectCursorDatum();
            for (int cursorIndex = 0; cursorIndex < CursorStateData.Length; cursorIndex++) {
                ObjectCursorDatum cursor = CursorStateData[cursorIndex];
                if (cursor.CursorState == state) {
                    newActive = cursor;
                    break;
                }
            }

            // If no cursor for current state is found, let the last active cursor be
            // (any cursor is better than an invisible cursor)
            if (newActive.Name == null) {
                return;
            }

            // If we come here, there is a cursor for the new state, 
            // so de-activate a possible earlier active cursor
            for (int cursorIndex = 0; cursorIndex < CursorStateData.Length; cursorIndex++) {
                ObjectCursorDatum cursor = CursorStateData[cursorIndex];
                if (cursor.CursorObject.activeSelf) {
                    cursor.CursorObject.SetActive(false);
                    break;
                }
            }

            // ... and set the cursor for the new state active.
            newActive.CursorObject.SetActive(true);
        }
    }

    protected override void InstantiateObjects() {
        foreach (var state in this.CursorStateData)
            Instantiate(state.CursorObject, new Vector3(), Quaternion.identity);

        foreach (var action in this.CursorActionData)
            Instantiate(action.CursorSecondary, new Vector3(), Quaternion.identity);
    }
}

