namespace HeadsUp
{
    using System;
    using System.Collections;
    using UnityEngine;

    [Obsolete("Not in use", true)]
    public class RotateMovement : MonoBehaviour
    {
        public float rotateSpeed = -1.0f;

        void OnEnable()
        {
            StartCoroutine("ContinuousRotation");
        }

        IEnumerator ContinuousRotation()
        {
            while (true)
            {
                transform.Rotate(0, 0, rotateSpeed);
                yield return new WaitForSecondsRealtime(0.01f);
            }
        }

    }
}
