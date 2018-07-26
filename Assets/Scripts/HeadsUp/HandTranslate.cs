namespace HeadsUp
{
    using HoloToolkit.Unity.InputModule;
    using System.Collections;
    using UnityEngine;

    public class HandTranslate : HandTransformation
    {
        protected override void ApplyTransformation(Vector3 pivotPosition, Vector3 newPosition, Vector3 delta, bool isPerpetual = true)
        {
            if (IsBeingPlaced)
            {
                var nextPosition = GazeManager.Instance.HitPosition;
                StartCoroutine(LerpToPosition(nextPosition, 0.1f));
                draggingPosition = nextPosition;
            }
        }

        protected IEnumerator LerpToPosition(Vector3 endPosition, float duration)
        {
            float t = 0;
            while (t < 1)
            {
                gameObject.transform.position = Vector3.Lerp(draggingPosition, endPosition, t);
                t += Time.deltaTime / duration;
                yield return null;
            }
            gameObject.transform.position = endPosition;
        }

    }
}
