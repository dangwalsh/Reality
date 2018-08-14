namespace HeadsUp {

    using UnityEngine;
    using System.Collections;

    public class SnapManagerScale : SnapManager {

        protected override void SetSnapVector() {

            startVector = menusManager.ThisModel.transform.localScale;
            endVector = new Vector3(1 / snapValue, 1 / snapValue, 1 / snapValue);
            handController.TransformValue = 1 / snapValue;
        }

        protected override IEnumerator LerpToSnap(Transform thisModel, float duration) {

            float t = 0;
            while (t < 1) {
                thisModel.localScale = Vector3.Lerp(startVector, endVector, t);
                t += Time.deltaTime / duration;
                yield return null;
            }
            thisModel.localScale = endVector;

        }
    }
}