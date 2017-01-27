//using UnityEngine;
//using UnityEngine.EventSystems;
//using HoloToolkit.Unity.InputModule;
//#if !UNITY_EDITOR
//using Reality.HoloLens;
//using UnityEngine.VR.WSA.Input;
//#endif

//public class GeometryManager : MonoBehaviour
//{
//    GameObject rootObject;
//#if !UNITY_EDITOR
//    GestureRecognizer recognizer;
//#else
//    public string Path;
//#endif
//    public static float[] Bounds;

//    void Start()
//    {
//#if !UNITY_EDITOR
//        ViewManager.FilePath = "";
//        this.recognizer = new GestureRecognizer();
//        this.recognizer.TappedEvent += OnTapped;
//        this.recognizer.StartCapturingGestures();
//#else
//        Bounds = Geometry.Initialize(this.Path, this.gameObject);
//#endif
//    }

//    void Update()
//    {
//#if !UNITY_EDITOR
//        if (ViewManager.FilePath != "")
//        {
//            Bounds = Geometry.Initialize(ViewManager.FilePath, this.gameObject);
//            ViewManager.FilePath = "";
//        }
//#endif
//        //this.gameObject.transform.Rotate(Vector3.up * Time.deltaTime * 10);
//    }

//#if !UNITY_EDITOR
//    async void OnTapped(InteractionSourceKind source, int tapCount, Ray headRay)
//    {
//        if (GazeManager.Instance.HitObject == null)
//            await ViewManager.SwitchViews();
//    }
//#endif
//}
