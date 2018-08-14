namespace HeadsUp
{
    using System;
    using UnityEngine;

    [Obsolete("Not in use", true)]
    public class LoadingCircle : MonoBehaviour
    {
        private RectTransform rectComponent;
        public float rotateSpeed = 200f;

        private void Start()
        {
            rectComponent = GetComponent<RectTransform>();
        }

        private void Update()
        {
            rectComponent.Rotate(0f, 0f, -rotateSpeed * Time.deltaTime);
        }
    }
}
