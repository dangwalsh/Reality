namespace HeadsUp {

    using System.Collections;
    using UnityEngine;

    public class RotateSnapManager : SnapManager {

        protected override void SetSnapVector() {

            startVector = menusManager.ThisModel.transform.rotation.eulerAngles;
            float snapped = Mathf.Round(startVector.y / snapValue) * snapValue;
            endVector = new Vector3(0, snapped, 0);
            handController.QuantizeValue = snapValue;
            handController.TransformValue = snapped;
        }

        protected override IEnumerator LerpToSnap(Transform thisTransform, float duration) {

            float t = 0;
            while (t < 1) {
                handController.OnClick();      
                thisTransform.eulerAngles = Vector3.Lerp(startVector, endVector, t);   
                t += Time.deltaTime / duration;
                handController.OnClick();
                yield return null;
            }
            thisTransform.eulerAngles = endVector;
        }
    }
}