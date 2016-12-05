using UnityEngine;
using UnityEngine.VR.WSA.Input;


public class Main : MonoBehaviour
{
    public string Path;
    GestureRecognizer mRecognizer;

    void Start()
    {
#if !UNITY_EDITOR
        UniversalWindows.ViewManager.FilePath = "";
        this.mRecognizer = new GestureRecognizer();
        this.mRecognizer.TappedEvent += OnTapped;
        this.mRecognizer.StartCapturingGestures();
#else
        Geometry.CreateObjects(this.Path);
#endif
    }

    void Update()
    {
#if !UNITY_EDITOR
        if (UniversalWindows.ViewManager.FilePath != "")
        {
            Geometry.CreateObjects(UniversalWindows.ViewManager.FilePath);
            UniversalWindows.ViewManager.FilePath = "";
        }
#endif
    }

#if !UNITY_EDITOR
    async void OnTapped(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        await UniversalWindows.ViewManager.SwitchViews();
    }
#endif
}
