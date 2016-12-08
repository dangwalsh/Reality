using UnityEngine;
using System;

#if !UNITY_EDITOR
using Reality.HoloLens;
using UnityEngine.VR.WSA.Input;
#endif


public class Main : MonoBehaviour
{
    GameObject rootObject;
#if !UNITY_EDITOR
    GestureRecognizer mRecognizer;
#else
    public string Path;
#endif

    void Start()
    {
        this.rootObject = new GameObject("Root");
#if !UNITY_EDITOR
        ViewManager.FilePath = "";
        this.mRecognizer = new GestureRecognizer();
        this.mRecognizer.TappedEvent += OnTapped;
        this.mRecognizer.StartCapturingGestures();
#else
        Geometry.Initialize(this.Path);
#endif
    }

    void Update()
    {
        rootObject.transform.Rotate(Vector3.up * Time.deltaTime * 10);
#if !UNITY_EDITOR
        if (ViewManager.FilePath != "")
        {
            Geometry.Initialize(ViewManager.FilePath);
            ViewManager.FilePath = "";
        }
#endif
    }

#if !UNITY_EDITOR
    async void OnTapped(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        await ViewManager.SwitchViews();
    }
#endif

    void OnFileChanged(object sender, EventArgs e)
    {
        Geometry.Initialize(sender as string);
    }
}
