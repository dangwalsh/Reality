namespace HeadsUp {

    using UnityEngine;

    public class HandRotate : HandTransformation {

        private void Awake() {
            this.TransformValue = HostTransform.rotation.eulerAngles.y;
        }

        protected override void ApplyTransformation(Vector3 pivotPosition, Vector3 newDraggingPosition, Vector3 delta, bool isPerpetual = true) {
            // sum the components of the delta vector to get a unified rotation magnitude and convert to degrees
            this.HandValue = delta.x + delta.y + delta.z;
            float rotationFactor = Mathf.Rad2Deg * this.HandValue;

            if (isPerpetual) {

                HostTransform.Rotate(Vector3.up, rotationFactor * Time.deltaTime);
            }
            else {

                HostTransform.Rotate(new Vector3(0, rotationFactor, 0));
                draggingPosition = newDraggingPosition;
            }

            this.TransformValue = HostTransform.rotation.eulerAngles.y;
        }
    }
}

