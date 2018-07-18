namespace HeadsUp
{
    using UnityEngine;
    using HoloToolkit.Unity;
    using HoloToolkit.Unity.InputModule;

    public class SourceManager : MonoBehaviour, ISourceStateHandler
    {
        public void OnSourceDetected(SourceStateEventData eventData)
        {
            Debug.Log("Source Detected");
        }

        public void OnSourceLost(SourceStateEventData eventData)
        {
            Debug.Log("Source Lost");
        }
    }
}
