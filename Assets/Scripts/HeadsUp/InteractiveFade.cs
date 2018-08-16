namespace HeadsUp
{
    using System.Collections;
    using UnityEngine;

    [RequireComponent(typeof(CanvasGroup))]
    public class InteractiveFade : MonoBehaviour
    {
        CanvasGroup group;

        void Start()
        {
            group = GetComponent<CanvasGroup>();
            group.alpha = 0;
        }

        public void HandlePointerEnter()
        {
            StartCoroutine(Fade(0, 1, 4));         
        }

        public void HandlePointerExit()
        {
            StartCoroutine(Fade(1, 0, 4));
        }

        IEnumerator Fade(float start, float end, float rate)
        {
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * rate;
                group.alpha = Mathf.Lerp(start, end, t);
                yield return null;
            }
        }
    }
}