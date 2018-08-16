namespace HeadsUp
{
    using System.Collections;
    using UnityEngine;

    public class InteractiveScale : MonoBehaviour
    {
        private float rate = 4.0f;
        private Vector3 increase = new Vector3(1.2f, 1.2f, 1.2f);

        public void HandlePointerEnter()
        {
            StartCoroutine(ScaleUp());
        }

        public void HandlePointerExit()
        {
            StartCoroutine(ScaleDown());
        }

        IEnumerator ScaleUp()
        {
            float t = 0.0f;
            while (t < 1.0f)
            {
                t += Time.deltaTime * rate;
                this.transform.localScale = Vector3.Lerp(Vector3.one, increase, Mathf.SmoothStep(0.0f, 1.0f, t));
                yield return null;
            }
        }

        IEnumerator ScaleDown()
        {
            float t = 0.0f;
            while (t < 1.0f)
            {
                t += Time.deltaTime * rate;
                this.transform.localScale = Vector3.Lerp(increase, Vector3.one, Mathf.SmoothStep(0.0f, 1.0f, t));
                yield return null;
            }
        }
    }
}
