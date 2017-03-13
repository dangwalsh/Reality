//using UnityEngine;
//using HoloToolkit.Unity.InputModule;
//using System;

//public class BoxManager : MonoBehaviour, IFocusable, IInputHandler, ISourceStateHandler
//{
//    public Transform HostTransform;
//    public float Factor = 5.0f;
//    public float Minimum = 0.01f;
//    public float Maximum = 100.0f;

//    bool isGazed;
//    bool isTransforming;
//    bool controlling;
//    uint currentInputSourceId;
//    IInputSource currentInputSource;
//    Vector3 currentHandPosition;
//    Vector3 newTransform;
//    Vector3 currentTransform;

//    public void OnFocusEnter()
//    {
//        if (isGazed)
//            return;
//        isGazed = true;
//    }

//    public void OnFocusExit()
//    {
//        isGazed = false;
//    }

//    public void OnInputDown(InputEventData eventData)
//    {
//        currentInputSource = eventData.InputSource;
//        currentInputSourceId = eventData.SourceId;
//        StartTransform();
//    }

//    public void OnInputUp(InputEventData eventData)
//    {
//        StopTransform();
//    }

//    public void OnSourceDetected(SourceStateEventData eventData)
//    {
//        // do nothing
//    }

//    public void OnSourceLost(SourceStateEventData eventData)
//    {
//        StopTransform();
//    }

//    void StartTransform()
//    {
//        controlling = !controlling;
//        var box = transform.Find("Bounding").gameObject;
//        box.SetActive(controlling);
//    }

//    void UpdateTransform()
//    {
//        // do nothing
//    }

//    void StopTransform()
//    {
//        // do nothing
//    }

//    void Start()
//    {
//        if (HostTransform == null)
//            HostTransform = transform;
//    }

//    void Update()
//    {
//        if (isTransforming)
//            UpdateTransform();
//    }
//}
