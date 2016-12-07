using UnityEngine;
using System;

#if !UNITY_EDITOR
using Reality.HoloLens;
using UnityEngine.VR.WSA.Input;
#endif


public class Main : MonoBehaviour
{
#if !UNITY_EDITOR
    GestureRecognizer mRecognizer;
#else
    public string Path;
#endif

    void Start()
    {
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
#if !UNITY_EDITOR
        if (ViewManager.FilePath != "")
        {
            Geometry.CreateObjects(ViewManager.FilePath);
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
