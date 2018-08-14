namespace HeadsUp
{
    using System;
    using System.Collections;
    using UnityEngine;

    [Obsolete("Not in use", true)]
    public class WaiterManager : MonoBehaviour
    {
        Camera mainCamera;

        void Start()
        {
            mainCamera = Camera.main;
        }

        void Update()
        {
            StartCoroutine(FollowCamera());
        }

        IEnumerator FollowCamera()
        {
            var timer = 0f;
            var start = transform.position;
            var target = Vector3.zero;

            while (timer < 1)
            {
                timer += Time.deltaTime;
                target = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 2));
                transform.position = Vector3.Lerp(start, target, timer);
                transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
                yield return null;
            }
        }
    }
}
