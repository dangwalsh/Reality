using UnityEngine;

#if !UNITY_EDITOR
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
