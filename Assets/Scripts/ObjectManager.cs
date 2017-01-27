using UnityEngine;
using HoloToolkit.Unity.InputModule;
#if !UNITY_EDITOR
using Reality.HoloLens;
using UnityEngine.VR.WSA.Input;
#endif

public class ObjectManager : MonoBehaviour
{
    public GameObject Prefab;
#if !UNITY_EDITOR
    GestureRecognizer recognizer;
#else
    public string Path;
#endif
    GameObject instance;

    void Start()
    {
#if !UNITY_EDITOR
        ViewManager.FilePath = "";
        this.recognizer = new GestureRecognizer();
        this.recognizer.TappedEvent += OnTapped;
        this.recognizer.StartCapturingGestures();
#else
        instance = (GameObject) Instantiate(this.Prefab, new Vector3(0, 0, 0), Quaternion.identity);
        Geometry.Initialize(this.Path, instance);
#endif
    }

    void Update()
    {
#if !UNITY_EDITOR
        if (ViewManager.FilePath != "")
        {
            if (instance == null)
            {
                instance = (GameObject)Instantiate(this.Prefab, new Vector3(0, 0, 0), Quaternion.identity);
                Geometry.Initialize(ViewManager.FilePath, instance);
                ViewManager.FilePath = "";
            }
            else
            {
                Destroy(instance);
            }
        }
#endif
    }

#if !UNITY_EDITOR
    async void OnTapped(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        if (GazeManager.Instance.HitObject == null)
        {
            await ViewManager.SwitchViews();
        }
    }
#endif
}
