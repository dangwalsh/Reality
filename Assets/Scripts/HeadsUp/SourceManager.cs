namespace HeadsUp
{
    using UnityEngine;
    using HoloToolkit.Unity.InputModule;
    using System;

    [Obsolete("Not in use", true)]
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
